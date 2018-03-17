using System.Collections.Generic;
using System.Web.Mvc;
using Chat.DAL.Models;
using Chat.DAL.Repositories.Interfaces;

namespace Chat.Web.Controllers
{
    public class BaseController : Controller
    {
        public IUnitOfWork unitOfWork { get; }

        public BaseController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public User GetUser
        {
            get { return unitOfWork.UserRepository.GetUser(User.Identity.Name); }
        }

        public IEnumerable<User> GetUsers
        {
            get { return unitOfWork.UserRepository.GetAll(); }
        }

        public string UserName()
        {
            return User.Identity.Name;
        }

        public bool IsAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }


        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}