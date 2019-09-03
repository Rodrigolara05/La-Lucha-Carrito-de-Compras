using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using TrabajoFinalWeb.Models;
using Hotel_UPC.Authorization;

namespace TrabajoFinalWeb.Controllers
{
    public class HomeController : Controller
    {
        private RestauranteEntitiesContext db = new RestauranteEntitiesContext();

        public ActionResult Index()
        {
            return View();
        }
        [UserLoggedOn]
        [AdminsOnly]
        public ActionResult Index_Administrador()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}