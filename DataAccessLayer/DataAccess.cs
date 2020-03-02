using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Util;

namespace DataAccessLayer
{
    public class AccessLayer
    {
        public static void FetchDataFromSQL(string DBServerName, string DBName, string tableToMigrate, out List<UserEventDTO> DataList)
        {
            DataList = new List<UserEventDTO>();
            string ConnectionString = Utility.GetTransactionDBConnectionstring(DBServerName, DBName);
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand("logs.GetEventlogRecords", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@TableName", tableToMigrate);
                    command.Parameters.AddWithValue("@IsDateRange", 0);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                UserEventDTO data = new UserEventDTO
                                {
                                    OrgId = reader["Org_id"] != DBNull.Value ? int.Parse(reader["Org_id"].ToString()) : 0,
                                    IpAddr = reader["ip"] != DBNull.Value ? reader["ip"].ToString() : string.Empty,
                                    UserId = reader["userid"] != DBNull.Value ? int.Parse(reader["userid"].ToString()) : 0,
                                    Url = reader["url"] != DBNull.Value ? reader["url"].ToString() : string.Empty,
                                    UserAgent = reader["user_agent"] != DBNull.Value ? reader["user_agent"].ToString() : string.Empty,
                                    BranchId = reader["branchid"] != DBNull.Value ? int.Parse(reader["branchid"].ToString()) : 0,
                                    EMRActionid = reader["EMRActionid"] != DBNull.Value ? int.Parse(reader["EMRActionid"].ToString()) : 0,
                                    EMRActionType = reader["EMRActiontype"] != DBNull.Value ? reader["EMRActiontype"].ToString() : string.Empty,
                                    Severity = reader["severity"] != DBNull.Value ? int.Parse(reader["severity"].ToString()) : 0,
                                    ResType = reader["restype"] != DBNull.Value ? reader["restype"].ToString() : string.Empty,
                                    ResDataJSON = reader["resdata_json"] != DBNull.Value ? reader["resdata_json"].ToString() : string.Empty,
                                    ResIds = reader["res_ids"] != DBNull.Value ? reader["res_ids"].ToString() : string.Empty,
                                    Sqlauditid = reader["sqlauditid"] != DBNull.Value ? int.Parse(reader["sqlauditid"].ToString()) : 0
                                };
                                DataList.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
