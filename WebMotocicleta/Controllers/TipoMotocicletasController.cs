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
    public class TipoMotocicletasController : ApiController
    {
        private HeroMotosEntities db = new HeroMotosEntities();

        // GET: api/TipoMotocicletas
        public IQueryable<TipoMotocicleta> GetTipoMotocicleta()
        {
            return db.TipoMotocicleta;
        }

        // GET: api/TipoMotocicletas/5
        [ResponseType(typeof(TipoMotocicleta))]
        public IHttpActionResult GetTipoMotocicleta(int id)
        {
            TipoMotocicleta tipoMotocicleta = db.TipoMotocicleta.Find(id);
            if (tipoMotocicleta == null)
            {
                return NotFound();
            }

            return Ok(tipoMotocicleta);
        }

        // PUT: api/TipoMotocicletas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoMotocicleta(int id, TipoMotocicleta tipoMotocicleta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoMotocicleta.IdTipoMoto)
            {
                return BadRequest();
            }

            db.Entry(tipoMotocicleta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoMotocicletaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TipoMotocicletas
        [ResponseType(typeof(TipoMotocicleta))]
        public IHttpActionResult PostTipoMotocicleta(TipoMotocicleta tipoMotocicleta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoMotocicleta.Add(tipoMotocicleta);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoMotocicleta.IdTipoMoto }, tipoMotocicleta);
        }

        // DELETE: api/TipoMotocicletas/5
        [ResponseType(typeof(TipoMotocicleta))]
        public IHttpActionResult DeleteTipoMotocicleta(int id)
        {
            TipoMotocicleta tipoMotocicleta = db.TipoMotocicleta.Find(id);
            if (tipoMotocicleta == null)
            {
                return NotFound();
            }

            db.TipoMotocicleta.Remove(tipoMotocicleta);
            db.SaveChanges();

            return Ok(tipoMotocicleta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoMotocicletaExists(int id)
        {
            return db.TipoMotocicleta.Count(e => e.IdTipoMoto == id) > 0;
        }
    }
}