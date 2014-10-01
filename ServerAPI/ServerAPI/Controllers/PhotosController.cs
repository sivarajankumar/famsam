using ServerAPI.CF_Models;
using ServerAPI.DAO;
using ServerAPI.DTO;
using ServerAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ServerAPI.Controllers
{
    public class PhotosController : ApiController
    {
        
        [HttpGet]
        public IHttpActionResult ListAllPhoto(int page, int size)
        {
            ApiResult result;
            List<PhotoDTO> photoDTOs = PhotoDAO.ListPhotos(1, page, size);
            result = new ApiResult("123", "Photos Result", photoDTOs);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult AddPhoto(HttpRequestMessage request, [FromBody] PhotoDTO photoDTO)
        {
            string authorization = request.Headers.GetValues("Authorization").FirstOrDefault();
            if (authorization == null)
            {
                return Unauthorized(new AuthenticationHeaderValue("authorization scheme"));
            }

            string token = Base64Utils.Base64Decode(authorization.Replace("Base ", ""));
            using (var db = new CF_FamsamEntities())
            {
                Session session = db.Session.Find(token);
                if (session == null) return Unauthorized(new AuthenticationHeaderValue("authorization scheme"));

                if (session.expired.Millisecond < DateTime.Now.Millisecond)
                {
                    //session expired
                    db.Session.Remove(session);
                    db.SaveChanges();
                    return Unauthorized(new AuthenticationHeaderValue("authorization scheme"));
                }
                else
                {
                    //session available. create new photo
                    //create general post
                    
                }


            }
        }
    }
}
