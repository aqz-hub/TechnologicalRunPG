using System.Windows;
using System.Windows.Input;
using TechnologicalRunPG.HW;
using System.IO.Ports;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace TechnologicalRunPG.UI
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        #region Binding
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        private Settings _settings;
        /// <summary>
        /// Настройки
        /// </summary>
        public Settings settings
        {
            get { return _settings; }
            set
            {
                if(value != null)
                {
                    _settings = value;
                    runHourBox.Text = _settings.Hours.ToString();
                    runMinutesBox.Text = _settings.Minutes.ToString();
                    comPortBoxLeft.SelectedItem = _settings.ComPortLeft;
                    comPortBoxRight.SelectedItem = _settings.ComPortRight;
                    modbusRangeBoxLeft.SelectedItem = _settings.ModbusRangeLeft;
                    modbusRangeBoxRight.SelectedItem = _settings.ModbusRangeRight;
                    OnPropertyChanged("settings");
                }
            }
        }
        #endregion

        public SettingsWindow()
        {
            InitializeComponent();

            settings = (Settings)FileManager.Deserialize(Pathes.settingsPath);

            AddComboBoxItems();

            GetComPorts(comPortBoxLeft);
            GetComPorts(comPortBoxRight);
        }
        /// <summary>
        /// Получить COM порты
        /// </summary>
        /// <param name="comboBox"></param>
        private void GetComPorts(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            foreach(string b in SerialPort.GetPortNames())
            {
                comboBox.Items.Add(b);
            }
        }
        /// <summary>
        /// Добавить данные во комбо боксы
        /// </summary>
        public void AddComboBoxItems()
        {
            modbusRangeBoxLeft.Items.Add("1-40");
            modbusRangeBoxLeft.Items.Add("41-80");

            modbusRangeBoxRight.Items.Add("1-40");
            modbusRangeBoxRight.Items.Add("41-80");
        }
        /// <summary>
        /// Принять настройки
        /// </summary>
        public void SaveSettings()
        {
            try
            {
                Settings stgs = new Settings(comPortBoxLeft.SelectedItem.ToString(),
                    comPortBoxRight.SelectedItem.ToString(),
                    modbusRangeBoxLeft.SelectedItem.ToString(),
                    modbusRangeBoxRight.SelectedItem.ToString(),
                    runHourBox.Text, runMinutesBox.Text);
                settings = stgs;
                FileManager.Serialize(Pathes.settingsPath, stgs);
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить настройки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Обработчики
        private void saveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            this.Hide();
        }
        /// <summary>
        /// Чтобы уметь двигать окно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        #region Кнопка закрытия окна
        private void closebutton_MouseEnter(object sender, MouseEventArgs e)
        {
            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.greyclosebutton);
        }
        private void closebutton_MouseLeave(object sender, MouseEventArgs e)
        {
            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.closebutton);
        }
        private void closebutton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
        }
        #endregion

        #endregion
    }
}
