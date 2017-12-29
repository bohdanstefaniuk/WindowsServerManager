using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BLL.Dto;
using BLL.Infrastructure;
using BLL.Interfaces;
using DataAccessLayer.Enums;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebUI.FullFramework.Models;

namespace WebUI.FullFramework.Controllers
{
    public class AccountController : Controller
    {
        private IUserService UserService => HttpContext.GetOwinContext().GetUserManager<IUserService>();

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var users = await UserService.GetUsers();

            return View(users);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var isEnabled = await UserService.IsUserEnabled(model.Email);

                if (isEnabled.HasValue && !isEnabled.Value || !isEnabled.HasValue)
                {
                    ModelState.AddModelError("", @"Ваша учетная запись была деактивирована");
                    return View(model);
                }

                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    Role = model.Role.ToString()
                };
                var operationDetails = await UserService.Create(userDto);
                if (operationDetails.Succedeed)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> SetUserEnableStatus(string id, bool status)
        {
            try
            {
                await UserService.ChangeIsEnabled(status, id);
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = $"{e.Message}" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = $"Success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeUserRole(ChangeRoleModel changeRoleModel)
        {
            if (ModelState.IsValid)
            {
                var result = await UserService.ChangeUserRole(changeRoleModel.Role, changeRoleModel.Email);

                if (result.Succedeed)
                {
                    return Redirect(changeRoleModel.RedirectUrl);
                }
                ModelState.AddModelError(result.Property, result.Message);
            }
            
            return View();
        }
    }
}