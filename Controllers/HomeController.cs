using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieApp.Data.Abstract;
using MovieApp.Models;
using System.Globalization;

namespace MovieApp.Controllers
{
    public class HomeController : Controller
    {
        private IMovieRepository movieRepository;
        public HomeController(IMovieRepository repository)
        {
            movieRepository = repository;
        }



        public IActionResult Index()
        {
            return View(movieRepository.GetAll().Where(i=>i.isHome).OrderByDescending(i=> i.MovieDate));
        }

        public IActionResult List()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }



    }
}
