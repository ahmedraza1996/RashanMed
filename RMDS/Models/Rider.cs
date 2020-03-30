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
    public class Rider
    {
        public int RIDERID { get; set; }
        [DisplayName("Full Name")]
        [Required(ErrorMessage = "Full Name is Required")]
        public string FULLNAME { get; set; }
        [StringLength(13,MinimumLength =13,  ErrorMessage ="CNIC must be 13 digits long")]
        [Required(ErrorMessage = "CNIC is Required")]
        public string CNIC { get; set; }
        [DisplayName("Contact Number")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CNIC must be 13 digits long")]

        [Required(ErrorMessage = "Contact number is Required")]
        public string CONTACTNUMBER { get; set; }
        [DisplayName("Address")]
        [Required(ErrorMessage = "Address is Required")]
        public string ADDRESS { get; set; }
        [DisplayName("Vehicle Number") ]
        [Required(ErrorMessage = "Vehicle Number is Required")]
        public string VEHICLEREGISTRATIONNUMBER { get; set; }
    }

    public class RiderManager : BaseManager
    {
        public static List<Rider> GetRider(string whereclause, MySqlConnection conn = null)
        {
            Rider ObjRider = new Rider();
            List<Rider> lstRider = new List<Rider>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from rider";
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
                                ObjRider = ReaderDataRider(reader);
                                lstRider.Add(ObjRider);
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
            return lstRider;
        }
        private static Rider ReaderDataRider(MySqlDataReader reader)
        {
            Rider objRider = new Rider();
            objRider.RIDERID = DbCheck.IsValidInt(reader["riderid"]);
            objRider.FULLNAME = DbCheck.IsValidString(reader["fullname"]);
            objRider.CNIC = DbCheck.IsValidString(reader["cnic"]);
            objRider.CONTACTNUMBER = DbCheck.IsValidString(reader["contactnumber"]);
            objRider.ADDRESS = DbCheck.IsValidString(reader["address"]);
            objRider.VEHICLEREGISTRATIONNUMBER = DbCheck.IsValidString(reader["vehicleregistrationnumber"]);

            return objRider;

        }
        public static string SaveRider(Rider ObjRider, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sRIDERID = "";
            sRIDERID = ObjRider.RIDERID.ToString();
            var templstEmp = GetRider("RIDERID = '" + sRIDERID + "'", conn);
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
                        sql = @"INSERT INTO rider(
                                                    FULLNAME,
                                                    CONTACTNUMBER,
                                                    CNIC,
                                                    VEHICLEREGISTRATIONNUMBER,
                                                    ADDRESS                                   
                                                    )
                                                    VALUES(
                                                    @FULLNAME,
                                                    @CONTACTNUMBER,
                                                    @CNIC,
                                                    @VEHICLEREGISTRATIONNUMBER,
                                                    @ADDRESS 
                                                   )";
                    }
                    else
                    {
                        sql = @"Update RIDER set
                                                    RIDERID=@RIDERID,                                                
                                                    FULLNAME=@FULLNAME,
                                                    CONTACTNUMBER=@CONTACTNUMBER,
                                                    VEHICLEREGISTRATIONNUMBER=@VEHICLEREGISTRATIONNUMBER,
                                                    ADDRESS = @ADDRESS,
                                                    CNIC=@CNIC
                                                    Where RIDERID=@RIDERID";
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
                        command.Parameters.AddWithValue("@RIDERID", ObjRider.RIDERID);
                    }
                    command.Parameters.AddWithValue("@FULLNAME", ObjRider.FULLNAME);
                    command.Parameters.AddWithValue("@CONTACTNUMBER", ObjRider.CONTACTNUMBER);
                    command.Parameters.AddWithValue("@VEHICLEREGISTRATIONNUMBER", ObjRider.VEHICLEREGISTRATIONNUMBER);
                    command.Parameters.AddWithValue("@ADDRESS", ObjRider.ADDRESS);
                    command.Parameters.AddWithValue("@CNIC", ObjRider.CNIC);

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