using TechnologicalRunPG.ElmaConnector;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace TechnologicalRunPG
{
    public class ElmaConnect
    {
        /// <summary>
        /// Строка подключения
        /// </summary>
        protected internal readonly static string connectionString = @"null";

        #region SQL запросы
        /// <summary>
        /// SQL - запрос с ответом.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<object> SqlQuery(string query)
        {
            SqlDataReader reader = null;
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
            catch
            {
                listRead = null;
            }
            return listRead;
        }
        /// <summary>
        /// SQL - запрос без ответа.
        /// </summary>
        /// <param name="query"></param>
        public static bool SqlQueryVoid(string query)
        {
            bool success = false;
            string sql = query;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    success = true;
                }
            }
            catch
            {
                success = false;
            }

            return success;
        }
        public static List<string[]> SqlQuery(string query, int kolParametrs)
        {
            SqlDataReader reader = null;
            List<string[]> data = new List<string[]>();
            string sql = query;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            data.Add(new string[kolParametrs]);
                            for (int i = 0; i < kolParametrs; i++)
                            {
                                data[data.Count - 1][i] = reader[i].ToString();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch
            {
                reader = null;
            }
            return data;
        }
        #endregion

        /// <summary>
        /// Закрыть задачу.
        /// </summary>
        /// <param name="user">ID пользователя.</param>
        /// <param name="passw">Пароль в явном виде.</param>
        /// <param name="taskId">ID задачи.</param>
        /// <param name="connectorUid">UID процесса.</param>
        /// <param name="fileName">Название протокола.</param>
        /// <returns></returns>
        public static void CloseTask(string user, string passw, long taskId, string protocolFileName)
        {
            WebData data = new WebData();
            WebDataItem path = new WebDataItem();
            WebDataItem filename = new WebDataItem();
            WebDataItem[] items = new WebDataItem[2];

            path.Data = new WebData();
            path.Name = "ProtocolPath";
            path.Value = Pathes.elmaProtocolPath + protocolFileName;
            items[0] = path;

            filename.Data = new WebData();
            filename.Name = "ProtocolFileName";
            filename.Value = protocolFileName;
            items[1] = filename;

            data.Items = items;
            Disable_CertificateValidation();
            WFPWebServiceSoapClient client = new WFPWebServiceSoapClient();     

            client.ExecuteTask(user, passw, taskId, "default", data);
        }
        /// <summary>
        /// Отключить проверку сертификата.
        /// </summary>
        static void Disable_CertificateValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                )
                {
                    return true;
                };
        }
    }
}