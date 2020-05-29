using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Data.Abstract;
using MovieApp.Entity;

namespace MovieApp.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryRepository repository;
        public CategoryController(ICategoryRepository _repo)
        {
            repository = _repo;
        }

         

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult List()
        {
            return View(repository.GetAll());
        }


        [HttpGet]
        public IActionResult Create(int? id)
        {
            if (id==null)
            {
                return View(new Category());
            }
            else
            {
                return View(repository.GetById((int)id));
            }

        }
        
        
        
         [HttpPost]
         public IActionResult Create(Category entity)
        {
            if (ModelState.IsValid)
            {

                repository.SaveCategory(entity);
                TempData["message"] = $"{entity.Name} saved.";
                return RedirectToAction("List");
            }


            return View(entity);
        }




        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(repository.GetById(id));
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int CategoryId)
        {
            repository.DeleteCategory(CategoryId);
            TempData["message"] = $"{CategoryId} deleted.";
            return RedirectToAction("List");
        }



    }
}
