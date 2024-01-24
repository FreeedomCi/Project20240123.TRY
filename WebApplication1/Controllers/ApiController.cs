using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Xml.Linq;
using WebApplication1.Models;
using WebApplication1.Models.dto;
using WebApplication1.Models.Infra;

namespace WebApplication1.Controllers
{
	public class ApiController : Controller
	{
		private readonly MyDBContext _dbContext;
        private readonly IWebHostEnvironment _host;
        public ApiController(MyDBContext dbContext, IWebHostEnvironment host)
		{
			_dbContext = dbContext;
			_host = host;
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
		public IActionResult Street(string destricts)
		{
			var streets = _dbContext.Addresses.Where(c => c.SiteId == destricts).Select(a => a.Road).Distinct();
			return Json(streets);
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
		//public IActionResult Register(string name, int age = 18)
		//{
		//	if (string.IsNullOrEmpty(name))
		//	{
		//		name = "Guest";
		//	}
		//	return Content($"{ name}, You're {age} years old.");
		//}

		public IActionResult CheckAccount(string name)
		{
			Member? member = _dbContext.Members.FirstOrDefault(p => p.Name == name);

            if (member != null)
            {
				return Content("帳號已存在");
			}
			return Content("帳號可使用");
        }
		public IActionResult RegisterHW03(string name)
		{
            Member? vm = _dbContext.Members.FirstOrDefault(p => p.Name == name);

            if (vm != null)
            {
                return Content("帳號已存在");
            }
            return Content("帳號可使用");


		}
		[HttpPost]
		public IActionResult RegisterHW03(Member member, IFormFile Avatar)
		{
            string fileName = "empty.jpg";
            if (Avatar != null)
            {
                fileName = Avatar.FileName;
            }
            string path = Path.Combine(_host.WebRootPath, "upload", fileName);
            using (var fs = new FileStream(path, FileMode.Create))
            {
                Avatar?.CopyTo(fs);
            }

            byte[]? imgbyte = null;

            using (var memorystream = new MemoryStream())
            {
                Avatar?.CopyTo(memorystream);
                imgbyte = memorystream.ToArray();
            }
            member.FileName = fileName;
            member.FileData = imgbyte;


            byte[] salt = GenerateRandomSalt();

			byte[] hashedPassword = HashUtility.ToSHA256(member.Password, salt);

			member.Salt = Convert.ToBase64String(salt);
			member.Password = Convert.ToBase64String(hashedPassword);

			_dbContext.Members.Add(member);
			_dbContext.SaveChanges();

			return Content("新增成功");

		}

		private byte[] GenerateRandomSalt()
		{
			byte[] salt = new byte[16]; // 16位元組的加鹽

			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}

			return salt;
		}


		//public IActionResult Register(Userdto _user)
		public IActionResult Register(Member member, IFormFile Avatar)
        {
			if (string.IsNullOrEmpty(member.Name))
			{
                member.Name = "Guest";
			}

			//string path = @"C:\Users\fongc\source\repos\Projects\Project20240123\Code\Project20240123.TRY\WebApplication1\wwwroot\upload";
   //         using (var fs = new FileStream(Path.Combine(path, _user.Avatar.FileName), FileMode.Create))
   //         {
   //             _user.Avatar.CopyTo(fs);
   //         }
		   string fileName = "empty.jpg";
            if (Avatar != null)
			{
				fileName = Avatar.FileName;
			}
            string path = Path.Combine(_host.WebRootPath, "upload", fileName);
            using (var fs = new FileStream(path, FileMode.Create))
            {
                Avatar?.CopyTo(fs);
            }

			byte[]? imgbyte = null;

            using (var memorystream = new MemoryStream())
			{
                Avatar?.CopyTo(memorystream);
                imgbyte = memorystream.ToArray();
            }
            member.FileName = fileName;
            member.FileData = imgbyte;

			_dbContext.Members.Add(member);
			_dbContext.SaveChanges();


			//return Content($"Hello {_user.Name}, You're {_user.Age} years old. Your email is {_user.Email}");
			//return Content($"{_user.Avatar?.FileName}-{_user.Avatar?.Length}-{_user.Avatar?.ContentType}");
			return Content("建置成功");
		}

		[HttpPost]
		public IActionResult Spots([FromBody]Searchdto _search)
		{
			var spots = _search.catagoryId == 0? _dbContext.SpotImagesSpots :_dbContext.SpotImagesSpots.Where(s => s.CategoryId == _search.catagoryId);

			if (!string.IsNullOrEmpty(_search.keyword))
			{
				spots = spots.Where(s => s.SpotTitle.Contains(_search.keyword) || s.SpotDescription.Contains(_search.keyword));
			}

			switch (_search.sortBy)
			{
				case "spotTitle":
					spots = _search.sortType == "asc" ? spots.OrderBy(s => s.SpotTitle) : spots.OrderByDescending(s => s.SpotTitle);
					break;
				case "CategoryId":
					spots = _search.sortType == "asc" ? spots.OrderBy(s => s.CategoryId) : spots.OrderByDescending(s => s.CategoryId);
					break;
				default:
					spots = _search.sortType == "asc" ? spots.OrderBy(s => s.SpotId) : spots.OrderByDescending(s => s.SpotId);
					break;
			}

			int TotalCount = spots.Count();
			int pageSize = _search.pageSize ?? 9;
			int TotalPages = (int)Math.Ceiling(TotalCount / (decimal)pageSize);
			int page = _search.page ?? 1;

			spots = spots.Skip((page - 1) * pageSize).Take(pageSize);

			SpotsPagingdto spotsPaging = new SpotsPagingdto();
			spotsPaging.TotalPages = TotalCount;
			spotsPaging.SpotsResult = spots.ToList();

			return Json(spotsPaging);
		}
	}
}
