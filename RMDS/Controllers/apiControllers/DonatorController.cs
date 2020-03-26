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
    public class DonatorController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Donate(Object Donation)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(Donation));
            object DonationDesc;
            test.TryGetValue("DonationDesc", out DonationDesc);
            string _DonationDesc = DonationDesc.ToString();
            object PickUpLat;
            test.TryGetValue("PickUpLat", out PickUpLat);
            double _PickUpLat = Convert.ToDouble(PickUpLat);
            object PickUpLng;
            test.TryGetValue("PickUpLng", out PickUpLng);
            double _PickUpLng = Convert.ToDouble(PickUpLng);
            object UserId;
            test.TryGetValue("UserId", out UserId);
            int _UserId = Convert.ToInt32 (UserId);


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
                        {"DonationDesc", _DonationDesc},
                        {"PickUpLat",_PickUpLat },
                        {"PickUpLng",_PickUpLng },
                        {"DonationStatusId",DonationStatusPending  }
                    };


                    var res = db.Query("userDonation").Insert(DonationObj);  //lastinsertedid
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
