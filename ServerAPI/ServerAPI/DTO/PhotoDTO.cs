using ServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.DTO
{
    public class PhotoDTO
    {
        public long Id { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public string BadQuality { get; set; }
    }

    public class PhotoApiResult
    {
        public static ApiResult ListPhotos { get; private set; }
        public static ApiResult PhotoNotFound { get; private set; }
        public static ApiResult PhotoResult { get; private set; }
        public static ApiResult UploadPhotoSuccess { get; private set; }

        static PhotoApiResult()
        {
            ListPhotos = new ApiResult("006", "photos result");
            PhotoNotFound = new ApiResult("007", "photo not found");
            PhotoResult = new ApiResult("008", "photo result");
            UploadPhotoSuccess = new ApiResult("009", "upload photo success");
        }
    }
}