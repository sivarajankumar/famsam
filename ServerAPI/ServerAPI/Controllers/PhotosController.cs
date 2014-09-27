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
using ServerAPI.DTO;
using System.Web;
using System.IO;

namespace ServerAPI.Controllers
{
    public class PhotosController : ApiController
    {
        private FamsamDB db = new FamsamDB();

        // GET: api/Photos
        public IHttpActionResult GetGeneralPost(int page, int size)
        {
            ApiResult result;

            var photoQuery = from p in db.GeneralPost.OfType<Photo>()
                             orderby p.LastUpdate descending
                             select p;
            Photo[] photosResult = photoQuery
                                    .Skip((page-1)*size).Take(size)
                                    .ToArray<Photo>();

            result = PhotoApiResult.ListPhotos;
            result.Content = photosResult;
            return Ok(result);
        }

        // GET: api/Photos/5
        [ResponseType(typeof(Photo))]
        public IHttpActionResult GetPhoto(long id)
        {
            ApiResult result;

            //TODO - authorize

            var post =db.GeneralPost.Find(id);

            if (post == null || !(post is Photo))
            {
                result = PhotoApiResult.PhotoNotFound;
                return Ok(result);
            }
            else
            {
                Photo photo = (Photo)post;

                //add to PhotoDTO
                PhotoDTO photoDTO = new PhotoDTO();
                photoDTO.Id = photo.Id;
                photoDTO.AuthorName = photo.Author.Firstname + photo.Author.Lastname;
                photoDTO.AuthorEmail = photo.Author.Email;
                photoDTO.LastUpdate = photo.LastUpdate;
                photoDTO.Description = photo.Description;
                photoDTO.ImageURL = photo.Url;
                photoDTO.BadQuality = photo.BadQuality;

                result = PhotoApiResult.PhotoResult;
                result.Content = photoDTO;
                return Ok(result);
            }

           
        }

        // TODO - PUT: api/Photos/5
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

        // TODO - POST: api/Photos
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
            ApiResult result;

            // TODO - authorize user

            var post = db.GeneralPost.Find(id);
            if (post == null || !(post is Photo))
            {
                result = PhotoApiResult.PhotoNotFound;
                return Ok(result);
            }
            else
            {
                Photo photo = (Photo)post;
                db.GeneralPost.Remove(photo);
                db.SaveChanges();

                return Ok();
            }

            
        }

        [HttpPost]
        [ActionName("upload-photos")]
        public IHttpActionResult UploadPhoto(long id)
        {
            ApiResult result;

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Get the uploaded image from the Files collection
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];

                if (httpPostedFile != null)
                {
                    // TODO - Validate the uploaded image(optional)

                    // Get the complete file path
                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), httpPostedFile.FileName);

                    // Save the uploaded file to "UploadedFiles" folder
                    httpPostedFile.SaveAs(fileSavePath);

                    result = PhotoApiResult.UploadPhotoSuccess;
                    result.Content = fileSavePath;
                    return Ok(result);
                }
            }

            return BadRequest();
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