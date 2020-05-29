using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieApp.Data.Abstract;
using MovieApp.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace MovieApp.Controllers
{
    public class MovieController : Controller
    {

        

        private IMovieRepository _movieRepository;
        private ICategoryRepository _categoryRepository;

        public MovieController(IMovieRepository movieRepo, ICategoryRepository categoryRepo)
        {

            _movieRepository = movieRepo;

            _categoryRepository = categoryRepo;
        }






        public IActionResult Index(int? id, string q)
        {

            var query = _movieRepository.GetAll();
                



            if (id != null)
            {
                query = query
                    .Where(i => i.CategoryId == id);



            }


            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(i => EF.Functions.Like(i.Title, "%" + q + "%") || EF.Functions.Like(i.Description, "%" + q + "%") || EF.Functions.Like(i.Body, "%" + q + "%"));
            }
            else
            {

            }




            return View(query.OrderByDescending(i => i.MovieDate));


        }




        public IActionResult Details(int id)
        {
            return View(_movieRepository.GetById(id));
        }

        public IActionResult List()
        {
            return View(_movieRepository.GetAll().OrderByDescending(i=> i.MovieDate));
        }


        [HttpGet]
        public IActionResult Create(int? id)
        {

            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(new Movie());


        }



        [HttpPost]
        public  IActionResult Create(Movie entity)
        {
            if (ModelState.IsValid)
            {

                
                _movieRepository.SaveMovie(entity);
                TempData["message"] = $"{entity.Title} created.";
                return RedirectToAction("List");
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(entity);


        }




        [HttpGet]
        public IActionResult Edit(int id)
        {



            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(_movieRepository.GetById(id));


        }



        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Edit(Movie entity, IFormFile file, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {

                

                if (file != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create)) 

                    {
                        await file.CopyToAsync(stream);

                        entity.Image = file.FileName;
                    }
                }

                if (file != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\videos", file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))

                    {
                        await file.CopyToAsync(stream);

                        entity.Video = file.FileName;
                    }
                }


                _movieRepository.SaveMovie(entity);
                TempData["message"] = $"{entity.Title} edited.";
                return RedirectToAction("List");
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(entity);


        }





        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_movieRepository.GetById(id));
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int MovieId)
        {
            _movieRepository.DeleteMovie(MovieId);
            TempData["message"] = $"{MovieId} deleted.";
            return RedirectToAction("List");
        }

    }

}
