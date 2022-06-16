using ChocoShop.Data.Entities;
using ChocoShop.DbAccess;
using ChocoShop.Extensions;
using ChocoShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;

namespace ChocoShop.Controllers
{
    [Authorize(Roles = Constants.Role.USER)]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IWebHostEnvironment _environment;


        public int UserId { get; set; }


        public UserController(ILogger<UserController> logger, IRepository<Product> productRepository, IRepository<CartItem> cartItemRepository,
            IRepository<User> userRepository,
            IRepository<Order> orderRepository,
            IRepository<Address> addressRepository,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var user = _userRepository.GetByID(Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value));

            return View(user);
        }

        [HttpGet]
        public IActionResult Orders()
        {
            var uId = Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);


            var products = _orderRepository.Get(o => o.CreatedBy.Id == uId &&
           (o.Status == Data.OrderStatus.Created
           || o.Status == Data.OrderStatus.Processing
           || o.Status == Data.OrderStatus.Shipped)
            ).OrderByDescending(t => t.Id).ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult OrderHistory()
        {
            var uId = Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);


            var products = _orderRepository.Get(o => o.CreatedBy.Id == uId &&
           (o.Status == Data.OrderStatus.Cancelled
           || o.Status == Data.OrderStatus.Delivered
           || o.Status == Data.OrderStatus.Rejected)
            ).OrderByDescending(t => t.Id).ToList();
            return View("Orders", products);
        }

        [HttpGet]
        public IActionResult OrderDetail(int Id = 0, int Status = 0)
        {
            ViewBag.Enviorment = null;
            var order = _orderRepository.GetByID(Id);
            if (order != null)
            {
                ViewBag.Enviorment = _environment;
                return View(order);
            }

            return RedirectToAction("Orders");
        }
        [HttpGet]
        public IActionResult Address()
        {
            var user = _userRepository.GetByID(Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value));
            if (!user.Addresses.Any(a => a.AddressType == Data.AddressType.BillingAddress))
            {
                user.Addresses.Add(new Address { AddressType = Data.AddressType.BillingAddress });
            }
            if (!user.Addresses.Any(a => a.AddressType == Data.AddressType.ShippingAddress))
            {
                user.Addresses.Add(new Address { AddressType = Data.AddressType.ShippingAddress });
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult Address(AddressModel model)
        {
            var user = _userRepository.GetByID(Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value));
            var addr = _addressRepository.GetByID(model.Id);


            if (addr == null)
            {
                _addressRepository.Insert(new Address
                {
                    AddressLIne1 = model.AddressLIne1,
                    AddressType = model.AddressType,
                    City = model.City,
                    State = model.State,
                    Street = model.Street,
                    User = user,
                    ZipCode = model.ZipCode
                });
            }
            else
            {
                addr.AddressLIne1 = model.AddressLIne1;
                addr.AddressType = model.AddressType;
                addr.City = model.City;
                addr.State = model.State;
                addr.Street = model.Street;
                addr.User = user;
                addr.ZipCode = model.ZipCode;
                _addressRepository.Update(addr);
            }

            return RedirectToAction("Address");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordModel());
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            int userId = Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var user = _userRepository.Get(u => u.Id == userId && u.Password == model.CurrentPassword.ToHash()).FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError("InvalidPassword", "Invalid Current Password!");
                return View(model);
            }
            user.Password = model.Password.ToHash();
            _userRepository.Update(user);

            return View(model);
        }
    }
}
