using famsam.serverapi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ServerAPI.Models
{
    public class FamsamDB : DbContext
    {
        //Family
        public virtual DbSet<Family> Family { get; set; }
        public virtual DbSet<FamilyRole> FamilyRole { get; set; }
        public virtual DbSet<NeighborRequest> NeighborRequest { get; set; }

        //Post
        public virtual DbSet<GeneralPost> GeneralPost { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        /*public virtual DbSet<Story> Story { get; set; }
        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
         * */

        //User
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        //User Activity
        public virtual DbSet<Sharing> Sharing { get; set; }
        public virtual DbSet<Like> Like {get;set;}
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<FamilyFollowing> FamilyFollowing { get; set; }

        //Session
        public virtual DbSet<Session> Session { get; set; }
    }
}