using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServerAPI.CF_Models
{
    public class Tag
    {
        public Tag()
        {
            this.GeneralPost = new HashSet<GeneralPost>();
        }

        public string name { get; set; }

        public virtual ICollection<GeneralPost> GeneralPost { get; set; }
    }
    public class GeneralPost
    {
        public const string STORY_POST_TYPE = "Story";
        public const string ALBUM_POST_TYPE = "Album";
        public const string PHOTO_POST_TYPE = "Photo";
        public GeneralPost()
        {
            this.Comment = new HashSet<Comment>();
            this.Tag = new HashSet<Tag>();
        }

        public long Id { get; set; }
        public string description { get; set; }
        public System.DateTime lastUpdate { get; set; }
        [Required]
        public string postType { get; set; }
        public long createUserId { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<User> LikeUser { get; set; }
        public virtual User CreateUser { get; set; }
        public virtual ICollection<Tag> Tag { get; set; }
        

        public virtual Story Story { get; set; }
        public virtual Album Album { get; set; }
        public virtual Photo Photo { get; set; }
        public string[] GetTagNameArray()
        {
            int size = Tag.Count;
            List<String> tagNameList = new List<String>();
            foreach (Tag tag in Tag)
            {
                tagNameList.Add(tag.name);
            }
            return tagNameList.ToArray();
        }
    }
    public class Story
    {
        public const string PUBLIC_PRIVACY = "public";
        public const string FAMILY_ONLY_PRIVACY = "family_only";
        public const string NEIGHBOR_ONLY_PRIVACY = "neighbor_only";
        public Story()
        {
            this.Album = new HashSet<Album>();
        }
        public long id { get; set; }
        //foreign key
        public long familyId { get; set; }
        public string title { get; set; }
        public string privacy { get; set; }
        public virtual ICollection<Album> Album { get; set; }
        public virtual GeneralPost Post { get; set; }
        public virtual Family Family { get; set; }
    }
    public class Album
    {
        public Album()
        {
            this.Photo = new HashSet<Photo>();
            this.Story = new HashSet<Story>();
        }
        public long id { get; set; }
        public string title { get; set; }

        public virtual ICollection<Photo> Photo { get; set; }
        public virtual ICollection<Story> Story { get; set; }
        public virtual GeneralPost Post { get; set; }
    }
    public class Photo
    {
        public Photo()
        {
            this.Album = new HashSet<Album>();
        }
        public long id { get; set; }
        public string url { get; set; }
        public string badQuality { get; set; }
        public virtual ICollection<Album> Album { get; set; }
        public virtual GeneralPost Post { get; set; }
    }

}