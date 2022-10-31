using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;
using TechnologicalRunPG.HW;

namespace TechnologicalRunPG
{
    public class Elma
    {        
        /// <summary>
        /// Проверить пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public ElmaUser CheckElmaUser(string userId, string login)
        {
            void Zapolnit(string[] str, ElmaUser usr)
            {
                usr.ElmaUserName = str[4];
                usr.ElmaUserId = str[1];
                usr.ElmaUserLastName = str[7];
                usr.ElmaUserFirstName = str[5];
                usr.ElmaUserMiddleName = str[6];
            }

            List<string[]> UsersSpisok = new List<string[]>();

            ElmaUser tempUser = new ElmaUser();

            string query = "SELECT" +
                           " us.Uid," +
                           " us.Id," +
                           " us.OrganizationGroupsHash," +
                           " us.Status," +
                           " us.UserName," +
                           " us.FirstName," +
                           " us.MiddleName," +
                           " us.LastName," +
                           " us.FullName" +
                           " FROM[User] as us" +
                           " where Status = 0" +
                           " and us.Id = " + userId;

            UsersSpisok = ElmaConnect.SqlQuery(query, 12);
            if (UsersSpisok.Count == 0)
            {
                return null;
            }
            else
            {
                try
                {
                    Zapolnit(UsersSpisok.Find(x => x[4].ToLower() == login.ToLower()), tempUser);

                    string SQLQuery = "select ks.Ocenka, ks.DataAttestacii, k.PeriodichnostjAttestacii" +
                                      " from Kompetencii_KompetenciyaSotr ks" +
                                      " left join Kompetencii k on k.Id = ks.Parent" +
                                      " where k.Name like '%Технологический прогон ПГ%' and Lineyka like 'ПГ'" +
                                      " and ks.Sotrudnik like '" + userId + "'";

                    List<object> result = ElmaConnect.SqlQuery(SQLQuery);
                    if(result.Count != 0)
                    {
                        tempUser.competences = new Competences();
                        tempUser.competences.score = result[0].ToString();
                        tempUser.competences.attestationDate = Convert.ToDateTime(result[1].ToString());
                        tempUser.competences.ticksReattestation = Convert.ToInt64(result[2]);
                    }
                    return tempUser;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
