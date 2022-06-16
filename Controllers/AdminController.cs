using AutoMapper;
using ChocoShop.Data.Entities;
using ChocoShop.DbAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChocoShop.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ChocoShop.Extensions;
using ChocoShop.Data;

namespace ChocoShop.Controllers
{
    //[Authorize(Roles = Constants.Role.ADMINISTRATOR)]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<Order> _orderRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public AdminController(ILogger<AdminController> logger, IRepository<User> userRepo, IRepository<Product> productRepo, IRepository<Order> orderRepo, IWebHostEnvironment environment, IMapper mapper)
        {
            _logger = logger;
            _userRepo = userRepo;
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _mapper = mapper;
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Products(int Id = 0)
        {
            ViewBag.DeletedMsg = string.Empty;
            if (Id > 0)
            {
                var product = _productRepo.GetByID(Id);
                if (product != null)
                {
                    product.IsDeleted = true;
                    _productRepo.Update(product);
                    var message = string.Format("<script>alert('Product: {0} has been deleted.')</script>", product.Name);
                    ViewBag.DeletedMsg = message;
                }
            }
            var products = _productRepo.Get(t => !t.IsDeleted).ToList();
            var model = ConvertProducrModel(products);
            return View(model);
        }

        [HttpGet]
        public IActionResult ManageProduct(int Id = 0)
        {
            var product = _productRepo.GetByID(Id);
            if (product != null)
            {
                var model = ConvertProductToProductModel(product);
                return View(model);
            }

            return View(new ProductModel());
        }

        [HttpPost]
        public IActionResult ManageProduct(ProductModel model)
        {
            var product = _productRepo.GetByID(model.Id);
            if (product != null)
            {
                product = ConvertProductModelToProduct(product, model);
                _productRepo.Update(product);
            }
            else
            {
                product = ConvertProductModelToProduct(product, model);
                _productRepo.Insert(product);

            }

            UploadedFile(model, product.Id);
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult Orders(int Id = 0, int Status = 0)
        {
            ViewBag.Message = string.Empty;
            if (Id > 0 && Status > 0 && Status <= 6)
            {
                var order = _orderRepo.GetByID(Id);
                if (order != null)
                {
                    order.Status = (OrderStatus)Status;
                    _orderRepo.Update(order);
                    ViewBag.Message = string.Format("<script>alert('Your order status for refrence no:{0}  has been updated.')</script>", order.ReferenceNumber);
                }
            }

            var products = _orderRepo.Get().OrderByDescending(t => t.Id).ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult OrderDetail(int Id = 0, int Status = 0)
        {
            ViewBag.Enviorment = null;
            var order = _orderRepo.GetByID(Id);
            if (order != null)
            {
                ViewBag.Enviorment = _environment;
                return View(order);
            }

            return RedirectToAction("Orders");
        }
        [HttpGet]
        public IActionResult Users()
        {
            var users = _userRepo.Get().ToList();
            return View(users);
        }

        private ProductModel ConvertProductToProductModel(Product product)
        {
            return new ProductModel
            {
                Id = product.Id,
                Description = product.Description,
                Discount = product.Discount,
                Name = product.Name,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                Tags = string.IsNullOrWhiteSpace(product.Tags) ? null : product.Tags.Split(',').ToList(),
                Title = product.Title

            };
        }

        private Product ConvertProductModelToProduct(Product product, ProductModel model)
        {
            if (product == null)
            {
                product = new Product();
            }

            product.Id = model.Id;
            product.Description = model.Description;
            product.Discount = model.Discount;
            product.Name = model.Name;
            product.Price = model.Price;
            product.QuantityInStock = model.QuantityInStock;
            product.Tags = (model.Tags != null && model.Tags.Any()) ? string.Join(",", model.Tags) : "";
            product.Title = model.Title;

            return product;
        }
        private void UploadedFile(ProductModel model, int Id)
        {
            if (model.Images != null && model.Images.Any())
            {

                var image = model.Images[0];
                var ext = System.IO.Path.GetExtension(image.FileName);
                var thumb = string.Format("{0}-{1}{2}", ImageConstants.thumbnail, Id, ext);
                string wwwRootPath = _environment.WebRootPath;
                string path = Path.Combine(wwwRootPath + "/product/", Id.ToString());
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                else
                {
                    var files = System.IO.Directory.GetFiles(path);
                    foreach (var file in files)
                    {
                        System.IO.File.Delete(file);
                    }
                }
                path = path + "/" + thumb;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

                if (model.Images.Count > 0)
                {
                    image = model.Images[1];
                    ext = System.IO.Path.GetExtension(image.FileName);
                    thumb = string.Format("{0}-{1}{2}", ImageConstants.detail, Id, ext);
                    wwwRootPath = _environment.WebRootPath;
                    path = Path.Combine(wwwRootPath + "/product/", Id.ToString());
                    path = path + "/" + thumb;
                    // path = Path.Combine(wwwRootPath + "/product/", thumb);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                }
            }

        }

        private List<ProductModel> ConvertProducrModel(List<Product> products)
        {
            var results = (from item in products
                           let Images = GetImages(item.Id).ToArray()
                           select new ProductModel
                           {
                               Description = item.Description,
                               Discount = item.Discount,
                               Id = item.Id,
                               Name = item.Name,
                               Price = item.Price,
                               QuantityInStock = item.QuantityInStock,
                               Tags = string.IsNullOrWhiteSpace(item.Tags) ? null : item.Tags.Split(',').ToList(),
                               Title = item.Title,
                               Thumbnail = (Images != null && Images.Any()) ? Images[0] : "",
                               DetailImage = (Images != null && Images.Any() && Images.Count() > 0) ? Images[1] : ""
                           }).ToList();
            return results;
        }
        private List<string> GetImages(int id)
        {
            string[] postedFiles = { "", "" };
            string wwwRootPath = _environment.WebRootPath;
            string path = Path.Combine(wwwRootPath + "/product/", id.ToString());
            if (System.IO.Directory.Exists(path))
            {
                var files = System.IO.Directory.GetFiles(path);

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    var displayPath = "/product/" + id + "/" + fileInfo.Name;
                    if (file.Contains(ImageConstants.thumbnail))
                    {
                        postedFiles[0] = displayPath;
                    }
                    else
                    {
                        postedFiles[1] = displayPath;
                    }
                }
            }
            return postedFiles.ToList();
        }

    }
}
