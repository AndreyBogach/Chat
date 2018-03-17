using System.Collections.Generic;
using Chat.DAL.Models;
using Chat.DAL.Models.Pagging;

namespace Chat.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        LoginResponse Authorize(string email, string password);
        RegisterResponse CheckLogin(string email);
        RegisterResponse Register(User account);
        OperationResult Update(User account);
        User GetUser(int id);
        User GetUser(string email);
        IEnumerable<User> GetAll(string email);
        PagingList<User> GetAll(int page, int pageSize);
        bool Delete(int user_id);
    }
}