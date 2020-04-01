using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMDS.Models.ViewModels
{
    public class DonationUser
    {

        public User User { get; set; }
        public UserDonation UsrDonation { get; set; }


    }
}