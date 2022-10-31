using System;

namespace TechnologicalRunPG.HW.RegisterStructure
{
    [Serializable]
    public class RegisterSensor
    {
        /// <summary>
        /// Имя регистра
        /// </summary>
        public string RegisterName { get; set; }
        /// <summary>
        /// Тип данных регистра
        /// </summary>
        public string RegisterType { get; set; }
        /// <summary>
        /// Минимальное значение регистра
        /// </summary>
        public int RegisterMinValue { get; set; }
        /// <summary>
        /// Максимальное значение регистра
        /// </summary>
        public int RegisterMaxValue { get; set; }
    }
}
