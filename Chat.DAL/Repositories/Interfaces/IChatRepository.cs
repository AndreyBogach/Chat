using Chat.DAL.Models.Pagging;

namespace Chat.DAL.Repositories.Interfaces
{
    public interface IChatRepository
    {
        PagingList<Models.Chat> GetAll(int sender_id, int receiver_id, int page, int pageSize);
        void Create(Models.Chat model);
        bool Delete(int message_id);
    }
}