using ServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//cai dbs day ha
//m xai ef hay cai j 
//giobf cai nay ne

namespace ServerAPI.DTO
{
    public class PhotoDTO
    {
        public int Id { get; set; }
        public long AuthorId { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string AuthorEmail { get; set; }
        public string AlbumId { get; set; }
        public Nullable<DateTime> LastUpdate { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public string BadQuality { get; set; }
        public string[] tags { get; set; }
    }

    public class PhotoApiResult
    {
        public static ApiResult ListPhotos { get; private set; }
        public static ApiResult PhotoNotFound { get; private set; }
        public static ApiResult PhotoResult { get; private set; }
        public static ApiResult UploadPhotoSuccess { get; private set; }
        public static ApiResult AddPhotoSuccess { get; private set; }
        public static ApiResult AddPhotoFail { get; private set; }
        static PhotoApiResult()
        {
            ListPhotos = new ApiResult("006", "photos result");
            PhotoNotFound = new ApiResult("007", "photo not found");
            PhotoResult = new ApiResult("008", "photo result");
            UploadPhotoSuccess = new ApiResult("009", "upload photo success");
            AddPhotoSuccess = new ApiResult("010", "Add Photo Success");
            AddPhotoFail = new ApiResult("011", "Add Photo Fail");
        }
    }
}