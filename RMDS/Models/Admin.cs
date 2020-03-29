using MySql.Data.MySqlClient;
using RMDS.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class Admin
    {
        public int ADMINID { get; set; }
        public string FULLNAME { get; set; }
        public string APASSWORD { get; set; }
        public string EMAIL { get; set; }
        public string USERNAME { get; set; }
       
    }

    public class AdminManager : BaseManager
    {
        public static List<Admin> GetAdmin(string whereclause, MySqlConnection conn = null)
        {
            Admin ObjAdmin = new Admin();
            List<Admin> lstAdmin = new List<Admin>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from admin";
                if (!string.IsNullOrEmpty(whereclause))
                    sql += " where " + whereclause;
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ObjAdmin = ReaderDataAdmin(reader);
                                lstAdmin.Add(ObjAdmin);
                            }
                        }
                        else
                        {
                        }
                    }
                    if (isConnArgNull == true)
                    {
                        connection.Dispose();
                    }


                }
            }
            //endtry
            catch (Exception ex)
            {

            }
            return lstAdmin;
        }
        private static Admin ReaderDataAdmin(MySqlDataReader reader)
        {
            Admin objAdmin = new Admin();
            objAdmin.ADMINID = DbCheck.IsValidInt(reader["Adminid"]);
            objAdmin.FULLNAME = DbCheck.IsValidString(reader["fullname"]);
            objAdmin.EMAIL = DbCheck.IsValidString(reader["email"]);
            objAdmin.APASSWORD = DbCheck.IsValidString(reader["apasswrd"]);
            objAdmin.USERNAME = DbCheck.IsValidString(reader["username"]);
            return objAdmin;

        }
        public static string SaveAdmin(Admin ObjAdmin, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sADMINID = "";
            sADMINID = ObjAdmin.ADMINID.ToString();
            var templstEmp = GetAdmin("ADMINID = '" + sADMINID + "'", conn);
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                using (MySqlCommand command = new MySqlCommand())
                {
                    string sql;
                    bool isEdit = true;
                    if (templstEmp.Count <= 0)
                    {
                        isEdit = false;
                        sql = @"INSERT INTO ADMIN(
                                                    FULLNAME,
                                                    EMAIL,
                                                    APASSWORD,
                                                    USERNAME,
                                                                                       
                                                    )
                                                    VALUES(
                                                    @FULLNAME,
                                                    @EMAIL,
                                                    @PASSWORD,
                                                    @USERNAME,
                                                    )";
                    }
                    else
                    {
                        sql = @"Update ADMIN set
                                                    ADMINID=@ADMINID,                                                
                                                    FULLNAME=@FULLNAME,
                                                    APASSWORD=@PASSWORD,
                                                    EMAIL=@EMAIL,
                                                    USERNAME = @USERNAME,
                                                    Where ADMINID=@ADMINID";
                    }
                    if (trans != null)
                    {
                        command.Transaction = trans;
                    }
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    if (isEdit)
                    {
                        command.Parameters.AddWithValue("@ADMINID", ObjAdmin.ADMINID);
                    }
                    command.Parameters.AddWithValue("@FULLNAME", ObjAdmin.FULLNAME);
                    command.Parameters.AddWithValue("@PASSWORD", ObjAdmin.APASSWORD);
                    command.Parameters.AddWithValue("@EMAIL", ObjAdmin.EMAIL);
                    command.Parameters.AddWithValue("@USERNAME", ObjAdmin.USERNAME);
                    int affectedRows = command.ExecuteNonQuery();
                    var lastInsertID = command.LastInsertedId;
                    if (affectedRows > 0)
                    {
                        returnMessage = "OK";
                    }
                    else
                    {
                        returnMessage = Shared.Constants.MSG_ERR_DBSAVE.Text;
                    }
                }

                if (isConnArgNull == true)
                {
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {

            }

            return returnMessage;
        }
    }
}