using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServerAPI.Models;
using ServerAPI.DTO;
using System.Diagnostics;
using System.Data.Entity;

namespace ServerAPI.DAO
{
    public class AlbumDAO
    {
        const string type = "Album";

        // list all albums of a user
        public List<AlbumDTO> ListAlbums(User user)
        {
            using (var context = new FamsamEntities())
            {
                var generalPost = user.GeneralPost.Where(g => g.postType.Equals(type)).ToList();
                AlbumDTO albumInstance = new AlbumDTO();
                List<AlbumDTO> listAlbum = new List<AlbumDTO>();
                foreach (var item in generalPost)
                {
                    albumInstance.Id = item.id;
                    albumInstance.Title = item.Album.title;
                    albumInstance.Description = item.description;
                    albumInstance.LastUpdate = item.lastUpdate;
                    albumInstance.PostType = item.postType;
                    albumInstance.AuthorFirstname = item.User.firstname;
                    albumInstance.AuthorLastname = item.User.lastname;
                    albumInstance.AuthorEmail = item.User.email;

                    listAlbum.Add(albumInstance);
                }

                return (listAlbum.Count() > 0) ? listAlbum : new List<AlbumDTO>(0);
            }
        }

        // current user create new album
        // return -1 if fail
        // return 0 if success
        public int CreateAlbum(AlbumDTO albumNew)
        {
            using (var context = new FamsamEntities())
            {
                if (albumNew == null) return -1;
                User user = context.User.FirstOrDefault(u => u.email.Equals(albumNew.AuthorEmail));
                if (user == null) return -1;
                GeneralPost post = new GeneralPost();
                post.id = DateTime.Now.Millisecond;
                post.description = albumNew.Description;
                post.lastUpdate = DateTime.Now;
                post.postType = type;
                post.author = user.id;
                Album album = new Album();
                album.id = post.id;
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
        public int EditAlbumDescription(AlbumDTO albumEdit)
        {
            using (var context = new FamsamEntities()) 
            {
                var post = context.GeneralPost.FirstOrDefault(p => p.id == albumEdit.Id);
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
        public int EditAlbumTitle(AlbumDTO albumEdit)
        {
            using (var context = new FamsamEntities())
            {
                var post = context.GeneralPost.FirstOrDefault(p => p.id == albumEdit.Id);
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

        // current user remove an album
        // return -1 if fail
        // return 0 if success
        public int RemoveAlbum(int albumId)
        {
            using (var context = new FamsamEntities())
            {
                var post = context.GeneralPost.FirstOrDefault(p => p.id == albumId);
                var album = context.GeneralPost.FirstOrDefault(a => a.id == albumId);
                return 0;
            }
        }
    }
}