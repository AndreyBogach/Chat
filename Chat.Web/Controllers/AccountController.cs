using System.Web.Mvc;
using System.Web.Security;
using Chat.DAL.Models;
using Chat.DAL.Repositories.Interfaces;
using Chat.Web.Models;

namespace Chat.Web.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork uow): base(uow)
        {                
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var check = unitOfWork.UserRepository.CheckLogin(model.Email);
                var userModel = new SignInViewModel { Email = model.Email };
                if (check != null)
                {
                    userModel.IsRegistered = true;
                    return View("SignIn", userModel);
                }
                else
                {
                    userModel.IsRegistered = false;
                    return View("SignIn", userModel);
                }
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SignIn(SignInViewModel model)
        {
            if (model.IsRegistered == true)
            {
                var response = unitOfWork.UserRepository.Authorize(model.Email, model.Password);
                if (response.Success == true)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Index", "Chat", new { area = "" });
                }
                else
                {
                    ModelState.AddModelError("", "Password is not correct!");
                    return View(model);
                }
            }
            else
            {
                #region Validation
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.Password) || model.Password.Length < 6)
                {
                    ModelState.AddModelError("Password", "Password invalid!");
                    return View(model);
                }
                #endregion

                var resUser = unitOfWork.UserRepository.Register(new User
                {
                    Email = model.Email,
                    Name = model.Name,
                    Password = model.Password
                });
                if (!resUser.Success)
                {
                    ModelState.AddModelError("", "Email exists!");
                    return View(model);
                }
                FormsAuthentication.SetAuthCookie(resUser.Email, false);
                return RedirectToAction("Index", "Chat", new { area = "" });
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}