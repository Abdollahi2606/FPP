using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyEshop.Data.Repositories;
using MyEshop.Models;

namespace MyEshop.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            // این متد برای بررسی اینکه ایمیل ورودی قبلا در بانک ثبت نام شده یا نه استفاده می شود
            // برای حالتی که ما به پراپرتی ایمیل از ویومدل اکانت صفت ریموت را نداده باشیم و اکشن
            // وریفای را برای آن صفت و چک کردن اینن موضوع با کتابخامه های ولیدیشن جی سان در خط 58 
            // تعریف نکرده باشیم
            // که نفاوت این دو روش این است که در جی سان اعتبارسنجی قبل ار ارسال به سرور انجام میگیرد
            // و در همان لحظه بررسی میکنئ اما در این روش بعد از فشردن کلید ثبت و ارسال به سرور 
            // این موضوع را چک می کند

            //if (!ModelState.IsValid)
            //{
            //    return View(register);
            //}

            //if (_userRepository.IsExistUserByEmail(register.Email.ToLower()))
            //{
            //    ModelState.AddModelError("Email", "ایمیل وارد شده قبلا ثبت نام نموده است");
            //    return View(register);
            //}

            Users user = new Users()
            {
                Email = register.Email.ToLower(),
                Password = register.Password,
                IsAdmin = false,
                RegisterDate = DateTime.Now
            };

            _userRepository.AddUser(user);

            return View("SuccessRegister", register);
        }
        #endregion

        public IActionResult VerifyEmail(string email)
        {
            if (_userRepository.IsExistUserByEmail(email.ToLower()))
            {
                return Json($"ایمیل {email} تکراری است");
            }
            return Json(true);
        }


        #region Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userRepository.GetUserForLogin(login.Email.ToLower(), login.Password);
            if (user == null)
            {
                ModelState.AddModelError("Email", "اطلاعات صحیح نیست");
                return View(login);
            }

            // برای به دست آوردن اطلاعات کاربر در سستم احراز هویت
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier , user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("IsAdmin",user.IsAdmin.ToString())// مرحله یک :برای اینکه هر کاربری دسترسی به ادمین نداشته باشد که باید میدل ویر آن را در استارت آپ بسازیم تا احراز هویت ادمین و یوزر را انجام دهد
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = login.RememberMe
            };

            HttpContext.SignInAsync(principal, properties);
            

            return Redirect("/");
        }
        #endregion


        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); 
            return Redirect("/Account/Login");
        }
    }
}
