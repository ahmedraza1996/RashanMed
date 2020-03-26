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

namespace RMDS.Controllers
{
    public class ReceiverController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage RequestDonation(Object Donation)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(Donation));
            object DonationDesc;
            test.TryGetValue("ReqDesc", out DonationDesc);
            string _DonationDesc = DonationDesc.ToString();
            object DropLat;
            test.TryGetValue("DropLat", out DropLat);
            double _DropLat = Convert.ToDouble(DropLat);
            object Droplng;
            test.TryGetValue("Droplng", out Droplng);
            double _DropLng = Convert.ToDouble(Droplng);
            object DonationTypeId;
            test.TryGetValue("DonationTypeId", out DonationTypeId);
            int _DonationTypeId = Convert.ToInt32(DonationTypeId);

            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
            db.Connection.Open();
            using (var scope = db.Connection.BeginTransaction())
            {
                try
                {
                    Dictionary<string, object> RequestObj = new Dictionary<string, object>()
                    {
                        {"RequestDate",DateTime.Now.Date },
                        {"ReqDesc", _DonationDesc},
                        {"DropLat",_DropLat },
                        {"Droplng",_DropLng },
                        {"DonationTypeId",Donation  }
                    };


                    var res = db.Query("userDonation").Insert(RequestObj);  //lastinsertedid
                    scope.Commit();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, int>() { { "RequestId", res } });
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
