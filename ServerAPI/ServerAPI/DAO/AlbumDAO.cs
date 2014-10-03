using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServerAPI.CF_Models;
using ServerAPI.DTO;
using System.Diagnostics;
using System.Data.Entity;

namespace ServerAPI.DAO
{
    public class AlbumDAO
    {
        private const string ALBUM_POST_TYPE = "Album";

        // list all albums of a user
        public static List<AlbumDTO> ListAlbums(long userId, int page, int size)
        {
            using (var context = new CF_FamsamEntities())
            {
                var user = context.User.FirstOrDefault(u => u.id == userId);
                var generalPost = user.CreatedPost.Where(c => c.postType.Equals(ALBUM_POST_TYPE)).Skip((page -1) * size).Take(size).ToList();
                AlbumDTO albumInstance = new AlbumDTO();
                List<AlbumDTO> listAlbum = new List<AlbumDTO>();
                foreach (var item in generalPost)
                {
                    albumInstance.Id = item.Id;
                    albumInstance.Title = item.Album.title;
                    albumInstance.Description = item.description;
                    albumInstance.LastUpdate = item.lastUpdate;
                    albumInstance.PostType = item.postType;
                    albumInstance.AuthorFirstname = item.CreateUser.firstname;
                    albumInstance.AuthorLastname = item.CreateUser.lastname;
                    albumInstance.AuthorEmail = item.CreateUser.email;

                    listAlbum.Add(albumInstance);
                }

                return (listAlbum.Count() > 0) ? listAlbum : new List<AlbumDTO>(0);
            }
        }

        // list albums by story of a user
        public static List<AlbumDTO> ListAlbumsByStory(int page, int size, long storyId)
        {
            using (var context = new CF_FamsamEntities())
            {
                var generalPost = context.GeneralPost.FirstOrDefault(g => g.Id == storyId);
                var listAlbum = generalPost.Story.Album.Skip((page -1) * size).Take(size).ToList();
                AlbumDTO albumInstance = new AlbumDTO();
                List<AlbumDTO> listAlbumDTO = new List<AlbumDTO>();
                foreach (var item in listAlbum)
                {
                    albumInstance.Id = item.id;
                    albumInstance.Title = item.title;
                    albumInstance.Description = item.Post.description;
                    albumInstance.LastUpdate = item.Post.lastUpdate;
                    albumInstance.PostType = item.Post.postType;
                    albumInstance.AuthorFirstname = item.Post.CreateUser.firstname;
                    albumInstance.AuthorLastname = item.Post.CreateUser.lastname;
                    albumInstance.AuthorEmail = item.Post.CreateUser.email;

                    listAlbumDTO.Add(albumInstance);
                }

                return (listAlbumDTO.Count() > 0) ? listAlbumDTO : new List<AlbumDTO>(0);
            }
        }

        // get album by albumid
        public static AlbumDTO GetAlbumById(long albumId)
        {
            using (var context = new CF_FamsamEntities())
            {
                var generalPost = context.GeneralPost.FirstOrDefault(g => g.Id == albumId);
                AlbumDTO albumInstance = new AlbumDTO();

                albumInstance.Id = generalPost.Id;
                albumInstance.Title = generalPost.Album.title;
                albumInstance.Description = generalPost.description;
                albumInstance.LastUpdate = generalPost.lastUpdate;
                albumInstance.PostType = generalPost.postType;
                albumInstance.AuthorFirstname = generalPost.CreateUser.firstname;
                albumInstance.AuthorLastname = generalPost.CreateUser.lastname;
                albumInstance.AuthorEmail = generalPost.CreateUser.email;
                //albumInstance.ListPhoto = generalPost.Album.Photo;

                return albumInstance;
            }
        }

        // current user create new album
        // return -1 if fail
        // return 0 if success
        public static int CreateAlbum(AlbumDTO albumNew)
        {
            using (var context = new CF_FamsamEntities())
            {
                if (albumNew == null) return -1;
                User user = context.User.FirstOrDefault(u => u.email.Equals(albumNew.AuthorEmail));
                if (user == null) return -1;
                GeneralPost post = new GeneralPost();
                post.Id = DateTime.Now.Millisecond;
                post.description = albumNew.Description;
                post.lastUpdate = DateTime.Now;
                post.postType = ALBUM_POST_TYPE;
                post.createUserId = user.id;
                Album album = new Album();
                album.id = post.Id;
                album.title = albumNew.Title;
                try
                {
                    foreach (var photo in albumNew.ListPhoto)
                    {
                        album.Photo.Add(context.Photo.FirstOrDefault(p => p.id == photo.Id));
                    }
                    context.GeneralPost.Add(post);
                    context.Album.Add(album);
                    context.SaveChanges();
                } catch (Exception ex)
                {
                    Debug.WriteLine("Exception: " + ex.StackTrace);
                    return -1;
                }
                return 0;
            }
        }

        // current user update album description
        // return -1 if fail
        // return 0 if success
        public static int EditAlbumDescription(AlbumDTO albumEdit)
        {
            using (var context = new CF_FamsamEntities()) 
            {
                var post = context.GeneralPost.FirstOrDefault(p => p.Id == albumEdit.Id);
                post.description = albumEdit.Description;
                post.lastUpdate = DateTime.Now;
                try
                {
                    context.Entry<GeneralPost>(post).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception: " + ex.StackTrace);
                    return -1;
                }
                return 0;
            }
        }

        // current user update album title
        // return -1 if fail
        // return 0 if success
        public static int EditAlbumTitle(AlbumDTO albumEdit)
        {
            using (var context = new CF_FamsamEntities())
            {
                var post = context.GeneralPost.FirstOrDefault(p => p.Id == albumEdit.Id);
                post.lastUpdate = DateTime.Now;
                var album = context.Album.FirstOrDefault(a => a.id == albumEdit.Id);
                album.title = albumEdit.Title;
                try
                {
                    context.Entry<Album>(album).State = EntityState.Modified;
                    context.Entry<GeneralPost>(post).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception: " + ex.StackTrace);
                    return -1;
                }
                return 0;
            }
        }

        // current user update album title
        // return -1 if fail
        // return 0 if success
        public static int EditAlbum(AlbumDTO albumEdit, List<PhotoDTO> listPhotoAdd, List<PhotoDTO> listPhotoRemove)
        {
            using (var context = new CF_FamsamEntities())
            {
                var post = context.GeneralPost.FirstOrDefault(p => p.Id == albumEdit.Id);
                var album = post.Album;
                post.lastUpdate = DateTime.Now;
                album.title = albumEdit.Title;
                // Nhut chua lam 0 diem perfomance =))
                // add new list of photos to album
                // remove list of photos from album
                try
                {
                    context.Entry<Album>(album).State = EntityState.Modified;
                    context.Entry<GeneralPost>(post).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception: " + ex.StackTrace);
                    return -1;
                }
                return 0;
            }
        }

        // current user remove an album
        // return -1 if fail
        // return 0 if success
        public static int RemoveAlbum(long albumId, bool agreeToRemove)
        {
            using (var context = new CF_FamsamEntities())
            {
                var album = context.Album.FirstOrDefault(a => a.id == albumId);
                if (agreeToRemove)
                {
                    try
                    {
                        var listPhoto = album.Photo;
                        album.Photo.Clear();
                        foreach(var photo in listPhoto) {
                            context.Photo.Remove(photo);
                            context.GeneralPost.Remove(photo.Post);
                        }
                        context.Album.Remove(album);
                        context.GeneralPost.Remove(album.Post);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception: " + ex.StackTrace);
                        return -1;
                    }
                }
                else
                {
                    try
                    {
                        album.Photo.Clear();
                        context.Album.Remove(album);
                        context.GeneralPost.Remove(album.Post);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception: " + ex.StackTrace);
                        return -1;
                    }
                }
                return 0;
            }
        }
    }
}