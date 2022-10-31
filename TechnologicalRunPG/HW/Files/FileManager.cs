using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace TechnologicalRunPG.HW
{
    public static class FileManager
    {
        /// <summary>
        /// Метод сериализации
        /// </summary>
        /// <param name="filename">Сериализуемый файл</param>
        /// <param name="obj">Сериализуемый объект</param>
        /// <returns></returns>
        public static bool Serialize(string filename, object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                try
                {
                    formatter.Serialize(fs, obj);
                    return true;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    return false;
                }
            }
        }
        /// <summary>
        /// Метод десериализации
        /// </summary>
        /// <param name="filename">Десериализуемый файл</param>
        /// <returns></returns>
        public static object Deserialize(string filename)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                try
                {
                    return formatter.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }
    }
}
