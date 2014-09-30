using ServerAPI.DTO;
using ServerAPI.CF_Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ServerAPI.DAO
{
    public class PhotoDAO
    {
        public const string PHOTO_POST_TYPE = "Photo";
        /// <summary>
        /// List photos
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns>List of photos (0 item at least)</returns>
        public static List<PhotoDTO> ListPhotos(int userId, int page, int size)
        {
            List<GeneralPost> photos = new List<GeneralPost>();
            using (var db = new CF_FamsamEntities())
            {
                var photoQuery = from p in db.GeneralPost
                                 where p.postType == PHOTO_POST_TYPE
                                 orderby p.lastUpdate descending
                                 select p;
                photos = photoQuery.Skip((page - 1) * size)
                                   .Take(size)
                                   .ToList<GeneralPost>();
            }
            List<PhotoDTO> result = new List<PhotoDTO>();
            //to dto
            foreach (GeneralPost photo in photos)
            {
                PhotoDTO photoDTO = new PhotoDTO();
                photoDTO.Id = photo.Id;
                photoDTO.AuthorFirstName = photo.CreateUser.firstname;
                photoDTO.AuthorLastName = photo.CreateUser.lastname;
                photoDTO.LastUpdate = photo.lastUpdate;
                photoDTO.Description = photo.description;
                photoDTO.ImageURL = photo.Photo.url;
                photoDTO.BadQuality = photo.Photo.badQuality;
                result.Add(photoDTO);
            }

            return result;
        }

        public static int CreatePhoto(PhotoDTO photoDTO)
        {
            //get user
            User user;
            using (var db = new CF_FamsamEntities())
            {

                var userQuery = from u in db.User
                                where u.email == photoDTO.AuthorEmail
                                select u;
                user = userQuery.FirstOrDefault();
                if (user == null)
                {
                    return -1;
                }

                //create photo and post
                DateTime lastUpdate = DateTime.Now;
                Photo photo = new Photo();
                photo.id = lastUpdate.Millisecond;
                photo.url = photoDTO.ImageURL;
                photo.badQuality = photoDTO.BadQuality;
                GeneralPost post = new GeneralPost();
                post.Id = photo.id;
                post.postType = PHOTO_POST_TYPE;
                post.Photo = photo;
                post.createUserId = user.id;
                post.CreateUser = user;
                post.lastUpdate = lastUpdate;

                foreach (string tagname in photoDTO.tags)
                {
                    Tag tag = db.Tag.Find(tagname);
                    if (tag == null)
                    {
                        tag = new Tag();
                        tag.name = tagname;
                        db.Tag.Add(tag);
                    }
                    post.Tag.Add(tag);
                }
                try { db.GeneralPost.Add(post); }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception on create photo: " + ex.ToString());
                }

                return 1;
            }

        }

        /// <summary>
        /// Update photo
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        public static int EditDescription(PhotoDTO photoDTO)
        {


            using (var db = new CF_FamsamEntities())
            {
                Photo photo = db.Photo.Find(photoDTO.Id);
                if (photo == null) return -1;

                //update photo
                DateTime lastUpdate = DateTime.Now;
                photo.Post.lastUpdate = lastUpdate;
                photo.Post.description = photoDTO.Description;
                try
                {
                    db.Entry(photo).State = EntityState.Modified;
                    db.SaveChanges();
                    return 1;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception on Edit photo description:" + ex.ToString());
                }
            }
            return 0;
        }

        public static void DeletePhoto(int photoId)
        {
            using (var db = new CF_FamsamEntities())
            {
                try
                {
                    Photo photo = db.Photo.Find(photoId);
                    if (photo != null) db.Photo.Remove(photo);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("exception on delete photo:" + ex);
                }
            }

        }


        internal static void ListPhotos(string p, int page, int size)
        {
            throw new NotImplementedException();
        }
    }


}