using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TechnologicalRunPG.CustomComponents
{
    /// <summary>
    /// Логика взаимодействия для Side.xaml
    /// </summary>
    public partial class Side : UserControl
    {
        #region COM-port
        /// <summary>
        /// COM port
        /// </summary>
        public SerialPort serial = new SerialPort();
        /// <summary>
        /// Имя COM - порта
        /// </summary>
        public string ComPortName = "";
        #endregion

        #region Списки
        /// <summary>
        /// Лист заявок на запрос ModBus.
        /// </summary>
        public Queue<Request> requests = new Queue<Request>();
        /// <summary>
        /// Список с ПГ
        /// </summary>
        public List<PG> pgs = new List<PG>();
        /// <summary>
        /// Список модулей
        /// </summary>
        public List<Module> modules = new List<Module>();
        #endregion

        /// <summary>
        /// Ссылка на главное окно
        /// </summary>
        public MainWindow target_form = null;

        public Side(MainWindow _target, string comportname, string MBadr)
        {
            InitializeComponent();
            ComPortName = comportname;
            SetModules(MBadr);
            target_form = _target;
            Dispatcher.Invoke(new Action(() => target_form.panel.Children.Add(this)));
            Task _task = new Task(ModBusCore);
            _task.Start();
        }

        /// <summary>
        /// Установить модуля
        /// </summary>
        public void SetModules(string modbusAdr)
        {
            #region Работа с адресами
            List<string> addresses = new List<string>();
            addresses.Add("1-10");
            addresses.Add("11-20");
            addresses.Add("21-30");
            addresses.Add("31-40");
            addresses.Add("41-50");
            addresses.Add("51-60");
            addresses.Add("61-70");
            addresses.Add("71-80");
            #endregion

            #region Работа с модулями
            switch (modbusAdr)
            {
                case "1-40":
                    for (int i = 0; i < 4; i++)
                    {
                        Module module = new Module(this, addresses[i])
                        {
                            Margin = new Thickness(0, 10, 0, 0)
                        };
                        modules.Add(module);
                        modulePanel.Children.Add(module);
                    }
                    break;
                case "41-80":
                    for (int i = 4; i < 8; i++)
                    {
                        Module module = new Module(this, addresses[i])
                        {
                            Margin = new Thickness(0, 10, 0, 0)
                        };
                        modules.Add(module);
                        modulePanel.Children.Add(module);
                    }
                    break;
            }
            #endregion
        }

        #region Поток обработки заявок
        /// <summary>
        /// Проверка есть ли в потоке заявки.
        /// </summary>
        /// <returns></returns>
        public bool IsRequests()
        {
            if (target_form.Running)
            {
                if (requests.Count() != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Бесконечный поток опроса.
        /// </summary>
        public void ModBusCore()
        {
            while (target_form.Running)
            {
                SpinWait.SpinUntil(IsRequests);

                if (requests.Count == 0)
                {
                    return;
                }
                Thread.Sleep(1);
                var queItem = requests.Dequeue();
                queItem.result = ModBusClass.SendFc_universal(serial, queItem.Message);

                try
                {
                    switch (queItem.Mode)
                    {
                        case RequestType.SerialNumber:
                            {
                                queItem.module.HandlerSerialNumber(queItem);
                                break;
                            }
                        case RequestType.RegistersData:
                            {
                                queItem.pg.HandlerRegister(queItem);
                                break;
                            }
                        case RequestType.BatteryCharge:
                            {
                                queItem.module.HandlerBatteryCharge(queItem);
                                break;
                            }
                        case RequestType.SensorsInfo:
                            {
                                queItem.pg.HandlerSensorsInfo(queItem);
                                break;
                            }
                        case RequestType.SensorData:
                            {
                                queItem.sensor.HandlerSensorData(queItem);
                                break;
                            }
                        case RequestType.SensorSerialNumber:
                            {
                                queItem.sensor.HandlerSensorSerialNumber(queItem);
                                break;
                            }
                        case RequestType.ModbusCheck:
                            {
                                queItem.module.HandlerModbusCheck(queItem);
                                break;
                            }
                        case RequestType.DeviceConnection:
                            {
                                queItem.pg.HandlerConnectionCheck(queItem);
                                break;
                            }
                        case RequestType.SystemErrors:
                            {
                                queItem.pg.HandlerSystemErrors(queItem);
                                break;
                            }
                        case RequestType.Sensor1Info:
                            {
                                queItem.pg.HandlerSensor1Info(queItem);
                                break;
                            }
                        case RequestType.Sensor2Info:
                            {
                                queItem.pg.HandlerSensor2Info(queItem);
                                break;
                            }
                        case RequestType.Sensor3Info:
                            {
                                queItem.pg.HandlerSensor3Info(queItem);
                                break;
                            }
                        case RequestType.Sensor4Info:
                            {
                                queItem.pg.HandlerSensor4Info(queItem);
                                break;
                            }
                        case RequestType.Version:
                            {
                                queItem.pg.HandlerVerison(queItem);
                                break;
                            }
                    }
                }
                catch
                {

                }
            }
        }
        #endregion

        #region Обработчики контрола
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ModBusClass.Close(serial);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ModBusClass.Open(serial, ComPortName);
        }
        #endregion
    }
}
