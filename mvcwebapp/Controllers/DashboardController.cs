

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvcwebapp.Data;

using mvcwebapp.Dtos;
using mvcwebapp.Models;
namespace mvcwebapp.Controllers
{
    public class DashboardController(AppDbContext context) : Controller
    {
        // GET: DashboardController
        public IActionResult Index()
        {
            var list= context.Products.Select(x=>new ProductDto
            {
                Id = x.Id,
                ProductName = x.ProductName,
                Description = x.Description,
                price = x.price,
                color = x.color
            }).ToList();

            ViewBag.Message = TempData["Message"]; // controller -> view (shown as a toast)

            return View(list);
        }

        // GET: /Dashboard/ProductForm  → shows the empty product form
        public IActionResult ProductForm()
        {
            return View(new ProductDto());
        }

        public async Task<IActionResult> CreateProduct(ProductDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.ProductName) || string.IsNullOrEmpty(dto.Description)
                || string.IsNullOrEmpty(dto.color) || dto.price <= 0)
            {
                ViewBag.ErrorMessage="Please fill in all fields";
                return View("ProductForm", dto);
            }

            var product = new Product
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                price = dto.price,
                color = dto.color
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product != null)
            {
                context.Products.Remove(product);
                TempData["Message"] = "Product deleted successfully."; // TempData survives the redirect
                await context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
