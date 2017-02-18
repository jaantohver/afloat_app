using System;

namespace aFLOAT.iOS
{
    public class NotificationEventArgs : EventArgs
    {
        public bool UpsideDown { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}