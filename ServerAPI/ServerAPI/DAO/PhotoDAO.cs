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
        /// <summary>
        /// List all photos
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns>List of photos (0 item at least)</returns>
        public static List<PhotoDTO> ListCurrentPhotos(long userId, int page, int size)
        {
            List<GeneralPost> photos = new List<GeneralPost>();
            List<PhotoDTO> result = new List<PhotoDTO>();
            using (var db = new CF_FamsamEntities())
            {
                var photoQuery = from p in db.GeneralPost
                                 where p.postType == GeneralPost.PHOTO_POST_TYPE
                                 orderby p.lastUpdate descending
                                 select p;
                    photos = photoQuery.Skip((page - 1) * size)
                                   .Take(size)
                                   .ToList<GeneralPost>();

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
            }
            return result;
        }

        public static List<PhotoDTO> ListOtherUserPhotos(long currentUserId, long otherUserId,long storyId, long albumId, int page, int size)
        {
            List<PhotoDTO> photoDTOs = new List<PhotoDTO>();
            using (var db = new CF_FamsamEntities())
            {
                //query album

                Album album = db.Story.Find(storyId).Album.Where<Album>(a => a.id == albumId).First();
                if (album != null)
                {
                    foreach (var photo in album.Photo)
                    {
                        PhotoDTO photoDTO = new PhotoDTO();
                        photoDTO.Id = photo.id;
                        photoDTO.AuthorFirstName = photo.Post.CreateUser.firstname;
                        photoDTO.AuthorLastName = photo.Post.CreateUser.lastname;
                        photoDTO.LastUpdate = photo.Post.lastUpdate;
                        photoDTO.Description = photo.Post.description;
                        photoDTO.ImageURL = photo.url;
                        photoDTO.BadQuality = photo.badQuality;
                        photoDTOs.Add(photoDTO);
                    }
                }
            }
            return photoDTOs;
        }

        /// <summary>
        /// List all photos by album
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="albumId"></param>
        /// <returns>List of photos (0 item at least)</returns>
        public static List<PhotoDTO> ListPhotosByAlbum(long userId, int page, int size, long albumId)
        {
            List<GeneralPost> photos = new List<GeneralPost>();
            List<PhotoDTO> result = new List<PhotoDTO>();
            using (var db = new CF_FamsamEntities())
            {
                var albumQuery = from p in db.GeneralPost
                                 where p.postType == GeneralPost.ALBUM_POST_TYPE && p.Id == albumId
                                 orderby p.lastUpdate descending
                                 select p;
                GeneralPost post = albumQuery.FirstOrDefault<GeneralPost>();
                if (post == null) return result;
                
                //to dto
                foreach (Photo photo in post.Album.Photo)
                {
                    PhotoDTO photoDTO = new PhotoDTO();
                    GeneralPost photoPost = photo.Post;
                    photoDTO.Id = post.Id;
                    photoDTO.AuthorFirstName = photoPost.CreateUser.firstname;
                    photoDTO.AuthorLastName = photoPost.CreateUser.lastname;
                    photoDTO.LastUpdate = photoPost.lastUpdate;
                    photoDTO.Description = photoPost.description;
                    photoDTO.ImageURL = photo.url;
                    photoDTO.BadQuality = photo.badQuality;
                    result.Add(photoDTO);
                }
            }
            return result;
        }

        /// <summary>
        /// Create a Photo entity of non-album.
        /// </summary>
        /// <param name="photoDTO"></param>
        /// <returns>-1 if user not found</returns>
        public static int AddPhoto(PhotoDTO photoDTO)
        {
            //get user
            User user;
            using (var db = new CF_FamsamEntities())
            {
                user = db.User.Find(photoDTO.AuthorId);
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
                post.postType = GeneralPost.PHOTO_POST_TYPE;
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
        public static PhotoDTO GetPhoto(long photoId)
        {
            using (var db = new CF_FamsamEntities())
            {
                Photo photo = db.Photo.Find(photoId);
                PhotoDTO photoDTO = new PhotoDTO();
                photoDTO.Id = photo.id;
                GeneralPost post = photo.Post;
                User user = post.CreateUser;
                photoDTO.AuthorId = user.id;
                photoDTO.AuthorEmail = user.email;
                photoDTO.LastUpdate = post.lastUpdate;
                photoDTO.Description = post.description;
                photoDTO.ImageURL = photo.url;
                photoDTO.BadQuality = photo.badQuality;
                photoDTO.tags = post.GetTagNameArray();
                return photoDTO;
            }
        }

        public static void DeletePhoto(long photoId)
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
    }


}