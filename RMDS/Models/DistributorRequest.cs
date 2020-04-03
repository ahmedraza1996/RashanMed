using MySql.Data.MySqlClient;
using RMDS.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class DistributorRequest
    {
        public int RequestID { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime? RequestDate { get; set; }
        public string RequestStatus { get; set; }
        public int UserID { get; set; }


    }
    public class DistributorRequestManager : BaseManager
    {
        public static List<DistributorRequest> GetDistributionRequest(string whereclause, MySqlConnection conn = null)
        {
            DistributorRequest ObjDD = new DistributorRequest();
            List<DistributorRequest> lstDD = new List<DistributorRequest>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from distributorrequest";
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
                                ObjDD = ReaderDataDistributorRequest(reader);
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
        private static DistributorRequest ReaderDataDistributorRequest(MySqlDataReader reader)
        {
            DistributorRequest objDD = new DistributorRequest();
            objDD.RequestID = DbCheck.IsValidInt(reader["RequestID"]);
            objDD.NumberOfPeople = DbCheck.IsValidInt(reader["NumberOfPeople"]);
            objDD.RequestDate = DbCheck.IsValidDateTime(reader["RequestDate"]);
            objDD.RequestStatus = DbCheck.IsValidString(reader["RequestStatus"]);
            objDD.UserID = DbCheck.IsValidInt(reader["UserId"]);
            return objDD;

        }
        public static string SaveDistributionRequest(DistributorRequest ObjDD, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sDDID = "";
            sDDID = ObjDD.RequestID.ToString();
            var templstEmp = DistributorRequestManager.GetDistributionRequest("RequestID = '" + sDDID + "'", conn);
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
                        sql = @"INSERT INTO DistributorRequest(
                                                    RequestDate,
                                                    NumberOfPeople,
                                                    UserId, 
                                                    RequestStatus
                                                                     
                                                    )
                                                    VALUES(
                                                    @RequestDate,
                                                    @NumberOfPeople,
                                                    @UserId,
                                                    @RequestStatus                                                   
                                                    )";
                    }
                    else
                    {
                        sql = @"Update DistributorRequest set
                                                    RequestID=@RequestID,                                                
                                                    RequestDate=@RequestDate,
                                                    RequestStatus=@RequestStatus,
                                                    UserId=@UserId,
                                                    NumberOfPeople=@NumberOfPeople
                                                    Where RequestID=@RequestID";
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
                        command.Parameters.AddWithValue("@RequestID", ObjDD.RequestID);
                    }
                    command.Parameters.AddWithValue("@RequestDate", ObjDD.RequestDate);
                    command.Parameters.AddWithValue("@RequestStatus", ObjDD.RequestStatus);
                    command.Parameters.AddWithValue("@UserId", ObjDD.UserID);
                    command.Parameters.AddWithValue("@NumberOfPeople", ObjDD.NumberOfPeople);

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