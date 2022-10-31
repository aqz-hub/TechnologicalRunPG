using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TechnologicalRunPG.HW;
using TechnologicalRunPG.HW.RegisterStructure;
using TechnologicalRunPG.CustomComponents;

namespace TechnologicalRunPG.UI
{
    /// <summary>
    /// Логика взаимодействия для RegistersWindow.xaml
    /// </summary>
    public partial class RegistersWindow : Window
    {
        #region Списки
        /// <summary>
        /// Список регистров
        /// </summary>
        public List<Register> registers = new List<Register>();
        /// <summary>
        /// Список старых регистров
        /// </summary>
        public List<Register> oldreg = new List<Register>();
        /// <summary>
        /// Список новых регистров
        /// </summary>
        public List<Register> newreg = new List<Register>();
        /// <summary>
        /// Таблица со старыми регистрами
        /// </summary>
        public List<Table> tableItems = new List<Table>(3);
        /// <summary>
        /// Таблица с новыми регистрами
        /// </summary>
        public List<Table> newItems = new List<Table>();
        #endregion

        public RegistersWindow()
        {
            InitializeComponent();
            DrawTable();
        }

        /// <summary>
        /// Рисуем таблицу
        /// </summary>
        public void DrawTable()
        {
            GetRegisters();
            for (int i = 0; i < oldreg.Count; i++)
            {
                tableItems.Add(new Table(oldreg[i].RegisterName, oldreg[i].RegisterType, oldreg[i].RegisterNumber.ToString(), oldreg[i].RegisterMinValue.ToString(), oldreg[i].RegisterMaxValue.ToString(), oldreg[i].RegisterGroupNumber.ToString(), oldreg[i].ReadFunction.ToString()));
            }
            for (int i = 0; i < newreg.Count; i++)
            {
                newItems.Add(new Table(newreg[i].RegisterName, newreg[i].RegisterType, newreg[i].RegisterNumber.ToString(), newreg[i].RegisterMinValue.ToString(), newreg[i].RegisterMaxValue.ToString(), newreg[i].RegisterGroupNumber.ToString(), newreg[i].ReadFunction.ToString()));
            }
            newRegTable.ItemsSource = newItems;
            table.ItemsSource = tableItems;
        }

        #region Работа с регистрами
        /// <summary>
        /// Получаем регистры
        /// </summary>
        public void GetRegisters()
        {
            oldreg = (List<Register>)FileManager.Deserialize(Pathes.registerPath);
            newreg = (List<Register>)FileManager.Deserialize(Pathes.registerV3Path);
        }
        /// <summary>
        /// Сохранить регистры
        /// </summary>
        public bool SaveRegisters(List<Table> items, string path)
        {
            registers.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                Register register = new Register();
                register.RegisterNumber = Convert.ToInt32(items[i].RegNumber);
                register.RegisterType = items[i].RegTypeData;
                register.RegisterName = items[i].RegName;
                register.RegisterMinValue = Convert.ToInt32(items[i].MinValue);
                register.RegisterMaxValue = Convert.ToInt32(items[i].MaxValue);
                register.ReadFunction = Convert.ToByte(items[i].Function);
                register.RegisterGroupNumber = Convert.ToInt32(items[i].RegGroup);
                registers.Add(register);
            }
            return FileManager.Serialize(path, registers);
        }
        #endregion

        #region Кнопка открытия окна
        private void closebutton_MouseEnter(object sender, MouseEventArgs e)
        {
            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.greyclosebutton);
        }

        private void closebutton_MouseLeave(object sender, MouseEventArgs e)
        {
            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.closebutton);
        }

        private void closebutton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }

        private void closebutton_Loaded(object sender, RoutedEventArgs e)
        {
            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.closebutton);
        }
        #endregion

        #region Обработчики
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.Left = System.Windows.Forms.Screen.AllScreens[1].WorkingArea.Location.X;
            this.Top = System.Windows.Forms.Screen.AllScreens[1].WorkingArea.Location.Y;
            this.WindowState = WindowState.Maximized;
        }

        private void saveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if(SaveRegisters(tableItems, Pathes.registerPath))
            {
                MessageBox.Show("Регистры успешно сохранены!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Hide();
            }
        }
        private void saveNewRegButton_Click(object sender, RoutedEventArgs e)
        {
            if(SaveRegisters(newItems, Pathes.registerV3Path))
            {
                MessageBox.Show("Регистры успешно сохранены!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Hide();
            }
        }
        #endregion
    }
    #region Класс таблицы
    public class Table
    {
        public Table(string regname, string regtype, string regnumber, string min, string max, string regGroup, string func)
        {
            this.RegName = regname;
            this.RegTypeData = regtype;
            this.RegNumber = regnumber;
            this.MinValue = min;
            this.MaxValue = max;
            this.Function = func;
            this.RegGroup = regGroup;
        }
        /// <summary>
        /// Имя регистра
        /// </summary>
        public string RegName { get; set; }
        /// <summary>
        /// Тип данных регистра
        /// </summary>
        public string RegTypeData { get; set; }
        /// <summary>
        /// Номер регистра
        /// </summary>
        public string RegNumber { get; set; }
        /// <summary>
        /// Минимальное значение
        /// </summary>
        public string MinValue { get; set; }
        /// <summary>
        /// Максимальное значение
        /// </summary>
        public string MaxValue { get; set; }
        /// <summary>
        /// Функция для чтения
        /// </summary>
        public string Function { get; set; }
        /// <summary>
        /// Группа регистра
        /// </summary>
        public string RegGroup { get; set; }
    }
    #endregion
}
