using System;

namespace TechnologicalRunPG.HW.RegisterStructure
{
    public class ReadedRegisters
    {
        /// <summary>
        /// Считанный регистр
        /// </summary>
        public Register register { get; set; }
        /// <summary>
        /// Значение регистра
        /// </summary>
        public string RegisterValue { get; set; }
        /// <summary>
        /// Триггер допуска регистра
        /// </summary>
        public bool IsOk { get; set; }
        /// <summary>
        /// Минимальное значение
        /// </summary>
        public string MinValue { get; set; }
        /// <summary>
        /// Максимальное значение
        /// </summary>
        public string MaxValue { get; set; }
    }
}
