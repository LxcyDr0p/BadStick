using System;

namespace Xbox_360_BadStick.Shared.EventArgs
{
    public class ProgressReportEventArgs : System.EventArgs
    {
        public ProgressReportEventArgs()
        {
            Message = String.Empty;
            Progress = 0;
        }

        public ProgressReportEventArgs(int percent)
        {
            Progress = percent;
            Message = string.Empty;
        }

        public string Message { get; internal set; }
        public int Progress { get; internal set; }
    }
}

    
