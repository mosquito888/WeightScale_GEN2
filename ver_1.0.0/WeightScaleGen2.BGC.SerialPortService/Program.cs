using Microsoft.Owin.Hosting;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace WeightScaleGen2.BGC.SerialPortService
{
    internal class Program
    {
        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow([In] IntPtr hWnd, [In] int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_MINIMIZE = 6;

        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpClient and make a request to api/SerialPort 
                HttpClient client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/SerialPort/RunService").Result;

                Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;

                ShowWindow(handle, SW_MINIMIZE); // Minimize the window
                ShowWindow(handle, SW_HIDE);     // Hide the window

                Console.ReadLine();
            }
        }
    }
}
