using System;
using System.Text.RegularExpressions;
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
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                ViewBag.Error = "Email and Password must not be empty.";
                return View();
            }
            var emailRegex = new Regex(@"^[a-zA-Z0-9@._-]+$");
            var passwordRegex = new Regex(@"^[a-zA-Z0-9!@#$%^&*()_+=\-{}[\]:;""'<>,.?/|\\]+$");

            if (!emailRegex.IsMatch(request.Email) || !passwordRegex.IsMatch(request.Password))
            {
                ViewBag.Error = "Invalid characters in Email or Password.";
                return View();
            }
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
            return View("Index");
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
                if (string.IsNullOrWhiteSpace(model.Email))
                {
                    ViewBag.Error = "Please enter your email.";
                    return View(model);
                }

                await LoginService.SendCode(model.Email);
                ViewBag.Message = "Verification code sent!";
                return View(model);
            }

            if (action == "signup")
            {
                if (!ModelState.IsValid)
                    return View(model);

                var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@gmail\.com$");
                var passwordRegex = new Regex(@"^[a-zA-Z0-9!@#$%^&*()_+=\-{}[\]:;""'<>,.?/|\\]+$");
                var codeRegex = new Regex(@"^[0-9]{6}$");

                if (string.IsNullOrWhiteSpace(model.Email) || !emailRegex.IsMatch(model.Email))
                {
                    ViewBag.Error = "Email must be a valid Gmail address.";
                    return View(model);
                }

                if (!passwordRegex.IsMatch(model.Password))
                {
                    ViewBag.Error = "Password contains invalid characters.";
                    return View(model);
                }

                if (model.Password.Length < 8 ||
                    !Regex.IsMatch(model.Password, @"[A-Z]") ||
                    !Regex.IsMatch(model.Password, @"[a-z]") ||
                    !Regex.IsMatch(model.Password, @"[0-9]") ||
                    !Regex.IsMatch(model.Password, @"[\W_]"))
                {
                    ViewBag.Error = "Password must be at least 8 characters and include upper case, lower case, number, and special character.";
                    return View(model);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    ViewBag.Error = "Password and Confirm Password do not match.";
                    return View(model);
                }

                if (string.IsNullOrWhiteSpace(model.Code) || !codeRegex.IsMatch(model.Code))
                {
                    ViewBag.Error = "Invalid or missing verification code.";
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
