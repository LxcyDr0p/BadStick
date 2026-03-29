using System;
using System.Windows.Forms;
using Xbox_360_BadStick;
using Serilog;

namespace Xbox_360_BadUpdate_USB_Tool
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            InitLogger();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("log-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Information("BadStick - starting.");
        }
    }
}
