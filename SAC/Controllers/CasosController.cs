using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAC.Controllers
{
    public class CasosController : Controller
    {
        // GET: Casos
        public ActionResult Index()
        {
          
            using (var db = new Models.dbModel())
            { 


            }
            return View();
        }

        public ActionResult Caso(int Caso)
        {
            using (var db = new Models.dbModel())
            {



            }
            return View();
            //return Json(Graficos, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TraerComentarios(int Caso)
        {
            using (var db = new Models.dbModel())
            {



            }
            return View();
            //return Json(Graficos, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TraerComite(int Caso)
        {
            using (var db = new Models.dbModel())
            {



            }
            return View();
            //return Json(Graficos, JsonRequestBehavior.AllowGet);
        }
    }
}