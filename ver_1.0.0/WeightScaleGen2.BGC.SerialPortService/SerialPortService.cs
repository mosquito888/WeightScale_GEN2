using System;
using System.IO.Ports;
using System.Threading.Tasks;
using WeightScaleGen2.BGC.SerialPortService.ServicesModels;

namespace WeightScaleGen2.BGC.SerialPortService
{
    public class SerialPortService
    {
        public Task<ReturnObject<decimal>> ConnectSerialPort(string type)
        {
            var result = new ReturnObject<decimal>();

            try
            {
                switch (type)
                {
                    case "A":
                        result.data = SerialPortType__A();
                        break;
                    case "B":
                        result.data = SerialPortType__B();
                        break;
                    case "C":
                        result.data = SerialPortType__C();
                        break;
                    default:
                        result.data = 0;
                        result.isCompleted = false;
                        result.message.Add("Can't Open Connect COM Port");
                        break;
                }

                result.isCompleted = true;
                result.message.Add("success");
            }
            catch (Exception ex)
            {
                result.isCompleted = false;
                result.message.Add(ex.Message);
            }

            return Task.FromResult(result);
        }
        /// <summary>
        /// KBI
        /// </summary>
        /// <returns></returns>
        private decimal SerialPortType__A()
        {
            using (SerialPort serialPort = new SerialPort())
            {
                serialPort.PortName = "COM1";
                serialPort.BaudRate = 1200;
                serialPort.Parity = Parity.Even; //-- 0=None, 2=Old, 3=Even,
                serialPort.DataBits = 7;
                serialPort.StopBits = StopBits.One;
                serialPort.Handshake = Handshake.None;
                serialPort.RtsEnable = true;
                serialPort.Open();
            }

            Random random = new Random();
            return random.Next();
        }
        /// <summary>
        /// KGI
        /// </summary>
        /// <returns></returns>
        private decimal SerialPortType__B()
        {
            using (SerialPort serialPort = new SerialPort())
            {
                serialPort.PortName = "COM1";
                serialPort.BaudRate = 9600;
                serialPort.Parity = Parity.Even; //-- 0=None, 2=Old, 3=Even, 
                serialPort.DataBits = 7;
                serialPort.StopBits = StopBits.One;
                serialPort.Handshake = Handshake.None;
                serialPort.RtsEnable = true;
                serialPort.Open();
            }

            Random random = new Random();
            return random.Next();
        }
        /// <summary>
        /// PTI,AGI,RBI,PGI
        /// </summary>
        /// <returns></returns>
        private decimal SerialPortType__C()
        {
            using (SerialPort serialPort = new SerialPort())
            {
                serialPort.PortName = "COM1";
                serialPort.BaudRate = 1200;
                serialPort.Parity = Parity.Even; //-- 0=None, 2=Old, 3=Even,
                serialPort.DataBits = 7;
                serialPort.StopBits = StopBits.One;
                serialPort.Handshake = Handshake.None;
                serialPort.RtsEnable = true;
                serialPort.Open();
            }

            Random random = new Random();
            return random.Next();
        }
    }
}
