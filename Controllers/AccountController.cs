using AutoMapper;
using ChocoShop.Data.Entities;
using ChocoShop.DbAccess;
using ChocoShop.Extensions;
using ChocoShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChocoShop.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Role> _roleRepo;
        private readonly IMapper _mapper;

        public AccountController(ILogger<AccountController> logger, IRepository<User> userRepo, IRepository<Role> roleRepo, IMapper mapper)
        {
            _logger = logger;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepo.Get(u => u.Email == model.Username && u.Password == model.Password.ToHash())
                    .FirstOrDefault();

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }


                await _signIn(user);
                var returnUrl = string.Empty;
                if (Request.Cookies.TryGetValue("returnUrl", out returnUrl))
                {
                    Response.Cookies.Delete("returnUrl");
                    return LocalRedirect(returnUrl);
                }

                if (user.Role.Name == Constants.Role.ADMINISTRATOR)
                {
                    return LocalRedirect("~/Admin/Index");
                }
                else
                {
                    return LocalRedirect("~/user/Index");
                }
            }
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);

                try
                {
                    user.Role = _roleRepo.Get(r => r.Name == Constants.Role.USER).First();
                    _userRepo.Insert(user);
                    await _signIn(user);
                    var returnUrl = string.Empty;
                    if (Request.Cookies.TryGetValue("returnUrl", out returnUrl))
                    {
                        Response.Cookies.Delete("returnUrl");
                        return LocalRedirect(returnUrl);
                    }

                    return LocalRedirect("~/user/Index");
                    //return LocalRedirect("~/Booking/Index");
                }
                catch (Exception ex)
                {
                    user.Password = string.Empty;
                    _logger.LogError("Unable to create user", User, ex);
                    ModelState.AddModelError("UserCreation", "Unable to create user please contact administrator.");
                    return View(model);
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("User {Name} logged out at {Time}.",
                User.Identity.Name, DateTime.UtcNow);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("~/Home/Index");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    object userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    var user = _userRepo.GetByID(userId);

                    if (user.Password == model.CurrentPassword.ToHash())
                    {
                        user.Password = model.Password.ToHash();
                        _userRepo.Update(user);
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        return LocalRedirect("~/Home/Index");
                    }

                    ModelState.AddModelError("CurrentPassword", "Invalid current password.");
                    return View(model);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Unable to change password", model, ex);
                    ModelState.AddModelError("Password", "Unable to change password. please contact administrator.");
                    return View(model);
                }
            }

            return View();
        }

        private async Task _signIn(User user)
        {
            var roleName = user.Role.Name;

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, roleName),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            _logger.LogInformation("User {Email} logged in at {Time}.",
                user.Email, DateTime.UtcNow);
        }


    }
}
