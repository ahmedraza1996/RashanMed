using MySql.Data.MySqlClient;
using RMDS.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class UserType
    {
        public int typeid { get; set; }
        public string typename { get; set; }

    }
    public class UserTypeManager : BaseManager
    {
        public static List<UserType> GetUserDonation(string whereclause, MySqlConnection conn = null)
        {
            UserType ObjDD = new UserType();
            List<UserType> lstDD = new List<UserType>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from usertype";
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
                                ObjDD = ReaderDataUserType(reader);
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
        private static UserType ReaderDataUserType(MySqlDataReader reader)
        {
            UserType objDD = new UserType();
            objDD.typeid = DbCheck.IsValidInt(reader["typeid"]);
            objDD.typename = DbCheck.IsValidString(reader["typename"]);
           
            return objDD;

        }
        public static string SaveUserType(UserType ObjDD, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sDDID = "";
            sDDID = ObjDD.typeid.ToString();
            var templstEmp = GetUserDonation("typeid = '" + sDDID + "'", conn);
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
                        sql = @"INSERT INTO usertype(
                                                    
                                                    typename,
                                                   
                                                    )
                                                    VALUES(
                                                    
                                                    @typename,
                                                                                                      
                                                    )";
                    }
                    else
                    {
                        sql = @"Update usertype set
                                                    typeid=@typeid,                                                
                                                    typename=@typename,
                                                  
                                                    Where typeid=@typeid";
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
                        command.Parameters.AddWithValue("@typeid", ObjDD.typeid);
                    }
                    command.Parameters.AddWithValue("@typename", ObjDD.typename);
                    
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