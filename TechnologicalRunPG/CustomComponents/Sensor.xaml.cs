using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TechnologicalRunPG.HW.RegisterStructure;
using TechnologicalRunPG.HW;

namespace TechnologicalRunPG.CustomComponents
{
    /// <summary>
    /// Логика взаимодействия для Sensor.xaml
    /// </summary>
    public partial class Sensor : UserControl, INotifyPropertyChanged
    {
        #region Пути к файлам с регистрами
        #endregion

        #region Стартовые адреса
        /// <summary>
        /// Стартовый адрес для чтения регистров
        /// </summary>
        public byte[] StartAddress = new byte[2];
        /// <summary>
        /// Стартовый адрес для чтения зав номера
        /// </summary>
        public byte[] StartAddressSerial = new byte[2];
        /// <summary>
        /// Стартовый адрес для чтение регистров ushort
        /// </summary>
        public ushort StartAddressUshort = 0;
        #endregion

        #region Списки с регистрами
        /// <summary>
        /// Таблица с регистрами
        /// </summary>
        List<SensorTable> tableItems = new List<SensorTable>(3);
        /// <summary>
        /// Список с данными регистров сенсора
        /// </summary>
        public List<RegisterSensor> registers = new List<RegisterSensor>();
        /// <summary>
        /// Считанные данные с регистров
        /// </summary>
        public List<ReadedDataSensor> readedSensorRegisters = new List<ReadedDataSensor>();
        #endregion

        #region Данные о сенсоре
        /// <summary>
        /// Тип сенсора
        /// </summary>
        public string SensorType = "";
        /// <summary>
        /// Имя сенсора (для новой версии)
        /// </summary>
        public string SensorName { get; set; }
        #endregion

        #region Ссылки
        /// <summary>
        /// Коллекция Series'ов для отобрежния на графике данных.
        /// </summary>
        SeriesCollection series;
        /// <summary>
        /// Ссылка на ПГшку
        /// </summary>
        PG pg = null;
        #endregion

        #region Binding
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string _SerialNumber;
        public string SerialNumber
        {
            get { return _SerialNumber; }
            set
            {
                if(value != "")
                {
                    _SerialNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        public string _formula;
        public string formula
        {
            get { return _formula; }
            set
            {
                if(value != "")
                {
                    _formula = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Единицы отображения
        /// </summary>
        public string Units;
        #endregion

        public Sensor(PG _target, string sensortype, ushort startAddress, string sensorName)
        {
            InitializeComponent();

            DataContext = this;
            pg = _target;
            SensorType = sensortype;
            SensorName = sensorName;
            typeLabel.Content = SensorType;
            StartAddressUshort = startAddress;
            sensorTable.ItemsSource = tableItems;

            GetRegisters();
            GetStartAddress();

            GetSerialNumberSensor();

            SetGraph();
            StartTimer();
            StartResetTimer();
        }

        #region Таймер для сброса графков
        /// <summary>
        /// Таймер для сброса графика
        /// </summary>
        System.Windows.Forms.Timer resetTimer = new System.Windows.Forms.Timer();
        /// <summary>
        /// Запустить таймер
        /// </summary>
        public void StartResetTimer()
        {
            resetTimer.Interval = 300000;
            resetTimer.Enabled = true;
            resetTimer.Tick += resetTimer_Tick;
            resetTimer.Start();
        }
        /// <summary>
        /// Тик таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void resetTimer_Tick(object sender, EventArgs e)
        {
            if(pg.IsRun && pg.Complete == null)
            {
                series.Clear();
                try
                {
                    SetGraph();
                }
                catch(Exception ex)
                {
                    //MessageBox.Show("Не удалось обновить график\n" + ex);
                }
            }
            else
            {
                resetTimer.Stop();
            }
        }
        #endregion

        #region Таймер для обновления концентрации
        /// <summary>
        /// Таймер для обновления заряда батареи
        /// </summary>
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        /// <summary>
        /// Запустить таймер
        /// </summary>
        public void StartTimer()
        {
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Tick += timerPorts_Tick;
            timer.Start();
        }
        /// <summary>
        /// Timer tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerPorts_Tick(object sender, EventArgs e)
        {
            if(pg.IsRun && pg.Complete == null)
            {
                UpdateConcentration();
            }
            else
            {
                timer.Stop();
            }
        }
        #endregion

        #region Получаем стартовые адреса
        /// <summary>
        /// Получить стартовый адрес
        /// </summary>
        public void GetStartAddress()
        {
            if (SensorType == "IR" && pg.TC)
            {
                StartAddress[0] = 0x01;
                StartAddress[1] = 0x90;
                StartAddressSerial[0] = 0x00;
                StartAddressSerial[1] = 0x1F;
            }
            if (SensorType == "TC" && pg.IR)
            {
                StartAddress[0] = 0x03;
                StartAddress[1] = 0x20;
                StartAddressSerial[0] = 0x00;
                StartAddressSerial[1] = 0x3D;
            }
            if (SensorType == "TC" && !pg.IR)
            {
                StartAddress[0] = 0x03;
                StartAddress[1] = 0x20;
                StartAddressSerial[0] = 0x00;
                StartAddressSerial[1] = 0x3D;
            }
            if (SensorType == "IR" && !pg.TC)
            {
                StartAddress[0] = 0x03;
                StartAddress[1] = 0x20;
                StartAddressSerial[0] = 0x00;
                StartAddressSerial[1] = 0x3D;
            }
            if (SensorType == "EC1")
            {
                StartAddress[0] = 0x00;
                StartAddress[1] = 0xC8;
                StartAddressSerial[0] = 0x00;
                StartAddressSerial[1] = 0x10;
            }
            if (SensorType == "EC2")
            {
                StartAddress[0] = 0x01;
                StartAddress[1] = 0x90;
                StartAddressSerial[0] = 0x00;
                StartAddressSerial[1] = 0x1F;
            }
            if (SensorType == "O2")
            {
                StartAddress[0] = 0x02;
                StartAddress[1] = 0x58;
                StartAddressSerial[0] = 0x00;
                StartAddressSerial[1] = 0x2E;
            }
        }
        #endregion

        #region Работа с графиком
        /// <summary>
        /// Обновить график с концентрацией
        /// </summary>
        public void UpdateConcentration()
        {
            DateModel dm = new DateModel();
            dm.DateTime = DateTime.Now;
            try
            {
                if(pg.NewVersion)
                {
                    dm.Value = Convert.ToInt32(pg.dataWindow.tableItems[FindRegister()].RegValue);
                }
                else
                {
                    dm.Value = Convert.ToInt32(pg.readedRegisters[FindRegister()].RegisterValue);
                }
                SetNewValueToSeries(dm, 0);
            }
            catch { }
        }
        /// <summary>
        /// Найти регистер
        /// </summary>
        /// <returns></returns>
        public int FindRegister()
        {
            if (pg.NewVersion)
            {
                switch(StartAddressUshort)
                {
                    case 200:
                        return pg.dataWindow.tableItems.FindIndex(item => item.RegName == "Концетрация % сенсор 1");
                    case 400:
                        return pg.dataWindow.tableItems.FindIndex(item => item.RegName == "Концетрация % сенсор 2");
                    case 600:
                        return pg.dataWindow.tableItems.FindIndex(item => item.RegName == "Концетрация % сенсор 3");
                    case 800:
                        return pg.dataWindow.tableItems.FindIndex(item => item.RegName == "Концетрация % сенсор 4");
                }
            }
            else
            {
                if (pg.readedRegisters.Count != 0)
                {
                    if (pg.IR && pg.TC && SensorType == "IR")
                    {
                        for(int i = 0; i < pg.readedRegisters.Count; i++)
                        {
                            Register register = (Register)pg.readedRegisters[i].register;
                            if(register.RegisterName == "EC2 Концентрация")
                            {
                                return i;
                            }
                        }
                    }
                    else if (pg.IR && !pg.TC && SensorType == "IR")
                    {
                        for (int i = 0; i < pg.readedRegisters.Count; i++)
                        {
                            Register register = (Register)pg.readedRegisters[i].register;
                            if (register.RegisterName == "TC Концентрация")
                            {
                                return i;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < pg.readedRegisters.Count; i++)
                        {
                            Register register = (Register)pg.readedRegisters[i].register;
                            if (register.RegisterName == SensorType + " " + "Концентрация")
                            {
                                return i;
                            }
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }
        public Func<double, string> Formatter { get; set; }
        /// <summary>
        /// Установить начальные настройки графика
        /// </summary>
        public void SetGraph()
        {
            var dayConfig = Mappers.Xy<DateModel>().X(dateModel => dateModel.DateTime.Ticks / TimeSpan.FromMilliseconds(200).Ticks).Y(dateModel => dateModel.Value);

            series = new SeriesCollection(dayConfig) { };
            Formatter = value => new System.DateTime((long)(value * TimeSpan.FromMilliseconds(200).Ticks)).ToString("t");

            graph.Series = series;
            graph.DataContext = this;

            DateModel dm = new DateModel();
            dm.DateTime = DateTime.Now;
            if(pg.NewVersion)
            {
                dm.Value = Convert.ToInt32(pg.dataWindow.tableItems[FindRegister()].RegValue);
            }
            else
            {
                dm.Value = Convert.ToInt32(pg.readedRegisters[FindRegister()].RegisterValue);
            }

            series.Add(new LineSeries()
            {
                PointGeometry = null,
                Fill = Brushes.Transparent
            });
            series[0].Values = new ChartValues<DateModel>();
            SetNewValueToSeries(dm, 0);
        }

        
        /// <summary>
        /// Добавить новое значение в Series
        /// </summary>
        /// <param name="dateModel"></param>
        public void SetNewValueToSeries(DateModel _dateModel, int _seriesNumber)
        {
            series[_seriesNumber].Values.Add(_dateModel);
        }
        #endregion

        #region Заявки
        /// <summary>
        /// Запрос на заводской номер сенсора
        /// </summary>
        /// <param name="start1"></param>
        /// <param name="start2"></param>
        public void GetSerialNumberSensor()
        {
            if(!pg.NewVersion)
            {
                Request request = new Request();
                request.Message = new byte[] { pg.ModbusAddressByte, 3, StartAddressSerial[0], StartAddressSerial[1], 00, 5 };
                request.result = new byte[0];
                request.Mode = RequestType.SensorSerialNumber;
                request.pg = pg;
                request.sensor = this;
                request.module = pg.module;
                request.SensorType = SensorType;
                request.priority = Priority.SensorSerialNumber;
                pg.module.side.requests.Enqueue(request);
            }
            else
            {
                GetSensorData();
            }
        }
        /// <summary>
        /// Получить основные данные на сенсор
        /// </summary>
        /// <param name="start1"></param>
        /// <param name="start2"></param>
        public void GetSensorData()
        {
            Request request = new Request();
            if(pg.NewVersion)
            {
                byte[] reg = BitConverter.GetBytes(StartAddressUshort);
                request.Message = new byte[] { pg.ModbusAddressByte, 3, reg[1], reg[0], 00, 0x2A };
            }
            else
            {
                request.Message = new byte[] { pg.ModbusAddressByte, 3, StartAddress[0], StartAddress[1], 00, 0x64 };
            }
            request.result = new byte[0];
            request.Mode = RequestType.SensorData;
            request.pg = pg;
            request.sensor = this;
            request.module = pg.module;
            request.SensorType = SensorType;
            request.priority = Priority.SensorData;
            pg.module.side.requests.Enqueue(request);
        }
        #endregion

        #region Обработчики заявок
        /// <summary>
        /// Обработчик заводского номера сенсора
        /// </summary>
        /// <param name="result"></param>
        public void HandlerSensorSerialNumber(Request result)
        {
            if (result.result != null)
            {
                #region Преобразование
                List<ushort> ush_data = new List<ushort>();
                ush_data.Add(PG.BytesToShort(result.result[3], result.result[4]));
                ush_data.Add(PG.BytesToShort(result.result[5], result.result[6]));
                ush_data.Add(PG.BytesToShort(result.result[7], result.result[8]));
                ush_data.Add(PG.BytesToShort(result.result[9], result.result[10]));
                ush_data.Add(PG.BytesToShort(result.result[11], result.result[12]));
                ushort[] massiv = ush_data.ToArray();
                #endregion

                SerialNumber = PG.UShortArrayToString(massiv);
                GetSensorData();
            }
            else
            {
                GetSerialNumberSensor();
            }
        }
        /// <summary>
        /// Обработчик данных сенсора
        /// </summary>
        /// <param name="result"></param>
        public void HandlerSensorData(Request result)
        {
            if (result.result != null)
            {
                readedSensorRegisters.Clear();

                int k = 0;
                int reg = 0;
                for (int j = 3; j < registers.Count + 3; j++)
                {
                    ReadedDataSensor data = new ReadedDataSensor();
                    data.RegName = registers[reg].RegisterName;
                    data.RegValue = ChangeRegType(result.result[j + k], result.result[j + k + 1], reg).ToString();
                    data.IsOk = CheckMinMax(data.RegValue, registers[reg].RegisterMinValue.ToString(), registers[reg].RegisterMaxValue.ToString(), data.RegName, registers[reg].RegisterType);
                    readedSensorRegisters.Add(data);
                    reg++;
                    k++;
                }
                pg.OutRange = FindOutOfRange();
                AddData(pg.NewVersion);
            }
            else
            {
                GetSensorData();
            }
        }
        #endregion

        #region Работа с регистрами
        /// <summary>
        /// Найти регистры вне допуска
        /// </summary>
        public bool FindOutOfRange()
        {
            var b = readedSensorRegisters.Find(item => !item.IsOk);
            if (b != null)
            {
                pg.RegERx++;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Преобразовать в тип регистра
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="type"></param>
        /// <param name="regindex"></param>
        /// <returns></returns>
        public string ChangeRegType(byte b1, byte b2, int regindex)
        {
            string result = "";
            switch (registers[regindex].RegisterType)
            {
                case "UnSig":
                    result = PG.BytesToShort(b1, b2).ToString();
                    break;
                case "Sig":
                    result = ((short)PG.BytesToShort(b1, b2)).ToString();
                    break;
                case "Long":
                    byte[] _long = { b1, b2 };
                    result = BitConverter.ToInt64(_long, 0).ToString();
                    break;
            }
            return result;
        }
        /// <summary>
        /// Проверка на минимальное и максимальное значение
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="regname"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CheckMinMax(string value, string min, string max, string regname, string type)
        {
            switch (type)
            {
                #region unsigned
                case "UnSig":
                    ushort value_ush = Convert.ToUInt16(value);
                    if (value_ush >= Convert.ToUInt16(min))
                    {
                        if (value_ush <= Convert.ToUInt16(max))
                        {
                            return true;
                        }
                        else
                        {
                            pg.dataWindow.AddLog("Превышено максимальное значение регистра " + regname + " на " + SensorType + " сенсоре!");
                            return false;
                        }
                    }
                    else
                    {
                        pg.dataWindow.AddLog("Значение регистра " + regname + " на " + SensorType + " было ниже допустимого!");
                        return false;
                    }
                #endregion

                #region signed
                case "Sig":
                    int value_sh = Convert.ToInt32(value);
                    if (value_sh >= Convert.ToInt32(min))
                    {
                        if (value_sh <= Convert.ToInt32(max))
                        {
                            return true;
                        }
                        else
                        {
                            pg.dataWindow.AddLog("Превышено максимальное значение регистра " + regname + " на " + SensorType + " сенсоре!");
                            return false;
                        }
                    }
                    else
                    {
                        pg.dataWindow.AddLog("Значение регистра " + regname + " на " + SensorType + " было ниже допустимого!");
                        return false;
                    }
                #endregion

                #region long
                case "Long":
                    ulong newValue = Convert.ToUInt64(value);
                    if (newValue >= Convert.ToUInt64(min))
                    {
                        if (newValue <= Convert.ToUInt64(max))
                        {
                            return true;
                        }
                        else
                        {
                            pg.dataWindow.AddLog("Превышено максимальное значение регистра " + regname + " на " + SensorType + " сенсоре!");
                            return false;
                        }
                    }
                    else
                    {
                        pg.dataWindow.AddLog("Значение регистра " + regname + " на " + SensorType + " было ниже допустимого!");
                        return false;
                    }
                #endregion

                default:
                    return false;
            }
        }
        /// <summary>
        /// Получаем имена регистров
        /// </summary>
        public void GetRegisters()
        {
            if(pg.NewVersion)
            {
                registers = (List<RegisterSensor>)FileManager.Deserialize(Pathes.sensorV3RegistersPath);
            }
            else
            {
                registers = (List<RegisterSensor>)FileManager.Deserialize(Pathes.sensorRegistersPath);
            }
        }
        /// <summary>
        /// Добавить данные на компонент
        /// </summary>
        public void AddData(bool version)
        {
            #region Table
            tableItems.Clear();
            for (int i = 0; i < readedSensorRegisters.Count; i++)
            {
                tableItems.Add(new SensorTable(readedSensorRegisters[i].RegName, readedSensorRegisters[i].RegValue, readedSensorRegisters[i].IsOk));
            }
            #endregion

            Dispatcher.Invoke(new Action(() => sensorTable.Items.Refresh()));

            if (version)
            {
                #region Формула газа
                List<ushort> ush_data = new List<ushort>();
                ush_data.Add(Convert.ToUInt16(readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Формула газа[0]")].RegValue));
                ush_data.Add(Convert.ToUInt16(readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Формула газа[1]")].RegValue));
                ush_data.Add(Convert.ToUInt16(readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Формула газа[2]")].RegValue));
                ush_data.Add(Convert.ToUInt16(readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Формула газа[3]")].RegValue));
                ush_data.Add(Convert.ToUInt16(readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Формула газа[4]")].RegValue));
                ush_data.Add(Convert.ToUInt16(readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Формула газа[5]")].RegValue));
                ush_data.Add(Convert.ToUInt16(readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Формула газа[6]")].RegValue));
                ushort[] data = ush_data.ToArray();
                formula = PG.UShortArrayToString(data);
                #endregion

                Units = ConvertUnits(SensorType, readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Единицы отображения")].RegValue);
            }
            else
            {
                #region Формула газа
                List<ushort> ush_data = new List<ushort>();
                ush_data.Add(Convert.ToUInt16(readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Формула газа[0][1]")].RegValue));
                ush_data.Add(Convert.ToUInt16(readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Формула газа[2][3]")].RegValue));
                ush_data.Add(Convert.ToUInt16(readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Формула газа[4][5]")].RegValue));
                ush_data.Add(Convert.ToUInt16(readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Формула газа[6][7]")].RegValue));
                ush_data.Add(Convert.ToUInt16(readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Формула газа[8][9]")].RegValue));
                ushort[] data = ush_data.ToArray();
                formula = PG.UShortArrayToString(data);
                #endregion

                Units = ConvertUnits(SensorType, readedSensorRegisters[readedSensorRegisters.FindIndex(item => item.RegName == "Допустимые единицы отображения")].RegValue);
            }
        }
        /// <summary>
        /// Преобразовать единицы отображения
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ConvertUnits(string sensorType, string value)
        {
            if (sensorType == "IR" || sensorType == "TC" || sensorType == "O2")
            {
                switch (value)
                {
                    case "0":
                        return "%";
                    case "1":
                        return "нкпр";
                    case "2":
                        return "мг/м3";
                    default: return "%";
                }
            }
            else if (sensorType.Contains("EC") || sensorType == "PID")
            {
                switch (value)
                {
                    case "0":
                        return "ppm";
                    case "1":
                        return "нкпр";
                    case "2":
                        return "мг/м3";
                    default: return "ppm";
                }
            }
            else
            {
                return "ppm";
            }
        }
        #endregion
    }
    #region Класс с данными сенсора
    /// <summary>
    /// Класс с данными сенсора
    /// </summary>
    public class ReadedDataSensor
    {
        /// <summary>
        /// Имя регистра
        /// </summary>
        public string RegName { get; set; }
        /// <summary>
        /// Значение регистра
        /// </summary>
        public string RegValue { get; set; }
        /// <summary>
        /// Регистр в допуске
        /// </summary>
        public bool IsOk { get; set; }
    }
    #endregion

    #region Класс таблицы
    public class SensorTable
    {
        public SensorTable(string name, string value, bool ok)
        {
            this.Name = name;
            this.Value = value;
            this.Property = ok;
        }
        /// <summary>
        /// Имя регистра
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Значение регистра
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Регистр в допуске
        /// </summary>
        public bool Property { get; set; }
    }
    #endregion

    #region Класс для графика
    /// <summary>
    /// Класс для графика
    /// </summary>
    public class DateModel
    {
        /// <summary>
        /// Текущее время
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// Значение
        /// </summary>
        public int Value { get; set; }
    }
    #endregion
}
