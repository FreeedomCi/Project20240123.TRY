using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeworkController : Controller
    {
        private readonly MyDBContext _dbContext;
        public HomeworkController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CheckAccount()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult CheckAccount(string name)
        //{

        //}
    }
}
