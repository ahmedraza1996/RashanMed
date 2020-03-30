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
    public class DistributorController : ApiController
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
            object NumberOfPeople;
            test.TryGetValue("NumberOfPeople", out NumberOfPeople);
            int _NumberOfPeople = Convert.ToInt32(NumberOfPeople);
           

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
                        {"DonationTypeId",DonationTypeId  },
                        {"RequestStatus",  ReqStatusPending},
                        {"NumberOfPeople", _NumberOfPeople }
                    };


                    var res = db.Query("DistributorRequest").Insert(RequestObj);  //lastinsertedid
                    scope.Commit();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, int>() { { "Data", res } });
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
