using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Datos;

namespace WebMotocicleta
{
    public class MotocicletasController : ApiController
    {
        //ref webapi; por defecto se busca el metodo de request (get, post, etc) segun comienze el nombre de la accion y sus parametros 
        //GET: api/Motocicletas
        public IHttpActionResult GetMotocicletas(string Nombre = "", bool? Activo = null, int numeroPagina = 1)
        {
            //ref webapi parametros;
            //ref webapi tipos de retorno de los metodos; cambiamos la devolucion generica del metodo: IQueryable<Motocicleta> por IHttpActionResult para poder devolver tambien RegistrosTotal
            int RegistrosTotal;
            //ref c#  var
            var Lista = Datos.GestorMotocicleta.Buscar(Nombre, Activo, numeroPagina, out RegistrosTotal);
            return Ok(new { Lista = Lista, RegistrosTotal = RegistrosTotal });
        }

        // GET: api/Motocicletas/5
        [ResponseType(typeof(Motocicleta))]
        public IHttpActionResult GetMotocicletas(int id)
        {
            Motocicleta motocicleta = Datos.GestorMotocicleta.BuscarPorId(id);
            //ref Ado generar objeto generico
            //Motocicleta motocicleta = Datos.GestorMotocicleta.ADOBuscarPorId(id);
            if (motocicleta == null)
            {
                return NotFound();  // status 404
            }
            return Ok(motocicleta);
        }

        // PUT: api/Motocicleta/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMotocicletas(int id, Motocicleta motocicleta)
        {
            if (!ModelState.IsValid)  //ref DataAnnotations; validar en el servidor ??
            {
                return BadRequest(ModelState);
            }

            if (id != motocicleta.IdMotocicleta)
            {
                return BadRequest();
            }

            Datos.GestorMotocicleta.Grabar(motocicleta);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Motocicletas
        [ResponseType(typeof(Motocicleta))]
        public IHttpActionResult PostMotocicletas(Motocicleta motocicleta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Datos.GestorMotocicleta.Grabar(motocicleta);

            return CreatedAtRoute("DefaultApi", new { id = motocicleta.IdMotocicleta }, motocicleta);
        }

        //DELETE: api/Motocicletas/1 
        [ResponseType(typeof(Motocicleta))]
        public IHttpActionResult DeleteMotocicletas(int id)
        {
            //new ApplicationException("error en base");   //ref??? throw no genera error dentro de webapi, continua normalmente?

            //ref EntityFramework baja logica
            Datos.GestorMotocicleta.ActivarDesactivar(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}