using Microsoft.AspNetCore.Mvc;
using PlastikMVC.Client;

namespace PlastikMVC.Controllers
{
    public class ProductController : Controller
    {

        private readonly ProductClient _productClient;

        public ProductController(ProductClient productClient)
        {
            _productClient = productClient;

        }

        public async Task<IActionResult> Index()
        {
            var products = await _productClient.GetAllProductsAsync();

            return View(products);
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await _productClient.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);

        }
    }
}
