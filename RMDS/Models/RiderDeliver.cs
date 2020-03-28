using MySql.Data.MySqlClient;
using RMDS.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class RiderDeliver
    {

        public int RIDEID { get; set; }
        public DateTime? ASSIGNEDDATE { get; set; }
        public int RIDERID { get;  set; }

        public int ACCEPTID { get; set; }

    }

    public  class RideDeliverManager: BaseManager
    {
        public static List<RiderDeliver> GetRideDeliever(string whereclause, MySqlConnection conn = null)
        {
            RiderDeliver ObjRD = new RiderDeliver();
            List<RiderDeliver> lstRD = new List<RiderDeliver>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from riderdeliver";
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
                                ObjRD = ReaderDataRiderDeliver(reader);
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
        private static RiderDeliver ReaderDataRiderDeliver(MySqlDataReader reader)
        {
            RiderDeliver objRD = new RiderDeliver();
            objRD.RIDEID = DbCheck.IsValidInt(reader["rideid"]);
            objRD.RIDERID = DbCheck.IsValidInt(reader["RiderID"]);
            objRD.ASSIGNEDDATE = DbCheck.IsValidDateTime(reader["assigndate"]);
            objRD.ACCEPTID = DbCheck.IsValidInt(reader["acceptid"]);

            return objRD;

        }
        public static string SaveRiderDeliver(RiderDeliver ObjRd, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sRDID = "";
            sRDID = ObjRd.RIDEID.ToString();
            var templstEmp = GetRideDeliever("RIDEID = '" + sRDID + "'", conn);
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
                        sql = @"INSERT INTO RiderDeliver(
                                                    ACCEPTID,
                                                    RIDERID,
                                                    ASSIGNDATE                
                                                    )
                                                    VALUES(
                                                    @ACCEPTID,
                                                    @RIDERID,
                                                    @ASSIGNDATE,
                                                   
                                                    )";
                    }
                    else
                    {
                        sql = @"Update RiderDeliver set
                                                    RIDEID=@RIDEID,                                                
                                                    RIDERID=@RIDERID,
                                                    ASSIGNDATE=@ASSIGNDATE,
                                                    ACCEPTID=@ACCEPTID,
                                                   
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
                    command.Parameters.AddWithValue("@ACCEPTID", ObjRd.ACCEPTID);

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