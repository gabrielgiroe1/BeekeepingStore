using BeekeepingStore.AppDbContext;
using BeekeepingStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeepingStore.Controllers
{
    public class HoneyController1 : Controller
    {

        [Authorize(Roles = "Admin,Executive")]
        public class ModelController : Controller
        {
            private readonly BeekeepingDbContext _db;
            [BindProperty]
            public HoneyViewModel honeyVM { get; set; }
            public ModelController(BeekeepingDbContext db)
            {
                _db = db;
                honeyVM = new HoneyViewModel()
                {
                    Makes = _db.Makes.ToList(),
                    Models = _db.Models.ToList(),
                    Honey = new Models.Honey()
                };
            }
            public IActionResult Index()
            {
                var Honey = _db.Honeys.Include(m => m.Make).Include(m => m.Model);
                return View(Honey.ToList());
            }
            //get method
            public IActionResult Create()
            {
                return View(honeyVM);
            }
            //post method
            [HttpPost, ActionName("Create")]
            public IActionResult CreatePost()
            {
                if (!ModelState.IsValid)
                {
                    return View(honeyVM);
                }
                _db.Honeys.Add(honeyVM.Honey);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            //    public IActionResult Edit(int id)
            //    {
            //        honeyVM.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
            //        if (honeyVM.Model == null)
            //        {
            //            return NotFound();
            //        }
            //        return View(honeyVM);
            //    }


            //    [HttpPost, ActionName("Edit")]
            //    public IActionResult EditPost(Model model)
            //    {
            //        if (!ModelState.IsValid)
            //        {
            //            return View(honeyVM);
            //        }
            //        _db.Update(honeyVM.Model);
            //        _db.SaveChanges();
            //        return RedirectToAction(nameof(Index));
            //    }
            //    [HttpPost]
            //    public IActionResult Delete(int id)
            //    {
            //        Model model = _db.Models.Find(id);
            //        if (model == null)
            //        {
            //            return NotFound();
            //        }
            //        _db.Models.Remove(model);
            //        _db.SaveChanges();
            //        return RedirectToAction(nameof(Index));
            //    }
        }
    }
}
