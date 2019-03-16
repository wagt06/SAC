using Helper;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAC.Models
{
    public class Usuarios

    {
        [Key]
        public int CodigoUsuario { get; set; }

        [Required]
        public string Usuario { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required]
        public string Contraseña { get; set; }

        public string Img { get; set; }

        public virtual Models.Seguridad.Rol Rol { get; set; }

        public int Roll { get; set; }


        public ResponseModel Autenticarse()
        {
            var rm = new ResponseModel();

            try
            {
                using (var ctx = new Models.dbModel())
                {
                    var usuario = ctx.Usuarios.Where(x => x.Correo == this.Correo && x.Contraseña == this.Contraseña).SingleOrDefault();
                    if (usuario != null)
                    {
                        SessionHelper.AddUserToSession(usuario.CodigoUsuario.ToString());
                        rm.SetResponse(true);
                    }
                    else
                    {
                        rm.SetResponse(false, "Acceso denegado al sistema");
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return rm;
        }

        public string ObtenerID(string User)
        {
            string usuario = "";

            try
            {
                using (var ctx = new Models.dbModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    usuario = ctx.Usuarios.Where(x => x.Usuario == User).FirstOrDefault().CodigoUsuario.ToString();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return usuario;
        }

        public Usuarios Obtener(int id)
        {
            var usuario = new Usuarios();

            try
            {
                using (var ctx = new Models.dbModel())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    usuario = ctx.Usuarios.Include("Rol")
                                          .Include("Rol.Permiso")
                                          .Where(x => x.CodigoUsuario == id).SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return usuario;
        }
    }

}