using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    public class eventsTablesController : ApiController
    {
        private NGOEventsEntities db = new NGOEventsEntities();

        // GET: api/eventsTables
        public IQueryable<eventsTable> GeteventsTables()
        {
            return db.eventsTables;
        }

        // GET: api/eventsTables/5
        [ResponseType(typeof(eventsTable))]
        public IHttpActionResult GeteventsTable(int id)
        {
            eventsTable eventsTable = db.eventsTables.Find(id);
            if (eventsTable == null)
            {
                return NotFound();
            }

            return Ok(eventsTable);
        }

        // PUT: api/eventsTables/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuteventsTable(int id, eventsTable eventsTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventsTable.eventID)
            {
                return BadRequest();
            }

            db.Entry(eventsTable).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!eventsTableExists(id))
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

        // POST: api/eventsTables
        [ResponseType(typeof(eventsTable))]
        public IHttpActionResult PosteventsTable(eventsTable eventsTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.eventsTables.Add(eventsTable);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = eventsTable.eventID }, eventsTable);
        }

        // DELETE: api/eventsTables/5
        [ResponseType(typeof(eventsTable))]
        public IHttpActionResult DeleteeventsTable(int id)
        {
            eventsTable eventsTable = db.eventsTables.Find(id);
            if (eventsTable == null)
            {
                return NotFound();
            }

            db.eventsTables.Remove(eventsTable);
            db.SaveChanges();

            return Ok(eventsTable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool eventsTableExists(int id)
        {
            return db.eventsTables.Count(e => e.eventID == id) > 0;
        }
    }
}