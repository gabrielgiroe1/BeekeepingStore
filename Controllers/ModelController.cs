using AutoMapper;
using BeekeepingStore.AppDbContext;
using BeekeepingStore.Controllers.Resources;
using BeekeepingStore.Models;
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
    [Authorize(Roles = "Admin,Executive")]
    public class ModelController : Controller
    {
        private readonly BeekeepingDbContext _db;
        private readonly IMapper _mapper; 
        [BindProperty]
        public ModelViewModel modelVM { get; set; }
        public ModelController(BeekeepingDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
            modelVM = new ModelViewModel()
            {
                Makes = _db.Makes.ToList(),
                Model = new Models.Model()
            };  
        }
        public IActionResult Index()
        {
            var model = _db.Models.Include(m => m.Make);
            return View(model);
        }
        public IActionResult Create()
        {
            return View(modelVM);            
        }
        [HttpPost,ActionName("Create")]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                return View(modelVM);
            }
            _db.Models.Add(modelVM.Model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            modelVM.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
            if (modelVM.Model == null)
            {
                return NotFound();
            }
            return View(modelVM);
        }


        [HttpPost, ActionName("Edit")]
        public IActionResult EditPost(Model model)
        {
            if (!ModelState.IsValid)
            {
                return View(modelVM);                
            }
            _db.Update(modelVM.Model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));           
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Model model = _db.Models.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            _db.Models.Remove(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        [HttpGet("api/models/{MakeID}")]   
        public IEnumerable<Model> Models(int MakeID)  
        {
            return _db.Models.ToList().Where(m => m.MakeId == MakeID);
        }

        [AllowAnonymous]
        [HttpGet("api/models")]    
        public IEnumerable<ModelResources> Models()
        {
          //  return _db.Models.ToList();
          var models = _db.Models.ToList();
       
           return _mapper.Map<List<Model>, List<ModelResources>>(models);

        //    var modelResources = models.Select(m => new ModelResources {
        //        Id = m.Id,
        //         Name = m.Name
        //}).ToList();
        
            
        }
    }
}
