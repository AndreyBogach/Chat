using System;
using System.Collections.Generic;
using System.Linq;
using Chat.DAL.Entities;
using Chat.DAL.Models;
using Chat.DAL.Models.Pagging;
using Chat.DAL.Repositories.Interfaces;

namespace Chat.DAL.Repositories
{
    public class ChatRepository : IChatRepository
    {
        ChatContext context;

        public ChatRepository(ChatContext ctx)
        {
            this.context = ctx;
        }

        public void Create(Models.Chat model)
        {
            if (model == null)
                return;

            var dbMessage = context.mdl_chat.Create();

            dbMessage.receiver_id = model.Reciever.Id;
            dbMessage.sender_id = model.Sender.Id;
            dbMessage.message = model.Message;
            dbMessage.creation_date = DateTime.UtcNow;

            context.mdl_chat.Add(dbMessage);
            context.SaveChanges();
        }

        public bool Delete(int message_id)
        {
            var dbMessage = context.mdl_chat.Find(message_id);

            if (dbMessage == null) return false;

            dbMessage.record_state = 1;
            context.SaveChanges();

            return true;
        }

        public PagingList<Models.Chat> GetAll(int sender_id, int receiver_id, int page, int pageSize)
        {
            var res = new PagingList<Models.Chat>();
            var valueItems = new List<Models.Chat>();

            var values = context.mdl_chat.Where(w => w.sender_id == sender_id && w.receiver_id == receiver_id ||
                                               (w.sender_id == receiver_id && w.receiver_id == sender_id) && w.record_state != 1)
                                         .OrderByDescending(o => o.creation_date).ToList();

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

        private List<Models.Chat> Map(List<mdl_chat> items)
        {
            var result = new List<Models.Chat>();
            if (items == null) return result;

            foreach (var s in items)
            {
                result.Add(new Models.Chat
                {
                    Id = s.id,
                    Message = s.message,
                    Sender = context.mdl_user.Where(w => w.id == s.sender_id)
                                             .Select(f => new User
                                             {
                                                 Id = f.id,
                                                 Avatar = f.avatar,
                                                 Name = f.name
                                             })
                                             .FirstOrDefault(),
                    Reciever = context.mdl_user.Where(w => w.id == s.receiver_id)
                                             .Select(f => new User
                                             {
                                                 Id = f.id,
                                                 Avatar = f.avatar,
                                                 Name = f.name
                                             })
                                             .FirstOrDefault(),
                    Date = s.creation_date
                });
            }
            return result;
        }
    }
}