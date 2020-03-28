using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string UPassword { get; set; }
        public string CNIC { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public int UserTypeId { get; set; }


     }
}