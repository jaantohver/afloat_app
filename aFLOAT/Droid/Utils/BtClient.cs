using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Bluetooth;

namespace aFLOAT.Droid
{
    public static class BtClient
    {
        static GattCallback gattCallback;
        static BluetoothSocket socket;
        static BluetoothAdapter adapter;

        public static BluetoothDevice Device;

        public static event EventHandler Scanning, Found, NotFound, Connecting, Connected, Disconnecting, Disconnected;

        public static void Init ()
        {
            if (adapter != null) {
                return;
            }

            adapter = BluetoothAdapter.DefaultAdapter;
            gattCallback = new GattCallback ();
            gattCallback.Connecting += OnDeviceConnecting;
            gattCallback.Connected += OnDeviceConnected;
            gattCallback.Disconnecting += OnDeviceDisconnecting;
            gattCallback.Disconnected += OnDeviceDisconencted;

            if (adapter == null) {
                Console.WriteLine ("Device does not support Bluetooth");
                return;
            }

            if (!adapter.IsEnabled) {
                Console.WriteLine ("Bluetooth is not enabled");
                return;
            }

            Scanning?.Invoke (null, EventArgs.Empty);

            ICollection<BluetoothDevice> pairedDevices = adapter.BondedDevices;

            if (pairedDevices.Count > 0) {
                // There are paired devices. Get the name and address of each paired device.
                foreach (BluetoothDevice d in pairedDevices) {
                    string deviceName = d.Name;
                    string deviceHardwareAddress = d.Address;

                    Console.WriteLine (deviceName + " - " + deviceHardwareAddress);

                    if (deviceHardwareAddress == "20:16:02:30:52:56" || deviceName == "aFloat") {
                        Device = d;

                        Found?.Invoke (null, EventArgs.Empty);

                        Console.WriteLine ("Device found. BondState = {0}", Device.BondState);
                    }
                }
            } else {
                Console.WriteLine ("No paired devices");
            }

            if (Device == null) {
                NotFound?.Invoke (null, EventArgs.Empty);

                return;
            }
        }

        public static void Connect (Context context)
        {
            Connecting?.Invoke (null, EventArgs.Empty);

            Device.ConnectGatt (context, false, gattCallback);
        }

        static void OnDeviceConnecting (object sender, EventArgs e)
        {
            Console.WriteLine ("### Device connecting");
        }

        static void OnDeviceConnected (object sender, EventArgs e)
        {
            Connected?.Invoke (null, EventArgs.Empty);

            Console.WriteLine ("### Device connected");
        }

        static void OnDeviceDisconnecting (object sender, EventArgs e)
        {
            Disconnecting?.Invoke (null, EventArgs.Empty);

            Console.WriteLine ("### Device disconnecting");
        }

        static void OnDeviceDisconencted (object sender, EventArgs e)
        {
            Disconnected?.Invoke (null, EventArgs.Empty);

            Console.WriteLine ("### Device disconnected");
        }
    }
}