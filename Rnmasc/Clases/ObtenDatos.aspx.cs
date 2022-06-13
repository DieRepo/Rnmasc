using MySql.Data.MySqlClient;
using Rnmasc.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rnmasc.Clases
{
    public partial class ObtenDatos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void GeneraReporte(object sender, EventArgs e)
        {
            InsertaDatos();
        }

        public void InsertaDatos()
        {
            long lastId = 0;
            MySqlConnection conexion = new MySqlConnection(System.Configuration.ConfigurationManager.AppSettings["sej"]);
            conexion.Open();

            List<ControlJuzgado> insertUno = new List<ControlJuzgado>();

            String consulta =
                "SELECT " +
                "centro.Abreviatura AS Centro, " +
                "te.Abreviatura AS Abrev, " +
                "concat(expediente.NumExp, ' / ', expediente.AnioExp) AS CarpetaAdmin, " +
                "concat(expediente.NumDocProcedencia, ' / ', expediente.AnioDocProcedencia) AS CausaExpediente, " +
                "jdos.cveDistrito AS CveDistrito, " +
                "expediente.idAdscProcedencia AS CveOrgano, " +
                "expediente.FechaReg AS FechaRegistro, " +
                "expediente.cveMediador cveMediador, " +
                "per.Paterno pa,per.Materno ma,per.Nombre nom " +
                "FROM " +
                "bdmediacion.tblexpediente AS expediente " +
                "LEFT JOIN bdmediacion.tblcentrom AS centro ON expediente.idCentroM = centro.idCentroM " +
                "LEFT JOIN htsj_gestion.tbljuzgados AS jdos ON expediente.idAdscProcedencia = jdos.IdJuzgado " +
                "LEFT JOIN htsj_gestion.tbladscripciones AS Ands ON expediente.idAdscProcedencia = Ands.CveAdscripcion " +
                "LEFT JOIN htsj_gestion.tblpersonal AS per ON expediente.cveMediador = per.NumEmpleado " +
                "LEFT JOIN bdmediacion.tblusuario AS u ON expediente.CveExp = u.CveExp " +
                "LEFT JOIN bdmediacion.tbltipoexpediente te ON expediente.idTipoExpediente = te.idTipoExpediente " +
                "WHERE " +
                "expediente.IdTipoProcedencia = 1 " +
                "AND expediente.FechaReg BETWEEN '2022-04-01 00:00:00' AND '2022-05-31 23:59:59' " +
                "AND(jdos.desJuz LIKE '%CONTROL%') " +
                "GROUP BY  Centro, CarpetaAdmin, FechaRegistro, CveOrgano, CausaExpediente " +
                "ORDER BY Centro, CarpetaAdmin, FechaRegistro, CveOrgano, CausaExpediente; ";

            MySqlCommand cmd = new MySqlCommand(consulta, conexion);
            cmd.CommandTimeout = 1800;
            MySqlDataReader r = cmd.ExecuteReader();


            while (r.Read())
            {
                ControlJuzgado c = new ControlJuzgado();
                c.Centromed = r.GetString("Centro");
                c.Abrev = r.GetString("Abrev");
                c.Carpetadminmed = r.GetString("CarpetaAdmin");
                c.Causaexpediente = r.GetString("CausaExpediente");
                c.Distrito = r.GetString("CveDistrito");
                c.Cveorgano = r.GetString("CveOrgano");
                c.Fecharadmed = r.GetDateTime("FechaRegistro").ToString("yyyy-MM-dd");
                c.CveMediador = r.GetString("cveMediador");
                c.Paterno = r.GetString("pa");
                c.Materno = r.GetString("ma");
                c.Nombre = r.GetString("nom");

                insertUno.Add(c);

                
            }

            conexion.Close();

            MySqlConnection conexion1 = new MySqlConnection(System.Configuration.ConfigurationManager.AppSettings["local"]);
            conexion1.Open();
            foreach (ControlJuzgado c in insertUno)
            {               
                string insert = "insert into rnmasc.tblcontroljuzgado(centromed,abrev,carpetadminmed,causaexpediente,distrito,cveorgano,fecharadmed) " +
                                "values('" + c.Centromed + "','" + c.Abrev + "','" + c.Carpetadminmed + "','" + c.Causaexpediente + "'," + c.Distrito + ",'" + c.Cveorgano + "','" + c.Fecharadmed + "') ";
                MySqlCommand cmd1 = new MySqlCommand(insert, conexion1);
                cmd1.ExecuteNonQuery();
                lastId = cmd1.LastInsertedId;

                MySqlConnection conexion2 = new MySqlConnection(System.Configuration.ConfigurationManager.AppSettings["local"]);
                conexion2.Open();
               
                    string insert2 = "insert into rnmasc.tblfacilitadores(cvecontroljuz,nombre,apellidoprim,apellidoseg,numempleado) " +
                    "values(" + lastId + ",'" + c.Nombre + "','" + c.Paterno + "','" + c.Materno + "'," + c.CveMediador + ") ";
                    MySqlCommand cmd2 = new MySqlCommand(insert2, conexion2);
                    cmd2.ExecuteNonQuery();
                
                conexion2.Close();
            }
            conexion1.Close();

            
        }
    }

}