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
    
    public partial class Photo
    {
        public Photo()
        {
            this.Report = new HashSet<Report>();
            this.Album = new HashSet<Album>();
        }
    
        public int id { get; set; }
        public string url { get; set; }
        public string badQuality { get; set; }
    
        public virtual GeneralPost GeneralPost { get; set; }
        public virtual ICollection<Report> Report { get; set; }
        public virtual ICollection<Album> Album { get; set; }
    }
}
