using BeekeepingStore.AppDbContext;
using BeekeepingStore.Models;
using BeekeepingStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
//using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using cloudscribe.Pagination.Models;


namespace BeekeepingStore.Controllers
{

    [Table("Honeys")]
    [Authorize(Roles = "Admin,Executive")]
    public class HoneyController : Controller
    {
        private readonly BeekeepingDbContext _db;

        //private readonly IHostingEnvironment _hostingEnviroment;
        private readonly IWebHostEnvironment _hostingEnviroment;
        [BindProperty]
        public HoneyViewModel HoneyVM { get; set; }


        public HoneyController(BeekeepingDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnviroment = hostingEnvironment;
            HoneyVM = new HoneyViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Honey = new Models.Honey()
            };
        }
        public IActionResult Index2()
        {
            var Honeys = _db.Honeys.Include(m => m.Make).Include(m => m.Model);
            return View(Honeys.ToList());
        }
        public IActionResult Index(int pageNoumber=1, int pageSize=2)
        {
            int ExcludeRecords = (pageSize * pageNoumber) - pageSize;
            var Honeys = _db.Honeys.Include(m => m.Make).Include(m => m.Model).
                Skip(ExcludeRecords).
                Take(pageSize);
            var result = new PagedResult<Honey>
            {
                Data = Honeys.AsNoTracking().ToList(),
                TotalItems = _db.Honeys.Count(),
                PageNumber = pageNoumber,
                PageSize = pageSize
            };
            return View(result);
        }
        //get method
        public IActionResult Create()
        {
            return View(HoneyVM);
        }
        //post method
        [HttpPost, ActionName("Create")]
       // [ValidateAntiForgeryToken]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                HoneyVM.Makes = _db.Makes.ToList();
                HoneyVM.Models = _db.Models.ToList();
                return View(HoneyVM);
            }
            _db.Honeys.Add(HoneyVM.Honey);
            _db.SaveChanges();

            //////////////
            //Save a Product Logic
            //////////////

            //Get a product id we have saved in database
            var HoneyId = HoneyVM.Honey.Id;

            //Get wwroothpath to save the file on server
            string wwwrootPath = _hostingEnviroment.WebRootPath;

            //Get the uploaded files
            var files = HttpContext.Request.Form.Files;

            //Get the references of DBSet for the product i just savesd in database
            var SaveProduct = _db.Honeys.Find(HoneyId);

            //Upload the files on server and save the image path of user have upload any file
            if (files.Count != 0)
            {
                var ImagePath = @"images\product\";
                var Extension = Path.GetExtension(files[0].FileName);
                var RelativeImagePath = ImagePath + HoneyId + Extension;
                var AbsolutImagePath = Path.Combine(wwwrootPath, RelativeImagePath);

                //Upload the file on server
                using (var fileStream = new FileStream(AbsolutImagePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                };

                //Set the image path on database
                SaveProduct.ImagePath = RelativeImagePath;
               // HoneyVM.Honey.ImagePath = SaveProduct.ImagePath;              
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            HoneyVM.Honey = _db.Honeys.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
            if (HoneyVM.Honey == null)
            {
                return NotFound();
            }
            return View(HoneyVM);
        }


        [HttpPost, ActionName("Edit")]
        public IActionResult EditPost(Model model)
        {
            if (!ModelState.IsValid)
            {
                HoneyVM.Makes = _db.Makes.ToList();
                HoneyVM.Models = _db.Models.ToList();
                return View(HoneyVM);
            }
            _db.Update(HoneyVM.Honey);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Honey model = _db.Honeys.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            _db.Honeys.Remove(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }

}
