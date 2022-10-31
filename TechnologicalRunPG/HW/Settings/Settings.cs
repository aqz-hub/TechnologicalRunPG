using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnologicalRunPG
{
    [Serializable]
    public class Settings
    {
        public Settings(string comportLeft, string comportRight, string modbusLeft, string modbusRight, string hour, string min)
        {
            ComPortLeft = comportLeft;
            ComPortRight = comportRight;
            ModbusRangeLeft = modbusLeft;
            ModbusRangeRight = modbusRight;
            Hours = Convert.ToInt32(hour);
            Minutes = Convert.ToInt32(min);
        }
        /// <summary>
        /// COM порт слева
        /// </summary>
        public string ComPortLeft { get; set; }
        /// <summary>
        /// COM порт справа
        /// </summary>
        public string ComPortRight { get; set; }
        /// <summary>
        /// Диапазон модбас адресов слева
        /// </summary>
        public string ModbusRangeLeft { get; set; }
        /// <summary>
        /// Диапазон модбас адресов справа
        /// </summary>
        public string ModbusRangeRight { get; set; }
        /// <summary>
        /// Часы прогона
        /// </summary>
        public int Hours { get; set; }
        /// <summary>
        /// Минуты прогона
        /// </summary>
        public int Minutes { get; set; }
    }
}
