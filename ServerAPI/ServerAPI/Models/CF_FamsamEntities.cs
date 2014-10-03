using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ServerAPI.CF_Models
{
    public class CF_FamsamEntities : DbContext
    {
        public DbSet<Family> Family { get; set; }
        public DbSet<FamilyRole> FamilyRole { get; set; }
        public DbSet<Neighborhood> Neighborhood { get; set; }
        public DbSet<NeighborRequest> NeighborRequest { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<GeneralPost> GeneralPost { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<Story> Story { get; set; }
        public DbSet<Album> Album { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<Sharing> Sharing { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //remove convention
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //----------------------User
            var user = modelBuilder.Entity<User>();
            user.HasKey(u => u.id);

            user.HasRequired(u => u.UserRole).WithMany().HasForeignKey(u => u.role);
            modelBuilder.Entity<UserRole>().HasKey(ur => ur.rolename);

            //----------------------Family
            //neighbor request
            var neighborRequest = modelBuilder.Entity<NeighborRequest>();
            neighborRequest.HasKey(n => new { n.familyId, n.userId });
            neighborRequest.HasRequired(n => n.User).WithOptional();
            neighborRequest.HasRequired(n => n.Family).WithOptional();
            //neighborhood
            var neighborhood = modelBuilder.Entity<Neighborhood>();
            neighborhood.HasKey(n => new { n.familyId, n.neighborId });
            neighborhood.HasRequired(f => f.Family).WithMany().HasForeignKey(n => n.familyId);
            neighborhood.HasRequired(f => f.Neighbor).WithMany().HasForeignKey(n => n.neighborId);
            //FamilyRole
            var familyRole = modelBuilder.Entity<FamilyRole>();
            familyRole.HasKey(fr => new { fr.familyId, fr.userId });
            familyRole.HasRequired(fr => fr.Family).WithMany(f => f.FamilyRole).HasForeignKey(fr => fr.familyId);
            familyRole.HasRequired(fr => fr.User).WithMany(u => u.FamilyRole).HasForeignKey(fr => fr.userId);
            //Followed by
            var family = modelBuilder.Entity<Family>();
            family.HasMany(f => f.User).WithMany(u => u.Family).Map(m =>
            {
                m.ToTable("Following");
                m.MapLeftKey("familyId");
                m.MapRightKey("userId");
            });
  
            //------------------------user activity
            //comment
            var comment = modelBuilder.Entity<Comment>();
            comment.HasKey(c => new { c.userId, c.postId });
            comment.HasRequired(c => c.User).WithMany().HasForeignKey(c => c.userId).WillCascadeOnDelete(false);
            comment.HasRequired(c => c.GeneralPost).WithMany(p => p.Comment).HasForeignKey(c => c.postId).WillCascadeOnDelete(false);
            //share
            var share = modelBuilder.Entity<Sharing>();
            share.HasKey(s => new {s.generalPostId, s.userId, s.sharedFamilyId});
            share.HasRequired(s => s.User).WithMany().HasForeignKey(s => s.userId).WillCascadeOnDelete(false);
            share.HasRequired(s => s.GeneralPost).WithMany().HasForeignKey(s => s.generalPostId).WillCascadeOnDelete(false);
            share.HasRequired(s => s.Family).WithMany().HasForeignKey(s => s.sharedFamilyId).WillCascadeOnDelete(false);
            //report
            var report = modelBuilder.Entity<Report>();
            report.HasKey(r => new { r.userId, r.photoId });
            report.HasRequired(r => r.User).WithMany().HasForeignKey(r => r.userId).WillCascadeOnDelete(false);
            report.HasRequired(r => r.Photo).WithMany().HasForeignKey(r => r.photoId).WillCascadeOnDelete(false);
            //like
            var generalPost = modelBuilder.Entity<GeneralPost>();
            generalPost.HasMany(p => p.LikeUser).WithMany(u => u.LikedPost).Map(m => 
            {
                m.ToTable("Likes");
                m.MapLeftKey("postId");
                m.MapRightKey("userId");
            });
            //create post
            generalPost.HasRequired(p => p.CreateUser).WithMany(u => u.CreatedPost).HasForeignKey(p => p.createUserId);

            //------------------------General Post
            //tag
            modelBuilder.Entity<GeneralPost>().HasMany(p => p.Tag).WithMany(t => t.GeneralPost).Map(m =>
                {
                    m.ToTable("Tagging");
                    m.MapLeftKey("generalPostId");
                    m.MapRightKey("tagname");
                });
            modelBuilder.Entity<Tag>().HasKey(t => t.name);
            ////Table-Per-Concrete Class Inheritance
            //modelBuilder.Entity<GeneralPost>().ToTable("GeneralPost");
            //modelBuilder.Entity<Story>().ToTable("Story");
            //modelBuilder.Entity<Album>().ToTable("Album");
            //modelBuilder.Entity<Photo>().ToTable("Photo");
            //is a
            modelBuilder.Entity<Story>().HasRequired(s => s.Post).WithOptional(p => p.Story).WillCascadeOnDelete(true);
            modelBuilder.Entity<Album>().HasRequired(a => a.Post).WithOptional(p => p.Album).WillCascadeOnDelete(true);
            modelBuilder.Entity<Photo>().HasRequired(ph => ph.Post).WithOptional(p => p.Photo).WillCascadeOnDelete(true);
            //family has story1
            modelBuilder.Entity<Story>()
                .HasRequired(s => s.Family).WithMany(f => f.Story).HasForeignKey(s => s.familyId);
            //story has album
            modelBuilder.Entity<Story>().HasMany(s => s.Album)
                .WithMany(a => a.Story)
                .Map(m => {
                    m.ToTable("Story_Album");
                    m.MapLeftKey("storyId");
                    m.MapRightKey("albumId");
                });
            //album has photo
            modelBuilder.Entity<Album>().HasMany(a => a.Photo)
                .WithMany(p => p.Album)
                .Map(m => {
                    m.ToTable("Album_Photo");
                    m.MapLeftKey("albumId");
                    m.MapRightKey("photoId");
                });

            //----------------------Session
            modelBuilder.Entity<Session>().HasRequired(s => s.User).WithOptional().Map(m => { m.MapKey("userId"); });
            modelBuilder.Entity<Session>().HasKey(s => s.token);
            //base.OnModelCreating(modelBuilder);
        }
    }
}