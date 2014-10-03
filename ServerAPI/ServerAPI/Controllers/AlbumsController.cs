using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServerAPI.CF_Models;
using ServerAPI.DTO;
using ServerAPI.DAO;

namespace ServerAPI.Controllers
{
    public class AlbumsController : ApiController
    {
        [HttpGet]
        [ActionName("listalbum")]
        public IHttpActionResult GetAllAlbums(long userId, int page, int size)
        {
            ApiResult result;
            List<AlbumDTO> listAlbum = AlbumDAO.ListAlbumsOfCurrentUser(userId, page, size);
            result = AlbumApiResult.ListAlbums;
            result.Content = listAlbum;
            return Ok(result);
        }

        [HttpGet]
        [ActionName("listalbumbystory")]
        public IHttpActionResult GetListAlbumsByStory(long userId, int page, int size, long storyId)
        {
            ApiResult result;
            List<AlbumDTO> listAlbum = AlbumDAO.ListAlbumsByStory(userId, page, size, storyId);
            result = AlbumApiResult.ListAlbums;
            result.Content = listAlbum;
            return Ok(result);
        }

        [HttpGet]
        [ActionName("getalbumbyid")]
        public IHttpActionResult GetAlbumById(long userId, int page, int size, long albumId)
        {
            ApiResult result;
            AlbumDTO album = AlbumDAO.GetAlbumById(userId, page, size, albumId);
            if (album == null)
            {
                result = AlbumApiResult.AlbumNotFound;
            }
            else
            {
                result = AlbumApiResult.AlbumResult;
                result.Content = album;
            }
            return Ok(result);
        }

        [HttpPost]
        [ActionName("addnewalbum")]
        public IHttpActionResult AddAlbum(AlbumDTO albumNew)
        {
            ApiResult result;
            int resultInt = AlbumDAO.CreateAlbum(albumNew);
            if (resultInt == -1)
            {
                result = AlbumApiResult.UploadAlbumFail;
            }
            else
            {
                result = AlbumApiResult.UploadAlbumSuccess;
            }
            return Ok(result);
        }

        [HttpPost]
        [ActionName("editalbumdescription")]
        public IHttpActionResult EditAlbumDescription(AlbumDTO albumEdit)
        {
            ApiResult result;
            int resultInt = AlbumDAO.EditAlbumDescription(albumEdit);
            if (resultInt == -1)
            {
                result = AlbumApiResult.UpdateAlbumFail;
            }
            else
            {
                result = AlbumApiResult.UpdateAlbumSuccess;
            }
            return Ok(result);
        }

        [HttpPost]
        [ActionName("editalbumtitle")]
        public IHttpActionResult EditAlbumTitle(AlbumDTO albumEdit)
        {
            ApiResult result;
            int resultInt = AlbumDAO.EditAlbumTitle(albumEdit);
            if (resultInt == -1)
            {
                result = AlbumApiResult.UpdateAlbumFail;
            }
            else
            {
                result = AlbumApiResult.UpdateAlbumSuccess;
            }
            return Ok(result);
        }

        [HttpPost]
        [ActionName("editalbum")]
        public IHttpActionResult EditAlbum(AlbumDTO albumEdit, List<PhotoDTO> listPhotoAdd, List<PhotoDTO> listPhotoRemove)
        {
            ApiResult result;
            int resultInt = AlbumDAO.EditAlbum(albumEdit, listPhotoAdd, listPhotoRemove);
            if (resultInt == -1)
            {
                result = AlbumApiResult.UpdateAlbumFail;
            }
            else
            {
                result = AlbumApiResult.UpdateAlbumSuccess;
            }
            return Ok(result);
        }

        [HttpPost]
        [ActionName("removealbum")]
        public IHttpActionResult RemoveAlbum(int albumId, bool agreeToRemove)
        {
            ApiResult result;
            int resultInt = AlbumDAO.RemoveAlbum(albumId, agreeToRemove);
            if (resultInt == -1)
            {
                result = AlbumApiResult.RemoveAlbumFail;
            }
            else
            {
                result = AlbumApiResult.RemoveAlbumSuccess;
            }
            return Ok(result);
        }
    }
}
