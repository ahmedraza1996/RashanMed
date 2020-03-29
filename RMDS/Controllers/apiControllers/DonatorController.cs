using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static RMDS.Shared.Constants;

namespace RMDS.Controllers
{

    [BasicAuthentication]
    public class DonatorController : ApiController
    {
        [HttpPost]

        public HttpResponseMessage Donate(Object Donation)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(Donation));
            object DonationDetails;
            test.TryGetValue("DonationDetails", out DonationDetails);
           
            object PickUpLat;
            test.TryGetValue("PickUpLat", out PickUpLat);
            double _PickUpLat = Convert.ToDouble(PickUpLat);
            object PickUpLng;
            test.TryGetValue("PickUpLng", out PickUpLng);
            double _PickUpLng = Convert.ToDouble(PickUpLng);
            object UserId;
            test.TryGetValue("UserId", out UserId);
            int _UserId = Convert.ToInt32 (UserId);
            object TypeId;
            test.TryGetValue("TypeId", out TypeId);
            int _TypeId = Convert.ToInt32(TypeId);

            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
            db.Connection.Open();
            using (var scope = db.Connection.BeginTransaction())
            {
                try
                {
                    Dictionary<string, object> DonationObj = new Dictionary<string, object>()
                    {
                        {"RequestDate",DateTime.Now.Date },
                        {"TypeId", _TypeId },
                        {"UserId", _UserId },
                        {"PickUpLat",_PickUpLat },
                        {"PickUpLng",_PickUpLng },
                        {"DonationStatus",DonationStatusPending  }
                        //donationdetails
                    };


                    var res = db.Query("userDonation").Insert(DonationObj);  //lastinsertedid
                    List<Dictionary<string, object>> _DonationDetails = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(DonationDetails.ToString());

                    foreach ( var item in _DonationDetails)
                    {
                        item.Add("DonationID", res);
                    }


                    scope.Commit();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, int>() { { "DonationId", res } });
                }
                catch (Exception ex)
                {
                    scope.Rollback();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }


        }
      

    }
}
