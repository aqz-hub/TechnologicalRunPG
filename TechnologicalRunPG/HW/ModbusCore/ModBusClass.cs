using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;

namespace TechnologicalRunPG
{
    class ModBusClass
    {
        #region Open\Close COM-port
        /// <summary>
        /// Открыть COM-порт
        /// </summary>
        /// <param name="portName"></param>
        /// <returns></returns>
        public static bool Open(SerialPort serialPort, string portName)
		{
			if(portName != "")
            {
				if (!serialPort.IsOpen)
				{
					serialPort.PortName = portName;
					serialPort.BaudRate = 19200;
					serialPort.DataBits = 8;
					serialPort.Parity = Parity.None;
					serialPort.StopBits = StopBits.One;
					serialPort.ReadTimeout = 2000;
					serialPort.WriteTimeout = 2000;

					try
					{
						serialPort.Open();
					}
					catch
					{
						return false;
					}
					return true;
				}
				else
				{
					return false;
				}
			}
			else
            {
				return false;
            }
		}
		/// <summary>
		/// Закрыть COM-порт
		/// </summary>
		/// <returns></returns>
		public static bool Close(SerialPort serialPort)
		{
			if (serialPort.IsOpen)
			{
				try
				{
					serialPort.Close();
				}
				catch
				{
					return false;
				}
				return true;
			}
			else
			{
				return false;
			}
		}
        #endregion

        #region CRC
        /// <summary>
        /// CRC Function
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static byte[] GetCRC(byte[] message)
		{
			//Функция расчета двух байт CRC 
			byte[] CRC = new byte[2];

			ushort CRCFull = 0xFFFF;
			byte CRCHigh = 0xFF, CRCLow = 0xFF;
			char CRCLSB;

			for (int i = 0; i < (message.Length) - 2; i++)//в зависимости от длинны сообщения пускаем цикл
			{
				CRCFull = (ushort)(CRCFull ^ message[i]);

				for (int j = 0; j < 8; j++)
				{
					CRCLSB = (char)(CRCFull & 0x0001);
					CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

					if (CRCLSB == 1)
						CRCFull = (ushort)(CRCFull ^ 0xA001);
				}
			}
			CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
			CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
			return CRC;
		}
		/// <summary>
		/// Метод подсчёта CRC
		/// </summary>
		/// <param name="data"></param>
		/// <param name="len"></param>
		/// <returns></returns>
		public static UInt16 ModRTU_CRC(byte[] data, int len)
		{
			UInt16 crc = 0xFFFF;

			for (int pos = 0; pos < len; pos++)
			{
				crc ^= (UInt16)data[pos];

				for (int i = 8; i != 0; i--)
				{
					if ((crc & 0x0001) != 0)
					{
						crc >>= 1;
						crc ^= 0xA001;
					}
					else
						crc >>= 1;
				}
			}
			return crc;
		}
        #endregion

        #region Универсальная функция чтения
        /// <summary>
        /// Универсальная команда чтения.
        /// </summary>
        public static byte[] SendFc_universal(SerialPort serialPort, byte[] _message)
		{
			byte[] result = null;
			byte address = _message[0];
			byte func = _message[1];
			byte start1 = _message[2];
			byte start2 = _message[3];
			byte registers1 = _message[4];
			byte registers2 = _message[5];
			if (serialPort.IsOpen)
			{
				#region Очистить буфферы 
				serialPort.DiscardOutBuffer();
				serialPort.DiscardInBuffer();
				#endregion

				#region Сформировать сообщение
				byte[] message = { address, func, start1, start2, registers1, registers2 };
				UInt16 crc = ModRTU_CRC(message, message.Length);
				byte crc1 = (byte)crc;
				byte[] crc2 = BitConverter.GetBytes(crc >> 8);
				byte[] messageCRC = { address, func, start1, start2, registers1, registers2, crc1, crc2[0] };
				#endregion

				#region Отправить сообщение
				try
				{
					serialPort.Write(messageCRC, 0, messageCRC.Length);
				}
				catch
				{
				}
				System.Threading.Thread.Sleep(400);
				#endregion

				#region Обработать ответ
				try
				{
					if (serialPort.BytesToRead > 0)
					{
						result = new byte[5 + registers2 * 2];
						serialPort.Read(result, 0, 5 + registers2 * 2);
					}
				}
				catch { }
				#endregion
			}
			else
			{
				result = null;
			}
			return result;
		}
        #endregion
    }
}
