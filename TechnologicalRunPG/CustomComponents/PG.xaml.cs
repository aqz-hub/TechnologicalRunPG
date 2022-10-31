using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TechnologicalRunPG.HW;
using TechnologicalRunPG.UI;
using TechnologicalRunPG.HW.RegisterStructure;
using TechnologicalRunPG.HW.Logger;
using TechnologicalRunPG.HW.ELMA;

namespace TechnologicalRunPG.CustomComponents
{
    /// <summary>
    /// Логика взаимодействия для PG.xaml
    /// </summary>
    public partial class PG : UserControl, INotifyPropertyChanged
    {
        #region Экземпляры
        /// <summary>
        /// Окно с данными
        /// </summary>
        public PGInfo dataWindow;
        /// <summary>
        /// Класс со статусами прогона
        /// </summary>
        Statuses status = new Statuses();
        /// <summary>
        /// Класс с цветами для заднего фона
        /// </summary>
        public ColorsGamma color = new ColorsGamma();
        /// <summary>
        /// Класс с картинками для отображения статуса
        /// </summary>
        StatusImages images = new StatusImages();
        /// <summary>
        /// Логгер
        /// </summary>
        Logger logger;
        /// <summary>
        /// Модуль
        /// </summary>
        public Module module;
        #endregion

        #region Привязка компонентов
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Статус прогона
        private bool _isRun;
        /// <summary>
        /// Идёт прогон (жёлтый фон)
        /// </summary>
        public bool IsRun
        {
            get { return _isRun; }
            set
            {
                _isRun = value;
                if (_isRun)
                {
                    BackColor = color.yellow;
                    StatusContent = status.runing;
                    RunVisibility = Visibility.Visible;
                    StatusVisibility = Visibility.Hidden;
                    OnPropertyChanged("IsRun");
                }
            }
        }
        private bool _connecting;
        /// <summary>
        /// Подключение
        /// </summary>
        public bool Connecting
        {
            get { return _connecting; }
            set
            {
                _connecting = value;
                if (_connecting)
                {
                    BackColor = color.grey;
                    StatusContent = status.loading;
                    StatusImage = images.waitImage;
                    RunVisibility = Visibility.Hidden;
                    StatusVisibility = Visibility.Visible;
                    OnPropertyChanged("Connecting");
                }
            }
        }
        private bool _disconnected;
        /// <summary>
        /// Обрыв связи
        /// </summary>
        public bool Disconnected
        {
            get { return _disconnected; }
            set
            {
                _disconnected = value;
                if(Complete == null)
                {
                    if (_disconnected)
                    {
                        BackColor = color.red;
                        StatusContent = status.disconnected;
                        StatusImage = images.disconnected;
                        RunVisibility = Visibility.Hidden;
                        StatusVisibility = Visibility.Visible;
                    }
                    else
                    {
                        if (!Warning)
                        {
                            BackColor = color.yellow;
                            StatusContent = status.runing;
                            RunVisibility = Visibility.Visible;
                            StatusVisibility = Visibility.Hidden;
                        }
                    }
                }
                OnPropertyChanged("Disconnected");
            }
        }
        private bool _warning;
        /// <summary>
        /// Системная ошибка
        /// </summary>
        public bool Warning
        {
            get { return _warning; }
            set
            {
                _warning = value;
                if(Complete == null)
                {
                    if (_warning)
                    {
                        BackColor = color.red;
                        StatusContent = status.warning;
                        StatusImage = images.warning;
                        RunVisibility = Visibility.Hidden;
                        StatusVisibility = Visibility.Visible;
                    }
                }
                OnPropertyChanged("Warning");
            }
        }
        private bool _outRange;
        /// <summary>
        /// Значение вне диапазона
        /// </summary>
        public bool OutRange
        {
            get { return _outRange; }
            set
            {
                _outRange = value;
                if(Complete == null)
                {
                    if (_outRange)
                    {
                        BackColor = color.red;
                        StatusContent = status.outrange;
                        StatusImage = images.outrange;
                        RunVisibility = Visibility.Hidden;
                        StatusVisibility = Visibility.Visible;
                    }
                }
                OnPropertyChanged("OutRange");
            }
        }
        #endregion

        #region Статус завершения прогона
        private bool? _complete;
        /// <summary>
        /// Прогон завершён
        /// </summary>
        public bool? Complete
        {
            get { return _complete; }
            set
            {
                _complete = value;
                IsRun = false;
                Warning = false;
                Disconnected = false;
                OutRange = false;
                RunVisibility = Visibility.Hidden;
                StatusVisibility = Visibility.Visible;
                StatusContent = status.complete;
                if (Convert.ToBoolean(_complete))
                {
                    BackColor = color.green;
                    StatusImage = images.okImage;
                    Dispatcher.Invoke(() => PDFClass.Create(this));
                    if(TaskFinded)
                    {
                        CloseTask();
                    }
                    else
                    {
                        TaskClosed = false;
                    }
                }
                else
                {
                    BackColor = color.red;
                    StatusImage = images.notOkay;
                }
                OnPropertyChanged("Complete");
            }
        }
        #endregion

        #region Images Visibility
        private Visibility _runVisibility;
        /// <summary>
        /// Видимость каритнки прогона
        /// </summary>
        public Visibility RunVisibility
        {
            get { return _runVisibility; }
            set
            {
                _runVisibility = value;
                OnPropertyChanged("RunVisibility");
            }
        }
        private Visibility _statusVisibility;
        /// <summary>
        /// Видимость картинки статуса прогона
        /// </summary>
        public Visibility StatusVisibility
        {
            get { return _statusVisibility; }
            set
            {
                _statusVisibility = value;
                OnPropertyChanged("StatusVisibility");
            }
        }
        #endregion

        #region border background color
        private System.Windows.Media.Brush _backcolor;
        /// <summary>
        /// Фон компонента
        /// </summary>
        public System.Windows.Media.Brush BackColor
        {
            get { return _backcolor; }
            set
            {
                _backcolor = value;
                OnPropertyChanged("BackColor");
            }
        }
        #endregion

        #region StatusImage
        private BitmapImage _statusimage;
        /// <summary>
        /// Картинка статуса прогона
        /// </summary>
        public BitmapImage StatusImage
        {
            get { return _statusimage; }
            set
            {
                _statusimage = value;
                OnPropertyChanged("StatusImage");
            }
        }
        #endregion

        #region StatusLabelContent
        private string _statusContent;
        /// <summary>
        /// Текстовый статус прогона
        /// </summary>
        public string StatusContent
        {
            get { return _statusContent; }
            set
            {
                _statusContent = value;
                OnPropertyChanged("StatusContent");
            }
        }
        #endregion

        #region TaskImage
        private BitmapImage _taskimage;
        /// <summary>
        /// Картинка задачи
        /// </summary>
        public BitmapImage TaskImage
        {
            get { return _taskimage; }
            set
            {
                _taskimage = value;
                OnPropertyChanged("TaskImage");
            }
        }
        #endregion

        #region ProtocolImage
        private BitmapImage _protocolImage;
        /// <summary>
        /// Картинка отображения статуса протокола
        /// </summary>
        public BitmapImage ProtocolImage
        {
            get { return _protocolImage; }
            set
            {
                _protocolImage = value;
                OnPropertyChanged("ProtocolImage");
            }
        }
        #endregion

        #region DeviceNumber
        private string _deviceNumber;
        /// <summary>
        /// Заводской номер
        /// </summary>
        public string DeviceNumber
        {
            get { return _deviceNumber; }
            set
            {
                if(value == "0" || value == string.Empty)
                {
                    _deviceNumber = "0";
                    Dispatcher.Invoke(() => deviceNumLabel.Foreground = System.Windows.Media.Brushes.Red);
                }
                else
                {
                    _deviceNumber = value;
                    task = module.side.target_form.GetTask(DeviceNumber);
                }
                OnPropertyChanged("DeviceNumber");
            }
        }
        #endregion

        #region BatteryCharge
        private int _batteryCharge;
        /// <summary>
        /// Заряд батареи
        /// </summary>
        public int BatteryCharge
        {
            get { return _batteryCharge; }
            set
            {
                _batteryCharge = value;
                OnPropertyChanged("BatteryCharge");
            }
        }
        #endregion

        #region ModbusAddress
        private string _modbusAddress;
        /// <summary>
        /// Модбас адрес
        /// </summary>
        public string ModbusAddress
        {
            get { return _modbusAddress; }
            set
            {
                if(value != "")
                {
                    _modbusAddress = value;
                    OnPropertyChanged("ModbusAddress");
                }
            }
        }
        private byte _modbusAddressByte;
        /// <summary>
        /// Модбас адрес byte
        /// </summary>
        public byte ModbusAddressByte
        {
            get { return _modbusAddressByte; }
            set
            {
                _modbusAddressByte = value;
                ModbusAddress = _modbusAddressByte.ToString();
                OnPropertyChanged("ModbusAddressByte");
            }
        }
        #endregion

        #region Версия прибора
        private bool _wrongVersion;
        /// <summary>
        /// Неверная версия
        /// </summary>
        public bool WrongVersion
        {
            get { return _wrongVersion; }
            set
            {
                _wrongVersion = value;
                if(_wrongVersion)
                {
                    BackColor = color.red;
                    StatusContent = status.wrongversion;
                    StatusImage = images.wrongverison;
                    RunVisibility = Visibility.Hidden;
                    StatusVisibility = Visibility.Visible;
                    OnPropertyChanged("WrongVersion");
                }           
            }
        }
        private bool _newVersion;
        /// <summary>
        /// Триггер новой версии
        /// </summary>
        public bool NewVersion
        {
            get { return _newVersion; }
            set
            {
                _newVersion = value;
                OnPropertyChanged("NewVersion");
            }
        }
        #endregion

        #region Protocol Path
        private string _protocolFileName;
        /// <summary>
        /// Путь к протоколу
        /// </summary>
        public string ProtocolFileName
        {
            get { return _protocolFileName; }
            set
            {
                if(value != "")
                {
                    _protocolFileName = value;
                }
                else
                {
                    dataWindow.AddLog("Не удалось сформировать протокол!");
                }
                OnPropertyChanged("ProtocolPath");
            }
        }
        #endregion

        #region Task
        private Task _task;
        /// <summary>
        /// Задача в Элме
        /// </summary>
        public Task task
        {
            get { return _task; }
            set
            {
                if(value != null)
                {
                    TaskFinded = true;
                    _task = value;
                }
                else
                {
                    TaskFinded = false;
                    dataWindow.AddLog("Не удалось найти задачу для данного прибора!");
                }
                OnPropertyChanged("task");
            }
        }
        #endregion

        #region Task statuses
        private bool _taskFinded;
        /// <summary>
        /// Статус поиска задачи
        /// </summary>
        public bool TaskFinded
        {
            get { return _taskFinded; }
            set
            {
                _taskFinded = value;
                if(_taskFinded)
                {
                    TaskImage = images.taskfinded;
                }
                else
                {
                    TaskImage = images.tasknotfinded;
                }
                OnPropertyChanged("TaskFinded");
            }
        }
        private bool _taskClosed;
        /// <summary>
        /// Статус закрытия задачи
        /// </summary>
        public bool TaskClosed
        {
            get { return _taskClosed; }
            set
            {
                _taskClosed = value;
                if(_taskClosed)
                {
                    TaskImage = images.taskclosed;
                }
                else
                {
                    TaskImage = images.tasknotclose;
                }
                OnPropertyChanged("TaskClosed");
            }
        }
        #endregion

        #region Protocol Status
        private bool _protocolCreated;
        /// <summary>
        /// Статус протокола
        /// </summary>
        public bool ProtocolCreated
        {
            get { return _protocolCreated; }
            set
            {
                _protocolCreated = value;
                if(_protocolCreated)
                {
                    ProtocolImage = images.protocolcreated;
                }
                else
                {
                    ProtocolImage = images.protocolnotcreated;
                }
                OnPropertyChanged("ProtocolCreated");
            }
        }
        #endregion

        #region Rx\Tx\ERx
        private int _rx;
        /// <summary>
        /// Количество опросов
        /// </summary>
        public int Rx
        {
            get { return _rx; }
            set
            {
                _rx = value;
                Dispatcher.Invoke(() => dataWindow.rxLabel.Content = _rx.ToString());
                OnPropertyChanged("Rx");
            }
        }
        private int _tx;
        /// <summary>
        /// Количество удачных опросов
        /// </summary>
        public int Tx
        {
            get { return _tx; }
            set
            {
                _tx = value;
                Dispatcher.Invoke(() => dataWindow.txLabel.Content = _tx.ToString());
                OnPropertyChanged("Tx");
            }
        }
        private int _erx;
        /// <summary>
        /// Количество не удачных опросов
        /// </summary>
        public int ERx
        {
            get { return _erx; }
            set
            {
                _erx = value;
                Dispatcher.Invoke(() => dataWindow.erxLabel.Content = _erx.ToString());
                OnPropertyChanged("ERx");
            }
        }
        private int _regerx;
        public int RegERx
        {
            get { return _regerx; }
            set
            {
                _regerx = value;
                OnPropertyChanged("RegERx");
            }
        }
        #endregion
        #endregion

        #region Даты
        /// <summary>
        /// Текущая дата
        /// </summary>
        public DateTime datenow;
        /// <summary>
        /// Время, в которое завершится прогон(устанавливается в настройках)
        /// </summary>
        public DateTime dateToFinish = new DateTime();
        #endregion

        #region Конфигурация
        /// <summary>
        /// IR сенсор
        /// </summary>
        public bool IR = false;
        /// <summary>
        /// TC сенсор
        /// </summary>
        public bool TC = false;
        /// <summary>
        /// EC1 сенсор
        /// </summary>
        public bool EC1 = false;
        /// <summary>
        /// EC2 сенсор
        /// </summary>
        public bool EC2 = false;
        /// <summary>
        /// O2 сенсор
        /// </summary>
        public bool O2 = false;
        /// <summary>
        /// EC3 сенсор
        /// </summary>
        public bool EC3 = false;
        /// <summary>
        /// PID сенсор
        /// </summary>
        public bool PID = false;
        /// <summary>
        /// Количество сенсоров
        /// </summary>
        public int sensorCount = 0;
        /// <summary>
        /// Список сенсоров
        /// </summary>
        public List<Sensor> sensors = new List<Sensor>();
        #endregion

        #region Ошибки
        /// <summary>
        /// Список системных ошибок ПГ
        /// </summary>
        private List<string> err_list { get; set; }
        /// <summary>
        /// Количество ошибок
        /// </summary>
        private long ERRORS = 0;
        #endregion

        #region Регистры
        /// <summary>
        /// Список групп регистров
        /// </summary>
        public List<RegisterGroup> registersGroups = new List<RegisterGroup>();
        /// <summary>
        /// Список считанных регистров
        /// </summary>
        public List<ReadedRegisters> readedRegisters = new List<ReadedRegisters>();
        /// <summary>
        /// Список регистров
        /// </summary>
        public List<Register> registers = new List<Register>();
        #endregion

        public PG(byte adr, Module mod)
        {
            InitializeComponent();

            module = mod;
            DataContext = this;
            dataWindow = new PGInfo(this);
            ModbusAddressByte = adr;
            datenow = DateTime.Now;
            chargeImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.notCharged);
            dateToFinish = datenow.AddHours(module.side.target_form.settingsWindow.settings.Hours).AddMinutes(module.side.target_form.settingsWindow.settings.Minutes);

            Connecting = true;
            Rx = 0;
            Tx = 0;
            ERx = 0;
            ProtocolCreated = false;

            StartTimerForTime();

            GetVersion();

            logger = new Logger(this);
        }

        #region Работа с задачей
        /// <summary>
        /// Закрыть задачу тех прогона
        /// </summary>
        public void CloseTask()
        {
            try
            {
                ElmaConnect.CloseTask(ElmaLogin.login, ElmaLogin.password, Convert.ToInt64(task.TaskId), ProtocolFileName);
                TaskClosed = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось закрыть задачу: ER" + DeviceNumber + "!\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                TaskClosed = false;
            }
        }
        #endregion

        #region Работа с ПГ
        /// <summary>
        /// Получить версию ПГ
        /// </summary>
        /// <returns></returns>
        public void GetPGVersion()
        {
            string query = @"select vp.VersiyaPribora1 from DGS210 dgs
                            left outer join VersiyaPribora vp on vp.Id = dgs.VersiyaPribora
                            where ZavNomer like '%" + DeviceNumber + "%'";
            List<object> result = ElmaConnect.SqlQuery(query);
            if(result.Count != 0)
            {
                if(result[0].ToString().Contains("2.4"))
                {
                    NewVersion = true;
                     GetSensor1Info(ModbusAddressByte);
                }
                else
                {
                    NewVersion = false;
                }
            }
            else
            {
                NewVersion = false;
            }
            ReturnRegisters();
        }
        #endregion

        #region Таймеры
        #region Таймер на вывод изменяемых данных
        /// <summary>
        /// Таймер для вывод изменяемых данных
        /// </summary>
        System.Windows.Forms.Timer timerTime = new System.Windows.Forms.Timer();
        /// <summary>
        /// Запустить таймер
        /// </summary>
        public void StartTimerForTime()
        {
            timerTime.Interval = 1000;
            timerTime.Enabled = true;
            timerTime.Tick += timerTime_Tick;
            time = DateTime.MinValue;
            timerTime.Start();
        }
        /// <summary>
        /// Оставшееся время
        /// </summary>
        TimeSpan timeToFinish = new TimeSpan();
        /// <summary>
        /// Время прогона
        /// </summary>
        DateTime time = new DateTime();
        /// <summary>
        /// Тик таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void timerTime_Tick(object sender, EventArgs e)
        { 
            if(DateTime.Now.CompareTo(dateToFinish) > 0)
            {
                GetVerdict();
            }
            else
            {
                if (ERx >= 50)
                {
                    Complete = false;
                    StopTimers();
                }
                else
                {
                    time = time.AddSeconds(1);
                    timeToFinish = dateToFinish - DateTime.Now;
                    dataWindow.runTimeLabel.Content = time.ToLongTimeString();
                    string[] spl = timeToFinish.ToString().Split('.');
                    dataWindow.timeToFinishLabel.Content = spl[0];
                }
            }
        }
        #endregion

        #region Таймер на опрос заряда батареи
        /// <summary>
        /// Таймер для обновления заряда батареи
        /// </summary>
        System.Windows.Forms.Timer timerCharge = new System.Windows.Forms.Timer();
        /// <summary>
        /// Запустить таймер
        /// </summary>
        public void StartTimer()
        {
            timerCharge.Interval = 60000;
            timerCharge.Enabled = true;
            timerCharge.Tick += timerPorts_Tick;
            timerCharge.Start();
        }
        /// <summary>
        /// Timer tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerPorts_Tick(object sender, EventArgs e)
        {
            module.GetBatteryCharge(ModbusAddressByte, this);
        }
        #endregion

        /// <summary>
        /// Остановить таймеры
        /// </summary>
        public void StopTimers()
        {
            timerCharge.Stop();
            timerTime.Stop();
            dateToFinish = Convert.ToDateTime("01.01.0001 0:00:00");
            Dispatcher.Invoke(new Action(() => dataWindow.timeToFinishLabel.Content = "00:00:00"));
        }
        #endregion

        #region Работа с сенсорами
        /// <summary>
        /// Добавить сенсор в список
        /// </summary>
        public void AddSensor(string sensortype, ushort startAddress, string sensorName)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Sensor sensor = new Sensor(this, sensortype, startAddress, sensorName);
                sensor.Margin = new Thickness(10, 10, 0, 0);
                sensors.Add(sensor);
            }));
        }
        /// <summary>
        /// Установить сенсора в окне
        /// </summary>
        public void SetSensors()
        {
            dataWindow.zavNomerText.Content = DeviceNumber;
            if (sensors.Count != 0)
            {
                for (int i = 0; i < sensors.Count; i++)
                {
                    try { dataWindow.sensorsPanel.Children.Add(sensors[i]); } catch { }
                }
            }
        }
        #endregion

        #region Работа с регистрами
        /// <summary>
        /// Получить регистры для разных версий
        /// </summary>
        public void ReturnRegisters()
        {
            if (NewVersion)
            {
                registers = (List<Register>)FileManager.Deserialize(Pathes.registerV3Path);
                dataWindow.SetNewRegTable();
            }
            else
            {
                registers = (List<Register>)FileManager.Deserialize(Pathes.registerPath);
            }
            SplitRegistersOnGroup();
        }
        /// <summary>
        /// Поделить регистры на группы
        /// </summary>
        public void SplitRegistersOnGroup()
        {
            for (int i = 0; i < registers.Count; i++)
            {
                List<Register> regs = new List<Register>();
                for (int j = 0; j < registers.Count; j++)
                {
                    if (registers[j].RegisterGroupNumber == i)
                    {
                        regs.Add(registers[j]);
                    }
                }
                if (regs.Count != 0)
                {
                    RegisterGroup group = new RegisterGroup();
                    group.registers = regs;
                    group.GroupNumber = i;
                    group.ReadFunction = regs[0].ReadFunction;
                    registersGroups.Add(group);
                }
            }
        }
        /// <summary>
        /// Формируем сообщения для групппы регистров
        /// </summary>
        public byte[] BuildMessageForGroup(int index)
        {
            #region Находим минимальный стартовый адрес в группе
            ushort start = Convert.ToUInt16(registersGroups[index].registers[0].RegisterNumber);
            #endregion

            #region Формируем сообщение
            byte[] message = new byte[8];
            message[0] = ModbusAddressByte;
            message[1] = registersGroups[index].ReadFunction;
            message[2] = (byte)(start >> 8);
            message[3] = (byte)start;
            message[4] = (byte)(registersGroups[index].registers.Count >> 8);
            message[5] = (byte)registersGroups[index].registers.Count;
            #endregion

            #region Высчитываем CRC
            byte[] CRC = ModBusClass.GetCRC(message);
            message[6] = CRC[0];
            message[7] = CRC[1];
            #endregion

            return message;
        }
        /// <summary>
        /// Завершение прогона
        /// </summary>
        public void GetVerdict()
        {
            int percent = 0;
            if(ERx != 0)
            {
                percent = Tx / ERx * 100;
            }
            percent = 100 - percent;
            if(percent <= 100 & percent >= 95)
            {
                if(RegERx < 25 && !OutRange)
                {
                    if(!Warning)
                    {
                        if(!WrongVersion)
                        {
                            if(!Disconnected)
                            {
                                Complete = true;
                            }
                            else
                            {
                                dataWindow.AddLog("Вердикт прогона: Не годен Причина: обрыв связи с ПГ");
                                Complete = false;
                            }
                        }
                        else
                        {
                            dataWindow.AddLog("Вердикт прогона: Не годен Причина: не верная версия прошики");
                            Complete = false;
                        }
                    }
                    else
                    {
                        dataWindow.AddLog("Вердикт прогона: Не годен Причина: системная ошибка в ПГ");
                        Complete = false;
                    }
                }
                else
                {
                    dataWindow.AddLog("Вердикт прогона: Не годен Причина: значения регистров вне допустимого диапазона");
                    Complete = false;
                }
            }
            else
            {
                dataWindow.AddLog("Вредикт прогона: Не годен Причина: не стабильная связь");
                Complete = false;
            }
            StopTimers();
        }
        /// <summary>
        /// Проверка регистров
        /// </summary>
        public bool CheckMinMax(Register reg, string value)
        {
            switch (reg.RegisterType)
            {
                #region unsigned
                case "UnSig":
                    ushort value_ush = Convert.ToUInt16(value);
                    if (value_ush >= Convert.ToUInt16(reg.RegisterMinValue))
                    {
                        if (value_ush <= Convert.ToUInt16(reg.RegisterMaxValue))
                        {
                            return true;
                        }
                        else
                        {
                            dataWindow.AddLog("Превышено максимальное значение регистра " + reg.RegisterName + "!");
                            return false;
                        }
                    }
                    else
                    {
                        dataWindow.AddLog("Значение регистра " + reg.RegisterName + " было ниже допустимого!");
                        return false;
                    }
                #endregion

                #region signed
                case "Sig":
                    int value_sh = Convert.ToInt32(value);
                    if (value_sh >= Convert.ToInt32(reg.RegisterMinValue))
                    {
                        if (value_sh <= Convert.ToInt32(reg.RegisterMaxValue))
                        {
                            return true;
                        }
                        else
                        {
                            dataWindow.AddLog("Превышено максимальное значение регистра " + reg.RegisterName + "!");
                            return false;
                        }
                    }
                    else
                    {
                        dataWindow.AddLog("Значение регистра " + reg.RegisterName + " было ниже допустимого!");
                        return false;
                    }
                #endregion

                #region long
                case "Long":
                    ulong newValue = Convert.ToUInt64(value);
                    if (newValue >= Convert.ToUInt64(reg.RegisterMinValue))
                    {
                        if (newValue <= Convert.ToUInt64(reg.RegisterMaxValue))
                        {
                            return true;
                        }
                        else
                        {
                            dataWindow.AddLog("Превышено максимальное значение регистра " + reg.RegisterName + "!");
                            return false;
                        }
                    }
                    else
                    {
                        dataWindow.AddLog("Значение регистра " + reg.RegisterName + " было ниже допустимого!");
                        return false;
                    }
                #endregion

                default:
                    return false;
            }
        }
        /// <summary>
        /// Проверить АЦП
        /// </summary>
        public void SetValues(ReadedRegisters reg, string value)
        {
            if(reg.MinValue == null)
            {
                reg.MinValue = value;
            }
            if(reg.MaxValue == null)
            {
                reg.MaxValue = value;
            }
            if (Convert.ToInt32(value) < Convert.ToInt32(reg.MinValue))
            {
                reg.MinValue = value;
            }
            else if (Convert.ToInt32(value) > Convert.ToInt32(reg.MaxValue))
            {
                reg.MaxValue = value;
            }
            else
                return;
        }
        /// <summary>
        /// Найти значение вне диапазона
        /// </summary>
        /// <returns></returns>
        public bool FindOutOfRange()
        {
            var b = readedRegisters.Find(item => !item.IsOk);
            if(b != null)
            {
                RegERx++;
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Функции преобразования
        /// <summary>
        /// Преобразование типа сенсора
        /// </summary>
        /// <param name="type"></param>
        public void GetSensorType(string type)
        {
            switch (type)
            {
                case "EC1":
                    sensorCount++;
                    EC1 = true;
                    break;
                case "EC2":
                    sensorCount++;
                    EC2 = true;
                    break;
                case "IR":
                    sensorCount++;
                    IR = true;
                    break;
                case "TC":
                    sensorCount++;
                    TC = true;
                    break;
                case "O2_1":
                    sensorCount++;
                    O2 = true;
                    break;
                case "O2_2":
                    sensorCount++;
                    O2 = true;
                    break;
                case "PID":
                    sensorCount++;
                    PID = true;
                    break;
                case "EC3":
                    sensorCount++;
                    EC3 = true;
                    break;
            }
        }
        /// <summary>
        /// Преобразовать тип сенсора в строковое представление
        /// </summary>
        /// <param name="sensor"></param>
        /// <returns></returns>
        public static string ConvertSensorTypeToString(byte sensor)
        {
            switch (sensor)
            {
                case 0:
                    return "none";
                case 1:
                    return "IR";
                case 2:
                    return "TC";
                case 3:
                    return "EC1";
                case 4:
                    return "EC2";
                case 5:
                    return "EC3";
                case 6:
                    return "PID";
                case 7:
                    return "O2_1";
                case 8:
                    return "O2_2";
                default: return "none";
            }
        }
        /// <summary>
        /// Функция склеивает 2 SHORTа в INT
        /// </summary>
        /// <param name="firstHalf">Первый SHORT</param>
        /// <param name="secondHalf">Второй SHORT</param>
        /// <returns>Получившийся INT</returns>
        public static int ShortsToInteger(ushort firstHalf, ushort secondHalf)
        {
            int reconstituted = (firstHalf << 16) | (secondHalf & 0xffff);
            return reconstituted;
        }
        /// <summary>
        /// Функция перевода массива слов в строку
        /// </summary>
        /// <param name="data">Массив слов</param>
        /// <returns>Строка</returns>
        public static string UShortArrayToString(ushort[] data)
        {
            byte[] asBytes = new byte[data.Length * sizeof(ushort)];
            Buffer.BlockCopy(data, 0, asBytes, 0, asBytes.Length);
            return ProccessString(Encoding.ASCII.GetString(asBytes));
        }
        /// <summary>
        /// Функция обработки строки 
        /// </summary>
        /// <param name="input_str">Входная строка</param>
        /// <returns>Выходная строка</returns>
        public static string ProccessString(string input_str)
        {
            string output_str = string.Empty;
            char[] arr;
            arr = input_str.ToCharArray();

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == 0)
                {
                    return output_str;
                }
                if ((Char.IsLetter(input_str, i)) || (Char.IsDigit(input_str, i)))
                {
                    output_str += input_str[i];
                }
            }
            return output_str;
        }
        /// <summary>
        /// Преобразовать настройки канала
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public string ConvertArrayToBin(byte b1, byte b2, int index)
        {
            byte[] byteResult = new byte[] { b2, b1 };
            string binary = Convert.ToString(BitConverter.ToInt16(byteResult, 0), 2);
            //Добавить незначащие нули
            int count = binary.Length;
            for (int i = 0; i < 16 - count; i++)
            {
                binary = "0" + binary;
            }
            string data = binary.Substring(index, 5);
            return data;
        }
        /// <summary>
        /// Преобразовать данные из регистра в определённый тип
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="index"></param>
        /// <param name="indexReg"></param>
        /// <returns></returns>
        public string ChangeRegType(byte b1, byte b2, Register reg)
        {
            string result = "";
            switch (reg.RegisterType)
            {
                case "UnSig":
                    result = BytesToShort(b1, b2).ToString();
                    break;
                case "Sig":
                    result = ((short)BytesToShort(b1, b2)).ToString();
                    break;
                case "Long":
                    byte[] _long = { b1, b2 };
                    result = BitConverter.ToInt64(_long, 0).ToString();
                    break;
            }
            return result;
        }
        /// <summary>
        /// Преобразовать массив байт в массив ushort
        /// </summary>
        /// <param name="massivByte"></param>
        /// <param name="massivUshort"></param>
        /// <returns></returns>
        public ushort[] ConvertByteArrayToUshortArray(byte[] massivByte)
        {
            ushort[] massivUshort = new ushort[5];
            Buffer.BlockCopy(massivByte, 0, massivUshort, 0, massivByte.Length);
            return massivUshort;
        }
        /// <summary>
        /// Функция объединения двух байт в USHORT
        /// </summary>
        /// <param name="b1">Старший байт</param>
        /// <param name="b2">Младший байт</param>
        /// <returns>Объединенный USHORT</returns>
        public static ushort BytesToShort(byte b1, byte b2)
        {
            int combined = b1 << 8 | b2;
            return (ushort)combined;
        }
        #endregion

        #region Заявки
        /// <summary>
        /// Получить системные ошибки
        /// </summary>
        public void GetSystemErrors()
        {
            Request request = new Request();
            request.Message = new byte[] { ModbusAddressByte, 4, 00, 01, 00, 04 };
            request.result = new byte[0];
            request.Mode = RequestType.SystemErrors;
            request.pg = this;
            request.module = module;
            request.priority = Priority.SystemErrors;
            module.side.requests.Enqueue(request);
        }
        /// <summary>
        /// Получить соединение с прибором
        /// </summary>
        public void GetDeviceConnection()
        {
            Request request = new Request();
            request.Message = new byte[] { ModbusAddressByte, 3, 00, 07, 00, 01 };
            request.result = new byte[0];
            request.Mode = RequestType.DeviceConnection;
            request.pg = this;
            request.module = module;
            request.priority = Priority.DeviceConnection;
            module.side.requests.Enqueue(request);
        }
        /// <summary>
        /// Получить информацию по 1 сенсору
        /// </summary>
        /// <param name="mb"></param>
        public void GetSensor1Info(byte mb)
        {
            Request request_sensor1 = new Request();
            request_sensor1.Message = new byte[] { mb, 3, 0, 0xC8, 0, 1 };
            request_sensor1.result = new byte[0];
            request_sensor1.Mode = RequestType.Sensor1Info;
            request_sensor1.pg = this;
            request_sensor1.module = module;
            request_sensor1.priority = Priority.SensorInfo;
            module.side.requests.Enqueue(request_sensor1);
        }
        /// <summary>
        /// Получить информацию по 2 сенсору
        /// </summary>
        /// <param name="mb"></param>
        public void GetSensor2Info(byte mb)
        {
            Request request_sensor2 = new Request();
            request_sensor2.Message = new byte[] { mb, 3, 01, 0x90, 0, 1 };
            request_sensor2.result = new byte[0];
            request_sensor2.Mode = RequestType.Sensor2Info;
            request_sensor2.pg = this;
            request_sensor2.module = module;
            request_sensor2.priority = Priority.SensorInfo;
            module.side.requests.Enqueue(request_sensor2);
        }
        /// <summary>
        /// Получить информацию по 3 сенсору
        /// </summary>
        /// <param name="mb"></param>
        public void GetSensor3Info(byte mb)
        {
            Request request_sensor3 = new Request();
            request_sensor3.Message = new byte[] { mb, 3, 0x02, 0x58, 0, 1 };
            request_sensor3.result = new byte[0];
            request_sensor3.Mode = RequestType.Sensor3Info;
            request_sensor3.pg = this;
            request_sensor3.module = module;
            request_sensor3.priority = Priority.SensorInfo;
            module.side.requests.Enqueue(request_sensor3);
        }
        /// <summary>
        /// Получить информацию по 4 сенсору
        /// </summary>
        /// <param name="mb"></param>
        public void GetSensor4Info(byte mb)
        {
            Request request_sensor4 = new Request();
            request_sensor4.Message = new byte[] { mb, 3, 0x3, 0x20, 0, 1 };
            request_sensor4.result = new byte[0];
            request_sensor4.Mode = RequestType.Sensor4Info;
            request_sensor4.pg = this;
            request_sensor4.module = module;
            request_sensor4.priority = Priority.SensorInfo;
            module.side.requests.Enqueue(request_sensor4);
        }
        /// <summary>
        /// Получить информацию о сенсорах
        /// </summary>
        public void GetSensorsInfo(byte mb)
        {
            Request request = new Request();
            request.Message = new byte[] { mb, 3, 00, 07, 00, 01 };
            request.result = new byte[0];
            request.Mode = RequestType.SensorsInfo;
            request.pg = this;
            request.module = module;
            request.priority = Priority.SensorInfo;
            module.side.requests.Enqueue(request);
        }
        /// <summary>
        /// Создание заявки на опрос регистров RO
        /// </summary>
        /// <param name="message"></param>
        public void GetRegisters(byte[] message, int regGr)
        {
            Request request = new Request();
            request.Message = message;
            request.result = new byte[0];
            request.Mode = RequestType.RegistersData;
            request.RegisterGroup = regGr;
            request.pg = this;
            request.module = module;
            request.priority = Priority.RO;
            module.side.requests.Enqueue(request);
        }
        /// <summary>
        /// Создание заявки на получение версии прошивки
        /// </summary>
        public void GetVersion()
        {
            Request request = new Request();
            request.Message = new byte[] { ModbusAddressByte, 3, 0, 5, 0, 1 };
            request.result = new byte[0];
            request.Mode = RequestType.Version;
            request.pg = this;
            request.module = module;
            request.priority = Priority.Version;
            module.side.requests.Enqueue(request);
        }
        #endregion

        #region Обработчики заявок
        /// <summary>
        /// Обработчик заявки на версию прошивки
        /// </summary>
        public void HandlerVerison(Request result)
        {
            if(result.result != null)
            {
                string version = "";
                if(NewVersion)
                {
                    version = BytesToShort(result.result[3], result.result[4]).ToString();
                }
                else
                {
                    version = BytesToShort(result.result[3], result.result[4]).ToString("X");
                }
                var b = module.side.target_form.versions.Find(item => item.Version == version);
                if(b == null)
                {
                    WrongVersion = true;
                }
                else
                {
                    WrongVersion = false;
                }
            }
            else
            {
                GetVersion();
            }
        }
        /// <summary>
        /// Обработчик проверки связи
        /// </summary>
        /// <param name="result"></param>
        public void HandlerConnectionCheck(Request result)
        {
            if (result.result == null)
            {
                result.pg = null;
                Dispatcher.Invoke(new Action(() => module.pgPanel.Children.Remove(this)));
                module.side.pgs.Remove(this);
                Dispatcher.Invoke(new Action(() => module.side.target_form.dataWindow.pgInfoPanel.Children.Clear()));
                Dispatcher.Invoke(new Action(() => module.side.target_form.DeviceCount -= 1));
            }
            else
            {
                result.pg.GetDeviceConnection();
            }
        }
        /// <summary>
        /// Обработчик заявки на системные ошибки
        /// </summary>
        /// <param name="result"></param>
        public void HandlerSystemErrors(Request result)
        {
            if(result.result != null)
            {
                if(NewVersion)
                {
                    ErrorCodes.Error1 = BytesToShort(result.result[3], result.result[4]);
                    ErrorCodes.Error2 = BytesToShort(result.result[5], result.result[6]);

                    err_list = new List<string>();
                    ERRORS = (long)((long)ErrorCodes.Error2 << 16 | (long)ErrorCodes.Error1);

                    if(ERRORS != 0)
                    {
                        for (byte i = 0; i < ErrorCodes.listStatusDevice_SERVICE.Count; i++)
                        {
                            if ((ERRORS & (long)1 << i) != 0)
                            {
                                string err = ErrorCodes.listStatusDevice_SERVICE.ElementAt(i) + Environment.NewLine;
                                err_list.Add(err);
                                dataWindow.AddLog(err);
                                Warning = true;
                            }
                        }
                    }
                    else
                    {
                        Warning = false;
                    }
                }
                else
                {
                    ErrorCodes.Error1 = BytesToShort(result.result[3], result.result[4]);
                    ErrorCodes.Error2 = BytesToShort(result.result[5], result.result[6]);
                    ErrorCodes.Error3 = BytesToShort(result.result[7], result.result[8]);
                    ErrorCodes.Error4 = BytesToShort(result.result[9], result.result[10]);

                    err_list = new List<string>();
                    ERRORS = (long)(((long)ErrorCodes.Error4 << 48) | ((long)ErrorCodes.Error3 << 32) | ((long)ErrorCodes.Error2 << 16) | ((long)ErrorCodes.Error1));

                    if(ERRORS != 0)
                    {
                        for (byte i = 0; i < ErrorCodes.listStatusDevice_SERVICE.Count; i++)
                        {
                            if ((ERRORS & ((long)1 << i)) != 0)
                            {
                                string err = ErrorCodes.listStatusDevice_SERVICE.ElementAt(i) + Environment.NewLine;
                                err_list.Add(err);
                                dataWindow.AddLog(err);
                                Warning = true;
                            }
                        }
                    }
                    else
                    {
                        Warning = false;
                    }
                }
            }

            if (IsRun)
            {
                GetSystemErrors();
            }
        }
        /// <summary>
        /// Обработчик данных 1 сенсора
        /// </summary>
        /// <param name="result"></param>
        public void HandlerSensor1Info(Request result)
        {
            if(result.result != null)
            {
                string sensorType = ConvertSensorTypeToString(result.result[4]);

                GetSensorType(sensorType);

                AddSensor(sensorType, 200, "sensor1");

                GetSensor2Info(result.Message[0]);
            }
            else
            {
                GetSensor1Info(result.Message[0]);
            }
        }
        /// <summary>
        /// Обработчик данных по 2 сенсору
        /// </summary>
        /// <param name="result"></param>
        public void HandlerSensor2Info(Request result)
        {
            if(result.result != null)
            {
                string sensorType = ConvertSensorTypeToString(result.result[4]);

                GetSensorType(sensorType);

                AddSensor(sensorType, 400, "sensor2");

                GetSensor3Info(result.Message[0]);
            }
            else
            {
                GetSensor2Info(result.Message[0]);
            }
        }
        /// <summary>
        /// Обработчик данных по 3 сенсору
        /// </summary>
        /// <param name="result"></param>
        public void HandlerSensor3Info(Request result)
        {
            if(result.result != null)
            {
                string sensorType = ConvertSensorTypeToString(result.result[4]);

                GetSensorType(sensorType);

                AddSensor(sensorType, 600, "sensor3");

                GetSensor4Info(result.Message[0]);
            }
            else
            {
                GetSensor3Info(result.Message[0]);
            }
        }
        /// <summary>
        /// Обработчик данных по 4 сенсору
        /// </summary>
        /// <param name="result"></param>
        public void HandlerSensor4Info(Request result)
        {
            if(result.result != null)
            {
                string sensorType = ConvertSensorTypeToString(result.result[4]);

                GetSensorType(sensorType);

                AddSensor(sensorType, 800, "sensor4");

                Dispatcher.Invoke(() => SetSensors());

                GetSystemErrors();
            }
            else
            {
                GetSensor4Info(result.Message[0]);
            }
        }
        /// <summary>
        /// Обработчик информации о сенсорах
        /// </summary>
        /// <param name="result"></param>
        public void HandlerSensorsInfo(Request result)
        {
            if (result.result != null)
            {
                if (sensors.Count == 0)
                {
                    string bindata = ConvertArrayToBin(result.result[3], result.result[4], 5);
                    char[] _sensors = bindata.ToArray();

                    byte[] message = { ModbusAddressByte, 3, 00, 0x4c, 00, 01 };
                    byte[] buff = ModBusClass.SendFc_universal(module.side.serial, message);

                    #region Определяем IR TC EC2
                    if(buff != null)
                    {
                        string binirec = ConvertArrayToBin(buff[3], buff[4], 7);
                        char[] ir_ec = binirec.ToArray();
                        if (_sensors[1] == '0') //Если есть IR или TC
                        {
                            if (ir_ec[0] == '1' && _sensors[0] == '0') //Если вместо EC2 стоит IR и включен TC
                            {
                                TC = true;
                                AddSensor("TC", 0, "");
                                IR = true;
                                AddSensor("IR", 0, "");
                            }
                            else
                            {
                                if (_sensors[0] == '0') //Только TC 0-ТС 1-IR
                                {
                                    TC = true;
                                    AddSensor("TC", 0, "");
                                }
                                else //Только IR
                                {
                                    IR = true;
                                    AddSensor("IR", 0, "");
                                }
                                if (_sensors[3] == '0') //EC2 сенсор 
                                {
                                    EC2 = true;
                                    AddSensor("EC2", 0, "");
                                }
                                else
                                {
                                    EC2 = false;
                                }
                            }
                        }
                        else //Нет TC и IR
                        {
                            TC = false;
                            IR = false;
                            if (_sensors[3] == '0')
                            {
                                EC2 = true;
                                AddSensor("EC2", 0, "");
                            }
                            else
                            {
                                EC2 = false;
                            }
                        }
                        #endregion

                        #region Определяем O2 EC1
                        if (_sensors[2] == '0')
                        {
                            O2 = true;
                            AddSensor("O2", 0, "");
                        }
                        else
                        {
                            O2 = false;
                        }
                        if (_sensors[4] == '0')
                        {
                            EC1 = true;
                            AddSensor("EC1", 0, "");
                        }
                        else
                        {
                            EC1 = false;
                        }
                        #endregion
                        Dispatcher.Invoke(() => SetSensors());
                        GetSystemErrors();
                    }
                    else
                    {
                        GetSensorsInfo(ModbusAddressByte);
                    }
                }
            }
            else
            {
                GetSensorsInfo(ModbusAddressByte);
            }
        }
        /// <summary>
        /// Обработчик регистров
        /// </summary>
        public void HandlerRegister(Request result)
        {
            Rx++;
            if (result.result != null)
            {
                Disconnected = false;
                Tx++;
                int j = 0;
                int reg = 1;
                readedRegisters.Clear();
                for (int i = 2; i < registersGroups[result.RegisterGroup].registers.Count + 2; i++)
                {
                    ReadedRegisters data = new ReadedRegisters();
                    data.register = registersGroups[result.RegisterGroup].registers[j];
                    data.RegisterValue = ChangeRegType(result.result[i + reg], result.result[i + reg + 1], (Register)data.register);
                    data.IsOk = CheckMinMax((Register)data.register, data.RegisterValue);
                    SetValues(data, data.RegisterValue);
                    readedRegisters.Add(data);

                    reg++; 
                    j++;
                }
                if (sensors.Count == 0)
                {
                    if(!NewVersion)
                    {
                        GetSensorsInfo(ModbusAddressByte);
                    }
                }

                OutRange = FindOutOfRange();

                dataWindow.UpdateTable(NewVersion);

                #region Logging
                try
                {
                    LogStructure log = new LogStructure();
                    log.RegistersToWrite = readedRegisters;
                    log.DeviceName = DeviceNumber;
                    log.Time = DateTime.Now;
                    logger.LogStructures.Add(log);
                }
                catch
                {
                    dataWindow.errorViewer.Items.Add("Не удалось записать лог");
                }
                #endregion
            }
            else
            {
                Disconnected = true;
                ERx++;
            }

            if (IsRun)
            {
                GetRegisters(BuildMessageForGroup(result.RegisterGroup), result.RegisterGroup);
            }
        }
        #endregion

        #region Работа с компонентом
        /// <summary>
        /// Установить компонент
        /// </summary>
        public void SetComponent()
        {
            module.side.target_form.dataWindow.pgInfoPanel.Children.Add(dataWindow);
        }
        /// <summary>
        /// Убрать компонент
        /// </summary>
        public void RemoveComponent()
        {
            module.side.target_form.dataWindow.pgInfoPanel.Children.Clear();
        }
        #endregion

        #region Обработчики
        /// <summary>
        /// Открыть окно с данными
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                RemoveComponent();
                SetComponent();
            }
            catch { }
        }
        /// <summary>
        /// Убрать ПГ с прогона
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Вы действительно хотите прервать тех. прогон для данного устройства?", "Прервать", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                module.pgPanel.Children.Remove(this);
                module.side.pgs.Remove(this);
                module.side.target_form.DeviceCount -= 1;
                module.GetModbusCheck(ModbusAddressByte);
                RemoveComponent();
            }
        }
        #endregion
    }
}
