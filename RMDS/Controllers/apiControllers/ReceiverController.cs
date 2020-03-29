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
using System.Threading;
using System.Web.Http;
using static RMDS.Shared.Constants;
namespace RMDS.Controllers
{
    [BasicAuthentication]
    public class ReceiverController : ApiController
    {
        
        [HttpGet]
        public HttpResponseMessage CheckAuthGet()
        {
            string username = Thread.CurrentPrincipal.Identity.Name;

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        

        [HttpPost]
        public HttpResponseMessage RequestDonation(Object Donation) { 


           // string username = Thread.CurrentPrincipal.Identity.Name;
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(Donation));
            object ReceiveDetail;
            test.TryGetValue("ReceiveDetail", out ReceiveDetail);
            string _ReceiveDetail = ReceiveDetail.ToString();
            object DropLat;
            test.TryGetValue("DropLat", out DropLat);
            double _DropLat = Convert.ToDouble(DropLat);
            object Droplng;
            test.TryGetValue("Droplng", out Droplng);
            double _DropLng = Convert.ToDouble(Droplng);
            object DonationTypeId;
            test.TryGetValue("DonationTypeId", out DonationTypeId);
            int _TypeId = Convert.ToInt32(DonationTypeId);
            object UserId;
            test.TryGetValue("UserId", out UserId);
            int _UserId = Convert.ToInt32(UserId);


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
                        {"UserId", _UserId},
                        {"DropLat",_DropLat },
                        {"Droplng",_DropLng },
                        {"TypeId",Donation  },
                        {"RequestStatus",  ReqStatusPending}
                    };


                    var res = db.Query("userReceiver").Insert(RequestObj);  //lastinsertedid
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
