using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TechnologicalRunPG.HW;

namespace TechnologicalRunPG.CustomComponents
{
    /// <summary>
    /// Логика взаимодействия для Module.xaml
    /// </summary>
    public partial class Module : UserControl, INotifyPropertyChanged
    {
        #region Binding
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        private string _modbusrange;
        /// <summary>
        /// Диапазон модбас адресов
        /// </summary>
        public string ModbusRange
        {
            get { return _modbusrange; }
            set
            {
                _modbusrange = value;
                OnPropertyChanged("ModbusRange");
            }
        }
        #endregion

        /// <summary>
        /// Диапазон модабас адресов
        /// </summary>
        public byte[] ModbusAddresses = new byte[0];
        /// <summary>
        /// Ссылка на сторону
        /// </summary>
        public Side side;

        public Module(Side _target, string modbusRange)
        {
            InitializeComponent();
            DataContext = this;
            refreshButton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.refresh);
            side = _target;
            ModbusRange = modbusRange;
            ModbusAddresses = ConvertToAddressArray(ModbusRange);
        }

        /// <summary>
        /// Убрать заявки из потока
        /// </summary>
        public void RemovePGs()
        {
            for (int i = 0; i < side.pgs.Count; i++)
            {
                for (int j = 0; j < pgPanel.Children.Count; j++)
                {
                    if (side.pgs[i] == pgPanel.Children[j])
                    {
                        side.pgs.Remove(side.pgs[i]);
                    }
                }
            }
        }

        #region Работа с адресами
        /// <summary>
        /// Преобразовать строку с адресами в байтовый массив
        /// </summary>
        /// <returns></returns>
        public byte[] ConvertToAddressArray(string value)
        {
            switch(value)
            {
                case "1-10":
                    return new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                case "11-20":
                    return new byte[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
                case "21-30":
                    return new byte[] { 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
                case "31-40":
                    return new byte[] { 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 };
                case "41-50":
                    return new byte[] { 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 };
                case "51-60":
                    return new byte[] { 51, 52, 53, 54, 55, 56, 57, 58, 59, 60 };
                case "61-70":
                    return new byte[] { 61, 62, 63, 64, 65, 66, 67, 68, 69, 70 };
                case "71-80":
                    return new byte[] { 71, 72, 73, 74, 75, 76, 77, 78, 79, 80 };
                default: return null;
            }
        }
        #endregion

        #region Работа с ПГшками
        /// <summary>
        /// Найти ПГшки
        /// </summary>
        public void FindPG()
        {
            for (int i = 0; i < ModbusAddresses.Count(); i++)
            {
                GetModbusCheck(ModbusAddresses[i]);
            }
        }
        /// <summary>
        /// Добавить одну ПГ
        /// </summary>
        public void SetPG(byte adr)
        {
            PG pg = new PG(adr, this);
            pg.Margin = new Thickness(5, 0, 0, 0);
            GetSerialNumber(adr, pg);
            pgPanel.Children.Add(pg);
            side.pgs.Add(pg);
        }
        #endregion

        #region Заявки
        /// <summary>
        /// Создание заявки на заводской номер
        /// </summary>
        public void GetSerialNumber(byte mb, PG pG)
        {
            Request request = new Request();
            request.Message = new byte[] { mb, 3, 0, 1, 0, 2 };
            request.result = new byte[0];
            request.Mode = RequestType.SerialNumber;
            request.pg = pG;
            request.module = this;
            pG.Connecting = true;
            request.priority = Priority.SerialNumber;
            side.requests.Enqueue(request);
        }
        /// <summary>
        /// Создание заявки на заряд батареи
        /// </summary>
        public void GetBatteryCharge(byte mb, PG pG)
        {
            Request request = new Request();
            request.Message = new byte[] { mb, 04, 00, 0x16, 00, 01 };
            request.result = new byte[0];
            request.Mode = RequestType.BatteryCharge;
            request.pg = pG;
            request.module = this;
            request.priority = Priority.BatteryCharge;
            side.requests.Enqueue(request);
        }
        /// <summary>
        /// Заявка на проверку модбас адреса
        /// </summary>
        public void GetModbusCheck(byte mbAdr)
        {
            Request request = new Request();
            request.Message = new byte[] { mbAdr, 4, 00, 00, 00, 0x01 };
            request.result = new byte[0];
            request.Mode = RequestType.ModbusCheck;
            request.module = this;
            request.priority = Priority.ModbusCheck;
            side.requests.Enqueue(request);
        }
        #endregion

        #region Обработчики заявок
        /// <summary>
        /// Обработчик заводского номера
        /// </summary>
        public void HandlerSerialNumber(Request result)
        {
            if (result.result != null)
            {
                result.pg.DeviceNumber = PG.ShortsToInteger(PG.BytesToShort(result.result[5], result.result[6]), PG.BytesToShort(result.result[3], result.result[4])).ToString();
                if (!result.pg.DeviceNumber.StartsWith("414"))
                {
                    result.pg.DeviceNumber = PG.ShortsToInteger(PG.BytesToShort(result.result[3], result.result[4]), PG.BytesToShort(result.result[5], result.result[6])).ToString();
                }
                result.pg.StartTimer();
                result.pg.GetPGVersion();
                result.pg.Connecting = false;
                result.pg.IsRun = true;
                for (int i = 0; i < result.pg.registersGroups.Count; i++)
                {
                    result.pg.GetRegisters(result.pg.BuildMessageForGroup(i), i);
                }
                GetBatteryCharge(result.Message[0], result.pg);
            }
            else
            {
                GetSerialNumber(result.Message[0], result.pg);
            }
        }
        /// <summary>
        /// Обработчик заряда батареи
        /// </summary>
        /// <param name="result"></param>
        public void HandlerBatteryCharge(Request result)
        {
            if (result.result != null)
            {
                result.pg.BatteryCharge = PG.BytesToShort(result.result[3], result.result[4]);
            }
            else
            {
                GetBatteryCharge(result.Message[0], result.pg);
            }
        }
        /// <summary>
        /// Обработчик проверки модбас адресов
        /// </summary>
        /// <param name="result"></param>
        public void HandlerModbusCheck(Request result)
        {
            if (result.result != null)
            {
                if(result.result.Count() != 0)
                {
                    int index = side.pgs.FindIndex(item => item.ModbusAddressByte == result.result[0]);
                    if (index == -1)
                    {
                        side.target_form.DeviceCount++;
                        Dispatcher.Invoke(() => SetPG(result.result[0]));
                    }
                }
            }
            else
            {
                GetModbusCheck(result.Message[0]);
            }
        }
        #endregion

        #region Обработчики компонента
        #region Кнопка перезапуска прогона
        private void refreshButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            refreshButton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.refresh_grey);
        }

        private void refreshButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            refreshButton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.refresh);
        }

        private void refreshButton_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Вы действительно хотите перезапустить прогон для данного модуля?", "Перезапуск", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                pgPanel.Children.Clear();
                RemovePGs();
                FindPG();
            }
        }
        #endregion
        private void connectBox_Checked(object sender, RoutedEventArgs e)
        {
            runBar.Visibility = Visibility.Visible;
            FindPG(); 
        }

        private void connectBox_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Вы действительно хотите отключить данный модуль от прогона?", "Отключение", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
            if(result == System.Windows.Forms.DialogResult.Yes)
            {
                runBar.Visibility = Visibility.Hidden;
                pgPanel.Children.Clear();
                RemovePGs();
            }
        }
        #endregion
    }
}
