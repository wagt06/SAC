using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAC.Controllers
{
    public class IndicadoresController : Controller
    {
        // GET: Indicadores
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListaIndicadores(int CodigoUsuario, int CodigoRoll)
        {
            var Indicadores = new List<Models.Graficos>();
            using (var db = new Models.dbModel()) {
                Indicadores.Add(new Models.Graficos
                {
                    Name = "Sugerencias",
                    Value = db.Quejas.Count()
                });
                Indicadores.Add(new Models.Graficos
                {
                    Name = "Casos",
                    Value = (from q in db.Casos
                             join u in db.Casos_Usuarios on q.Id_Caso 
                             equals u.Id_Caso
                             join r in db.Estados_Casos on q.Codigo_Estado equals r.Codigo_Estado
                             where u.Id_Usuario == CodigoUsuario && u.Id_Rol == CodigoRoll && r.Escierre == false select new { u.Id_Caso }).Count()
            });
            }
            return Json(Indicadores, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Graficos()
        {
            List<int[]> Graficos = new List<int[]>();
            using (var db = new Models.dbModel())
            {

                var r = db.Database.SqlQuery<Models.Graficos>("SELECT COUNT(MONTH(q.Fecha))Value,CONVERT(NVARCHAR,MONTH(q.Fecha)) Name FROM dbo.Quejas q Where CodigoTipo = 1 AND  YEAR(q.Fecha) = YEAR(getDATE())  GROUP BY MONTH(q.Fecha)", 1).ToList();
                var e = db.Database.SqlQuery<Models.Graficos>("SELECT COUNT(MONTH(q.Fecha))Value,CONVERT(NVARCHAR,MONTH(q.Fecha)) Name FROM dbo.Quejas q Where CodigoTipo = 2 AND   YEAR(q.Fecha) = YEAR(getDATE())  GROUP BY MONTH(q.Fecha)", 2).ToList();
                var p = db.Database.SqlQuery<Models.Graficos>("SELECT COUNT(MONTH(q.Fecha))Value,CONVERT(NVARCHAR,MONTH(q.Fecha)) Name FROM dbo.Quejas q Where CodigoTipo = 3 AND   YEAR(q.Fecha) = YEAR(getDATE())  GROUP BY MONTH(q.Fecha)", 3).ToList();
                var meses = 0;
                int[] list1, list2, list3;
                list1 = new int[12]; //Clientes
                list2 = new int[12]; //Empleados
                list3 = new int[12]; //Proveedores

                while (meses <= 11)
                {
                    meses = meses + 1;
                    list1[meses - 1] = 0;
                    foreach (var i in r)
                    {
                        if (meses == int.Parse(i.Name))
                        {
                            list1[meses - 1] = i.Value;
                        }
                    }
                }
                meses = 0;
                while (meses <= 11)
                {
                    meses = meses + 1;
                    list2[meses - 1] = 0;
                    foreach (var i in e)
                    {
                        if (meses == int.Parse(i.Name))
                        {
                            list2[meses - 1] = i.Value;
                        }
                    }
                }
                meses = 0;
                while (meses <= 11)
                {
                    meses = meses + 1;
                    list3[meses - 1] = 0;
                    foreach (var i in p)
                    {
                        if (meses == int.Parse(i.Name))
                        {
                            list3[meses - 1] = i.Value;
                        }
                    }
                }
                Graficos.Add(list1);
                Graficos.Add(list2);
                Graficos.Add(list3);
            }
            // string consulta = "SELECT COUNT(MONTH(q.Fecha))Cant,MONTH(q.Fecha) mes FROM dbo.Quejas q   GROUP BY MONTH(q.Fecha)";
            return Json(Graficos, JsonRequestBehavior.AllowGet);
        }
    }
}