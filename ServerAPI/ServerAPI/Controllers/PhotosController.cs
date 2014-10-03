using ServerAPI.CF_Models;
using ServerAPI.DAO;
using ServerAPI.DTO;
using ServerAPI.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        [ActionName("list")]
        public IHttpActionResult ListAllPhoto(int page = 1, int size = 10)
        {
            ApiResult result;
            List<PhotoDTO> photoDTOs = PhotoDAO.ListPhotosByAlbum(1, page, size, 0);
            result = new ApiResult("123", "Photos Result", photoDTOs);
            return Ok(result);
        }

        [HttpGet]
        [ActionName("listByAlbum")]
        public IHttpActionResult ListPhotoByAlbum(int page = 1, int size = 10, int albumId = 0)
        {
            ApiResult result;
            List<PhotoDTO> photoDTOs = PhotoDAO.ListPhotosByAlbum(1, page, size, albumId);
            result = new ApiResult("123", "Photos Result", photoDTOs);
            return Ok(result);
        }

        [HttpPost]
        [ActionName("add")]
        public HttpResponseMessage AddPhoto(HttpRequestMessage request, [FromBody] PhotoDTO photoDTO)
        {
            long aResult = SessionDAO.Authentication(request.Headers);
            switch (aResult)
            {
                case -401:
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                    
                case -403:
                    return Request.CreateResponse(HttpStatusCode.Forbidden);
                    
                default:
                    photoDTO.AuthorId = aResult; 
                    int cResult = PhotoDAO.AddPhoto(photoDTO);
                    if (cResult != -1)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, PhotoApiResult.AddPhotoSuccess);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, PhotoApiResult.AddPhotoFail);
                    }
            }
        }

        [HttpPut]
        [ActionName("editDescription")]
        public HttpResponseMessage EditDescription([FromBody] PhotoDTO photoDTO)
        {
            return Request.CreateResponse(HttpStatusCode.OK, PhotoApiResult.PhotoResult);
        }
    }
}
