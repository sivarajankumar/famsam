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
using System.Web;

namespace ServerAPI.Controllers
{
    public class AlbumsController : ApiController
    {
        private FamsamDB db = new FamsamDB();

        // GET: api/Albums
        public IHttpActionResult GetGeneralPost()
        {
            string input = "";
            var session = HttpContext.Current.Session;
            if (session != null)
            {
                if (session["Time"] == null)
                    session["Time"] = DateTime.Now;
                return Ok("Session Time: " + session["Time"] + input);
            }
            return Ok("Session is not availabe" + input);
            //IQueryable<Album> query = from a in db.GeneralPost.OfType<Album>()
            //                                select a;
            //return Ok(query.ToArray());
        }

        // GET: api/Albums/5
        [ResponseType(typeof(Album))]
        public IHttpActionResult GetAlbum(long id)
        {
            
            Album album = (Album)db.GeneralPost.Find(id);
            IQueryable<Album> query = from a in db.GeneralPost.OfType<Album>()
                                            select a;
            if (album == null)
            {
                return NotFound();
            }

            return Ok(album);
        }

        // PUT: api/Albums/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAlbum(long id, Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != album.Id)
            {
                return BadRequest();
            }

            db.Entry(album).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
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

        // POST: api/Albums
        [ResponseType(typeof(Album))]
        public IHttpActionResult PostAlbum(Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GeneralPost.Add(album);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = album.Id }, album);
        }

        // DELETE: api/Albums/5
        [ResponseType(typeof(Album))]
        public IHttpActionResult DeleteAlbum(long id)
        {
            Album album = (Album)db.GeneralPost.Find(id);
            if (album == null)
            {
                return NotFound();
            }

            db.GeneralPost.Remove(album);
            db.SaveChanges();

            return Ok(album);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlbumExists(long id)
        {
            return db.GeneralPost.Count(e => e.Id == id) > 0;
        }
    }
}