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
    public class HomeController : ApiController
    {

        //public HttpResponseMessage GetRiderDetail(Object User)
        //{
        //    var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(Donation));


        [HttpPost]
        public HttpResponseMessage Login(Object User)
        {
            HttpStatusCode statusCode = HttpStatusCode.Unauthorized;
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(User));
            object Cred;
            test.TryGetValue("Cred", out Cred);
            object Password;
            test.TryGetValue("Password", out Password);
            string _Password = Password.ToString();

            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("User").Where("Username", Cred).Where("Email", Cred).Get().Cast<IDictionary<string, object>>();
            response.ElementAt(0).Remove("DapperRow");
            bool hasData = (response != null) ? true : false;

            if (hasData)
            {
                response.ElementAt(0).TryGetValue("Password", out object pass);

                if (_Password.Equals(pass))
                {
                    statusCode = HttpStatusCode.OK;
                    return Request.CreateResponse(statusCode, response.ElementAt(0));
                }
                else
                {
                    return Request.CreateErrorResponse(statusCode, "Invalid Password");
                }

            }
            else
            {
                statusCode = HttpStatusCode.NotFound;
                return Request.CreateErrorResponse(statusCode, "User not found");
            }

        }

        [HttpPost]
        public HttpResponseMessage loginWithCnic(Object User)
        {
            HttpStatusCode statusCode = HttpStatusCode.Unauthorized;
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(User));
            object Cred;
            test.TryGetValue("Cred", out Cred);
            object Password;
            test.TryGetValue("Password", out Password);
            string _Password = Password.ToString();

            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("User").Where("CNIC", Cred).Get().Cast<IDictionary<string, object>>(); ;
            response.ElementAt(0).Remove("DapperRow");
            bool hasData = (response != null) ? true : false;

            if (hasData)
            {
                response.ElementAt(0).TryGetValue("Password", out object pass);

                if (_Password.Equals(pass))
                {
                    statusCode = HttpStatusCode.OK;
                    return Request.CreateResponse(statusCode, response.ElementAt(0));
                }
                else
                {
                    return Request.CreateErrorResponse(statusCode, "Invalid Password");
                }

            }
            else
            {
                statusCode = HttpStatusCode.NotFound;
                return Request.CreateErrorResponse(statusCode, "User not found");
            }

        }
        public HttpResponseMessage SignUp(Object User)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(User));
            //email password usertype


            object _Username;
            test.TryGetValue("Username", out _Username);
            string Username = _Username.ToString();
            object _Email;
            test.TryGetValue("Email", out _Email);
            string Email = _Email.ToString();
            object _Password;
            test.TryGetValue("UPassword", out _Password);
            string Password = _Password.ToString();
            object _UsertypeID;
            test.TryGetValue("UsertypeID", out _UsertypeID);
            int UsertypeID = Convert.ToInt32(_UsertypeID);

            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
            db.Connection.Open();
            using (var scope = db.Connection.BeginTransaction())
            {
                try
                {
                    Dictionary<string, object> ObjUser = new Dictionary<string, object>()
                    {
                        {"Email",Email },
                        {"UPassword", Password },
                        {"Username", Username },
                        {"UsertypeID",UsertypeID },

                    };

                    var res = db.Query("User").Insert(ObjUser);  //lastinsertedid
                    scope.Commit();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, int>() { { "UserId", res } });

                }
                catch (Exception ex)
                {
                    scope.Rollback();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }



        }
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
                    response.ElementAt(0).Remove("DapperRow");

                    response.ElementAt(0).Add("CNIC", CNIC);
                    response.ElementAt(0).Add("FullName", FullName);
                    response.ElementAt(0).Add("Contact", Contact);
                    response.ElementAt(0).Add("Address", Address);

                    var res = db.Query("User").Where("UserId", UserId).Update(response.ElementAt(0));
                    scope.Commit();
                    return Request.CreateResponse(HttpStatusCode.Created, response.ElementAt(0));


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

