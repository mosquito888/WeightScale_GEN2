using Microsoft.Owin.Hosting;
using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;

namespace WeightScaleGen2.BGC.SerialPortService
{
    public class Program
    {
        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow([In] IntPtr hWnd, [In] int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_MINIMIZE = 6;
        public static string latestWeight = "0.00";

        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN Web API
            WebApp.Start<Startup>(url: baseAddress);
            Console.WriteLine("Web API started at " + baseAddress);

#if DEBUG
            Random random = new Random();
            latestWeight = $"{random.Next().ToString("F2")}";
            Console.ReadLine();
#else

            // Setup SerialPort
            string portType = System.Configuration.ConfigurationManager.AppSettings["PortType"];
            string configPortName = System.Configuration.ConfigurationManager.AppSettings["PortName"];
            string configBaudRate = System.Configuration.ConfigurationManager.AppSettings["BaudRate"];
            string configParity = System.Configuration.ConfigurationManager.AppSettings["Parity"];
            string configDataBits = System.Configuration.ConfigurationManager.AppSettings["DataBits"];
            string configStopBits = System.Configuration.ConfigurationManager.AppSettings["StopBits"];
            string configHake = System.Configuration.ConfigurationManager.AppSettings["Hake"];
            string configRtsEnable = System.Configuration.ConfigurationManager.AppSettings["RtsEnable"];

            Console.WriteLine("Get config OK..");

            string portName = configPortName;
            int baudRate = Convert.ToInt32(configBaudRate);

            // 1 = None, 2 = Old, 3 = Even, 4 = Mark, 5 = Space
            Parity parity = Parity.None;
            if (configParity == "1")
            {
                parity = Parity.None;
            }
            else if (configParity == "2")
            {
                parity = Parity.Odd;
            }
            else if (configParity == "3")
            {
                parity = Parity.Even;
            }
            else if (configParity == "4")
            {
                parity = Parity.Mark;
            }
            else if (configParity == "5")
            {
                parity = Parity.Space;
            }

            int dataBits = Convert.ToInt32(configDataBits);

            // None, 2 = One, 3 = Two, 4 = OnePointFive
            StopBits stopBits = StopBits.None;
            if (configStopBits == "1")
            {
                stopBits = StopBits.None;
            }
            else if (configStopBits == "2")
            {
                stopBits = StopBits.One;
            }
            else if (configStopBits == "3")
            {
                stopBits = StopBits.Two;
            }
            else if (configStopBits == "4")
            {
                stopBits = StopBits.OnePointFive;
            }

            // 1 = None, 2 = XOnXOff, 3 = RequestToSend, 4 = RequestToSendXOnXOff
            Handshake handshake = Handshake.None;
            if (configHake == "1")
            {
                handshake = Handshake.None;
            }
            else if (configHake == "2")
            {
                handshake = Handshake.XOnXOff;
            }
            else if (configHake == "3")
            {
                handshake = Handshake.RequestToSend;
            }
            else if (configHake == "4")
            {
                handshake = Handshake.RequestToSendXOnXOff;
            }

            bool rtsEnable = configRtsEnable == "1" ? true : false;

            Console.WriteLine("Create variable OK..");

            //switch (portType)
            //{
            //    case "A":
            //        portName = ;
            //        baudRate = 9600;
            //        parity = Parity.Even; //-- 0=None, 2=Old, 3=Even, 
            //        dataBits = 7;
            //        stopBits = StopBits.One;
            //        handshake = Handshake.None;
            //        rtsEnable = true;
            //        break;
            //    case "B": // PTI
            //        portName = "COM1";
            //        baudRate = 9600;
            //        parity = Parity.Even; //-- 0=None, 2=Old, 3=Even, 
            //        dataBits = 7;
            //        stopBits = StopBits.One;
            //        handshake = Handshake.None;
            //        rtsEnable = true;
            //        break;
            //    case "C": // AGI
            //        portName = "COM1";
            //        baudRate = 1200;
            //        parity = Parity.Even; //-- 0=None, 2=Old, 3=Even,
            //        dataBits = 7;
            //        stopBits = StopBits.One;
            //        handshake = Handshake.None;
            //        rtsEnable = true;
            //        break;
            //    default:
            //        portName = "COM1";
            //        baudRate = 9600;
            //        parity = Parity.Even; //-- 0=None, 2=Old, 3=Even, 
            //        dataBits = 7;
            //        stopBits = StopBits.One;
            //        handshake = Handshake.None;
            //        rtsEnable = true;
            //        break;
            //}

            Console.WriteLine("Set variable OK..");

            try
            {
                switch (portType)
                {
                    case "A":
                        latestWeight = "0.00";
                        break;
                    case "B": // PTI
                        #region B
                        SerialPort portB = new SerialPort();
                        portB.PortName = portName;
                        portB.BaudRate = baudRate;
                        portB.Parity = parity;
                        portB.DataBits = dataBits;
                        portB.StopBits = stopBits;
                        portB.Handshake = handshake;
                        portB.RtsEnable = rtsEnable;

                        Console.WriteLine("Set serial port OK..");

                        portB.DataReceived += (sender, e) =>
                        {
                            try
                            {
                                Console.WriteLine("Start read port OK..");

                                string data1 = portB.ReadExisting(); // อ่านข้อมูลทั้งหมดที่มี

                                Console.WriteLine("Raw Data 1: " + data1);

                                string data2 = data1.Substring(5, 11);

                                Console.WriteLine("Raw Data 2: " + data2);

                                string data3 = data2.Substring(0, 5);

                                Console.WriteLine("Raw Data 3: " + data3);

                                if (decimal.TryParse(data3, out decimal weight))
                                {
                                    latestWeight = weight.ToString("F2");
                                    Console.WriteLine("Weight updated: " + latestWeight);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid weight data received: " + data2);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error reading serial data: " + ex.Message);
                            }
                        };

                        Console.WriteLine("Set data received OK..");

                        portB.Open();

                        Console.WriteLine("Set port open OK..");

                        Console.ReadLine();

                        portB.Close();

                        Console.WriteLine("Set port close OK..");
                        #endregion
                        break;
                    case "C": // AGI-IN
                        #region C
                        SerialPort portC = new SerialPort();
                        portC.PortName = portName;
                        portC.BaudRate = baudRate;
                        portC.Parity = parity;
                        portC.DataBits = dataBits;
                        portC.StopBits = stopBits;
                        portC.Handshake = handshake;
                        portC.RtsEnable = rtsEnable;

                        portC.Encoding = Encoding.ASCII; // ปรับตามที่อุปกรณ์รองรับ
                        portC.ParityReplace = 63; // ค่า '?' ใน ASCII

                        Console.WriteLine("Set serial port OK..");

                        portC.DataReceived += (sender, e) =>
                        {
                            try
                            {
                                Console.WriteLine("Start read port OK..");

                                string data1 = portC.ReadLine();

                                Console.WriteLine("Raw Data 1: " + data1);

                                string data2 = data1.Substring(5, 11);

                                Console.WriteLine("Raw Data 2: " + data2);

                                string data3 = data2.Substring(0, 5);

                                Console.WriteLine("Raw Data 3: " + data3);

                                if (decimal.TryParse(data3, out decimal weight))
                                {
                                    latestWeight = weight.ToString("F2");
                                    Console.WriteLine("Weight updated: " + latestWeight);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid weight data received: " + data2);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error reading serial data: " + ex.Message);
                            }
                        };

                        Console.WriteLine("Set data received OK..");

                        portC.Open();

                        Console.WriteLine("Set port open OK..");

                        Console.ReadLine();

                        portC.Close();

                        Console.WriteLine("Set port close OK..");
                        #endregion
                        break;
                    case "D": // AGI-OUT
                        #region D
                        SerialPort portD = new SerialPort();
                        portD.PortName = portName;
                        portD.BaudRate = baudRate;
                        portD.Parity = parity;
                        portD.DataBits = dataBits;
                        portD.StopBits = stopBits;
                        portD.Handshake = handshake;
                        portD.RtsEnable = rtsEnable;

                        portD.Encoding = Encoding.ASCII; // ปรับตามที่อุปกรณ์รองรับ
                        portD.ParityReplace = 63; // ค่า '?' ใน ASCII

                        Console.WriteLine("Set serial port OK..");

                        portD.DataReceived += (sender, e) =>
                        {
                            try
                            {
                                Console.WriteLine("Start read port OK..");

                                //string data1 = portD.ReadLine();
                                string data1 = portD.ReadExisting(); // อ่านข้อมูลทั้งหมดที่มี

                                Console.WriteLine("Raw Data 1: " + data1);

                                string data2 = data1.Substring(5, 11);

                                Console.WriteLine("Raw Data 2: " + data2);

                                string data3 = data2.Substring(0, 5);

                                Console.WriteLine("Raw Data 3: " + data3);

                                if (decimal.TryParse(data3, out decimal weight))
                                {
                                    latestWeight = weight.ToString("F2");
                                    Console.WriteLine("Weight updated: " + latestWeight);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid weight data received: " + data2);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error reading serial data: " + ex.Message);
                            }
                        };

                        Console.WriteLine("Set data received OK..");

                        portD.Open();

                        Console.WriteLine("Set port open OK..");

                        Console.ReadLine();

                        portD.Close();

                        Console.WriteLine("Set port close OK..");
                        #endregion
                        break;

                    default:
                        latestWeight = "0.00";
                        break;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
            //ShowWindow(handle, SW_MINIMIZE); // Minimize the window
            //ShowWindow(handle, SW_HIDE);     // Hide the window
#endif
        }

        //private void Clock_Tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.txtTime.Text = dt.Time();
        //        // ดึงน้ำหนัก
        //        int lvRow = txtStringScale.Lines.Length;

        //        string data4 = "";
        //        string data3 = "";
        //        string data2 = "";
        //        string data1 = "";
        //        string Check = "";

        //        double number = 0;

        //        //PTI,AGI
        //        if (lvRow > 3)
        //        {
        //            data1 = txtStringScale.Lines[lvRow - 3];

        //            //AGI,RGI
        //            if (data1.Length > 1)
        //            {
        //                //KGI
        //                data1 = data1.Replace(" ", "0");
        //                Check = data1.Substring(2, 1);
        //                data1 = data1.Substring(4);
        //                data1 = data1.Trim();
        //                //RGI,AGI
        //                number = Convert.ToDouble(data1) / 1000;
        //                number = number / 1000;
        //                if (Convert.ToInt16(Check) > 1)
        //                {
        //                    number = number * -1;
        //                }

        //                this.txtWeightScaleW_In.Text = string.Format("{0:#,0.#}", number);
        //                this.txtWeightScale_In.Text = string.Format("{0:#,0.#}", number);

        //                if (lvRow > 62)
        //                {
        //                    if (txtStringScale.Lines[lvRow - 56].Length > 1)
        //                    {
        //                        data4 = txtStringScale.Lines[lvRow - 56];
        //                    }
        //                    else
        //                    {
        //                        data4 = txtStringScale.Lines[lvRow - 57];
        //                    }

        //                    if (txtStringScale.Lines[lvRow - 45].Length > 1)
        //                    {
        //                        data3 = txtStringScale.Lines[lvRow - 45];
        //                    }
        //                    else
        //                    {
        //                        data3 = txtStringScale.Lines[lvRow - 46];
        //                    }

        //                    if (txtStringScale.Lines[lvRow - 25].Length > 1)
        //                    {
        //                        data2 = txtStringScale.Lines[lvRow - 25];
        //                    }
        //                    else
        //                    {
        //                        data2 = txtStringScale.Lines[lvRow - 24];
        //                    }

        //                    if (txtStringScale.Lines[lvRow - 3].Length > 1)
        //                    {
        //                        data1 = txtStringScale.Lines[lvRow - 3];
        //                    }
        //                    else
        //                    {
        //                        data1 = txtStringScale.Lines[lvRow - 2];
        //                    }

        //                }

        //                if (data1 == data2 && data1 == data3 && data1 == data4)
        //                {
        //                    this.btnReciveWeight2.Visible = true;
        //                    this.btnWeightScaleW_In.Visible = true;
        //                }
        //                else
        //                {
        //                    this.btnReciveWeight2.Visible = false;
        //                    this.btnWeightScaleW_In.Visible = false;
        //                }
        //            }
        //        }

        //        if (sec <= 10)
        //        {
        //            if (Convert.ToInt32(number) != 0)
        //            {
        //                this.Clock.Stop();
        //                MessageBox.Show(" น้ำหนักเริ่มต้นต้องเป็น 0 ");

        //                if (serialPort.IsOpen == true)
        //                {
        //                    serialPort.Close();
        //                }
        //                serialPort.Dispose();

        //                this.Close();

        //                FormMain fm = new FormMain();
        //                fm.Username = _name;
        //                fm.ID = _id;
        //                fm.CompanyCode = _companyCode;
        //                fm.Companyname = _companyName;
        //                fm.Show();
        //            }
        //            sec++;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
    }
}
