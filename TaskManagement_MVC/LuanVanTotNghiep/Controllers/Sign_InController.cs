using System;
using LuanVanTotNghiep.Models;
using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Controllers
{
    public class Sign_InController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        private readonly Login_Repo LoginService = new Login_Repo();

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await LoginService.LoginAsync(request);

            if (result != null && result.User != null)
            {
                HttpContext.Session.SetString("UserId", result.User.UserId.ToString());
                HttpContext.Session.SetString("Name", result.User.Name.ToString());
                HttpContext.Session.SetString("Role", result.User.Role ?? "User");
                HttpContext.Session.SetString("PictureUrl", result.User.PictureUrl ?? "");
                if (result.User.Role == "User")
                    return RedirectToAction("Index", "Home");
                else if(result.User.Role == "Admin")
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }

            ViewBag.Error = "Email or password is incorrect.";
            return View();
        }


        [HttpGet]
        public IActionResult ForgotPass()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPass(string email)
        {
            bool result = await LoginService.ForgotPassword(email);
            if (result)
                return RedirectToAction("Index", "Sign_In");
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult SignUp() => View();
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpItem model, string action)
        {
            if (action == "sendcode")
            {
                await LoginService.SendCode(model.Email);
                ViewBag.Message = "Verification code sent!";
                return View(model);
            }

            if (action == "signup")
            {
                if (!ModelState.IsValid)
                    return View(model);
                if (string.IsNullOrEmpty(model.Email) || !model.Email.EndsWith("@gmail.com"))
                {
                    ViewBag.Error = "Email must be a valid ddress.";
                    return View(model);
                }
                if (model.Password != model.ConfirmPassword)
                {
                    ViewBag.Error = "Password and Confirm Password do not match.";
                    return View(model);
                }


                if (string.IsNullOrWhiteSpace(model.Code))
                {
                    ViewBag.Error = "Please enter the verification code.";
                    return View(model);
                }
                var request = new RegisterRequest
                {
                    Email = model.Email,
                    Password = model.Password,
                    Code = model.Code,
                    Name = model.Email.Split('@')[0]
                };
                bool result = await LoginService.Register(request);
                if (result)
                    return RedirectToAction("Index", "Sign_In");
                ViewBag.Error = "Registration failed. Check your verification code.";
                return View(model);
            }

            return RedirectToAction("Login");
        }


    }
}
