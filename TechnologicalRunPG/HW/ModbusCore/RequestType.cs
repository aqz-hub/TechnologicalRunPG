namespace TechnologicalRunPG
{
    public class RequestType
    {
        /// <summary>
        /// Режим чтения заводского номера
        /// </summary>
        public const string SerialNumber = "SerialNumber";
        /// <summary>
        /// Режим чтения заряда батареи
        /// </summary>
        public const string BatteryCharge = "BatteryCharge";
        /// <summary>
        /// Режим чтения версии
        /// </summary>
        public const string Version = "Version";
        /// <summary>
        /// Режим проверки модбас адреса
        /// </summary>
        public const string ModbusCheck = "ModbusCheck";
        /// <summary>
        /// Режим проверки связи с прибором
        /// </summary>
        public const string DeviceConnection = "DeviceConnection";
        /// <summary>
        /// Режим чтения данных о сенсорах
        /// </summary>
        public const string SensorsInfo = "SensorsInfo";
        /// <summary>
        /// Режим чтения данных сенсора 1
        /// </summary>
        public const string Sensor1Info = "Sensor1Info";
        /// <summary>
        /// Режим чтения данных сенсора 2
        /// </summary>
        public const string Sensor2Info = "Sensor2Info";
        /// <summary>
        /// Режим чтения данных сенсор 3
        /// </summary>
        public const string Sensor3Info = "Sensor3Info";
        /// <summary>
        /// Режим чтения данных сенсор 4
        /// </summary>
        public const string Sensor4Info = "Sensor4Info";
        /// <summary>
        /// Режим чтения регистров сенсора
        /// </summary>
        public const string SensorData = "SensorData";
        /// <summary>
        /// Режим чтения заводского номера сенсора
        /// </summary>
        public const string SensorSerialNumber = "SensorSerialNumber";
        /// <summary>
        /// Режим чтения регистров
        /// </summary>
        public const string RegistersData = "RegistersData";
        /// <summary>
        /// Режим чтения системных ошибок
        /// </summary>
        public const string SystemErrors = "SystemErrors";
    }
}
