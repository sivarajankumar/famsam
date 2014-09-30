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
using ServerAPI.CF_Models;
using System.Diagnostics;

namespace ServerAPI.Controllers
{
    public class Photos1Controller : ApiController
    {
        private CF_FamsamEntities db = new CF_FamsamEntities();

        // GET: api/Photos1
        public IQueryable<Photo> GetPhoto()
        {
            return db.Photo;
        }

        // GET: api/Photos1/5
        [ResponseType(typeof(Photo))]
        public IHttpActionResult GetPhoto(int id)
        {
            Photo photo = db.Photo.Find(id);
            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }

        // PUT: api/Photos1/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPhoto(int id, Photo photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != photo.id)
            {
                return BadRequest();
            }

            db.Entry(photo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(id))
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

        // POST: api/Photos1
        [ResponseType(typeof(Photo))]
        public IHttpActionResult PostPhoto(Photo photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            db.Photo.Add(photo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PhotoExists(photo.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = photo.id }, photo);
        }

        // DELETE: api/Photos1/5
        [ResponseType(typeof(Photo))]
        [ActionName("delete")]
        public IHttpActionResult DeletePhoto(int id)
        {
            Photo photo = db.Photo.Find(id);
            if (photo == null)
            {
                Debug.WriteLine("Delete photo_____________________________________");
                return NotFound();
            }

            db.Photo.Remove(photo);
            db.SaveChanges();

            return Ok(photo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhotoExists(int id)
        {
            return db.Photo.Count(e => e.id == id) > 0;
        }
    }
}