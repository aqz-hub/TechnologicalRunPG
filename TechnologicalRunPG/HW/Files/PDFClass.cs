using System;
using Spire.Pdf;
using Spire.Pdf.Tables;
using Spire.Pdf.Graphics;
using System.Drawing;
using System.IO;
using TechnologicalRunPG.CustomComponents;
using System.Collections.Generic;

namespace TechnologicalRunPG.HW
{
    partial class PDFClass
    {
        /// <summary>
        /// Создать протокол с подтягиваем всех данных
        /// </summary>
        /// <param name="pg"></param>
        public static void Create(PG pg)
        {
            List<string> table = new List<string>();
            table.Add("Criterion\nКритерий;Minimum value\nМинимальное значение;Maximum value\nМаксимальное значение;Allowed value\nДопустимое значение");

            try
            {
                if (pg.NewVersion)
                {
                    #region Новая версия ПГ
                    for (int i = 0; i < pg.sensors.Count; i++)
                    {
                        switch(pg.sensors[i].SensorName)
                        {
                            case "sensor1":
                                table.Add("ADC sensor 1\nАЦП сенсор 1; " + 
                                    pg.dataWindow.tableItems.Find(item => item.RegName == "Текущее АЦП сенсор 1").RegValue + "; " + 
                                    pg.sensors[i].readedSensorRegisters.Find(item => item.RegName == "АЦП нуля").RegValue + " - " +
                                    pg.sensors[i].readedSensorRegisters.Find(item => item.RegName == "АЦП диапазона").RegValue);
                                break;
                            case "sensor2":
                                table.Add("ADC sensor 2\nАЦП сенсор 2; " +
                                    pg.dataWindow.tableItems.Find(item => item.RegName == "Текущее АЦП сенсор 2").RegValue + "; " +
                                    pg.sensors[i].readedSensorRegisters.Find(item => item.RegName == "АЦП нуля").RegValue + " - " +
                                    pg.sensors[i].readedSensorRegisters.Find(item => item.RegName == "АЦП диапазона").RegValue);
                                break;
                            case "sensor3":
                                table.Add("ADC sensor 3\nАЦП сенсор 3; " +
                                    pg.dataWindow.tableItems.Find(item => item.RegName == "Текущее АЦП сенсор 3").RegValue + "; " +
                                    pg.sensors[i].readedSensorRegisters.Find(item => item.RegName == "АЦП нуля").RegValue + " - " +
                                    pg.sensors[i].readedSensorRegisters.Find(item => item.RegName == "АЦП диапазона").RegValue);
                                break;
                            case "sensor4":
                                table.Add("ADC sensor 4\nАЦП сенсор 4; " +
                                    pg.dataWindow.tableItems.Find(item => item.RegName == "Текущее АЦП сенсор 4").RegValue + "; " +
                                    pg.sensors[i].readedSensorRegisters.Find(item => item.RegName == "АЦП нуля").RegValue + " - " +
                                    pg.sensors[i].readedSensorRegisters.Find(item => item.RegName == "АЦП диапазона").RegValue);
                                break;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Старая версия ПГ
                    for (int i = 0; i < pg.sensors.Count; i++)
                    {
                        switch(pg.sensors[i].SensorType)
                        {
                            case "EC1":
                                table.Add("ADC EC1 sensor\nАЦП EC1 сенсор; " +
                                    pg.readedRegisters.Find(item => item.register.RegisterName == "EC1 АЦП").MinValue + ";" +
                                    pg.readedRegisters.Find(item => item.register.RegisterName == "EC1 АЦП").MaxValue + "; -100 - 100");
                                break;
                            case "EC2":
                                table.Add("ADC EC2 sensor\nАЦП EC2 сенсор; " +
                                    pg.readedRegisters.Find(item => item.register.RegisterName == "EC2 АЦП").MinValue + ";" +
                                    pg.readedRegisters.Find(item => item.register.RegisterName == "EC2 АЦП").MaxValue + "; -100 - 100");
                                break;
                            case "O2":
                                table.Add("ADC O2 sensor\nАЦП O2 сенсор; " +
                                    pg.readedRegisters.Find(item => item.register.RegisterName == "O2 АЦП").MinValue + ";" +
                                    pg.readedRegisters.Find(item => item.register.RegisterName == "O2 АЦП").MaxValue + "; -950 - (-850)");
                                break;
                            case "TC":
                                table.Add("ADC TC sensor\nАЦП TC сенсор; " +
                                    pg.readedRegisters.Find(item => item.register.RegisterName == "TC АЦП").MinValue + ";" +
                                    pg.readedRegisters.Find(item => item.register.RegisterName == "TC АЦП").MaxValue + "; -3250 - (-3150)");
                                break;
                            case "IR":
                                string range = (float.Parse(pg.sensors.Find(item => item.SensorType == "IR").readedSensorRegisters.Find(item => item.RegName == "НКПР Коэффициент x100").RegValue) /
                                            float.Parse(pg.sensors.Find(item => item.SensorType == "IR").readedSensorRegisters.Find(item => item.RegName == "Диапазон концентрации").RegValue.Replace("0", ""))).ToString();
                                if (pg.IR && pg.TC)
                                {
                                    table.Add("Concentration IR sensor\nКонцентрация IR сенсор; " +
                                        pg.readedRegisters.Find(item => item.register.RegisterName == "EC2 Концентрация").MinValue + ";" +
                                        pg.readedRegisters.Find(item => item.register.RegisterName == "EC2 Концентрация").MaxValue + "; 0 - " + range);
                                }
                                else
                                {
                                    table.Add("Concentration IR sensor\nКонцентрация IR сенсор; " +
                                        pg.readedRegisters.Find(item => item.register.RegisterName == "TC Концентрация").MinValue + ";" +
                                        pg.readedRegisters.Find(item => item.register.RegisterName == "TC Концентрация").MaxValue + "; 0 - " + range);
                                }
                                break;
                        }
                    }
                    #endregion
                }

                #region RunTime
                int runTime = Convert.ToInt32(pg.module.side.target_form.settingsWindow.runHourBox.Text) * 60 + Convert.ToInt32(pg.module.side.target_form.settingsWindow.runMinutesBox.Text);
                #endregion

                #region Temperature & Pressure
                string temp = "";
                string pres = "";
                if (pg.NewVersion)
                {
                    temp = (float.Parse(pg.dataWindow.tableItems.Find(item => item.RegName == "Температура").RegValue) / 100).ToString();
                    pres = (float.Parse(pg.dataWindow.tableItems.Find(item => item.RegName == "Давление в мм").RegValue) / 100).ToString();
                }
                else
                {
                    temp = (float.Parse(pg.readedRegisters.Find(item => item.register.RegisterName == "Темп 1 х100").MinValue) / 100).ToString() + ";" +
                            (float.Parse(pg.readedRegisters.Find(item => item.register.RegisterName == "Темп 1 х100").MaxValue) / 100).ToString();

                    pres = (float.Parse(pg.readedRegisters.Find(item => item.register.RegisterName == "Давление Lo").MinValue) / 100).ToString() + ";" +
                        (float.Parse(pg.readedRegisters.Find(item => item.register.RegisterName == "Давление Lo").MaxValue) / 100).ToString();
                }
                string vref = pg.readedRegisters.Find(item => item.register.RegisterName == "V_ref").MinValue + ";" + pg.readedRegisters.Find(item => item.register.RegisterName == "V_ref").MaxValue;
                #endregion

                table.Add("Temperature\nТемпература, °C;" + temp + ";" + "-50 - 60");
                table.Add("Pressure\nДавление, кПа;" + pres + ";" + "96 - 103");
                table.Add("Vref\nVref;" + vref + ";" + "1230 - 1255");

                pg.ProtocolFileName = CreateProtocol(pg.module.side.target_form.protocolSettings.ProtocolNumber++, pg.DeviceNumber, pg.ModbusAddress, pg.module.side.target_form.nameLabel.Content.ToString(),
                    "", pg.Rx, pg.Tx, runTime.ToString(), pg.NewVersion, table.ToArray(), temp, pres, vref);
                pg.ProtocolCreated = true;

            }
            catch (Exception ex)
            {
                pg.ProtocolCreated = false;
                pg.dataWindow.AddLog("Произошла ошибка при формировании протокола ER" + pg.DeviceNumber + ". Ошибка: " + ex.Message);
            }
        }
    
        /// <summary>
        /// Верхний колонтитул.
        /// </summary>
        static PdfPageTemplateElement CreateHeaderTech_new(PdfDocument doc, string _headerText_right, string _headerText_left, string _protocolNumber)
        {
            #region Get page size
            SizeF pageSize = doc.PageSettings.Size;
            #endregion

            #region Create a PdfPageTemplateElement object as header space
            PdfPageTemplateElement headerSpace = new PdfPageTemplateElement(pageSize.Width, 60);
            headerSpace.Foreground = false;
            #endregion

            #region Declare two float variables
            float x = 100;
            #endregion

            #region Raw text in header space
            PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("Times New Roman", 12f, FontStyle.Bold), true);
            PdfStringFormat formatRight = new PdfStringFormat(PdfTextAlignment.Right);
            PdfStringFormat formatLeft = new PdfStringFormat(PdfTextAlignment.Left);

            headerSpace.Graphics.DrawLine(PdfPens.Black, new PointF(163, 31), new PointF(163, 52));
            SizeF size = font.MeasureString(_headerText_left, formatRight);
            headerSpace.Graphics.DrawString(_headerText_left, font, PdfBrushes.Black, pageSize.Width - x - size.Width - 180, 60 - (size.Height + 5), formatRight);


            SizeF sizeLeft = font.MeasureString(_headerText_left, formatLeft);
            headerSpace.Graphics.DrawString(_protocolNumber, font, PdfBrushes.Black, pageSize.Width - x - sizeLeft.Width - 170, 67 - (sizeLeft.Height + 5), formatLeft);
            headerSpace.Graphics.DrawString(_headerText_right, font, PdfBrushes.Black, pageSize.Width - x - sizeLeft.Width - 67, 60 - (sizeLeft.Height + 5), formatLeft);
            #endregion

            #region Return headerSpace
            return headerSpace;
            #endregion
        }
        /// <summary>
        /// Создание протокола
        /// </summary>
        /// <param name="nomerprotokola">Номер протокола</param>
        /// <param name="zavnum">Зав. номер прибора</param>
        /// <param name="poloshenie">Положение на стенде</param>
        /// <param name="user1">Исполнитель</param>
        /// <param name="ProcUid">Uid процесса</param>
        /// <param name="_rx">Кол-во всех запросов</param>
        /// <param name="_tx">Кол-во удачных запросов</param>
        /// <param name="progonTime">Время прогона</param>
        /// <param name="pg_version">Версия ПГ</param>
        /// <param name="adcvalues">Значения АЦП</param>
        /// <param name="adcranges">Диапазоны АЦП</param>
        /// <param name="concvalues">Значения концентрации</param>
        /// <param name="concranges">Диапазоны концентрации</param>
        /// <param name="temp">Температура</param>
        /// <param name="pressure">Давление в мм</param>
        /// <param name="vref">V ref</param>
        /// <returns></returns>
        public static string CreateProtocol(int nomerprotokola,
                                          string zavnum,
                                          string poloshenie,
                                          string user1,
                                          string ProcUid,
                                          int _rx,
                                          int _tx,
                                          string progonTime,
                                          bool pg_version,  
                                          string[] values, 
                                          string temp, 
                                          string pressure, 
                                          string vref)
        {
            #region Создать документ
            PdfDocument doc = new PdfDocument();
            #endregion

            #region Настройка отступов
            PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
            PdfMargins margin = new PdfMargins();
            margin.Top = 0;
            margin.Bottom = 0;
            margin.Left = 60;
            margin.Right = 0;

            PdfSection section = doc.Sections.Add();
            section.PageSettings.Margins = margin;
            section.PageSettings.Size = PdfPageSize.A4;
            #endregion

            #region Создать страницу
            PdfPageBase page = section.Pages.Add();
            float y = 50;
            float yPlus = 13;
            #endregion

            #region Оформление страницы и шрифтов
            PdfTrueTypeFont fontHead = new PdfTrueTypeFont(new Font("Times New Roman", 11f, System.Drawing.FontStyle.Bold), true);
            PdfTrueTypeFont font1 = new PdfTrueTypeFont(new Font("Times New Roman", 9f, System.Drawing.FontStyle.Regular), true);

            PdfTrueTypeFont fontBold = new PdfTrueTypeFont(new Font("Times New Roman", 9f, System.Drawing.FontStyle.Bold), true);
            PdfStringFormat format = new PdfStringFormat();
            format.LineSpacing = 20f;
            PdfStringFormat formatHearers = new PdfStringFormat();
            formatHearers.LineSpacing = 20f;
            formatHearers.Alignment = PdfTextAlignment.Center;
            formatHearers.LineAlignment = PdfVerticalAlignment.Middle;

            float pageWidth = page.Canvas.ClientSize.Width;
            PdfBrush brush = PdfBrushes.Black;
            PdfBrush brush3 = PdfBrushes.Gray;
            RectangleF rctg1 = new RectangleF(0, 70, page.Canvas.ClientSize.Width / 2, 100);
            PdfStringFormat centerAlignment = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Bottom);
            #endregion

            #region Определение наименования датчика
            string naim_izdeliya = "PG ERIS-414 / ПГ ЭРИС-414";
            string a = zavnum;
            #endregion

            #region Создание верхнего колонтитула
            PdfMargins margins = new PdfMargins(0, 0, 50, 0);

            string textHeader_right = "TEST REPORT\n" +
                "ПРОТОКОЛ ИСПЫТАНИЙ";

            string textHeader_left = "TECHNOLOGICAL TEST\nТЕХНОЛОГИЧЕСКИЙ ПРОГОН";

            doc.Template.Top = CreateHeaderTech_new(doc, textHeader_left, textHeader_right, "№" + nomerprotokola);

            //apply blank templates to other parts of page template
            doc.Template.Bottom = new PdfPageTemplateElement(doc.PageSettings.Size.Width, margins.Bottom);
            doc.Template.Left = new PdfPageTemplateElement(margins.Left, doc.PageSettings.Size.Height);
            doc.Template.Right = new PdfPageTemplateElement(margins.Right, doc.PageSettings.Size.Height);
            #endregion

            #region Main table
            y = y + yPlus;
            String[] data1 = {
                    "Product;Изделие;" + naim_izdeliya,
                    "Ser. №;Зав. №:;" + "ER" + zavnum,
                    "Test date;Дата испытаний;" + DateTime.Now.ToShortDateString()
                    };
            String[][] dataSource1
                = new String[data1.Length][];
            for (int i = 0; i < data1.Length; i++)
            {
                dataSource1[i] = data1[i].Split(';');
            }
            PdfTable table1 = new PdfTable();
            table1.Style.CellPadding = 2;
            table1.Style.HeaderSource = PdfHeaderSource.Rows;
            table1.Style.HeaderRowCount = 0;
            table1.Style.ShowHeader = false;
            table1.Style.HeaderStyle.Font = font1;
            table1.Style.DefaultStyle.Font = font1;
            table1.Style.DefaultStyle.StringFormat = format;
            table1.DataSource = dataSource1;
            table1.Columns[0].Width = 100;
            table1.Columns[1].Width = 150;
            table1.Columns[2].Width = 500;
            table1.Columns[0].StringFormat = new PdfStringFormat();
            table1.Columns[0].StringFormat.Alignment = PdfTextAlignment.Left;
            table1.Columns[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            table1.Columns[1].StringFormat = new PdfStringFormat();
            table1.Columns[1].StringFormat.Alignment = PdfTextAlignment.Center;
            table1.Columns[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            table1.Columns[2].StringFormat = new PdfStringFormat();
            table1.Columns[2].StringFormat.Alignment = PdfTextAlignment.Left;
            table1.Columns[2].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            table1.Style.HeaderStyle.StringFormat = new PdfStringFormat();
            table1.Style.HeaderStyle.StringFormat.Alignment = PdfTextAlignment.Center;

            PdfLayoutResult result1 = table1.Draw(page, new PointF(0, y));
            y = y + result1.Bounds.Height + 5;
            #endregion

            #region Test
            y = y + yPlus;
            page.Canvas.DrawString("TEST PARAMETERS / ПАРАМЕТРЫ ИСПЫТАНИЯ ", fontBold, brush, 250, y, formatHearers);
            y = y + yPlus;
            String[] data2 = {
                    "Position;Положение;" + poloshenie,
                    "Inpacting component;Воздействующий компонент;Air / Воздух",
                    "Supply voltage, V;Напряжение питания;5",
                    "Exposure time, min;Время воздействия;" + progonTime
                    };
            String[][] dataSource2
                = new String[data2.Length][];
            for (int i = 0; i < data2.Length; i++)
            {
                dataSource2[i] = data2[i].Split(';');
            }
            PdfTable table2 = new PdfTable();
            table2.Style.CellPadding = 2;
            table2.Style.HeaderSource = PdfHeaderSource.Rows;
            table2.Style.HeaderRowCount = 0;
            table2.Style.ShowHeader = false;
            table2.Style.HeaderStyle.Font = font1;
            table2.Style.DefaultStyle.Font = font1;
            table2.Style.DefaultStyle.StringFormat = format;
            table2.DataSource = dataSource2;
            table2.Columns[0].Width = 120;
            table2.Columns[1].Width = 170;
            table2.Columns[2].Width = 200;
            table2.Columns[0].StringFormat = new PdfStringFormat();
            table2.Columns[0].StringFormat.Alignment = PdfTextAlignment.Left;
            table2.Columns[0].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

            table2.Columns[1].StringFormat = new PdfStringFormat();
            table2.Columns[1].StringFormat.Alignment = PdfTextAlignment.Left;
            table2.Columns[1].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

            table2.Columns[2].StringFormat = new PdfStringFormat();
            table2.Columns[2].StringFormat.Alignment = PdfTextAlignment.Center;
            table2.Columns[2].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;

            table2.Style.HeaderStyle.StringFormat = new PdfStringFormat();
            table2.Style.HeaderStyle.StringFormat.Alignment = PdfTextAlignment.Center;

            PdfLayoutResult result2 = table2.Draw(page, new PointF(0, y));
            y = y + result2.Bounds.Height + 5;
            #endregion

            #region Третяя таблица
            y = y + yPlus;
            page.Canvas.DrawString("TESTING LACILITIES / СРЕДСТВА ИСПЫТАНИЯ", fontBold, brush, 250, y, formatHearers);
            y = y + yPlus;
            String[] data3 = {
                    "Name\nНаименование;Ser.№\nЗав.№;Number of certificate\nНомер аттестата;Validity of the certificate\nСрок действия аттестата",
                    "Стенд технологического прогона;С.1;1;01.01.2022",

                    };
            String[][] dataSource3
                = new String[data3.Length][];
            for (int i = 0; i < data3.Length; i++)
            {
                dataSource3[i] = data3[i].Split(';');
            }
            PdfTable table3 = new PdfTable();
            table3.Style.CellPadding = 2;
            table3.Style.HeaderSource = PdfHeaderSource.Rows;
            table3.Style.HeaderRowCount = 0;
            table3.Style.ShowHeader = false;
            table3.Style.HeaderStyle.Font = font1;
            table3.Style.DefaultStyle.Font = font1;
            table3.DataSource = dataSource3;
            table3.Columns[0].Width = 120;
            table3.Columns[1].Width = 170;
            table3.Columns[2].Width = 200;
            table3.Columns[3].Width = 200;

            for (int i = 0; i < 4; i++)
            {
                table3.Columns[i].StringFormat = new PdfStringFormat();
                table3.Columns[i].StringFormat.Alignment = PdfTextAlignment.Center;
                table3.Columns[i].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            }

            table3.Style.HeaderStyle.StringFormat = new PdfStringFormat();
            table3.Style.HeaderStyle.StringFormat.Alignment = PdfTextAlignment.Center;

            PdfLayoutResult result3 = table3.Draw(page, new PointF(0, y));
            y = y + result3.Bounds.Height + 5;
            #endregion   

            #region Четвертая таблица
            y = y + yPlus;
            page.Canvas.DrawString("TEST RESULT / РЕЗУЛЬТАТЫ ИСПЫТАНИЯ", fontBold, brush, 250, y, formatHearers);
            page.Canvas.DrawString("1. The test result by the criterion «stability of values»", fontBold, brush, 0, y, format);
            y = y + yPlus;
            page.Canvas.DrawString("1. Резульатат испытания по критерию «стабильность значений»", fontBold, brush, 0, y, format);
            y = y + yPlus;
            y = y + yPlus;
            String[] data4 = values;
            String[][] dataSource4
                = new String[data4.Length][];
            for (int i = 0; i < data4.Length; i++)
            {
                dataSource4[i] = data4[i].Split(';');
            }
            PdfTable table4 = new PdfTable();
            table4.Style.CellPadding = 2;
            table4.Style.HeaderSource = PdfHeaderSource.Rows;
            table4.Style.HeaderRowCount = 0;
            table4.Style.ShowHeader = false;
            table4.Style.HeaderStyle.Font = font1;
            table4.Style.DefaultStyle.Font = font1;
            table4.DataSource = dataSource4;
            table4.Columns[0].Width = 150;
            table4.Columns[1].Width = 100;
            table4.Columns[2].Width = 100;
            table4.Columns[3].Width = 100;

            for (int i = 0; i < 4; i++)
            {
                table4.Columns[i].StringFormat = new PdfStringFormat();
                table4.Columns[i].StringFormat.Alignment = PdfTextAlignment.Left;
                table4.Columns[i].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            }

            table4.Style.HeaderStyle.StringFormat = new PdfStringFormat();
            table4.Style.HeaderStyle.StringFormat.Alignment = PdfTextAlignment.Center;

            PdfLayoutResult result4 = table4.Draw(page, new PointF(0, y));
            y = y + result4.Bounds.Height + 5;
            #endregion

            #region Шестая таблица
            y = y + yPlus;
            page.Canvas.DrawString("2. Test result by the criterion «stability of digital output»", fontBold, brush, 0, y, format);
            y = y + yPlus;
            page.Canvas.DrawString("2. Результат испытания по критерию «стабильность цифрового выхода»", fontBold, brush, 0, y, format);
            y = y + yPlus;
            page.Canvas.DrawString("Results of polling of digital output / Результаты опроса цифрового выхода", fontBold, brush, 0, y, format);
            y = y + yPlus;
            y = y + yPlus;
            int percent = (_rx / _tx) * 100;
            String[] data6 = {
                    "Criterion\nКритерий;Measured value\nИзмеренное значение;Allowed value\nДопустимое значение",
                    "Number of RS-485 (Tx) polls\nКоличество опросов по RS-485 (Tx);" + _tx + "; - ",
                    "Number of RS-485 (Rx) replies\nКоличество ответов по RS-485 (Rx);" + _tx + "/" +_rx + "=" + percent + "%" + ";95 - 100%",
                    };
            String[][] dataSource6
                = new String[data6.Length][];
            for (int i = 0; i < data6.Length; i++)
            {
                dataSource6[i] = data6[i].Split(';');
            }
            PdfTable table6 = new PdfTable();
            table6.Style.CellPadding = 2;
            table6.Style.HeaderSource = PdfHeaderSource.Rows;
            table6.Style.HeaderRowCount = 0;
            table6.Style.ShowHeader = false;
            table6.Style.HeaderStyle.Font = font1;
            table6.Style.DefaultStyle.Font = font1;
            table6.DataSource = dataSource6;
            table6.Columns[0].Width = 250;
            table6.Columns[1].Width = 100;
            table6.Columns[2].Width = 100;

            for (int i = 0; i < 3; i++)
            {
                table6.Columns[i].StringFormat = new PdfStringFormat();
                table6.Columns[i].StringFormat.Alignment = PdfTextAlignment.Left;
                table6.Columns[i].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            }

            table6.Style.HeaderStyle.StringFormat = new PdfStringFormat();
            table6.Style.HeaderStyle.StringFormat.Alignment = PdfTextAlignment.Center;

            PdfLayoutResult result6 = table6.Draw(page, new PointF(0, y));
            y = y + result6.Bounds.Height + 5;
            #endregion

            #region Седьмая таблица
            y = y + yPlus;
            page.Canvas.DrawString("CONCLUSION / ЗАКЛЮЧЕНИЕ", fontBold, brush, 250, y, formatHearers);
            y = y + yPlus;
            String[] data7 = {
                    "Conclusion\nЗаключение;Soft version\nВерсия ПО;Engineer\nИсполнитель;Process UID\nИдентификатор процесса",
                    "OK\t ГОДЕН;"+ MainWindow.Version +";" +user1 + ";" +ProcUid

                    };
            String[][] dataSource7
                = new String[data7.Length][];
            for (int i = 0; i < data7.Length; i++)
            {
                dataSource7[i] = data7[i].Split(';');
            }
            PdfTable table7 = new PdfTable();
            table7.Style.CellPadding = 2;
            table7.Style.HeaderSource = PdfHeaderSource.Rows;
            table7.Style.HeaderRowCount = 0;
            table7.Style.ShowHeader = false;
            table7.Style.HeaderStyle.Font = font1;
            table7.Style.DefaultStyle.Font = font1;
            table7.DataSource = dataSource7;
            table7.Columns[0].Width = 50;
            table7.Columns[1].Width = 50;
            table7.Columns[2].Width = 90;
            table7.Columns[3].Width = 150;

            for (int i = 0; i < 4; i++)
            {
                table7.Columns[i].StringFormat = new PdfStringFormat();
                table7.Columns[i].StringFormat.Alignment = PdfTextAlignment.Center;
                table7.Columns[i].StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
            }

            PdfLayoutResult result7 = table7.Draw(page, new PointF(0, y));
            y = y + result7.Bounds.Height + 5;
            #endregion

            #region Создать папку и скопировать туда протокол
            string filename = @"Тех.Прогон ПГ №" + nomerprotokola + " " + zavnum + ".pdf";
            try
            {
                Directory.CreateDirectory(Pathes.networkProtocolPath);
            }
            catch { }
            doc.SaveToFile(Pathes.networkProtocolPath + filename);
            doc.Close();

            try
            {
                Directory.CreateDirectory(Pathes.localProtocolPath);
            }
            catch { System.Windows.MessageBox.Show("Не удалось создать папку для хранения протоколов"); }
            doc.SaveToFile(Pathes.localProtocolPath + filename);
            doc.Close();
            return filename;
            #endregion
        }
    }
}
