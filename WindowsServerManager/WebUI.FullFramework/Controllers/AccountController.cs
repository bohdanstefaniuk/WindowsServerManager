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

        [Authorize]
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
                    return View("SuccessRegister");
                }
                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeUserRole(string redirectUrl, string email, Role newRole)
        {
            if (ModelState.IsValid)
            {
                var result = await UserService.ChangeUserRole(newRole, email);

                if (result.Succedeed)
                {
                    return Redirect(redirectUrl);
                }
                ModelState.AddModelError(result.Property, result.Message);
            }
            
            return View();
        }
    }
}