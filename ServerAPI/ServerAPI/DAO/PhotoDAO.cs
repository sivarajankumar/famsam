using ServerAPI.DTO;
using ServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAPI.DAO
{
    public class PhotoDAO
    {
        /// <summary>
        /// List photos
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns>List of photos (0 item at least)</returns>
        public static List<PhotoDTO> ListPhotos(int page, int size)
        {
            List<GeneralPost> photos = new List<GeneralPost>();
            using (var db = new FamsamEntities())
            {
                var photoQuery = from p in db.GeneralPost
                                 where p.postType == "Photo"
                                 orderby p.lastUpdate descending
                                 select p;
                photos = photoQuery
                                            .Skip((page - 1) * size)
                                            .Take(size)
                                            .ToList<GeneralPost>();
            }
            List<PhotoDTO> result = new List<PhotoDTO>();
            //to dto
            foreach (GeneralPost photo in photos)
            {
                PhotoDTO photoDTO = new PhotoDTO();
                photoDTO.Id = photo.id;
                photoDTO.AuthorFirstName = photo.User.firstname;
                photoDTO.AuthorLastName = photo.User.lastname;
                photoDTO.LastUpdate = photo.lastUpdate;
                photoDTO.Description = photo.description;
                photoDTO.ImageURL = photo.Photo.url;
                photoDTO.BadQuality = photo.Photo.badQuality;
                result.Add(photoDTO);
            }

            return result;
        }


    }


}