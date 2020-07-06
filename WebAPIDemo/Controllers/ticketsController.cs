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
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    public class ticketsController : ApiController
    {
        private NGOEventsEntities db = new NGOEventsEntities();

        // GET: api/tickets
        public IQueryable<ticket> Gettickets()
        {
            return db.tickets;
        }

        // GET: api/tickets/5
        [ResponseType(typeof(ticket))]
        public IHttpActionResult Getticket(int id)
        {
            ticket ticket = db.tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        // PUT: api/tickets/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putticket(int id, ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticket.ticketID)
            {
                return BadRequest();
            }

            db.Entry(ticket).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ticketExists(id))
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

        // POST: api/tickets
        [ResponseType(typeof(ticket))]
        public IHttpActionResult Postticket(ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tickets.Add(ticket);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ticket.ticketID }, ticket);
        }

        // DELETE: api/tickets/5
        [ResponseType(typeof(ticket))]
        public IHttpActionResult Deleteticket(int id)
        {
            ticket ticket = db.tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }

            db.tickets.Remove(ticket);
            db.SaveChanges();

            return Ok(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ticketExists(int id)
        {
            return db.tickets.Count(e => e.ticketID == id) > 0;
        }
    }
}