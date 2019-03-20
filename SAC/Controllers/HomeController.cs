﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string sugerencia)
        {
            ViewBag.Sugerencia =  (sugerencia is null)?false:true ;
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

        [System.Web.Mvc.Authorize]
        public ActionResult Panel(int? Login = 0)
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.Login = Login;

            return View();
        }
    }
}