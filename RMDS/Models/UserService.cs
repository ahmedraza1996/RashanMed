using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class UserService
    {

        public static bool Login(string Username, string Password)
        {
            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            //response = db.Query("User").Where("Username", Username).OrWhere("Email", Username).OrWhere("Cnic", Username).Get().Cast<IDictionary<string, object>>();
            //response.ElementAt(0).Remove("DapperRow");
            //bool hasData = (response != null) ? true : false;

            //if (hasData)
            //{
            //    response.ElementAt(0).TryGetValue("Password", out object pass);

            //    if (Password.Equals(pass))
            //    {

            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }

            //}
            //else
            //{

            //    return false;
            //}

            return true; 
        }
    }
}