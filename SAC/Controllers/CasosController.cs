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
            var lista = new List<Models.Resoluciones.Casos>();
            using (var db = new Models.dbModel())
            {
                lista = db.Casos.ToList();
            }
            return View(lista);
        }

        public ActionResult Resolucion(int Caso)
        {
            var _Caso = new Models.Resoluciones.CasoVista();
            var _Estados = new  List<Models.Resoluciones.Estados_Casos>();
            using (var db = new Models.dbModel())
            {
                _Caso._Caso  = db.Casos.Where(x => x.Id_Caso == Caso).FirstOrDefault();
                _Caso._Responsable = db.Usuarios.Where(x => x.CodigoUsuario == _Caso._Caso.Codigo_Responsable).FirstOrDefault().Usuario;
                _Caso._Areas = db.Area.ToList();
                _Estados = db.Estados_Casos.ToList();

                _Caso._Comentarios =(from c in db.Comentarios_Casos join u in db.Usuarios on c.Codigo_Observador
                                  equals u.CodigoUsuario
                                  select new Models.Resoluciones.Comentarios_vista {
                                        Id_Comentario = c.Id_Comentario,
                                        Id_Caso = c.Id_Caso,
                                        Codigo_Observador = c.Codigo_Observador,
                                        Observador = u.Usuario,
                                        Comentario = c.Comentario,
                                        Fecha_Comentario=  c.Fecha_Comentario
                                  }).Where(x => x.Id_Caso == Caso).ToList();

                _Caso._Observadores = (from o in db.Casos_Usuarios
                                       join u in db.Usuarios on o.Id_Usuario
                                       equals u.CodigoUsuario
                                       select new Models.Resoluciones.Casos_Usuarios_vista
                                       {
                                           Id_Caso = o.Id_Caso,
                                           Id_Caso_Usuario = o.Id_Caso_Usuario,
                                           Id_Rol = o.Id_Rol,
                                           Id_Usuario = o.Id_Usuario,
                                           Usuario = u.Usuario
                                       }).Where(x => x.Id_Caso == Caso).ToList();
                //(from q in db.Quejas
                // join a in db.Area on q.CodigoDepartamento equals a.CodigoArea
                // select new { codigo = q.CodigoQueja, Tipo = q.CodigoTipo, Sucursal = q.CodigoSucursal, Codigo_Area = q.CodigoDepartamento, Area = a.NombreArea, q.Empleado, q.Queja, q.Fecha }).ToList();
            }
            ViewBag.Estados = _Estados;
            return View(_Caso);
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