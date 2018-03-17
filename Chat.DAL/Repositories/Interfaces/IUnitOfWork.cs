using System;

namespace Chat.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        UserRepository UserRepository { get; }
        ChatRepository ChatRepository { get; }
    }
}