using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.DTO
{
    public class AlbumDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<DateTime> LastUpdate { get; set; }
        public string PostType { get; set; }
        public string AuthorFirstname { get; set; }
        public string AuthorLastname { get; set; }
        public string AuthorEmail { get; set; }
        public ICollection<PhotoDTO> ListPhoto { get; set; }
    }

    public class AlbumApiResult
    {
        public static ApiResult ListAlbums { get; private set; }
        public static ApiResult AlbumNotFound { get; private set; }
        public static ApiResult AlbumResult { get; private set; }
        public static ApiResult UploadAlbumSuccess { get; private set; }
        public static ApiResult UploadAlbumFail { get; private set; }
        public static ApiResult UpdateAlbumSuccess { get; private set; }
        public static ApiResult UpdateAlbumFail { get; private set; }
        public static ApiResult RemoveAlbumSuccess { get; private set; }
        public static ApiResult RemoveAlbumFail { get; private set; }

        static AlbumApiResult()
        {
            ListAlbums = new ApiResult("010", "List Album Result");
            AlbumNotFound = new ApiResult("011", "Album Not Found");
            AlbumResult = new ApiResult("012", "Album Result");
            UploadAlbumSuccess = new ApiResult("013", "Upload Album Successful");
            UploadAlbumFail = new ApiResult("014", "Upload Album Fail");
            UpdateAlbumSuccess = new ApiResult("015", "Update Album Successful");
            UpdateAlbumFail = new ApiResult("016", "Update Album Fail");
            RemoveAlbumSuccess = new ApiResult("017", "Remove Album Successful");
            RemoveAlbumFail = new ApiResult("018", "Remove Album Fail");
        }
    }
}