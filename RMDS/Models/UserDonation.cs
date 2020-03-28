using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class UserDonation
    {
        public int DonationID { get; set; }
        public DateTime? RequestDate { get; set; }
        public double PickUpLat { get; set; }
        public double PickUpLng { get; set; }
        public string DonationStatus { get; set;}
        public int UserId { get; set; }
        public int TypeId { get; set; }

    
    }
}