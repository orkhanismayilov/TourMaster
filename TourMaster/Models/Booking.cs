//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TourMaster.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Booking
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int TravelerId { get; set; }
        public System.DateTime BookingDate { get; set; }
        public System.DateTime BookedStart { get; set; }
        public System.DateTime BookedEnd { get; set; }
        public decimal BookedPrice { get; set; }
        public int Status { get; set; }
    
        public virtual Tour Tour { get; set; }
        public virtual User User { get; set; }
    }
}
