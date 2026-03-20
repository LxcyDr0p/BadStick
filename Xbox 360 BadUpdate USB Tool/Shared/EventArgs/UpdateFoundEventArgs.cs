using Xbox_360_BadStick.Shared.Enums;

namespace Xbox_360_BadStick.Shared.EventArgs
{
    public class UpdateFoundEventArgs : System.EventArgs
    {
        public UpdateType UpdateType { get; internal set; }
    }
}