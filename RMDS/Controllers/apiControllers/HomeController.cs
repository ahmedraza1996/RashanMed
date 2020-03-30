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

namespace RMDS.Controllers.apiControllers
{
    [BasicAuthentication]
    public class HomeController : ApiController
    {

        //public HttpResponseMessage GetRiderDetail(Object User)
        //{
        //    var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(Donation));


        [HttpPost]
        public HttpResponseMessage UpdateProfileInfo(Object UserInfo)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(UserInfo));
            Object UserId;
            test.TryGetValue("UserID", out UserId);
            Object CNIC;
            test.TryGetValue("CNIC", out CNIC);
            Object FullName;
            test.TryGetValue("FullName", out FullName);
            Object Contact;
            test.TryGetValue("Contact", out Contact);
            Object Address;
            test.TryGetValue("Address", out Address);

            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
            db.Connection.Open();
            using (var scope = db.Connection.BeginTransaction())
            {
                try
                {
                    IEnumerable<IDictionary<string, object>> response;
                    response = db.Query("User").Where("UserID", UserId).Get().Cast<IDictionary<string, object>>(); ;
                    var strResponse = response.ElementAt(0).ToString().Replace("DapperRow,", "").Replace("=", ":");
                    Dictionary<string, object> temp = JsonConvert.DeserializeObject<Dictionary<string, object>>(strResponse);
                    temp["cnic"] = CNIC;
                    temp["fullname"] = FullName;
                    temp["contact"] = Contact;
                    temp["address"] = Address;
                    //temp.Add("CNIC", CNIC);
                    //temp.Add("FullName", FullName);
                    //temp.Add("Contact", Contact);
                    //temp.Add("Address", Address);

                    var res = db.Query("User").Where("UserId", UserId).Update(temp);
                    scope.Commit();
                    return Request.CreateResponse(HttpStatusCode.Created, temp);


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

