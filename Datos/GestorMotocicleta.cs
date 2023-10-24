
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Datos
{
    public class GestorMotocicleta
    {

        public static IEnumerable<Motocicleta> Buscar(string Nombre, bool? Activo, int numeroPagina, out int RegistrosTotal)
        {

            //ref Entity Framework

            using (HeroMotosEntities db = new HeroMotosEntities())     //el using asegura el db.dispose() que libera la conexion de la base
            {
                IQueryable<Motocicleta> consulta = db.Motocicleta.Include(x => x.TipoMotocicleta);  // incluir obj hijos evitando lazy load (y tambien error de serializacion)

                // aplicar filtros
                //ref LinQ
                //Expresiones lambda, metodos de extension
                if (!string.IsNullOrEmpty(Nombre))
                    consulta = consulta.Where(x => x.Nombre.ToUpper().Contains(Nombre.ToUpper()));    // equivale al like '%TextoBuscar%'
                if (Activo != null)
                    consulta = consulta.Where(x => x.Activo == Activo);
                RegistrosTotal = consulta.Count();

                // ref EF; consultas paginadas
                int RegistroDesde = (numeroPagina - 1) * 10;
                var Lista = consulta.OrderBy(x => x.Nombre).Skip(RegistroDesde).Take(10).AsNoTracking().ToList(); // la instruccion sql recien se ejecuta cuando hacemos ToList()
                return Lista;
            }

        }



        public static Motocicleta BuscarPorId(int sId)
        {
            using (HeroMotosEntities db = new HeroMotosEntities())
            {
                return db.Motocicleta.Include(x => x.TipoMotocicleta).Where(x => x.IdMotocicleta == sId).FirstOrDefault();
                //return db.Motocicleta.Find(sId);
            }

        }

        public static void Grabar(Motocicleta DtoSel)
        {
            // validar campos
            string erroresValidacion = "";
            if (string.IsNullOrEmpty(DtoSel.Nombre))
                erroresValidacion += "Nombre es un dato requerido; ";
            if (DtoSel.Precio == null || DtoSel.Precio == 0)
                erroresValidacion += "Precio es un dato requerido; ";
            if (DtoSel.CodigoDeBarra == null)
                erroresValidacion += "Codigo De Barra es un dato requerido; ";
            if (DtoSel.IdTipoMoto == null || DtoSel.IdTipoMoto == 0)
                erroresValidacion += "Tipo de Motocicleta es un dato requerido; ";
            if (DtoSel.Stock == null)
                erroresValidacion += "Stock es un dato requerido; ";
            if (DtoSel.FechaAlta == null)
                erroresValidacion += "Fecha de Alta es un dato requerido; ";
            if (DtoSel.Activo == null)
                erroresValidacion += "Activo es un dato requerido; ";
            if (!string.IsNullOrEmpty(erroresValidacion))
                throw new Exception(erroresValidacion);

            // grabar registro
            using (HeroMotosEntities db = new HeroMotosEntities())
            {
                try
                {
                    if (DtoSel.IdMotocicleta != 0)
                    {
                        DtoSel.TipoMotocicleta = null;
                        db.Entry(DtoSel).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Motocicleta.Add(DtoSel);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Contains("UQ_Motocicleta_Nombre"))
                        throw new ApplicationException("Ya existe otra Motocicleta con ese Nombre");
                    else
                        throw;
                }
            }
        }


        public static void ActivarDesactivar(int IdMotocicleta)
        {
            using (HeroMotosEntities db = new HeroMotosEntities())
            {
                //ref Entity Framework; ejecutar codigo sql directo
                db.Database.ExecuteSqlCommand("Update Motocicleta set Activo = case when ISNULL(activo,1)=1 then 0 else 1 end  where IdMotocicleta = @IdMoto",
                    new SqlParameter("@IdMoto", IdMotocicleta)
                    );
            }
        }



        public static Motocicleta ADOBuscarPorId(int IdMotocicleta)
        {
            //ref ADO; Recuperar cadena de conexión de web.config
            string CadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["HeroMotosAdo"].ConnectionString;
            //ref ADO; objetos conexion, comando, parameters y datareader
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = CadenaConexion;
            Motocicleta art = null;
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "select * from Motocicleta c where c.IdMotocicleta = @IdMoto";
                cmd.Parameters.Add(new SqlParameter("@IdMoto", IdMotocicleta));
                SqlDataReader dr = cmd.ExecuteReader();
                // con el resultado cargar una entidad
                //ref ADO; generar un objeto entidad
                if (dr.Read())
                {
                    art = new Motocicleta();
                    art.IdMotocicleta = (int)dr["IdMotocicleta"];
                    art.Nombre = dr["Nombre"].ToString();
                    if (dr["Precio"] != DBNull.Value)
                        art.Precio = (decimal)dr["Precio"];
                    if (dr["CodigoDeBarra"] != DBNull.Value)
                        art.CodigoDeBarra = dr["CodigoDeBarra"].ToString();
                    if (dr["IdTipoMoto"] != DBNull.Value)
                        art.IdTipoMoto = (int)dr["IdTipoMoto"];
                    if (dr["Stock"] != DBNull.Value)
                        art.Stock = (int)dr["Stock"];
                    if (dr["Activo"] != DBNull.Value)
                        art.Activo = (bool)dr["Activo"];
                    if (dr["FechaAlta"] != DBNull.Value)
                        art.FechaAlta = (DateTime)dr["FechaAlta"];
                }
                dr.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
            return art;
        }
    }
}

