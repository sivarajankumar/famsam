using ServerAPI.DAO;
using ServerAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    }
}
