using System;
using System.Collections.Generic;
using System.Linq;
using Chat.DAL.Entities;
using Chat.DAL.Models;
using Chat.DAL.Models.Pagging;
using Chat.DAL.Repositories.Interfaces;
using static Chat.DAL.Models.LoginResponse;

namespace Chat.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        ChatContext context;

        public UserRepository(ChatContext ctx)
        {
            this.context = ctx;
        }

        public LoginResponse Authorize(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return LoginResponse.InvalidUserData;

            var user = context.mdl_user.FirstOrDefault(f => f.email == email && f.record_state != 1);

            if (user == null)
                return LoginResponse.EmailNotFound;

            if (user.password != password)
                return LoginResponse.InvalidCredential;

            return new LoginResponse()
            {
                UserId = user.id,
                Name = user.name,
                Email = user.email,
                Avatar = user.avatar,

                ErrorCode = ErrorCodeType.None
            };
        }

        public RegisterResponse CheckLogin(string email)
        {
            var user = context.mdl_user.FirstOrDefault(f => f.email == email && f.record_state != 1);
            if (user != null)
                return RegisterResponse.EmailExists;

            return null;
        }

        public bool Delete(int user_id)
        {
            var dbUser = context.mdl_user.Find(user_id);

            if (dbUser == null) return false;

            dbUser.record_state = 1;
            context.SaveChanges();

            return true;
        }

        public IEnumerable<User> GetAll(string email)
        {
            var res = context.mdl_user
                    .Where(w => w.email != email && w.record_state != 1)
                    .OrderByDescending(o => o.creation_date).ToList();
            return Map(res);
        }

        public PagingList<User> GetAll(int page, int pageSize)
        {
            var res = new PagingList<User>();
            var valueItems = new List<User>();

            var values = context.mdl_user.Where(w => w.record_state != 1).OrderByDescending(o => o.creation_date).ToList();

            var result = values.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            res.Items = Map(result);
            res.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = values.Count()
            };

            return res;
        }

        public User GetUser(string email)
        {
            var user = context.mdl_user.Where(w => w.email == email && w.record_state != 1).FirstOrDefault();
            return user == null ? null : user.ToUser();
        }

        public User GetUser(int id)
        {
            var user = context.mdl_user.Where(w => w.id == id && w.record_state != 1).FirstOrDefault();
            return user == null ? null : user.ToUser();
        }

        public RegisterResponse Register(User account)
        {
            if (account == null)
                return RegisterResponse.InvalidUserData;

            if (string.IsNullOrEmpty(account.Password) || account.Password.Length < 6)
                return RegisterResponse.PasswordInvalid;

            var user = context.mdl_user.FirstOrDefault(f => f.email == account.Email && f.record_state != 1);
            if (user != null)
                return RegisterResponse.EmailExists;

            var dbUser = context.mdl_user.Create();

            dbUser.name = account.Name;
            dbUser.avatar = "https://i.pinimg.com/originals/7c/c7/a6/7cc7a630624d20f7797cb4c8e93c09c1.png";
            dbUser.email = account.Email;
            dbUser.password = account.Password;
            dbUser.creation_date = DateTime.UtcNow;

            context.mdl_user.Add(dbUser);
            context.SaveChanges();

            return new RegisterResponse()
            {
                UserId = dbUser.id,
                Name = dbUser.name,
                Email = dbUser.email,
                ErrorCode = RegisterResponse.ErrorCodeType.None
            };
        }

        public OperationResult Update(User account)
        {
            var result = OperationResult.CreateWithSuccess();

            if (account == null)
                return OperationResult.CreateWithError("Invalid user data!");

            var dbUser = context.mdl_user.Find(account.Id);

            if (dbUser != null)
            {
                dbUser.name = account.Name;
                if (!string.IsNullOrEmpty(account.Avatar))
                    dbUser.avatar = account.Avatar;
                dbUser.record_state = 2;

                context.SaveChanges();
            }

            return result;
        }

        private static List<User> Map(List<mdl_user> items)
        {
            var result = new List<User>();
            if (items == null) return result;

            foreach (var s in items)
            {
                result.Add(new User
                {
                    Id = s.id,
                    Name = s.name,
                    Email = s.email,
                    Avatar = s.avatar,
                    Date = s.creation_date
                });
            }
            return result;
        }
    }

    public static class DbHelper
    {
        public static User ToUser(this mdl_user s)
        {
            return new User()
            {
                Id = s.id,
                Name = s.name,
                Email = s.email,
                Avatar = s.avatar,
                Date = s.creation_date
            };
        }
    }
}