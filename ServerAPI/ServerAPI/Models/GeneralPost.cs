using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace famsam.serverapi.Models
{
    
    public abstract class GeneralPost
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<Tag> Tags { get; set; }
        public User Author { get; set; }
    }

    public class Tag
    {
        [Key]
        public string name { get; set; }

        public List<GeneralPost> GeneralPost { get; set; }
    }

    public class Story : GeneralPost
    {
        public string Title { get; set; }
        public string Privacy { get; set; }
        public List<Album> Albums { get; set; }

        public static string PUBLIC_PRIVACY = "PUBLIC";
        public static string FAMILY_ONLY = "FAMILY ONLY";
        public static string NEIGHBOR_ONLY = "NEIGHBOR ONLY";
    }

    public class Album : GeneralPost
    {
        public string Title { get; set; }
        public List<Story> Stories { get; set; }
        public List<Photo> Photos { get; set; }
    }

    public class Photo : GeneralPost
    {
        public string Url { get; set; }
        public string BadQuality { get; set; }
        public List<Album> Albums { get; set; }
    }
}