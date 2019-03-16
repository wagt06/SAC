using Helper;
using Models;
using SAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Commons
{
    public class FrontUser
    {
        public static bool TienePermiso(RolesPermisos valor)
        {
            var usuario = FrontUser.Get();
            return usuario.Rol.Permiso.Where(x => x.PermisoID == valor)
                               .Any();
        }

        public static Usuarios Get()
        {
            return new Usuarios().Obtener(SessionHelper.GetUser());
        }
    }
}