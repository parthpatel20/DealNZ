//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DealsNZ.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WishList
    {
        public int WishlistID { get; set; }
        public int DealId { get; set; }
        public int UserId { get; set; }
        public Nullable<System.DateTime> AddedOn { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
    }
}
