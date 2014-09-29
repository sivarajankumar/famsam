using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.DTO
{
    public class AlbumDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<DateTime> LastUpdate { get; set; }
        public string PostType { get; set; }
        public string AuthorFirstname { get; set; }
        public string AuthorLastname { get; set; }
        public string AuthorEmail { get; set; }
        public List<PhotoDTO> ListPhoto { get; set; }
    }

    public class AlbumApiResult
    {
        public static ApiResult ListAlbums { get; private set; }
        public static ApiResult AlbumNotFound { get; private set; }
        public static ApiResult AlbumResult { get; private set; }
        public static ApiResult UploadAlbumSuccess { get; private set; }

        static AlbumApiResult()
        {
            ListAlbums = new ApiResult("010", "albums result");
            AlbumNotFound = new ApiResult("011", "album not found");
            AlbumResult = new ApiResult("012", "album result");
            UploadAlbumSuccess = new ApiResult("013", "upload album success");
        }
    }
}