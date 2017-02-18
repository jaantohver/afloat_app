using System;

using Foundation;
using CoreBluetooth;

namespace aFLOAT.iOS
{
    public static class BleClient
    {
        static CBPeripheral peripheral;
        static CBCentralManager manager;

        public static bool IsConnected;

        public static event EventHandler Connected, Disconnected;
        public static event EventHandler<CBDiscoveredPeripheralEventArgs> DeviceDiscovered;

        public static void Init ()
        {
            manager = new CBCentralManager ();
            manager.UpdatedState += OnManagerUpdatedState;
            manager.DiscoveredPeripheral += OnManagerDiscoveredPeripheral;
            manager.ConnectedPeripheral += OnManagerConnectedPeripheral;
            manager.DisconnectedPeripheral += OnManagerDisconnectedPeripheral;
        }

        public static void Connect (CBPeripheral peripheral)
        {
            if (BleClient.peripheral != null) {
                return;
            }

            BleClient.peripheral = peripheral;

            manager.StopScan ();
            manager.ConnectPeripheral (peripheral);
        }

        static void OnManagerUpdatedState (object sender, EventArgs e)
        {
            if ((int)manager.State == (int)CBManagerState.PoweredOn) {
                PeripheralScanningOptions opts = new PeripheralScanningOptions ();
                opts.AllowDuplicatesKey = true;

                manager.ScanForPeripherals (peripheralUuids: null, options: opts.Dictionary);
            }
        }

        static void OnManagerDiscoveredPeripheral (object sender, CBDiscoveredPeripheralEventArgs e)
        {
            if (peripheral != null) {
                return;
            }

            if (string.IsNullOrWhiteSpace (e.Peripheral.Name)) {
                return;
            }

            Console.WriteLine ("Discovered peripheral");

            DeviceDiscovered?.Invoke (null, e);
        }

        static void OnManagerConnectedPeripheral (object sender, CBPeripheralEventArgs e)
        {
            Console.WriteLine ("Connected peripheral");

            peripheral.DiscoveredService += OnPeripheralDiscoveredService;
            peripheral.DiscoveredCharacteristic += OnPeripheralDiscoveredCharacteristic;
            peripheral.UpdatedCharacterteristicValue += OnPeripheralUpdatedCharacteristicValue;
            peripheral.DiscoverServices ();
        }

        static void OnManagerDisconnectedPeripheral (object sender, CBPeripheralErrorEventArgs e)
        {
            Console.WriteLine ("Disconnected peripheral: " + e.Error?.ToString ());

            peripheral.DiscoveredService -= OnPeripheralDiscoveredService;
            peripheral.DiscoveredCharacteristic -= OnPeripheralDiscoveredCharacteristic;
            peripheral.UpdatedCharacterteristicValue -= OnPeripheralUpdatedCharacteristicValue;
            peripheral = null;

            IsConnected = false;

            if (Disconnected != null) {
                Disconnected (null, EventArgs.Empty);
            }

            Connect (e.Peripheral);
        }

        static void OnPeripheralDiscoveredService (object sender, NSErrorEventArgs e)
        {
            if (e.Error != null) {
                Console.WriteLine (e.Error);

                peripheral.DiscoverServices ();

                return;
            }

            foreach (CBService s in (sender as CBPeripheral).Services) {
                Console.WriteLine ("Discovered service {0}", s);

                peripheral.DiscoverCharacteristics (s);
            }
        }

        static void OnPeripheralDiscoveredCharacteristic (object sender, CBServiceEventArgs e)
        {
            foreach (CBCharacteristic c in e.Service.Characteristics) {
                Console.WriteLine ("Discovered characteristic {0}", c);
            }
        }

        static void OnPeripheralUpdatedCharacteristicValue (object sender, CBCharacteristicEventArgs e)
        {
            if (e.Characteristic.Value == null) {
                return;
            }

            Console.WriteLine ("Updated characteristic value {0}", string.Join (",", e.Characteristic.Value.ToArray ()));
        }
    }
}