using TechnologicalRunPG.HW;

namespace TechnologicalRunPG
{
    public class ElmaUser
    {
        /// <summary>
        /// Логин сотрудника.
        /// </summary>
        public string ElmaUserName { get; set; }
        /// <summary>
        /// MD5 Hash пароля.
        /// </summary>
        public string ElmaMD5Hash { get; set; }
        /// <summary>
        /// Id сотрудника.
        /// </summary>
        public string ElmaUserId { get; set; }
        /// <summary>
        /// Фамилия сотрудника.
        /// </summary>
        public string ElmaUserLastName { get; set; }
        /// <summary>
        /// Имя сотрудника.
        /// </summary>
        public string ElmaUserFirstName { get; set; }
        /// <summary>
        /// Отчество сотрудника.
        /// </summary>
        public string ElmaUserMiddleName { get; set; }
        /// <summary>
        /// Компетенция сотрудника.
        /// </summary>
        public Competences competences { get; set; }
    }
}
