using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	public class ApiController : Controller
	{
		private readonly MyDBContext _dbContext;
		public ApiController(MyDBContext dbContext)
		{
			_dbContext = dbContext;
		}
		public IActionResult Index()
		{
			System.Threading.Thread.Sleep(3000);
            //return View();
            //return Content("Hello");
            //return Content("<h1>Hello</h1>", "text/html");
            return Content("中文會亂碼", "text/plain", System.Text.Encoding.UTF8);
		}
		public IActionResult Cities()
		{
			var cities = _dbContext.Addresses.Select(a=> a.City).Distinct();
			return Json(cities);
		}
		public IActionResult Destricts(string city)
		{
			var districts = _dbContext.Addresses.Where(c => c.City==city).Select(a=>a.SiteId).Distinct();
			return Json(districts);
        }
		public IActionResult Avatar( int id = 1)
		{
			Member? member = _dbContext.Members.Find(id);
			if (member != null)
			{
				byte[] img = member.FileData;
				return File(img , "image/jpeg");
			}

			return NotFound();
		}
		public IActionResult Register(string name, int age = 18)
		{
			if (string.IsNullOrEmpty(name))
			{
				name = "Guest";
			}
			return Content($"{ name}, You're {age} years old.");
		}

		public IActionResult CheckAccount(string name)
		{
			Member? member = _dbContext.Members.FirstOrDefault(p => p.Name == name);

            if (member != null)
            {
				return Content("帳號已存在");
			}
			return Content("帳號可使用");
        }
    }
}
