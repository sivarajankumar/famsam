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

        // list all albums of current user
        public static List<AlbumDTO> ListAlbumsOfCurrentUser(long currentUserId, int page, int size)
        {
            using (var context = new CF_FamsamEntities())
            {
                var user = context.User.FirstOrDefault(u => u.id == currentUserId);
                var albumList = user.CreatedPost.Where(c => c.postType.Equals(GeneralPost.ALBUM_POST_TYPE)).Skip((page - 1) * size).Take(size).ToList();
                AlbumDTO albumInstance = new AlbumDTO();
                List<AlbumDTO> listAlbumDTO = new List<AlbumDTO>();
                foreach (var item in albumList)
                {
                    albumInstance.Id = item.Id;
                    albumInstance.Title = item.Album.title;
                    albumInstance.Description = item.description;
                    albumInstance.LastUpdate = item.lastUpdate;
                    albumInstance.AuthorFirstname = item.CreateUser.firstname;
                    albumInstance.AuthorLastname = item.CreateUser.lastname;
                    albumInstance.AuthorEmail = item.CreateUser.email;
                    albumInstance.ListPhoto = PhotoDAO.ListPhotosByAlbum(currentUserId, page, size, item.Id);

                    listAlbumDTO.Add(albumInstance);
                }

                return (listAlbumDTO.Count() > 0) ? listAlbumDTO : new List<AlbumDTO>(0);
            }
        }

        // list all albums of a user
        // kho qua assign cho Nhut
        public static List<AlbumDTO> ListAlbumsOfOtherUser(long currentUserId, long userId, int page, int size)
        {
            using (var context = new CF_FamsamEntities())
            {
                var currentUser = context.User.FirstOrDefault(u => u.id == currentUserId);
                var user = context.User.FirstOrDefault(u => u.id == userId);
                var storyPostList = user.CreatedPost.Where(c => c.postType.Equals(GeneralPost.STORY_POST_TYPE)).Skip((page -1) * size).Take(size).ToList();
                AlbumDTO albumInstance = new AlbumDTO();
                List<AlbumDTO> listAlbumDTO = new List<AlbumDTO>();
                foreach (var item in storyPostList)
                {
                    bool check = false;
                    if (item.Story.privacy.Equals(Story.PUBLIC_PRIVACY))
                    {
                        check = true;
                    }
                    if (item.Story.privacy.Equals(Story.FAMILY_ONLY_PRIVACY) && CheckUsersInFamily(currentUserId, userId, item.Story.familyId))
                    {
                        check = true;
                    }
                    //if (item.Story.privacy.Equals(Story.NEIGHBOR_ONLY_PRIVACY) && (CheckUsersInFamily(currentUserId, userId, item.Story.familyId)))
                    //albumInstance.Id = item.Id;
                    //albumInstance.Title = item.Album.title;
                    //albumInstance.Description = item.description;
                    //albumInstance.LastUpdate = item.lastUpdate;
                    //albumInstance.AuthorFirstname = item.CreateUser.firstname;
                    //albumInstance.AuthorLastname = item.CreateUser.lastname;
                    //albumInstance.AuthorEmail = item.CreateUser.email;
                    //albumInstance.ListPhoto = PhotoDAO.ListPhotosByAlbum(userId, page, size, item.Id);

                    listAlbumDTO.Add(albumInstance);
                }

                return (listAlbumDTO.Count() > 0) ? listAlbumDTO : new List<AlbumDTO>(0);
            }
        }

        // list albums by story of a user
        public static List<AlbumDTO> ListAlbumsByStory(long userId, int page, int size, long storyId)
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
                    albumInstance.AuthorFirstname = item.Post.CreateUser.firstname;
                    albumInstance.AuthorLastname = item.Post.CreateUser.lastname;
                    albumInstance.AuthorEmail = item.Post.CreateUser.email;
                    albumInstance.ListPhoto = PhotoDAO.ListPhotosByAlbum(userId, page, size, item.id);

                    listAlbumDTO.Add(albumInstance);
                }

                return (listAlbumDTO.Count() > 0) ? listAlbumDTO : new List<AlbumDTO>(0);
            }
        }

        // get album by albumid
        public static AlbumDTO GetAlbumById(long userId, int page, int size, long albumId)
        {
            using (var context = new CF_FamsamEntities())
            {
                var generalPost = context.GeneralPost.FirstOrDefault(g => g.Id == albumId);
                AlbumDTO albumInstance = new AlbumDTO();

                albumInstance.Id = generalPost.Id;
                albumInstance.Title = generalPost.Album.title;
                albumInstance.Description = generalPost.description;
                albumInstance.LastUpdate = generalPost.lastUpdate;
                albumInstance.AuthorFirstname = generalPost.CreateUser.firstname;
                albumInstance.AuthorLastname = generalPost.CreateUser.lastname;
                albumInstance.AuthorEmail = generalPost.CreateUser.email;
                albumInstance.ListPhoto = PhotoDAO.ListPhotosByAlbum(userId, page, size, generalPost.Id);

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
                post.description = albumEdit.Description;
                try
                {
                    // add new list of photos to album
                    if (listPhotoAdd.Count > 0)
                    {
                        foreach (var newPhoto in listPhotoAdd)
                        {
                            album.Photo.Add(context.Photo.FirstOrDefault(p => p.id == newPhoto.Id));
                        }
                    }
                    // remove list of photos from album
                    if (listPhotoRemove.Count > 0)
                    {
                        foreach (var removePhoto in listPhotoRemove)
                        {
                            album.Photo.Remove(context.Photo.FirstOrDefault(p => p.id == removePhoto.Id));
                        }
                    }
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

        // check if current user and target user are in one family.
        public static bool CheckUsersInFamily(long userId, long otherUserId, long familyId)
        {
            using (var context = new CF_FamsamEntities())
            {
                var userFamilyRole = context.FamilyRole.FirstOrDefault(fr => fr.userId == userId && fr.familyId == familyId);
                var otherUserFamilyRole = context.FamilyRole.FirstOrDefault(fr => fr.userId == otherUserId && fr.familyId == familyId);
                if (userFamilyRole != null && otherUserFamilyRole != null)
                {
                    return true;
                }
                return false;
            }
        }
    }
}