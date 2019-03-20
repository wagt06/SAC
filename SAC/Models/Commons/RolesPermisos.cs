using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Commons
{
    public enum RolesPermisos
    {
        #region Quejas 
        Ver_Listas_de_Quejas = 1,
        Crear_Casos = 2,
        Reclasificacion_de_Casos = 3,
        Ver_Lista_Casos = 4,
        Dictamen_casos = 5,
        ////GuardarResolucion = 8,
        EscribirComentarios = 9,
        #endregion

        #region Panel
        Ver_Panel = 6,

        #endregion

        #region Panel
        Ver_lista_Usuarios = 7,
        #endregion

        #region Usuarios
        ModificarRoll =10
        #endregion

    }
}