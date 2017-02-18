using System;

using CoreBluetooth;

namespace aFLOAT.iOS
{
    public class Device : IEquatable<Device>
    {
        int rssi = -127;
        public int RSSI {
            get {
                return rssi;
            }
            set {
                if (value < 0) {
                    rssi = value;
                }
            }
        }

        public CBPeripheral Peripheral { get; set; }

        public bool Equals (Device other)
        {
            return Peripheral.Identifier.Equals (other.Peripheral.Identifier);
        }
    }
}