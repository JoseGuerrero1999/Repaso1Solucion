using Microsoft.Data;
using Microsoft.Data.SqlClient;
using Repaso1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Repaso1.Controllers
{
    public class EmpleadoController : Controller
    {
        string cadena = @"server =DESKTOP-R91HK72; database=Practicando1; Trusted_Connection = True;" +
      " MultipleActiveResultSets=True;TrustServerCertificate=False; Encrypt=False";
        IEnumerable<Empleado> empleados(string  nombre)
        {
            List<Empleado> temporal = new List<Empleado>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_busqueda_empleado", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Empleado()
                    {
                        idempleado = dr.GetInt32(0),
                        nombre = dr.GetString(1),
                        email = dr.GetString(2),
                        fecha = dr.GetDateTime(3),
                        tipodescrip = dr.GetString(4)
                    });
                }
            }
            return temporal;
        }

        public  async Task<IActionResult> Listado(string nombre)
        {
            return View(await Task.Run(()=> empleados(nombre)));
        }
    }
}
