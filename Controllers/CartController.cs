using ChocoShop.Data.Entities;
using ChocoShop.DbAccess;
using ChocoShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ChocoShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<CartItem> _cartItemRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Order> _orderRepository;
        public int UserId { get; set; }


        public CartController(ILogger<CartController> logger, IRepository<Product> productRepository, IRepository<CartItem> cartItemRepository,
            IRepository<User> userRepository,
            IRepository<Order> orderRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _cartItemRepository = cartItemRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public IActionResult AddToCart(int ProductId, int Quantity)
        {
            UserId = Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var user = _userRepository.GetByID(UserId);

            var p = _productRepository.GetByID(Convert.ToInt32(ProductId));
            var cartItem = new CartItem
            {
                CreatedDate = DateTime.Now,
                Item = p,
                Quantity = Quantity,
                User = user
            };
            _cartItemRepository.Insert(cartItem);
            return Json(1);
        }

        [Authorize(Roles = Constants.Role.USER)]
        public IActionResult Index()
        {
            UserId = Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var user = _userRepository.GetByID(UserId);

            var cart = _cartItemRepository.Get(u => u.User.Id == UserId).ToList();
            var uniqueCart = from c in cart
                             group c by c.Item.Id into g
                             select new { ProductId = g.Key, Quantity = g.Select(s => s.Quantity).Sum() };

            var productIds = uniqueCart.Select(s => s.ProductId).ToArray();
            var Products = _productRepository.Get(p => productIds.Contains(p.Id));
            var pq = from p in Products
                     join q in uniqueCart on p.Id equals q.ProductId
                     select new ProductQtyMap
                     {
                         Product = p,
                         Quantity = q.Quantity
                     };

            var model = new ShoppingCartModel
            {
                Products = pq.ToList()
            };
            model.Subtotal = pq.Sum(x =>  x.TotalPrice);
            model.ShippingAddress = user.Addresses[0];
            model.BillingAddress = user.Addresses[1];
            return View(model);
        }

        [Authorize(Roles = Constants.Role.USER)]
        public IActionResult Checkout()
        {
            UserId = Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var user = _userRepository.GetByID(UserId);

            var cart = _cartItemRepository.Get(u => u.User.Id == UserId).Select(s => new { Id = s.Id, ProductId = s.Item.Id, Quantity = s.Quantity });
            var uniqueCart = from c in cart
                             group c by c.ProductId into g
                             select new { ProductId = g.Key, Quantity = g.Select(s => s.Quantity).Sum() };
            

            var Order = new Order
            {
                BillingAddress = user.Addresses.First(u => u.AddressType == Data.AddressType.BillingAddress),
                ShippingAddress = user.Addresses.First(u => u.AddressType == Data.AddressType.ShippingAddress),
                CreatedBy = user,
                Discount = 0,
                ReferenceNumber = DateTime.Now.Ticks.ToString(),
                Status = Data.OrderStatus.Created,
                UpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                Items = new List<OrderItem>()
            };
            foreach (var c in uniqueCart)
            {
                var p = _productRepository.GetByID(c.ProductId);
                Order.Items.Add(new OrderItem { Product = p, Quantity = c.Quantity, ProductPrice = p.Price });
            }

            var pq = Order.Items.Select(s => new ProductQtyMap { Product = s.Product, Quantity = s.Quantity }).ToList();
            var model = new ShoppingCartModel
            {
                Products = pq,
                IsCheckout = true
            };

            model.Subtotal = pq.Sum(x => x.TotalPrice);
            model.ShippingAddress = user.Addresses[0];
            model.BillingAddress = user.Addresses[1];
            Order.GrandTotal = model.GrandTotal;
            Order.SubTotal = model.Subtotal;
            Order.Tax = model.Tax;
            Order.Shipping = model.Shipping;
            model.OrderNumber = Order.ReferenceNumber;


            _orderRepository.Insert(Order);
            try
            {
                foreach (var p in cart)
                {
                    _cartItemRepository.Delete(p.Id);
                }
            }
            catch
            {

            }

            return View("Index", model);
        }

    }
}
