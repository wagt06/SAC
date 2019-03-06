using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SAC.Controllers
{
    public class LoginController : Controller
    {
        // GET: login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Usuario, string Contraseña)
        {
            var _usuario = new Models.Usuarios();

            using (var db = new Models.dbModel()) {
                _usuario = db.Usuarios.Where(U => U.Usuario == Usuario).FirstOrDefault();
            }
            if (_usuario != null) { 
                if (_usuario.Usuario == Usuario && _usuario.Contraseña == Contraseña)
                {
                    //Response.Cookies["Usuario"].Value = Usuario;
                    //Response.Cookies["Usuario"].Expires = DateTime.Now.AddDays(100);
                    FormsAuthentication.SetAuthCookie(Usuario, false);
                    return Json(1);
                }
            }
            return Json(0);
        }
        // GET: login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: login/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: login/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: login/Delete/5
        public ActionResult Delete(int id)
        {
           // var r = HttpContext.Request.Cookies["Usuario"];
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //FormsAuthentication.SetAuthCookie(Usuario, false);
                FormsAuthentication.SignOut();
               // HttpContext.Request.Cookies.Remove(r.Name);
            }
            return RedirectToAction("index", "home");
        }

        // POST: login/Delete/5
        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                var r = HttpContext.Request.Cookies["Usuario"];
                if (r.Name != "") {
                    HttpContext.Request.Cookies.Remove(r.Name);
                }
                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
        }
    }
}
