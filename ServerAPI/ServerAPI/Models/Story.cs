//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServerAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Story
    {
        public Story()
        {
            this.Sharing = new HashSet<Sharing>();
            this.Album = new HashSet<Album>();
        }
    
        public int id { get; set; }
        public string title { get; set; }
        public string privacy { get; set; }
    
        public virtual GeneralPost GeneralPost { get; set; }
        public virtual ICollection<Sharing> Sharing { get; set; }
        public virtual ICollection<Album> Album { get; set; }
    }
}
