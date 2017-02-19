#pragma warning disable XA0001 // Find issues with Android API usage
using System;

using Android.Bluetooth;

using static System.Console;

namespace aFLOAT.Droid
{
    public class GattCallback : BluetoothGattCallback
    {
        public event EventHandler Connecting, Connected, Disconnecting, Disconnected;

        public override void OnConnectionStateChange (BluetoothGatt gatt, GattStatus status, ProfileState newState)
        {
            base.OnConnectionStateChange (gatt, status, newState);

            switch (newState) {
            case ProfileState.Connecting:
                Connecting?.Invoke (this, EventArgs.Empty);

                break;
            case ProfileState.Connected:
                Connected?.Invoke (this, EventArgs.Empty);

                break;
            case ProfileState.Disconnecting:
                Disconnecting?.Invoke (this, EventArgs.Empty);

                break;
            case ProfileState.Disconnected:
                Disconnected?.Invoke (this, EventArgs.Empty);

                break;
            }
        }
    }
}
#pragma warning restore XA0001 // Find issues with Android API usage