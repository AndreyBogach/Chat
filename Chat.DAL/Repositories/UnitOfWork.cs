using System;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;

namespace Chat.DAL.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        ChatContext _context;
        
        private UserRepository userRepository;
        private ChatRepository chatRepository;

        private bool disposed = false;

        public UnitOfWork()
        {
            _context = new ChatContext();
        }

        public UserRepository UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository(_context);
                }
                return userRepository;
            }
        }

        public ChatRepository ChatRepository
        {
            get
            {
                if (this.chatRepository == null)
                {
                    this.chatRepository = new ChatRepository(_context);
                }
                return chatRepository;
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}