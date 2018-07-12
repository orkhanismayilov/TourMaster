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
    
    public partial class Tour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tour()
        {
            this.Bookings = new HashSet<Booking>();
            this.Feedbacks = new HashSet<Feedback>();
            this.TourImages = new HashSet<TourImage>();
        }
    
        public int Id { get; set; }
        public int GuideId { get; set; }
        public int FromId { get; set; }
        public int DestinationId { get; set; }
        public decimal Price { get; set; }
        public int CurrencyId { get; set; }
        public string Category { get; set; }
        public int Duration { get; set; }
        public int DurationTypeId { get; set; }
        public Nullable<int> AccomodationId { get; set; }
        public Nullable<int> AccomodationLevelId { get; set; }
        public string Vehicle { get; set; }
        public int MainImageId { get; set; }
        public string Description { get; set; }
        public System.DateTime PostedDate { get; set; }
        public int Status { get; set; }
    
        public virtual AccomodationLevel AccomodationLevel { get; set; }
        public virtual Accomodation Accomodation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual City City { get; set; }
        public virtual City City1 { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual DurationType DurationType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TourImage> TourImages { get; set; }
        public virtual TourImage TourImage { get; set; }
        public virtual User User { get; set; }
    }
}
