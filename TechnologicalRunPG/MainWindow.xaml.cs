using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TechnologicalRunPG.UI;
using TechnologicalRunPG.HW;
using TechnologicalRunPG.CustomComponents;
using System.IO.Ports;
using System.Xml;

namespace TechnologicalRunPG
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Ссылки
        /// <summary>
        /// Окно с регистрами
        /// </summary>
        public RegistersWindow regWindow = new RegistersWindow();
        /// <summary>
        /// Окно с настройками
        /// </summary>
        public SettingsWindow settingsWindow = new SettingsWindow();
        /// <summary>
        /// Фотография
        /// </summary>
        public Picture picture;
        /// <summary>
        /// Окно с данными
        /// </summary>
        public DataWindow dataWindow = new DataWindow();
        /// <summary>
        /// Окно с легендой прогона
        /// </summary>
        public LegendWindow legendWindow = new LegendWindow();
        /// <summary>
        /// Левая сторона
        /// </summary>
        Side side_left = null;
        /// <summary>
        /// Правая сторона
        /// </summary>
        Side side_right = null;
        #endregion

        #region Привязки
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Количество ПГ на прогоне
        public int _deviceCount;
        /// <summary>
        /// Количество ПГ на прогоне
        /// </summary>
        public int DeviceCount
        {
            get { return _deviceCount; }
            set
            {
                _deviceCount = value;
                OnPropertyChanged("DeviceCount");
            }
        }
        #endregion

        #region Статус прогона
        public bool _running;
        /// <summary>
        /// Статус прогона
        /// </summary>
        public bool Running
        {
            get { return _running; }
            set
            {
                _running = value;
                if (value)
                {
                    statusLabel.Content = "включен";
                    DeviceCount = 0;
                    statusLabel.Foreground = System.Windows.Media.Brushes.Green;
                    stopButton.Visibility = Visibility.Visible;
                    startButton.Visibility = Visibility.Hidden;
                }
                else
                {
                    statusLabel.Content = "отключен";
                    DeviceCount = 0;
                    statusLabel.Foreground = System.Windows.Media.Brushes.Red;
                    startButton.Visibility = Visibility.Visible;
                    stopButton.Visibility = Visibility.Hidden;
                }
                OnPropertyChanged("Running");
            }
        }
        #endregion

        #region Настройки протокола
        private ProtocolSettings _protocolsettings;
        /// <summary>
        /// Настройки протокола
        /// </summary>
        public ProtocolSettings protocolSettings
        {
            get { return _protocolsettings; }
            set
            {
                if(value != null)
                {
                    _protocolsettings = value;
                    OnPropertyChanged("ProtocolSettings");
                }
            }
        }
        #endregion
        #endregion

        #region Версии
        /// <summary>
        /// Список актуальных версий
        /// </summary>
        public List<Versions> versions = new List<Versions>();
        /// <summary>
        /// Версия программы
        /// </summary>
        public static string Version { get; set; }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            SetImage();
            //this.Hide();
            //ElmaLogin login = new ElmaLogin(this);
            //login.ShowDialog();

            legendButton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.legend);

            DataContext = this;
            GetProgrammVersion();
            GetElmaVerisons();
            Running = false;

            protocolSettings = (ProtocolSettings)FileManager.Deserialize(Pathes.protocolSettingsPath);

            dataWindow.Show();
            StartTimer();
        }

        /// <summary>
        /// Получить версию программы
        /// </summary>
        public void GetProgrammVersion()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Version.xml");
            versionLabel.Content = "v." + Convert.ToSingle(doc.GetElementsByTagName("version")[0].InnerText.Replace('.', ',')).ToString().Replace(",", ".");
            Version = versionLabel.Content.ToString().Replace("v.", "");
        }
        /// <summary>
        /// Сохранить номер протокола
        /// </summary>
        private void SaveProtocolNumber()
        {
            FileManager.Serialize(Pathes.protocolSettingsPath, protocolSettings);
        }

        #region Работа с элмой
        /// <summary>
        /// Получить все версии из элмы
        /// </summary>
        public void GetElmaVerisons()
        {
            string query = @"select Versiya from ReestrPO
                            where Status like 'В производство' and Versiya like '%ПГ%'";
            List<object> result = ElmaConnect.SqlQuery(query);
            if (result.Count != 0)
            {
                for(int i = 0; i < result.Count; i++)
                {
                    string[] spl = result[i].ToString().Split(' ');
                    Versions version = new Versions()
                    {
                        Version = spl[spl.Length - 1].Replace(".", "").Replace(",", "")
                    };
                    versions.Add(version);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Не удалось установить связь с Elma!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Получить задачу
        /// </summary>
        public HW.ELMA.Task GetTask(string deviceNumber)
        {
            string query = @"SELECT TaskBase.Id FROM TaskBase
                            LEFT OUTER JOIN WorkflowBookmark ON Taskbase.WorkflowBookmark = WorkflowBookmark.Id
                            LEFT OUTER JOIN WorkflowInstance ON WorkflowBookmark.Instance = WorkflowInstance.Id
                            LEFT OUTER JOIN WorkflowProcess ON WorkflowInstance.Process = WorkflowProcess.Id
                            LEFT OUTER JOIN P_SborkaPG ON WorkflowInstance = WorkflowInstance.Id
                            LEFT OUTER JOIN DGS210 dgs on dgs.id = P_SborkaPG.DGS210
                            where
                            TaskBase.Subject like '%Технологический прогон ПГ%' and
                            TaskBase.EndWorkDate is null and dgs.ZavNomer like '" + deviceNumber + "'";
            List<object> result = ElmaConnect.SqlQuery(query);
            if (result.Count != 0)
            {
                return new HW.ELMA.Task() { TaskId = result[0].ToString() };
            }
            return null;
        }
        #endregion

        #region Таймер для вывода даты и времени
        /// <summary>
        /// Timer
        /// </summary>
        System.Windows.Forms.Timer dateTimer = new System.Windows.Forms.Timer();
        /// <summary>
        /// Запустить таймер
        /// </summary>
        public void StartTimer()
        {
            dateTimer.Enabled = true;
            dateTimer.Tick += dateTimer_Tick;
            dateTimer.Interval = 1000;
        }
        /// <summary>
        /// Timer tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dateTimer_Tick(object sender, EventArgs e)
        {
            dateLabel.Content = DateTime.Now.ToString();
        }
        #endregion

        #region Работа со сторонами
        /// <summary>
        /// Установить стороны
        /// </summary>
        public void SetSides()
        {
            side_left = new Side(this, settingsWindow.settings.ComPortLeft, settingsWindow.settings.ModbusRangeLeft);
            side_left.Margin = new Thickness(20, 0, 0, 0);
            side_right = new Side(this, settingsWindow.settings.ComPortRight, settingsWindow.settings.ModbusRangeRight);
            side_right.Margin = new Thickness(50, 0, 0, 0);
        }
        /// <summary>
        /// Убрать стороны
        /// </summary>
        public void DeleteSides()
        {
            panel.Children.Clear();
        }
        #endregion

        #region Запуск тех прогона
        /// <summary>
        /// Запустить прогон
        /// </summary>
        public void StartStopTechRun()
        {
            if (Running)
            {
                Running = false;
                dataWindow.pgInfoPanel.Children.Clear();
                DeviceCount = 0;
                DeleteSides();
            }
            else
            {
                Running = true;
                SetSides();
            }
        }
        #endregion

        #region Elma
        /// <summary>
        /// Установить картинку.
        /// </summary>
        public void SetImage()
        {
            picture = new Picture();
            picture.Margin = new Thickness(1425, 10, 0, 0);
            stack.Children.Add(picture);
            stack.UpdateLayout();
        }
        #endregion

        #region Обработчики
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            StartStopTechRun();
        }
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            if(Running)
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show("Идёт технологичский прогон! Вы действительно хотите прервать его?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == System.Windows.Forms.DialogResult.Yes)
                {
                    StartStopTechRun();
                }
            }
            else
            {
                StartStopTechRun();
            }
        }

        #region Кнопка открытия легенды
        private void legendButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            legendButton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.legendgrey);
        }

        private void legendButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            legendButton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.legend);
        }

        private void legendButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            legendWindow.Show();
        }
        #endregion

        #region Кнопка закрытия окна
        private void closebutton_Loaded(object sender, RoutedEventArgs e)
        {
            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.closebutton);
        }
        private void closebutton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Running)
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show("Идёт технологичский прогон! Вы действительно хотите прервать его?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveProtocolNumber();
                    Environment.Exit(0);
                }
            }
            else
            {
                SaveProtocolNumber();
                Environment.Exit(0);
            }
        }

        private void closebutton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.greyclosebutton);
        }

        private void closebutton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.closebutton);
        }
        #endregion

        #region Кнопка открытия настроек
        private void opensettingsbutton_Loaded(object sender, RoutedEventArgs e)
        {
            opensettingsbutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.settings_icon);
        }
        /// <summary>
        /// Открыть настройки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void opensettingsbutton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            settingsWindow.ShowDialog();
        }

        private void opensettingsbutton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            opensettingsbutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.settings_icon_grey);
        }

        private void opensettingsbutton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            opensettingsbutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.settings_icon);
        }
        #endregion

        #region Кнопка открытия окна с регистрами
        private void openRegisterWindow_Loaded(object sender, RoutedEventArgs e)
        {
            openRegisterWindow.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.register);
        }

        private void openRegisterWindow_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            openRegisterWindow.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.register_grey);
        }

        private void openRegisterWindow_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            openRegisterWindow.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.register);
        }

        private void openRegisterWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            regWindow.ShowDialog();
        }
        #endregion

        #region Управление окнами
        /// <summary>
        /// Текущий экран.
        /// </summary>
        public static int curScreen = 0;
        private void changeWindowButton_Loaded(object sender, RoutedEventArgs e)
        {
            changeWindowButton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.changewindowbutton);
            this.WindowState = WindowState.Normal;
            this.Left = System.Windows.Forms.Screen.AllScreens[1].WorkingArea.Location.X;
            this.Top = System.Windows.Forms.Screen.AllScreens[1].WorkingArea.Location.Y;
            curScreen = 1;
            this.WindowState = WindowState.Maximized;
        }

        private void changeWindowButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            changeWindowButton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.greychangewindow);
        }

        private void changeWindowButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            changeWindowButton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.changewindowbutton);
        }

        private void changeWindowButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            #region Управление расположением окна
            if (System.Windows.Forms.Screen.AllScreens.Count() > 1)
            {
                if (curScreen == 0)
                {
                    this.WindowState = WindowState.Normal;
                    this.Left = System.Windows.Forms.Screen.AllScreens[1].WorkingArea.Location.X;
                    this.Top = System.Windows.Forms.Screen.AllScreens[1].WorkingArea.Location.Y;
                    curScreen = 1;
                    this.WindowState = WindowState.Maximized;

                    dataWindow.WindowState = WindowState.Normal;
                    dataWindow.Left = System.Windows.Forms.Screen.AllScreens[0].WorkingArea.Location.X;
                    dataWindow.Top = System.Windows.Forms.Screen.AllScreens[0].WorkingArea.Location.Y;
                    dataWindow.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                    this.Left = System.Windows.Forms.Screen.AllScreens[0].WorkingArea.Location.X;
                    this.Top = System.Windows.Forms.Screen.AllScreens[0].WorkingArea.Location.Y;
                    curScreen = 0;
                    this.WindowState = WindowState.Maximized;

                    dataWindow.WindowState = WindowState.Normal;
                    dataWindow.Left = System.Windows.Forms.Screen.AllScreens[1].WorkingArea.Location.X;
                    dataWindow.Top = System.Windows.Forms.Screen.AllScreens[1].WorkingArea.Location.Y;
                    dataWindow.WindowState = WindowState.Maximized;
                }
            }
            #endregion
        }
        #endregion
        #endregion

        #region PWROFF/PWRON
        bool pwr = false;
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (pwr)
            {
                pwr = false;
                pwroffButton.Visibility = Visibility.Hidden;
                pwronButton.Visibility = Visibility.Hidden;
            }
            else
            {
                pwr = true;
                pwroffButton.Visibility = Visibility.Visible;
                pwronButton.Visibility = Visibility.Visible;
            }
        }
        public static void PwrOn(SerialPort Port, bool OnOff)
        {
            if (Port.IsOpen)
            {
                Port.DiscardOutBuffer();
                Port.DiscardInBuffer();

                byte[] messageon = { Encoding.ASCII.GetBytes("#")[0],
                                    Encoding.ASCII.GetBytes("A")[0],
                                    Encoding.ASCII.GetBytes("L")[0],
                                    Encoding.ASCII.GetBytes("L")[0],
                                    Encoding.ASCII.GetBytes("#")[0],
                                    Encoding.ASCII.GetBytes("P")[0],
                                    Encoding.ASCII.GetBytes("W")[0],
                                    Encoding.ASCII.GetBytes("R")[0],
                                    Encoding.ASCII.GetBytes("O")[0],
                                    Encoding.ASCII.GetBytes("N")[0]
                                        };
                byte[] messageoff = { Encoding.ASCII.GetBytes("#")[0],
                                    Encoding.ASCII.GetBytes("A")[0],
                                    Encoding.ASCII.GetBytes("L")[0],
                                    Encoding.ASCII.GetBytes("L")[0],
                                    Encoding.ASCII.GetBytes("#")[0],
                                    Encoding.ASCII.GetBytes("P")[0],
                                    Encoding.ASCII.GetBytes("W")[0],
                                    Encoding.ASCII.GetBytes("R")[0],
                                    Encoding.ASCII.GetBytes("O")[0],
                                    Encoding.ASCII.GetBytes("F")[0],
                                    Encoding.ASCII.GetBytes("F")[0]
                                    };
                byte[] messagesave = {Encoding.ASCII.GetBytes("#")[0],
                                    Encoding.ASCII.GetBytes("A")[0],
                                    Encoding.ASCII.GetBytes("L")[0],
                                    Encoding.ASCII.GetBytes("L")[0],
                                    Encoding.ASCII.GetBytes("#")[0],
                                    Encoding.ASCII.GetBytes("S")[0],
                                    Encoding.ASCII.GetBytes("A")[0],
                                    Encoding.ASCII.GetBytes("V")[0],
                                    Encoding.ASCII.GetBytes("E")[0]
                                    };
                try
                {
                    if (OnOff)
                    {
                        Port.Write(messageon, 0, messageon.Length);
                        Port.Write(messagesave, 0, messagesave.Length);
                    }
                    else
                    {
                        Port.Write(messageoff, 0, messageoff.Length);

                    }
                }
                catch
                {

                }
                Thread.Sleep(100);
            }

        }

        private void pwroffButton_Click(object sender, RoutedEventArgs e)
        {
            PwrOn(side_left.serial, false);
            PwrOn(side_right.serial, false);
        }

        private void pwronButton_Click(object sender, RoutedEventArgs e)
        {
            PwrOn(side_left.serial, true);
            PwrOn(side_right.serial, true);
        }
        #endregion
    }
}