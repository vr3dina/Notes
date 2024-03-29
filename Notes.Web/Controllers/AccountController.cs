﻿using Notes.DB.Repositories;
using Notes.DB.Repositories.Interfaces;
using Notes.Web.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace Notes.Web.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        IUserRepositoty UserRepository;

        public AccountController()
        {
            UserRepository = new NHUserRepository();
        }

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Что-то пошло не так!");
                return View(model);
            }

            var user = UserRepository.LoadByLogin(model.Login);

            if (user == null || user.Password != model.Password)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(model);
            }

            FormsAuthentication.SetAuthCookie(user.Login, false);

            return RedirectToAction("Index", "Note");
        }

        [AllowAnonymous]
        public ActionResult Singup()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Singup(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Что-то пошло не так!");
                return View(model);
            }

            var user = UserRepository.LoadByLogin(model.Login);

            if (user != null)
            {
                ModelState.AddModelError("", "Пользователь с таким именем уже есть");
                return View(model);
            }

            UserRepository.Save(new DB.User() { Login = model.Login, Password = model.Password });

            return RedirectToAction("Login", "Account");
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }

}