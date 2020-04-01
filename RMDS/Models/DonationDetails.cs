using MySql.Data.MySqlClient;
using RMDS.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class DonationDetails
    {

        public int DETID { get; set;  }
        public string ITEMNAME { get; set; }
        public int QUANTITY { get; set; }
    
        public int DONATIONID { get; set; }

    }
    public class DonationDetailManager: BaseManager {
        public static List<DonationDetails> GetDonationDetails(string whereclause, MySqlConnection conn = null)
        {
            DonationDetails ObjDD = new DonationDetails();
            List<DonationDetails> lstDD = new List<DonationDetails>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from donationdetails";
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
                                ObjDD = ReaderDataDonationDet(reader);
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
        private static DonationDetails ReaderDataDonationDet(MySqlDataReader reader)
        {
            DonationDetails objDD = new DonationDetails();
            objDD.DETID = DbCheck.IsValidInt(reader["detid"]);
            objDD.ITEMNAME = DbCheck.IsValidString(reader["itemname"]);
            objDD.QUANTITY = DbCheck.IsValidInt(reader["quantity"]);
            objDD.DONATIONID = DbCheck.IsValidInt(reader["donationid"]);
          
            return objDD;

        }
        public static string SaveDonationDetail(DonationDetails ObjDD, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sDDID = "";
            sDDID = ObjDD.DETID.ToString();
            var templstEmp = GetDonationDetails("DetID = '" + sDDID + "'", conn);
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
                        sql = @"INSERT INTO Donationdetails(
                                                    ITEMNAME,
                                                    QUANTITY,
                                                    DONATIONID
                                                                     
                                                    )
                                                    VALUES(
                                                    @ITEMNAME,
                                                    @QUANTITY,
                                                    @DONATIONID
                                                   
                                                    )";
                    }
                    else
                    {
                        sql = @"Update Donationdetails set
                                                    DETID=@DETID,                                                
                                                    ITEMNAME=@ITEMNAME,
                                                    QUANTITY=@QUANTITY,
                                                    DONATIONID=@DONATIONID,
                                                   
                                                    Where DETID=@DETID";
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
                        command.Parameters.AddWithValue("@DETID", ObjDD.DETID);
                    }
                    command.Parameters.AddWithValue("@ITEMNAME", ObjDD.ITEMNAME);
                    command.Parameters.AddWithValue("@QUANTITY", ObjDD.QUANTITY);
                    command.Parameters.AddWithValue("@DONATIONID", ObjDD.DONATIONID);
                    
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