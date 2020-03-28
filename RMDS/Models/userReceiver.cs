using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class userReceiver
    {

        public int AcceptID { get; set; }
        public DateTime? RequestDate { get; set; }
        public double DropLat { get; set; }
        public double DropLng { get; set; }
        public string RequestStatus { get; set; }
        public int UserId { get; set; }
        public int TypeId { get; set; }
    }
}