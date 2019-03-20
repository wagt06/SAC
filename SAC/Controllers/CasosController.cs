using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Commons;

namespace SAC.Controllers
{
    public class CasosController : Controller
    {
        // GET: Casos
        [Authorize]
        public ActionResult Index(DateTime? fechaIni , DateTime? FechaFin = null,int Estado = 1)
        {
            HttpCookie _UserInfo = Request.Cookies["UserInfo"];
            int CodigoUsuario = int.Parse(_UserInfo["Codigo"]);
            var lista = new List<Models.Resoluciones.Casos>();
            if (fechaIni == null || FechaFin == null) {
                fechaIni = DateTime.Now.AddDays(-30);
                FechaFin = DateTime.Now;
            }

            FechaFin = FechaFin.Value.AddDays(1);
            using (var db = new Models.dbModel()) 
            {
                var estados = db.Estados_Casos.ToList();
                List<Models.Resoluciones.Casos_Usuarios> CasosdeUsuario = db.Casos_Usuarios.Where(x => x.Id_Usuario == CodigoUsuario).ToList();

               var  lista2 = new List<Models.Resoluciones.Casos>();
            
                if (Estado == 0) {

                    foreach (var caso in CasosdeUsuario) {

                        lista2.Add(db.Casos.Where(x => x.Id_Caso == caso.Id_Caso && x.Fecha_Creacion >=fechaIni && x.Fecha_Creacion <=FechaFin).FirstOrDefault());
                    }
                    lista2.RemoveAll(item => item == null);

                    lista = (from c in lista2
                             join e in estados on c.Codigo_Estado equals e.Codigo_Estado
                             select new Models.Resoluciones.Casos
                             {
                                 Id_Caso = c.Id_Caso,
                                 Codigo_Estado = c.Codigo_Estado,
                                 Codigo_Responsable = c.Codigo_Responsable,
                                 NombreResponsable = db.Usuarios.Where(u => u.CodigoUsuario == c.Codigo_Responsable).FirstOrDefault().Usuario,
                                 Fecha_Inicio = c.Fecha_Inicio,
                                 NombreEstado = e.Nombre_Estado,
                                 Fecha_Creacion = c.Fecha_Creacion
                             }).ToList();   
                }
                else {
                    foreach (var caso in CasosdeUsuario)
                    {

                        lista2.Add(db.Casos.Where(x => x.Id_Caso == caso.Id_Caso && x.Fecha_Creacion >= fechaIni && x.Fecha_Creacion <= FechaFin && x.Codigo_Estado == Estado).FirstOrDefault());
                    }
                    lista2.RemoveAll(item => item == null);
                    lista = (from c in lista2
                             join e in estados on c.Codigo_Estado equals e.Codigo_Estado
                             select new Models.Resoluciones.Casos
                             {
                                 Id_Caso = c.Id_Caso,
                                 Codigo_Estado = c.Codigo_Estado,
                                 Codigo_Responsable = c.Codigo_Responsable,
                                 NombreResponsable = db.Usuarios.Where(u => u.CodigoUsuario == c.Codigo_Responsable).FirstOrDefault().Usuario,
                                 Fecha_Inicio = c.Fecha_Inicio,
                                 NombreEstado = e.Nombre_Estado,
                                 Comentario = c.Comentario,
                                 Codigo_Queja = c.Codigo_Queja,
                                 Fecha_Creacion = c.Fecha_Creacion
                             }).ToList();
                }
                ViewBag.Casos = CasosdeUsuario;
                ViewBag.Estados = estados;
                ViewBag.Estado = Estado;
                ViewBag.Title = "Mis Casos";
            }
         
            return View(lista);
        }

        [Authorize]
        public ActionResult Create(int codigo) {
            var caso = new Models.Resoluciones.Casos();
            HttpCookie _UserInfo = Request.Cookies["UserInfo"];
            int CodigoUsuario = int.Parse(_UserInfo["Codigo"]);

           

            using (var db = new  Models.dbModel()) {

                var sugerencia = (db.Quejas.Where(x => x.CodigoQueja == codigo).FirstOrDefault());



                //Set Otros casos con estado Finalizado
                List<Models.Resoluciones.Casos> CasosAnteriores = db.Casos.Where(x => x.Codigo_Queja == codigo).ToList();
                foreach (var casos in CasosAnteriores) {
                    casos.Codigo_Estado = 4;
                    casos.Fecha_Modificacion = DateTime.Now;
                    db.SaveChanges();
                }


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

        [Authorize]
        public ActionResult Resolucion(int Caso)
        {
            var _Caso = new Models.Resoluciones.CasoVista();
            var _Estados = new List<Models.Resoluciones.Estados_Casos>();
            var Tipos = new List<Models.TiposQuejas>();

            HttpCookie _UserInfo = Request.Cookies["UserInfo"];
            int CodigoUsuario = int.Parse(_UserInfo["Codigo"]);

            try
            {
                using (var db = new Models.dbModel())
                {
                    _Caso._Caso = db.Casos.Where(x => x.Id_Caso == Caso).FirstOrDefault();

                    if (_Caso._Caso.Codigo_Responsable != 0)
                    {

                        _Caso._Responsable = db.Usuarios.Where(x => x.CodigoUsuario == _Caso._Caso.Codigo_Responsable).FirstOrDefault().Usuario;
                    }

                    _Caso._Areas = db.Area.ToList();
                    _Estados = db.Estados_Casos.ToList();

                    _Caso._Queja = db.Quejas.Where(x => x.CodigoQueja == _Caso._Caso.Codigo_Queja).FirstOrDefault();
                    Tipos = db.TiposQuejas.Where(x => x.Activo == true).ToList();


                    var Usuarios = db.Usuarios.ToList();
                    var Observadores = db.Casos_Usuarios.Where(x => x.Id_Caso == Caso).ToList();




                    _Caso._Observadores = (from u in db.Usuarios
                                           join o in db.Casos_Usuarios.Where(X => X.Id_Caso == Caso).DefaultIfEmpty() on
                     u.CodigoUsuario equals o.Id_Usuario into gj
                                           from subpet in gj.DefaultIfEmpty()
                                           select new Models.Resoluciones.Casos_Usuarios_vista
                                           {
                                               Id_Caso = subpet.Id_Caso,
                                               Id_Usuario = u.CodigoUsuario,

                                               Id_Caso_Usuario = subpet.Id_Caso_Usuario,
                                               Id_Rol = subpet.Id_Rol,

                                               Select = subpet.Id_Usuario != null ? true : false,
                                               Usuario = u.Usuario
                                           }).ToList();

                    //if (!FrontUser.TienePermiso(RolesPermisos.Crear_Casos) && _Caso._Observadores.Where(x=>x.Id_Usuario == CodigoUsuario).SingleOrDefault() == null ) {
                    //    return RedirectToAction("index");
                    //}

                }
                _Caso._Comentarios = Comentarios(Caso);
                ViewBag.Estados = _Estados;
                ViewBag.Roll_Caso = CodigoUsuario == _Caso._Caso.Codigo_Responsable ? 1 : 0;
                ViewBag.Tipos = Tipos;
                return View(_Caso);
                //return Json("1");
            }
            catch (Exception)
            {
                ViewBag.Estados = _Estados;
                return RedirectToAction("index");
                throw;
            }
       
   
            
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
                if (Comentario == "" && Codigo_Estado > 2 )
                {
                    
                    return Redirect("~/Casos/Resolucion/?Caso=" + Id_Caso);
                }
                
                using (var db = new Models.dbModel())
                {
                    var caso = db.Casos.Where(x => x.Id_Caso == Id_Caso).FirstOrDefault();
                    var sugerencia = (db.Quejas.Where(x => x.CodigoQueja == caso.Codigo_Queja).FirstOrDefault());

                    if (Codigo_Estado == 2)
                    {
                        sugerencia.CodigoEstado = Codigo_Estado;
                        caso.Fecha_Inicio = DateTime.Now;
                    }
                    else
                    {
                        sugerencia.CodigoEstado = Codigo_Estado;
                        caso.Fecha_Finalizacion = DateTime.Now;
                    }
                
                    caso.Codigo_Responsable = Codigo_Responsable;
                    caso.Comentario = Comentario;
                    caso.Codigo_Estado = Codigo_Estado;
                    caso.Fecha_Modificacion = DateTime.Now;
                    db.SaveChanges();
                }
                    return Json("Resolucion");
                //return Json(Url.Content("~/Caso/Resolucion/?Caso=" + Id_Caso), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex )
            {
                return Json("Ocurrio un error al guardar!"+ ex.Message);
               // throw;
            }

        }

        [HttpPost]
        public ActionResult GuardarCaso(Models.Resoluciones.Casos Caso, List<int> Observadores = null, List<int> Responsable = null)
        {
            try
            {
                
                    using (var db = new Models.dbModel())
                    {
                        var _caso = db.Casos.Where(x => x.Id_Caso == Caso.Id_Caso).FirstOrDefault();
                        if (_caso != null)
                        {

                        if (_caso.Codigo_Estado == null || _caso.Codigo_Estado == 1)
                        {

                            _caso.CodigoTipo = Caso.CodigoTipo;
                            _caso.CodigoDepartamento = Caso.CodigoDepartamento;
                            _caso.Fecha_Creacion = DateTime.Now;


                            if (Responsable != null)
                            {
                                _caso.Codigo_Responsable = Responsable.FirstOrDefault();
                            }

                            //Agregando usuarios  al caso
                            var _UsuariosCasos = new List<Models.Resoluciones.Casos_Usuarios>();

                            //Eliminando Usuarios Anteriores
                            db.Casos_Usuarios.RemoveRange(db.Casos_Usuarios.Where(x => x.Id_Caso == Caso.Id_Caso).ToList());

                            //usuario responsable
                            if (Responsable != null)
                            {
                                _UsuariosCasos.Add(new Models.Resoluciones.Casos_Usuarios { Id_Caso = Caso.Id_Caso, Id_Usuario = Responsable.First(), Id_Rol = 1 });
                            }
                            //usuario Observadores
                            if (Observadores != null)
                            {
                                foreach (int ob in Observadores.Where(x => x != _caso.Codigo_Responsable).Distinct())
                                {
                                    _UsuariosCasos.Add(new Models.Resoluciones.Casos_Usuarios { Id_Caso = Caso.Id_Caso, Id_Usuario = ob, Id_Rol = 2 });
                                }
                            }
                            if (_UsuariosCasos.Count > 0)
                            {
                                db.Casos_Usuarios.AddRange(_UsuariosCasos);
                            }
                            db.SaveChanges();
                        }
                        else {
                            return Json("Este caso ya este en el estado Iniciado!, no se puede modificar, actualize la pagina!");
                        }
                    }
                }
                return Json("Se guardaron las Asignaciones!");
            }
            catch (Exception ex)
            {
                return Json("Ocurrio un error: "+ ex.Message + "!");
               // throw;
            }
            
           // return Redirect("~/Casos/Resolucion/?Caso="+ Caso.Id_Caso);
                //return RedirectToAction("Resolucion", "Casos",Caso.Id_Caso);
        }


    }
}