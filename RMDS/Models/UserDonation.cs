using MySql.Data.MySqlClient;
using RMDS.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class UserDonation
    {
        public int DonationID { get; set; }
        [DisplayName("Request Date")]
        public DateTime? RequestDate { get; set; }
        // public double PickUpLat { get; set; }
        // public double PickUpLng { get; set; }

        [DisplayName("Donation Status")]
        public string DonationStatus { get; set;}
        
        public int UserId { get; set; }
        
        public int DonationTypeId { get; set; }


        //helper
        [DisplayName("Full Name")]
        public string UserFullName { get; set; }

        [DisplayName("Donation Type")]

        public string DonationType { get; set; }

        public List<DonationDetails> lstDonationDet { get; set; }
        public List<string> ItemName { get; set; }

        public List<int>Quantity { get; set; }
    }
    public class UserDonationManager : BaseManager
    {
        public static List<UserDonation> GetUserDonation(string whereclause, MySqlConnection conn = null)
        {
            UserDonation ObjDD = new UserDonation();
            List<UserDonation> lstDD = new List<UserDonation>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from userdonation";
                if (!string.IsNullOrEmpty(whereclause))
                    sql += " where " + whereclause;
                sql += " order by requestdate";
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
                                ObjDD = ReaderDataUserDonation(reader);
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
        private static UserDonation ReaderDataUserDonation(MySqlDataReader reader)
        {
            UserDonation objDD = new UserDonation();
            objDD.DonationID = DbCheck.IsValidInt(reader["donationid"]);
            objDD.DonationStatus = DbCheck.IsValidString(reader["DonationStatus"]);
            objDD.RequestDate = DbCheck.IsValidDateTime(reader["RequestDate"]);
            objDD.DonationTypeId = DbCheck.IsValidInt(reader["DonationTypeId"]);
            objDD.UserId = DbCheck.IsValidInt(reader["UserId"]);
            return objDD;

        }
        public static string SaveUserDonation(UserDonation ObjDD, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sDDID = "";
            sDDID = ObjDD.DonationID.ToString();
            var templstEmp = GetUserDonation("DonationID = '" + sDDID + "'", conn);
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
                        sql = @"INSERT INTO UserDonation(
                                                    RequestDate,
                                                    DonationStatus,
                                                    UserId, 
                                                    DonationTypeId
                                                                     
                                                    )
                                                    VALUES(
                                                    @RequestDate,
                                                    @DonationStatus,
                                                    @UserId,
                                                    @DonationTypeId                                                   
                                                    )";
                    }
                    else
                    {
                        sql = @"Update UserDonation set
                                                    DonationID=@DonationID,                                                
                                                    RequestDate=@RequestDate,
                                                    DonationStatus=@DonationStatus,
                                                    UserId=@UserId,
                                                    DonationTypeId=@DonationTypeId
                                                    Where DonationID=@DonationID";
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
                        command.Parameters.AddWithValue("@DonationID", ObjDD.DonationID);
                    }
                    command.Parameters.AddWithValue("@RequestDate", ObjDD.RequestDate);
                    command.Parameters.AddWithValue("@DonationStatus", ObjDD.DonationStatus);
                    command.Parameters.AddWithValue("@UserId", ObjDD.UserId);
                    command.Parameters.AddWithValue("@DonationTypeId", ObjDD.DonationTypeId);

                    int affectedRows = command.ExecuteNonQuery();
                    var lastInsertID = command.LastInsertedId;
                    if (affectedRows > 0)
                    {
                        returnMessage = lastInsertID.ToString();
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



        //select* from userdonation d, user u , donationtype t where d.donationstatus='pending ' and d.userid= u.userid and t.DonationTypeId= d.DonationTypeId
        public static List<UserDonation> GetPendingDonation(string whereclause, MySqlConnection conn = null)
        {
            UserDonation ObjDD = new UserDonation();
            List<UserDonation> lstDD = new List<UserDonation>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql= "select * from userdonation U , donationtype d, user S where DonationStatus='Pending' and u.donationtypeid=d.donationtypeid and u.userid= s.userid order by u.requestdate ";
             //   string sql = "select u.fullname,u.userid, t.typename, d.requestdate, d.donationstatus, d.donationid from userdonation d, user u , donationtype t where d.donationstatus='pending ' and d.userid= u.userid and t.DonationTypeId= d.DonationTypeId";
                if (!string.IsNullOrEmpty(whereclause))
                    sql += "and where " + whereclause;
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
                                ObjDD = ReaderDataPending(reader);
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
        private static UserDonation ReaderDataAll(MySqlDataReader reader)
        {
            UserDonation objDD = new UserDonation();
            objDD.DonationID = DbCheck.IsValidInt(reader["donationid"]);
            objDD.DonationStatus = DbCheck.IsValidString(reader["DonationStatus"]);
            objDD.RequestDate = DbCheck.IsValidDateTime(reader["RequestDate"]);
            objDD.DonationType = DbCheck.IsValidString(reader["typename"]);

            objDD.UserId = DbCheck.IsValidInt(reader["userid"]);
            objDD.UserFullName = DbCheck.IsValidString(reader["fullname"]);
        
            return objDD;

        }

        //select* from userdonation d, user u , donationtype t where d.donationstatus='pending ' and d.userid= u.userid and t.DonationTypeId= d.DonationTypeId
        public static List<UserDonation> GetAll(string whereclause, MySqlConnection conn = null)
        {
            UserDonation ObjDD = new UserDonation();
            List<UserDonation> lstDD = new List<UserDonation>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from userdonation U , donationtype d, user S where u.donationtypeid=d.donationtypeid and u.userid= s.userid order by u.requestdate ";
                //   string sql = "select u.fullname,u.userid, t.typename, d.requestdate, d.donationstatus, d.donationid from userdonation d, user u , donationtype t where d.donationstatus='pending ' and d.userid= u.userid and t.DonationTypeId= d.DonationTypeId";
                if (!string.IsNullOrEmpty(whereclause))
                    sql += "and where " + whereclause;
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
                                ObjDD = ReaderDataAll(reader);
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
        private static UserDonation ReaderDataPending(MySqlDataReader reader)
        {
            UserDonation objDD = new UserDonation();
            objDD.DonationID = DbCheck.IsValidInt(reader["donationid"]);
            objDD.DonationStatus = DbCheck.IsValidString(reader["DonationStatus"]);
            objDD.RequestDate = DbCheck.IsValidDateTime(reader["RequestDate"]);
            objDD.DonationType = DbCheck.IsValidString(reader["typename"]);

            objDD.UserId = DbCheck.IsValidInt(reader["userid"]);
            objDD.UserFullName = DbCheck.IsValidString(reader["fullname"]);

            return objDD;

        }


    }
}