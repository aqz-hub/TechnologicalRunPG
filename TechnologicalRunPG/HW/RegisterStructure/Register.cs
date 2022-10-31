using System;

namespace TechnologicalRunPG.HW.RegisterStructure
{
    [Serializable]
    public class Register
    {
        /// <summary>
        /// Номер регистра
        /// </summary>
        public int RegisterNumber { get; set; }
        /// <summary>
        /// Тип данных регистра
        /// </summary>
        public string RegisterType { get; set; }
        /// <summary>
        /// Имя регистра
        /// </summary>
        public string RegisterName { get; set; }
        /// <summary>
        /// Минимальное значение регистра
        /// </summary>
        public int RegisterMinValue { get; set; }
        /// <summary>
        /// Максимальное значение регистра
        /// </summary>
        public int RegisterMaxValue { get; set; }
        /// <summary>
        /// Функция чтения
        /// </summary>
        public byte ReadFunction { get; set; }
        /// <summary>
        /// Номер группы регистра
        /// </summary>
        public int RegisterGroupNumber { get; set; }
    }
}
