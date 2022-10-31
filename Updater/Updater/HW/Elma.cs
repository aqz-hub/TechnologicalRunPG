using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Updater.HW
{
    class Elma
    {
        public static async Task<List<object>> SqlQueryAsync(string query)
        {
            SqlDataReader reader = null;
            string connectionString = @"Server=10.59.4.20;Initial Catalog=eris1901;User ID=galihanovdd;Password=SQLGhbdtn!";
            string sql = query;
            List<object> listRead = new List<object>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                                listRead.Add(reader[i].ToString());
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                listRead = null;
            }
            return listRead;
        }
        public static List<object> SqlQuery(string query)
        {
            SqlDataReader reader = null;
            string connectionString = @"Server=10.59.4.20;Initial Catalog=eris1901;User ID=galihanovdd;Password=SQLGhbdtn!";
            string sql = query;
            List<object> listRead = new List<object>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                                listRead.Add(reader[i].ToString());
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                listRead = null;
            }
            return listRead;
        }
    }
}
