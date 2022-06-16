using ChocoShop.Data.Entities;
using ChocoShop.DbAccess;
using ChocoShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ChocoShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Product> _productRepository;

        public HomeController(ILogger<HomeController> logger, IRepository<Product> productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var featuredProduct = _productRepository.Get(p => p.Tags.Contains("Featured") && !p.IsDeleted).ToList().Take(4);
            var BestSellerProduct = _productRepository.Get(p => p.Tags.Contains("Best-Seller") && !p.IsDeleted).ToList().Take(4);
            return View(new HomeModel
            {
                Featured  = featuredProduct ,
                BestSeller = BestSellerProduct
            });
        }

        [HttpGet]
        public IActionResult Search(string searchTerm="", int pagenumber = 1)
        {
            int totalRec = 10;
            searchTerm = string.IsNullOrEmpty(searchTerm)? string.Empty: searchTerm.ToLower();
            var products = string.IsNullOrEmpty(searchTerm) ? _productRepository.Get(p => !p.IsDeleted).ToList()
                : _productRepository.Get(p => !p.IsDeleted && (p.Name.ToLower().Contains(searchTerm) || p.Title.ToLower().Contains(searchTerm) || p.Description.ToLower().Contains(searchTerm))).ToList();
            int totalRecords = products.Count();
            var fromRec = pagenumber ==1 ? 1: pagenumber * totalRec;
            var toRec = fromRec + totalRec;
            products = products.Skip(fromRec).Take(toRec).ToList();
            var model = new ProductSearchResponseModel
            {
                Products = products,
                TotalPage = totalRecords < 10 ? 1: totalRecords / totalRec
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Product(int id)
        {

            var p = _productRepository.GetByID(id);
            if (p == null)
            {
                return NotFound();
            }

            return View(p);
        }



        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Faq()
        {
            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ShippingInfo()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ReturnPolicy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TermsAndConditions()
        {
            return View();
        }
    }
}
