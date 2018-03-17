using System.Web;
using System.Web.Mvc;
using Chat.Common;
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

        public string UserName()
        {
            return User.Identity.Name;
        }

        public bool IsAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }

        public string Storage(HttpPostedFileBase upload)
        {
            var path = string.Empty;

            if (upload != null && upload.ContentLength > 0)
                path = AzureBlobStorage.StoreFile(upload);

            return path;
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}