using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Collections;
using System.Web.UI;

namespace SAC.Controllers
{
    public class SugerenciasController : Controller
    {
        // GET: Sugerencias
        [Authorize]
        public ActionResult Index()
        {

            if (Request.Cookies["Usuario"] != null) {
                string CookieValue = Request.Cookies["Usuario"].Value;
            }

            var session = HttpContext.Request.Cookies["Usuario"];

            List<Models.Quejas> Quejas = new List<Models.Quejas>();
            using (var db = new Models.dbModel()) {

                Quejas = db.Quejas.OrderByDescending(x => x.Fecha).ToList();
                ViewBag.Estados = db.Estados_Casos.ToList();
            }
            return View(Quejas);
        }

        // GET: Sugerencias/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var nombreEquipo = Request.ServerVariables["AUTH_USER"].ToString();
            var IP = Request.ServerVariables["REMOTE_HOST"].ToString();

            Models.Quejas q = new Models.Quejas();
            using (var db = new Models.dbModel()) {
                q = db.Quejas.Where(x => x.CodigoQueja == id).First();
            }
            return Json(q, JsonRequestBehavior.AllowGet);
        }

        // GET: Sugerencias/Create
        public ActionResult Create()
        {
            List<Models.Area> areas = new List<Models.Area>();
            List<Models.TiposQuejas> tipos = new List<Models.TiposQuejas>();
            using (var db = new Models.dbModel())
            {
                areas = db.Area.Where(A => A.Activo == true).ToList();
                tipos = db.TiposQuejas.Where(t => t.Activo == true).ToList();
            }
            ViewBag.Areas = areas;
            ViewBag.Tipos = tipos;
            return View(new Models.Quejas());
        }

        // POST: Sugerencias/Create
        [HttpPost]
        public ActionResult Create(SAC.Models.Quejas Quejas, FormCollection collection, string Queja)
        {
            string CuerpoCorreo = string.Empty;
            try
            {
                if (!ModelState.IsValid) {
                    return View(Queja);
                }
                using (var db = new Models.dbModel())
                {
                    Quejas.Fecha = DateTime.Now;
                    db.Quejas.Add(Quejas);
                    db.SaveChanges();


                    CuerpoCorreo = PopulateBody(Quejas);
                    EnviarCorreo("Sugerencia Desde Sitio WEB", "info@paisas.club", "Correo Generado API Quejas", CuerpoCorreo, "No APlica", 2);
                    return Redirect("~/home/index?sugerencia=1");
                }
            }
            catch
            {
                return View(Queja);
            }
        }

        private string PopulateBody(Models.Quejas Quejas)
        {
            string body = string.Empty;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(Server.MapPath("~/Content/newletters/blog-stories.html")))
            {
                body = reader.ReadToEnd();
            }
            string Area;
            string Tipo;

            using (var db = new Models.dbModel()) {
                Area = db.Area.Where(x => x.CodigoArea == Quejas.CodigoDepartamento).FirstOrDefault().NombreArea;
                Tipo = db.TiposQuejas.Where(x => x.CodigoTipoQueja == Quejas.CodigoTipo).FirstOrDefault().Descripcion;
            }
           

            body = body.Replace("{{Nombre}}", Quejas.Empleado);
            body = body.Replace("{{Area}}", Area);
            body = body.Replace("{{Tipo}}", Tipo);
            body = body.Replace("{{Queja}}", Quejas.Queja);
            return body;
        }

        private string PlantillaCorreoContacto(string Nombre, string telefono, string correo, string mensaje)
        {
            string body = string.Empty;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(Server.MapPath("~/Content/newletters/Cliente-email-contacto.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{Nombre}}", Nombre);
            body = body.Replace("{{Telefono}}", telefono);
            body = body.Replace("{{Correo}}", correo);
            body = body.Replace("{{Mensaje}}", mensaje);
            return body;
        }

        // GET: Sugerencias/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sugerencias/Edit/5
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

        // GET: Sugerencias/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sugerencias/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult EnviarCorreo(string nombre, string correo, string asunto, string message, string Telefono, int Dest) {
            try
            {

                Models.Config_mail conf = new Models.Config_mail();
                using (var db = new Models.dbModel())
                {
                    conf = db.Config_mail.ToList().FirstOrDefault();
                }

                var lista = Dest == 1 ? conf.MailDestino.Split(';') : conf.MailDestinoSug.Split(';');
                foreach (var r in lista)
                {
                    var fromAddress = new MailAddress(conf.Mail, conf.Asunto);
                    var toAddress = new MailAddress(r, nombre);
                    string fromPassword = conf.Pass;
                    string subject = asunto;

                    string body = String.Empty;
                    if (Dest == 1)
                    {
                        //body = string.Format(@"<strong>Cliente: </strong> {0} <br>
                        //                         <strong> Correo: </strong> {1} <br>
                        //                         <strong> Telefono: </strong> : {2} <br>
                        //                         <strong> Asunto : </strong> {3} <br>
                        //                         <strong> Mensaje: </strong> {4} <br>
                        //                          Este correo fue enviado desde el sitio WEB", nombre, correo, Telefono, asunto, message);
                        body = PlantillaCorreoContacto(nombre, Telefono, correo, message);
                    }
                    else {
                        body = message;
                    }

                    var smtp = new SmtpClient
                    {
                        Host = conf.Host,
                        Port = int.Parse(conf.Port),
                        EnableSsl = conf.EnableSSl,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var messages = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                    {
                        messages.IsBodyHtml = true;
                        smtp.Send(messages);
                    }
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
                throw;
            }

        }

        public ActionResult ExportSugerencias()
        {

            using (var db = new Models.dbModel()) {
                var listaSugerencias = (from q in db.Quejas
                                        join a in db.Area on q.CodigoDepartamento equals a.CodigoArea
                                        select new { codigo = q.CodigoQueja, Tipo = q.CodigoTipo, Sucursal = q.CodigoSucursal, Codigo_Area = q.CodigoDepartamento, Area = a.NombreArea, q.Empleado, q.Queja, q.Fecha }).ToList();



                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = listaSugerencias;
                grid.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;

                grid.DataBind();

                Response.ClearContent();
                var fName = string.Format("Sugerencias-{0}", DateTime.Now.ToString("s"));
                Response.AddHeader("content-disposition", "attachement; filename=" + fName + ".xls");
                Response.ContentType = "application/excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }

            return View();

        }
    }
}

