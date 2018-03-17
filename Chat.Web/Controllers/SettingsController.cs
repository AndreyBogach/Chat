using System.Web;
using System.Web.Mvc;
using Chat.DAL.Models;
using Chat.DAL.Repositories.Interfaces;
using Chat.Web.Models;

namespace Chat.Web.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {
        public SettingsController(IUnitOfWork uow): base(uow)
        {
        }

        public ActionResult Edit()
        {
            var user = GetUser;
            var model = new SettingsViewModel { Id = user.Id, Name = user.Name, Avatar = user.Avatar };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SettingsViewModel model, HttpPostedFileBase logo)
        {
            var logo_path = Storage(logo);
            var res = unitOfWork.UserRepository.Update(new User
            {
                Id = model.Id,
                Name = model.Name,
                Avatar = logo_path
            });

            return RedirectToAction("Index", "Chat", new { area = "" });
        }
    }
}