using System;
using System.Collections.Generic;
using TechnologicalRunPG.HW.RegisterStructure;

namespace TechnologicalRunPG.HW.Logger
{
    class LogStructure
    {
        /// <summary>
        /// Лист для записи туда регистров.
        /// </summary>
        public List<ReadedRegisters> RegistersToWrite = new List<ReadedRegisters>();
        /// <summary>
        /// Номер прибора
        /// </summary>
        public string DeviceName { get; set; } = "";
        /// <summary>
        /// Время точки.
        /// </summary>
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
