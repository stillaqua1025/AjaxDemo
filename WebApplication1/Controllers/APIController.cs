using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Unicode;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class APIController : Controller
    {
        private readonly MydbContext _db;
        private readonly IWebHostEnvironment _environment;
        public APIController(MydbContext db,IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }  
        public IActionResult Index()
        {
            System.Threading.Thread.Sleep(5000);
            string content = "卍解,千本桜景厳!";
            return Content(content,"text/plain",Encoding.UTF8);
        }
        public IActionResult Cities()
        {
            var cities = _db.Addresses.Select(x => x.City).Distinct();
            return Json(cities);
        }
        public IActionResult Sites() 
        {
            var sites = _db.Addresses.Select(x=>new { x.City, x.SiteId }).Distinct();
            return Json(sites);
        }
        public IActionResult SitesByCity(string City)
        {
            var sites = _db.Addresses.Where(x=>x.City==City).Select(x => x.SiteId).Distinct();
            return Json(sites);
        }
        public IActionResult Roads()
        {
            var roads = _db.Addresses.Select(x => new { x.SiteId, x.Road }).Distinct();
            return Json(roads);
        }
        public IActionResult RoadsBySide(string Side)
        {
            var roads = _db.Addresses.Where(x => x.SiteId == Side).Select(x => x.Road).Distinct();
            return Json(roads);
        }
        public async Task<IActionResult> Avatar(int id = 1)
        {
            Member? member = await _db.Members.FindAsync(id);
            if (member != null)
            {
                byte[] img = member.FileData;
                if (img != null)
                {
                    return File(img, "image/jpeg");
                }
            }
            return NotFound();
        }
        [Route("/API/register/{Name?}/{Email?}/{Age?}")]
        //public IActionResult register(string Name,string Email,int Age = 20) 
        public IActionResult register(CUserDTo _user)
        {
            if (string.IsNullOrEmpty(_user.Name))
            {
                _user.Name = "???";
            }

            //驗證姓名是否存在
            var q = _db.Members.Where(x=>x.Name==_user.Name).Select(x=>x.Name).ToList();
            if (q.Count == 0)
            {
                return Content("帳號可使用", "text/palin", Encoding.UTF8);
            }
            else 
            {
                return Content("帳號已存在", "text/palin", Encoding.UTF8);
            }

            //上傳圖片
            //取得根目錄路徑
            //if (_user.Photo != null)
            //{
            //    string path = Path.Combine(_environment.WebRootPath, "uploads", _user.Photo.FileName);
            //    using (var filestream = new FileStream(path, FileMode.Create))
            //    {
            //        _user.Photo.CopyTo(filestream);
            //    }
            //}
            //return Content($"{_user.Name}-{_user.Email}-{_user.Age}","text/palin",Encoding.UTF8);
            //return Content($"{_user.Photo.FileName}-{_user.Photo.Length}-{_user.Photo.ContentType}", "text/palin", Encoding.UTF8);
        }
    }
}
