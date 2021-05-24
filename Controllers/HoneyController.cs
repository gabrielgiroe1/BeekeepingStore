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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BeekeepingStore.Controllers
{

    [Table("Honeys")]
    [Authorize(Roles =Helpers.Roles.Admin +"," +Helpers.Roles.Executive)]
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
        [AllowAnonymous]
        public IActionResult Index(string searchStringModel, string searchString,
            string sortOrder, int pageNoumber = 1, int pageSize = 2)
        {
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentFilter = searchString;
            ViewBag.PriceSortParam = string.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            int ExcludeRecords = (pageSize * pageNoumber) - pageSize;

            var Honeys = from b in _db.Honeys.Include(m => m.Make).Include(m => m.Model)
                         select b;
            var HoneyCount = Honeys.Count();
            if (!String.IsNullOrEmpty(searchString))
            {
                Honeys = Honeys.Where(b => b.Make.Name.Contains(searchString)).
                    Where(b => b.Model.Name.Contains(searchStringModel));
                HoneyCount = Honeys.Count();
            }

            // sorting Logic
            {
                switch (sortOrder)
                {
                    case "price_desc":
                        Honeys = Honeys.OrderByDescending(b => b.Price);
                        break;
                    default:
                        Honeys = Honeys.OrderBy(b => b.Price);
                        break;
                }
            }

            Honeys = Honeys.
             Skip(ExcludeRecords).
             Take(pageSize);

            var result = new PagedResult<Honey>
            {
                Data = Honeys.AsNoTracking().ToList(),
                TotalItems = HoneyCount,
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
            UploadImageIfAvailable();
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void UploadImageIfAvailable()
        {
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

                //get the extension of submitted file 
                var Extension = Path.GetExtension(files[0].FileName);

                //create a relative image path to be saved in database table
                var RelativeImagePath = ImagePath + HoneyId + Extension;

                //create the absolute image path to upload the physical file on server
                var AbsolutImagePath = Path.Combine(wwwrootPath, RelativeImagePath);

                //Upload the file on server
                using (var fileStream = new FileStream(AbsolutImagePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                };

                //Set the image path on database
                SaveProduct.ImagePath = RelativeImagePath;
            }

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            HoneyVM.Honey = _db.Honeys.SingleOrDefault(m => m.Id == id);

            //filter the models associated to the selested make
            HoneyVM.Models = _db.Models.Where(m => m.MakeId == HoneyVM.Honey.MakeID);
            if (HoneyVM.Honey == null)
            {
                return NotFound();
            }
            return View(HoneyVM);
        }


        [HttpPost, ActionName("Edit")]

        // [ValidateAntiForgeryToken]
        public IActionResult EditPost()
        {
            if (!ModelState.IsValid)
            {
                HoneyVM.Makes = _db.Makes.ToList();
                HoneyVM.Models = _db.Models.ToList();
                return View(HoneyVM);
            }
            _db.Honeys.Update(HoneyVM.Honey);
            UploadImageIfAvailable();
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
        [AllowAnonymous]
        [HttpGet]
        public IActionResult View(int id)
        {
            HoneyVM.Honey = _db.Honeys.SingleOrDefault(m => m.Id == id);

            if (HoneyVM.Honey == null)
            {
                return NotFound();
            }
            return View(HoneyVM);
        }
    }

}
