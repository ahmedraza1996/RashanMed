using MySql.Data.MySqlClient;
using RMDS.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class User
    {
        public int UserID { get; set; }
        [DisplayName("Full Name")]
        [Required(ErrorMessage = "Full name is Required")]
        public string FullName { get; set; }
        public string Username { get; set; }
        public string UPassword { get; set; }
        [Required(ErrorMessage="CNIC number is required")]
        public string CNIC { get; set; }
       
        [Required(ErrorMessage = "Contact number is required")]

        public string Contact { get; set; }
        
        [Required(ErrorMessage = "Address number is required")]

        public string Address { get; set; }
        public int UserTypeId { get; set; }

        public string Email { get; set;  }

    }
    public class UserManager : BaseManager
    {
        public static List<User> GetUser(string whereclause, MySqlConnection conn = null)
        {
            User ObjDD = new User();
            List<User> lstDD = new List<User>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from User";
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
                                ObjDD = ReaderDataUser(reader);
                                lstDD.Add(ObjDD);
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
            return lstDD;
        }
        private static User ReaderDataUser(MySqlDataReader reader)
        {
            User objDD = new User();
            objDD.UserID = DbCheck.IsValidInt(reader["UserID"]);
            objDD.FullName = DbCheck.IsValidString(reader["fullname"]);
            objDD.Username = DbCheck.IsValidString(reader["username"]);
            objDD.UPassword = DbCheck.IsValidString(reader["upassword"]);
            objDD.CNIC = DbCheck.IsValidString(reader["cnic"]);
            objDD.Contact = DbCheck.IsValidString(reader["Contact"]);
            objDD.Address = DbCheck.IsValidString(reader["address"]);
            objDD.UserTypeId = DbCheck.IsValidInt(reader["usertypeid"]);
            objDD.Email = DbCheck.IsValidString(reader["email"]);



            return objDD;

        }
        public static string SaveUser(User ObjDD, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sDDID = "";
            sDDID = ObjDD.UserID.ToString();
            var templstEmp = GetUser("UserID = '" + sDDID + "'", conn);
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
                        sql = @"INSERT INTO User(
                                                    FullName,
                                                    Username,
                                                    UPassword, 
                                                    CNIC, 
                                                    Contact,
                                                    Address,
                                                    UserTypeId,
                                                    Email
                                                                     
                                                    )
                                                    VALUES(
                                                    @FullName,
                                                    @Username,
                                                    @UPassword,
                                                    @CNIC , 
                                                    @Contact,
                                                    @Address,
                                                    @UserTypeId,
                                                    @Email
                                                    )";
                    }
                    else
                    {
                        sql = @"Update User set
                                                    UserID=@UserID,                                                
                                                    FullName=@FullName,
                                                    Username=@Username,
                                                    UPassword=@UPassword,
                                                    CNIC=@CNIC,
                                                    Contact=@Contact,
                                                    Address=@Address,
                                                    UserTypeId=@UserTypeId,
                                                    Email=@Email
                                                    Where UserID=@UserID";
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
                        command.Parameters.AddWithValue("@UserID", ObjDD.UserID);
                    }
                    command.Parameters.AddWithValue("@FullName", ObjDD.FullName);
                    command.Parameters.AddWithValue("@Username", ObjDD.Username);
                    command.Parameters.AddWithValue("@UPassword", ObjDD.UPassword);
                    command.Parameters.AddWithValue("@Contact", ObjDD.Contact);
                    command.Parameters.AddWithValue("@CNIC", ObjDD.CNIC);
                    command.Parameters.AddWithValue("@Address", ObjDD.Address);
                    command.Parameters.AddWithValue("@UserTypeId", ObjDD.UserTypeId);
                    command.Parameters.AddWithValue("@Email", ObjDD.Email);
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

        private static User ReaderDataUserName(MySqlDataReader reader)
        {
            User objDD = new User();
            objDD.UserID = DbCheck.IsValidInt(reader["UserID"]);
            objDD.FullName = DbCheck.IsValidString(reader["fullname"]);
            
            return objDD;

        }
        public static List<User> GetUserName(string whereclause, MySqlConnection conn = null)
        {
            User ObjDD = new User();
            List<User> lstDD = new List<User>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from User";
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
                                ObjDD = ReaderDataUserName(reader);
                                lstDD.Add(ObjDD);
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
            return lstDD;
        }
    }
}