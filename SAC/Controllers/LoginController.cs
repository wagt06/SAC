using System;
using System.Collections.Generic;
using SAC.Tags;
using System.IO;
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
            List<Models.Usuarios> Lista = new List<Models.Usuarios>();
            using (var db = new Models.dbModel()){
                Lista = db.Usuarios.ToList();

            }
                return View(Lista);
        }

        [NoLoginAttribute]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Usuario, string Contraseña)
        {
            var _usuario = new Models.Usuarios();

            using (var db = new Models.dbModel())
            {
                _usuario = db.Usuarios.Where(U => U.Usuario == Usuario).FirstOrDefault();
            }
            if (_usuario != null)
            {
                if (_usuario.Usuario == Usuario && _usuario.Contraseña == Contraseña)
                {
                    HttpCookie _User_Info = new HttpCookie("UserInfo");
                    _User_Info["img"] = _usuario.Img;
                    _User_Info["Codigo"] = _usuario.CodigoUsuario.ToString();
                    _User_Info["Roll"] = 1.ToString();
                   
                   var  rm = _usuario.Autenticarse();

                    if (rm.response)
                    {
                        rm.href = Url.Content("~/home");
                    }

                    Response.Cookies.Add(_User_Info);
                    //Response.Cookies["Usuario"].Expires = DateTime.Now.AddDays(100);
                    //FormsAuthentication.SetAuthCookie(Usuario, false);
                    return Json(1);
                }
            }
            return Json(0);
        }
        // GET: login/Details/5
        public ActionResult Details(int Usuario)
        {
            var _usuario = new Models.Usuarios();
            using (var db = new Models.dbModel())
            {
                _usuario = db.Usuarios.Where(U => U.CodigoUsuario == Usuario).FirstOrDefault();
            }

            return View(_usuario);
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
                if (r.Name != "")
                {
                    HttpContext.Request.Cookies.Remove(r.Name);
                }
                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int Usuario)
        {
            var _usuario = new Models.Usuarios();
            if (Usuario != 0) {
                using (var db = new Models.dbModel())
                {
                    _usuario = db.Usuarios.Where(U => U.CodigoUsuario == Usuario).FirstOrDefault();
                }
            }
            return View(_usuario);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Models.Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                using (var db = new Models.dbModel())
                {
                    var _usuario = db.Usuarios.Where(U => U.CodigoUsuario == usuario.CodigoUsuario).FirstOrDefault();
                    if (_usuario is null)
                    {
                        db.Usuarios.Add(usuario);
                    }
                    else
                    {
                        _usuario.Contraseña = usuario.Contraseña;
                        _usuario.Correo = usuario.Correo;
                        _usuario.Img = usuario.Img;
                       
                    }
                    db.SaveChanges();
                    //HttpCookie _User_Info = new HttpCookie("UserInfo");
                    //_User_Info["img"] = usuario.Img;
                    //Response.Cookies.Set(_User_Info);
                }
            }
            return View(usuario);
        }

        public ActionResult GuardarRutaImg(int Codigo)
        {

            var r = new List<Models.UploadFilesResult>();

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;

                string Ruta = "~/Content/img/Users";
                string savedFileName = Path.Combine(Server.MapPath(Ruta), Path.GetFileName(hpf.FileName));
                hpf.SaveAs(savedFileName); // Save the file
                r.Add(new Models.UploadFilesResult()
                {
                    Name = Ruta.Remove(0,1)  +"/"+Path.GetFileName(hpf.FileName),
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType
                });
            }
            return Json(r);
        } 
    }
    
}
