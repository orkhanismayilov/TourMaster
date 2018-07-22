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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.BookingRequests = new HashSet<BookingRequest>();
            this.Bookings = new HashSet<Booking>();
            this.Feedbacks = new HashSet<Feedback>();
            this.Notifications = new HashSet<Notification>();
            this.PrivateMessages = new HashSet<PrivateMessage>();
            this.PrivateMessages1 = new HashSet<PrivateMessage>();
            this.Tours = new HashSet<Tour>();
        }
    
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public System.DateTime Birthday { get; set; }
        public string Password { get; set; }
        public int CityId { get; set; }
        public string Phone { get; set; }
        public string ProfileImage { get; set; }
        public int OverallRating { get; set; }
        public int AccountType { get; set; }
        public System.DateTime RegisteredDate { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string GooglePlus { get; set; }
        public string Twitter { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingRequest> BookingRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual City City { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notification> Notifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrivateMessage> PrivateMessages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrivateMessage> PrivateMessages1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tour> Tours { get; set; }
    }
}
