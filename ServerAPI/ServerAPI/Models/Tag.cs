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
    
    public partial class Tag
    {
        public Tag()
        {
            this.GeneralPost = new HashSet<GeneralPost>();
        }
    
        public string name { get; set; }
    
        public virtual ICollection<GeneralPost> GeneralPost { get; set; }
    }
}