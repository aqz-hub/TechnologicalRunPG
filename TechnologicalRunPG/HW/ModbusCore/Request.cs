using TechnologicalRunPG.CustomComponents;

namespace TechnologicalRunPG
{
    public class Request
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public byte[] Message;
        /// <summary>
        /// Ответ
        /// </summary>
        public byte[] result = new byte[0];
        /// <summary>
        /// Режим
        /// </summary>
        public string Mode;
        /// <summary>
        /// Групппа регистров
        /// </summary>
        public int RegisterGroup;
        /// <summary>
        /// Тип сенсора
        /// </summary>
        public string SensorType;
        /// <summary>
        /// Модуль
        /// </summary>
        public Module module;
        /// <summary>
        /// Экземпляр ПГ
        /// </summary>
        public PG pg;
        /// <summary>
        /// Экземпляр сенсора
        /// </summary>
        public Sensor sensor;
        /// <summary>
        /// Приоритетность заявки
        /// </summary>
        public Priority priority;
    }
}
