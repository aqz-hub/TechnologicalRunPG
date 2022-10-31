using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnologicalRunPG
{
    public class Pathes
    {
        /// <summary>
        /// Путь к файлу с настройками протокола
        /// </summary>
        protected internal static readonly string protocolSettingsPath = @"C:\Технологический прогон ПГ\protocol.cfg";
        /// <summary>
        /// Путь к файлу с настройками
        /// </summary>
        protected internal static readonly string settingsPath = @"C:\Технологический прогон ПГ\settings.cfg";

        /// <summary>
        /// Путь к файлу с регистрами для новой версии
        /// </summary>
        protected internal static readonly string registerV3Path = @"C:\Технологический прогон ПГ\registersV3.reg";
        /// <summary>
        /// Путь к файлу с регистрами для старой версии
        /// </summary>
        protected internal static readonly string registerPath = @"C:\Технологический прогон ПГ\registers.reg";

        /// <summary>
        /// Путь к файлу с регистрами
        /// </summary>
        protected internal static readonly string sensorRegistersPath = @"C:\Технологический прогон ПГ\registersSensor.reg";
        /// <summary>
        /// Путь к файлу с регистрами для новой версии
        /// </summary>
        protected internal static readonly string sensorV3RegistersPath = @"C:\Технологический прогон ПГ\registersSensorV3.reg";

        /// <summary>
        /// Путь к протоколу на сервере
        /// </summary>
        protected internal static readonly string networkProtocolPath = @"\\10.59.4.20\Exchange2\Протокола_Тех.прогон_ПГ\" + DateTime.Now.ToString("yyyy") + @"\" + DateTime.Now.ToString("MM") + @"\";
        /// <summary>
        /// Локальный путь протокола
        /// </summary>
        protected internal static readonly string localProtocolPath = @"C:\Протоколы тех. прогон ПГ\" + DateTime.Now.ToString("yyyy") + @"\" + DateTime.Now.ToString("MM") + @"\";
        /// <summary>
        /// Путь к протоколу для элмы
        /// </summary>
        protected internal static readonly string elmaProtocolPath = @"D:\ELMA3 - Standart\UserConfig\Files\Exchange\Протокола_Тех.прогон_ПГ\" + DateTime.Now.ToString("yyyy") + @"\" + DateTime.Now.ToString("MM") + @"\";
    }
}
