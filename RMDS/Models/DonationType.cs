﻿using MySql.Data.MySqlClient;
using RMDS.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMDS.Models
{
    public class DonationType
    {
        public int DonationTypeId { get; set; }
        public string TypeName { get; set; }

    }
    public class DonationTypeManager : BaseManager
    {
        public static List<DonationType> GetDonationType(string whereclause, MySqlConnection conn = null)
        {
            DonationType ObjDD = new DonationType();
            List<DonationType> lstDD = new List<DonationType>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from DonationType";
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
                                ObjDD = ReaderDataDonationType(reader);
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
        private static DonationType ReaderDataDonationType(MySqlDataReader reader)
        {
            DonationType objDD = new DonationType();
            objDD.DonationTypeId = DbCheck.IsValidInt(reader["DonationTypeId"]);
            objDD.TypeName = DbCheck.IsValidString(reader["TypeName"]);

            return objDD;

        }
        public static string SaveDonationType(DonationType ObjDD, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sDDID = "";
            sDDID = ObjDD.DonationTypeId.ToString();
            var templstEmp = GetDonationType("typeid = '" + sDDID + "'", conn);
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
                        command.Parameters.AddWithValue("@typeid", ObjDD.DonationTypeId);
                    }
                    command.Parameters.AddWithValue("@typename", ObjDD.TypeName);

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