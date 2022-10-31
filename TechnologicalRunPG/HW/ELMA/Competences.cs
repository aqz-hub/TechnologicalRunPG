using System;

namespace TechnologicalRunPG.HW
{
    /// <summary>
    /// Класс хранения данных о компетенции сотрудника
    /// </summary>
    public class Competences
    {
        /// <summary>
        /// Оценка сотрудника на текущем процессе.
        /// </summary>
        public string score = "";
        /// <summary>
        /// Дата аттестации сотрудника.
        /// </summary>
        public DateTime attestationDate = new DateTime();
        /// <summary>
        /// Дата переаттестации сотрудника.
        /// </summary>
        public Int64 ticksReattestation = 0;
        /// <summary>
        /// Доступ человека к работе.
        /// </summary>
        public bool access = false;
        /// <summary>
        /// Предупреждающее сообщение, если скоро заканчивается аттестация.
        /// </summary>
        public string warningMessage = "";
        /// <summary>
        /// Проверить аттестацию сотрудника.
        /// </summary>
        /// <returns></returns>
        public void CheckAttestation()
        {
            DateTime attestationDeadLine = new DateTime();
            attestationDeadLine = attestationDate.AddTicks(ticksReattestation);
            if (attestationDeadLine > DateTime.Now)
            {
                access = true;
                int difference = (attestationDeadLine - DateTime.Now).Days;
                if (difference < 7)
                {
                    warningMessage = "До конца Вашей аттестации осталось " + difference + " дней. Пройдите аттестацию повторно!";
                }
            }
            else
            {
                access = false;
                warningMessage = "Ваша аттестация закончилась " + attestationDeadLine.ToShortDateString() + " пройдите аттестацию повторно!";
            }
        }
    }
}
