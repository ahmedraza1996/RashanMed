using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class UserPersonalInfo
    {
        public int UserID { get; set; }
        public int DateOfBirth { get; set; }
        public char Gender { get; set; }
        
        public int NumberofDependents { get; set;  }
        public char MaritalStatus { get; set; }
        public string Residence { get; set;  }
        public int HouseSize { get; set;  }
        public int NumberOfEarners { get; set;  }
        public int OverallMonthlyIncome { get; set; }
        public string Profession { get; set; }


    }
}