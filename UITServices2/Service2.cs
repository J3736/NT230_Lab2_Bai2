using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace UITServices2
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Timer và Service Controller
        /// </summary>
        ServiceController sc = new ServiceController("Service1");
        private Timer timer1 = new Timer();
        /// <summary>
        /// 30s sẽ bật tắt dịch vụ 1 lần
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            // Thoi gian 30s 1 lan bat tat services.
            timer1.Elapsed += new ElapsedEventHandler(time_Tick);
            timer1.Interval = 30000;
            timer1.Enabled = true;
            WriteToFile(DateTime.Now + ": Bắt đầu bật/tắt Services1 mỗi 30s.");
        }
        /// <summary>
        /// Bật tắt Service mỗi 30s
        /// </summary>
        private void time_Tick(object source, ElapsedEventArgs e)
        {
            ServiceController sc1 = new ServiceController("Service1");
           if (sc1.Status.Equals(ServiceControllerStatus.Stopped)|| 
                sc.Status.Equals(ServiceControllerStatus.StopPending))
           {
                sc1.Start();
                WriteToFile(DateTime.Now + ": Service1: đang tắt, chạy Service1");
           }
           else
            {
                sc1.Stop();
                WriteToFile(DateTime.Now + ": Service1 đang chạy, tắt Service1");
            }
        }
        /// <summary>
        /// Stop cả 2 services.
        /// </summary>
        protected override void OnStop()
        {
            timer1.Enabled = true;
            WriteToFile(DateTime.Now + ": Tắt Services1 và Services2");
            sc.Stop();
        }
        /// <summary>
        /// Check Service
        /// </summary>
        /// <returns></returns>
        public string checksc()
        {
            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    return "Running";
                case ServiceControllerStatus.Stopped:
                    return "Stopped";
                case ServiceControllerStatus.Paused:
                    return "Paused";
                case ServiceControllerStatus.StopPending:
                    return "Stopping";
                case ServiceControllerStatus.StartPending:
                    return "Starting";
                default:
                    return "Status Changing";
            }
        }
        /// <summary>
        ///  Write to file
        /// </summary>
        /// <param name="Message"></param>
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" +
            DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
