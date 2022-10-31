using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TechnologicalRunPG.HW.RegisterStructure;

namespace TechnologicalRunPG.CustomComponents
{
    /// <summary>
    /// Логика взаимодействия для PGInfo.xaml
    /// </summary>
    public partial class PGInfo : UserControl, INotifyPropertyChanged
    {
        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        /// <summary>
        /// Таблица с регистрами
        /// </summary>
        public List<DataTable> tableItems = new List<DataTable>();
        /// <summary>
        /// Ссылка на ПГшку
        /// </summary>
        PG target_form = null;

        public PGInfo(PG _target)
        {
            InitializeComponent();
            target_form = _target;
            DataContext = this;
            dataTable.Items.Clear();
            dataTable.ItemsSource = tableItems;
        }

        /// <summary>
        /// Добавить лог
        /// </summary>
        public void AddLog(string text)
        {
            Dispatcher.Invoke(() =>
            {
                if (!errorViewer.Items.Contains(text))
                {
                    errorViewer.Items.Add("[" + DateTime.Now.ToString().Split(' ')[1]  + "]: " + text);
                }
            });
        }

        #region Работа с таблицей
        /// <summary>
        /// Задать таблицу для новой версии
        /// </summary>
        public void SetNewRegTable()
        {
            tableItems.Clear();
            foreach(var b in target_form.registers)
            {
                tableItems.Add(new DataTable(b.RegisterName, "0", true));
            }
        }
        /// <summary>
        /// Обновить таблицу
        /// </summary>
        public void UpdateTable(bool version)
        {
            if(version)
            {
                foreach (var b in target_form.readedRegisters)
                {
                    Register register = (Register)b.register;
                    tableItems[tableItems.FindIndex(item => item.RegName == register.RegisterName)].RegValue = b.RegisterValue;
                    tableItems[tableItems.FindIndex(item => item.RegName == register.RegisterName)].Property = b.IsOk;
                }
            }
            else
            {
                tableItems.Clear();
                for (int i = 0; i < target_form.readedRegisters.Count(); i++)
                {
                    Register register = (Register)target_form.readedRegisters[i].register;
                    tableItems.Add(new DataTable(register.RegisterName, target_form.readedRegisters[i].RegisterValue, target_form.readedRegisters[i].IsOk));
                }
            }
            Dispatcher.Invoke(new Action(() => dataTable.Items.Refresh()));
            Dispatcher.Invoke(new Action(() => chargeLabel.Content = target_form.BatteryCharge + "%"));
        }
        #endregion
    }

    #region Класс таблицы
    public class DataTable
    {
        public DataTable(string regname, string regValue, bool ok)
        {
            this.RegName = regname;
            this.RegValue = regValue;
            this.Property = ok;
        }
        /// <summary>
        /// Имя регистра
        /// </summary>
        public string RegName { get; set; }
        /// <summary>
        /// Значение регистра
        /// </summary>
        public string RegValue { get; set; }
        /// <summary>
        /// Цвет ячейки
        /// </summary>
        public bool Property { get; set; }
    }
    #endregion
}
