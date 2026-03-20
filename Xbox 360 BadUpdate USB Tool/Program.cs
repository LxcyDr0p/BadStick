using System;
using System.Windows.Forms;
using Xbox_360_BadStick;

namespace Xbox_360_BadUpdate_USB_Tool
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
