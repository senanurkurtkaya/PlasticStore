using Microsoft.AspNetCore.Mvc;
using PlastikMVC.Client;

namespace PlastikMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ProductClient _productClient;

        public CategoryController(ProductClient productClient)
        {
            _productClient = productClient;
        }

        [HttpGet("/Category/{id}/Products")]
        public async Task<IActionResult> ProductsByCategory(int id)
        {
            var products = await _productClient.GetProductsByCategoryIdAsync(id);
            ViewBag.categoryId = id;
            return View(products);
        }
    }
}
