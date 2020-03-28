using MySql.Data.MySqlClient;
using RMDS.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class RiderDonation
    {
        public int RIDEID { get; set; }
        public DateTime? ASSIGNEDDATE { get; set; }
        public int RIDERID { get; set; }

        public int DONATIONID { get; set; }
    }
    public class RideDonationManager : BaseManager
    {
        public static List<RiderDonation> GetRiderDonation(string whereclause, MySqlConnection conn = null)
        {
            RiderDonation ObjRD = new RiderDonation();
            List<RiderDonation> lstRD = new List<RiderDonation>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from riderdonation";
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
                                ObjRD = ReaderDataRiderDonation(reader);
                                lstRD.Add(ObjRD);
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
            return lstRD;
        }
        private static RiderDonation ReaderDataRiderDonation(MySqlDataReader reader)
        {
            RiderDonation objRD = new RiderDonation();
            objRD.RIDEID = DbCheck.IsValidInt(reader["rideid"]);
            objRD.RIDERID = DbCheck.IsValidInt(reader["RiderID"]);
            objRD.ASSIGNEDDATE = DbCheck.IsValidDateTime(reader["assigndate"]);
            objRD.DONATIONID = DbCheck.IsValidInt(reader["donationid"]);

            return objRD;

        }
        public static string SaveRiderDeliver(RiderDonation ObjRd, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sRDID = "";
            sRDID = ObjRd.RIDEID.ToString();
            var templstEmp = GetRiderDonation("RIDEID = '" + sRDID + "'", conn);
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
                        sql = @"INSERT INTO RiderDonation(
                                                    DONATIONID,
                                                    RIDERID,
                                                    ASSIGNDATE                
                                                    )
                                                    VALUES(
                                                    @DONATIONID,
                                                    @RIDERID,
                                                    @ASSIGNDATE,
                                                   
                                                    )";
                    }
                    else
                    {
                        sql = @"Update RiderDonation set
                                                    RIDEID=@RIDEID,                                                
                                                    RIDERID=@RIDERID,
                                                    ASSIGNDATE=@ASSIGNDATE,
                                                    DONATIONID=@DONATIONID,
                                                   
                                                    Where RIDEID=@RIDEID";
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
                        command.Parameters.AddWithValue("@RIDEID", ObjRd.RIDEID);
                    }
                    command.Parameters.AddWithValue("@RIDERID", ObjRd.RIDERID);
                    command.Parameters.AddWithValue("@ASSIGNDATE", ObjRd.ASSIGNEDDATE);
                    command.Parameters.AddWithValue("@DONATIONID", ObjRd.DONATIONID);

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