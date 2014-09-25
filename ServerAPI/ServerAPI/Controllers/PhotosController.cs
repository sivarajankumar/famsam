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
using ServerAPI.Models;
using famsam.serverapi.Models;

namespace ServerAPI.Controllers
{
    public class PhotosController : ApiController
    {
        private FamsamDB db = new FamsamDB();

        // GET: api/Photos
        public IHttpActionResult GetGeneralPost(int page, int size)
        {
            return Ok(db.GeneralPost.ToList<GeneralPost>());
        }

        // GET: api/Photos/5
        [ResponseType(typeof(Photo))]
        public IHttpActionResult GetPhoto(long id)
        {
            Photo photo = (Photo)db.GeneralPost.Find(id);
            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }

        // PUT: api/Photos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPhoto(long id, Photo photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != photo.Id)
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

        // POST: api/Photos
        [ResponseType(typeof(Photo))]
        public IHttpActionResult PostPhoto(Photo photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GeneralPost.Add(photo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = photo.Id }, photo);
        }

        // DELETE: api/Photos/5
        [ResponseType(typeof(Photo))]
        public IHttpActionResult DeletePhoto(long id)
        {
            Photo photo = (Photo)db.GeneralPost.Find(id);
            if (photo == null)
            {
                return NotFound();
            }

            db.GeneralPost.Remove(photo);
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

        private bool PhotoExists(long id)
        {
            return db.GeneralPost.Count(e => e.Id == id) > 0;
        }
    }
}