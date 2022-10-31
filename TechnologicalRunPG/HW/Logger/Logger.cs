using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using TechnologicalRunPG.CustomComponents;
using TechnologicalRunPG.HW.RegisterStructure;

namespace TechnologicalRunPG.HW.Logger
{
    class Logger
    {
        /// <summary>
        /// Лист для хранения запией.
        /// </summary>
        public List<LogStructure> LogStructures = new List<LogStructure>();
        /// <summary>
        /// Таймер записи.
        /// </summary>
        Timer timer;
        /// <summary>
        /// Путь к файлу для записи логов.
        /// </summary>
        string Path { get; } = @"C:\Technological Run PG Logs\";
        PG target_form = null;
        /// <summary>
        /// Конструктор для запуска таймера.
        /// </summary>
        public Logger(PG pg)
        {
            target_form = pg;
            Thread taskTimer = new Thread(StartTimer);
            taskTimer.Start();
        }
        /// <summary>
        /// Запуск таймера
        /// </summary>
        public void StartTimer()
        {
            #region Запуск таймера на обновление 
            TimerCallback tm = new TimerCallback(WriteData);
            timer = new Timer(tm, 0, 0, 60000);
            #endregion
        }
        /// <summary>
        /// Записать данные в файлы.
        /// </summary>
        /// <param name="obj"></param>
        public void WriteData(object obj)
        {
            List<LogStructure> LogStructuresLocal = new List<LogStructure>();
            for (int i = 0; i < LogStructures.Count; i++)
            {
                LogStructuresLocal.Add(LogStructures[i]);
            }
            #region Проверить директорию записи

            #endregion
            foreach (var b in LogStructuresLocal)
            {
                #region Проходим по всем записям и пишем каждый датчик в свой файл

                #region Определить название файла для этой записи
                string filePath = Path + DateTime.Now.Year + $"\\{DateTime.Now.Month}\\{b.DeviceName}" + ".csv";
                #endregion

                if (!Directory.Exists(Path + DateTime.Now.Year + $"\\{DateTime.Now.Month}\\"))
                {
                    Directory.CreateDirectory(Path + DateTime.Now.Year + $"\\{DateTime.Now.Month}\\");
                }
                string toWrite = "";

                #region Сделать шапку в файле, если такого парня ещё не существует
                if (!File.Exists(filePath))
                {
                    toWrite += "Время;";
                    foreach (var c in b.RegistersToWrite)
                    {
                        Register register = (Register)c.register;
                        toWrite += $"{register.RegisterName} ({register.RegisterNumber});";
                    }
                    toWrite += "\n";
                }
                #endregion

                #region Создать строку регистров
                toWrite += b.Time + ";";
                foreach (var c in b.RegistersToWrite)
                {
                    toWrite += c.RegisterValue + ";";
                }
                toWrite += "\n";
                #endregion

                #region Записать строку в файл
                if(!ReserveFile(toWrite, filePath))
                {
                    //target_form.dataWindow.errorViewer.Items.Add("Не удалось записать лог в файл: " + filePath); 
                }
                #endregion
                #endregion

                LogStructures.Remove(b);
            }
        }
        /// <summary>
        /// Резервный файл с сохранением. 
        /// </summary>
        public bool ReserveFile(string _toWrite, string path)
        {
            #region Попробовать записть в файл, вернуть результат
            try
            {
                File.AppendAllText(path, _toWrite, Encoding.GetEncoding(1251));
                return true;
            }
            catch
            {
                return false;
            }
            #endregion
        }
    }
}
