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
        public ActionResult Index(DateTime? fechaIni , DateTime? FechaFin = null,int Estado = 0)
        {
            var lista = new List<Models.Resoluciones.Casos>();
            if (fechaIni == null) {
                fechaIni = DateTime.Now.AddDays(-30);
                FechaFin = DateTime.Now;
            }
            using (var db = new Models.dbModel()) 
            {
                if (Estado == 0) {

                    lista = db.Casos.ToList();
                }
                else {
                    lista = db.Casos.Where(x => x.Fecha_Creacion >= fechaIni && x.Fecha_Creacion <= FechaFin && x.Codigo_Estado == Estado).ToList();
                }
               
                ViewBag.Estados = db.Estados_Casos.ToList();
                ViewBag.Title = "Mis Casos";
            }
         
            return View(lista);
        }

        public ActionResult Create(int codigo) {
            var caso = new Models.Resoluciones.Casos();
            
            using (var db = new  Models.dbModel()) {

                var sugerencia = (db.Quejas.Where(x => x.CodigoQueja == codigo).FirstOrDefault());

                //caso = (from c in db.Casos
                //        join e in db.Estados_Casos.Where(x=>x.Escierre == false ) on c.Codigo_Estado equals e.Codigo_Estado 
                //        select new Models.Resoluciones.Casos {
                //            Id_Caso = c.Id_Caso
                //        }).FirstOrDefault();


                if (sugerencia != null) {

                    caso.Codigo_Queja = sugerencia.CodigoQueja;
                    caso.CodigoDepartamento = sugerencia.CodigoDepartamento;
                    caso.CodigoTipo = sugerencia.CodigoTipo;
                    caso.Codigo_Estado = 1;
                    caso.Fecha_Creacion = DateTime.Now;
                    caso.Fecha_Modificacion = DateTime.Now;
                    db.Casos.Add(caso);
                    db.SaveChanges();
                }
               
            }

            return Redirect("~/Casos/Resolucion/?Caso=" + caso.Id_Caso);
        }

        public ActionResult Resolucion(int Caso)
        {
            var _Caso = new Models.Resoluciones.CasoVista();
            var _Estados = new List<Models.Resoluciones.Estados_Casos>();
            var Tipos = new List<Models.TiposQuejas>();
   
            using (var db = new Models.dbModel())
            {
                _Caso._Caso = db.Casos.Where(x => x.Id_Caso == Caso).FirstOrDefault();

                if (_Caso._Caso.Codigo_Responsable != 0) {

                    _Caso._Responsable = db.Usuarios.Where(x => x.CodigoUsuario == _Caso._Caso.Codigo_Responsable).FirstOrDefault().Usuario;
                }

                _Caso._Areas = db.Area.ToList();
                _Estados = db.Estados_Casos.ToList();

                _Caso._Queja = db.Quejas.Where(x => x.CodigoQueja == _Caso._Caso.Codigo_Queja).FirstOrDefault();
                Tipos = db.TiposQuejas.Where(x => x.Activo == true).ToList();


                var Usuarios = db.Usuarios.ToList();
                var Observadores = db.Casos_Usuarios.Where(x => x.Id_Caso == Caso).ToList();

                


                _Caso._Observadores = (from u in db.Usuarios join  o in db.Casos_Usuarios.Where(X => X.Id_Caso == Caso).DefaultIfEmpty() on
                                       u.CodigoUsuario equals o.Id_Usuario into gj
                                       from subpet in gj.DefaultIfEmpty()
                                       select new Models.Resoluciones.Casos_Usuarios_vista
                                       {
                                           Id_Caso = subpet.Id_Caso,
                                           Id_Usuario = u.CodigoUsuario,

                                           Id_Caso_Usuario = subpet.Id_Caso_Usuario,
                                           Id_Rol = subpet.Id_Rol,
                                          
                                           Select = subpet.Id_Usuario!=null ?true:false,
                                           Usuario = u.Usuario
                                       }).ToList();

            }
            _Caso._Comentarios = Comentarios(Caso);
            ViewBag.Estados = _Estados;

            ViewBag.Tipos = Tipos;
            return View(_Caso);
            //return Json(Graficos, JsonRequestBehavior.AllowGet);
        }

        public List<Models.Resoluciones.Comentarios_vista> Comentarios(int Id_Caso)
        {
            try
            {
                List<Models.Resoluciones.Comentarios_vista> Coment = new List<Models.Resoluciones.Comentarios_vista>();
                using (var db = new Models.dbModel())
                {
                    var com = db.Comentarios_Casos.ToList();
                    var usu = db.Usuarios.ToList();

                    Coment = (from c in com
                              join u in usu on c.Codigo_Observador
                                            equals u.CodigoUsuario
                              select new Models.Resoluciones.Comentarios_vista
                              {
                                  Id_Comentario = c.Id_Comentario,
                                  Id_Caso = c.Id_Caso,
                                  Codigo_Observador = c.Codigo_Observador,
                                  Observador = u.Usuario,
                                  Comentario = c.Comentario,
                                  Img = u.Img,
                                  Fecha_Comentario = c.Fecha_Comentario
                              }).Where(x => x.Id_Caso == Id_Caso).ToList();

                    return Coment;
                }
            }
            catch (Exception)
            {
                return new List<Models.Resoluciones.Comentarios_vista>();
                throw;
            }

        }

        [HttpPost]
        public ActionResult GuardarComentarios(int Id_Caso, int Codigo_Observador, string Comentario) {
            try
            {
                if (Comentario == "") {
                    return Json("-1", JsonRequestBehavior.AllowGet);
                }
                var _comentario = new Models.Resoluciones.Comentarios_Casos();
                _comentario.Id_Caso = Id_Caso;
                _comentario.Codigo_Observador = Codigo_Observador;
                _comentario.Comentario = Comentario;
                _comentario.Fecha_Comentario = DateTime.Now;


                using (var db = new Models.dbModel())
                {
                    db.Comentarios_Casos.Add(_comentario);
                    db.SaveChanges();
                }
                return Redirect("~/Casos/Resolucion/?Caso=" + Id_Caso);
                //return Json(Url.Content("~/Caso/Resolucion/?Caso=" + Id_Caso), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
                throw;
            }

        }

        [HttpPost]
        public ActionResult GuardarResolucion(int Id_Caso, int Codigo_Responsable, string Comentario,int Codigo_Estado)
        {
            try
            {
                if (Comentario == "")
                {
                    return Json("-1", JsonRequestBehavior.AllowGet);
                }
                
                using (var db = new Models.dbModel())
                {
                    var caso = db.Casos.Where(x => x.Id_Caso == Id_Caso).FirstOrDefault();
                    caso.Codigo_Responsable = Codigo_Responsable;
                    caso.Comentario = Comentario;
                    caso.Codigo_Estado = Codigo_Estado;
                    caso.Fecha_Modificacion = DateTime.Now;
                    db.SaveChanges();
                }
                return Redirect("~/Casos/Resolucion/?Caso=" + Id_Caso);
                //return Json(Url.Content("~/Caso/Resolucion/?Caso=" + Id_Caso), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
                throw;
            }

        }

        [HttpPost]
        public ActionResult GuardarCaso(Models.Resoluciones.Casos Caso, List<int> Observadores = null, List<int> Responsable = null)
        {
            using (var db = new Models.dbModel()) {
                var _caso = db.Casos.Where(x => x.Id_Caso == Caso.Id_Caso).FirstOrDefault();

                if (_caso != null) {

                    _caso.CodigoTipo = Caso.CodigoTipo;
                    _caso.CodigoDepartamento = Caso.CodigoDepartamento;


                    if(Responsable != null) {
                        _caso.Codigo_Responsable = Responsable.FirstOrDefault();
                    }

                    //Agregando usuarios  al caso
                    var _UsuariosCasos = new List<Models.Resoluciones.Casos_Usuarios>();

                    //Eliminando Usuarios Anteriores
                    db.Casos_Usuarios.RemoveRange(db.Casos_Usuarios.Where(x => x.Id_Caso == Caso.Id_Caso).ToList());
                    db.SaveChanges();

                    //usuario responsable
                    if (Responsable != null) {
                        _UsuariosCasos.Add(new Models.Resoluciones.Casos_Usuarios { Id_Caso = Caso.Id_Caso, Id_Usuario = Responsable.First(), Id_Rol = 1 });
                    }
                    //usuario Observadores
                    if (Observadores != null) {
                        foreach (int ob in Observadores.Distinct())
                        {
                            _UsuariosCasos.Add(new Models.Resoluciones.Casos_Usuarios { Id_Caso = Caso.Id_Caso, Id_Usuario = ob, Id_Rol = 2 });
                        }
                    }
                    if (_UsuariosCasos.Count > 0) {
                        db.Casos_Usuarios.AddRange(_UsuariosCasos);
                    }
                    db.SaveChanges();
                }
            }
            return Redirect("~/Casos/Resolucion/?Caso="+ Caso.Id_Caso);
                //return RedirectToAction("Resolucion", "Casos",Caso.Id_Caso);
        }


    }
}