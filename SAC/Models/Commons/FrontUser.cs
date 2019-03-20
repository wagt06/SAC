using Helper;
using Models;
using SAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Models.Commons
{
    public class FrontUser
    {

        [System.Web.Mvc.Authorize]
        public static bool TienePermiso(RolesPermisos valor)
        {
            try
            {
                var usuario = FrontUser.Get();
                return usuario.Rol.Permiso.Where(x => x.PermisoID == valor)
                                   .Any();
            }
            catch (Exception)
            {
                SAC.Controllers.LoginController login = new SAC.Controllers.LoginController();
                login.Login();
                throw;
            }
 
        }

        public static Usuarios Get()
        {
            return new Usuarios().Obtener(SessionHelper.GetUser());
        }
    }
}