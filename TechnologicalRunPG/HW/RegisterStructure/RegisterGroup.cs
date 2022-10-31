using System.Collections.Generic;

namespace TechnologicalRunPG.HW.RegisterStructure
{
    public class RegisterGroup
    {
        /// <summary>
        /// Регистры
        /// </summary>
        public List<Register> registers { get; set; }
        /// <summary>
        /// Номер группы регистров
        /// </summary>
        public int GroupNumber { get; set; }
        /// <summary>
        /// Номер функции чтения для данной группы
        /// </summary>
        public byte ReadFunction { get; set; }
    }
}
