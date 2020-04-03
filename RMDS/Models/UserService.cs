using MySql.Data.MySqlClient;
using Newtonsoft.Json;
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
            try
            {
                IEnumerable<IDictionary<string, object>> response;
                response = db.Query("User").Where("Username", Username).OrWhere("Email", Username).OrWhere("Cnic", Username).Get().Cast<IDictionary<string, object>>();

                var strResponse = response.ElementAt(0).ToString().Replace("DapperRow,", "").Replace("=", ":");
                Dictionary<string, string> temp = JsonConvert.DeserializeObject<Dictionary<string, string>>(strResponse);
                bool hasData = (temp != null) ? true : false;

                if (hasData)
                {
                    temp.TryGetValue("upassword", out string pass);
                    Password = Password.ToString();
                    string pw = Crypto.Hash(Password);

                    if (pass.Equals(pw))
                    {

                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {

                    return false;
                }

            }
            
            catch(Exception ex)
            {

            }
            return false;
        }
    }
}