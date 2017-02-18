using System;

using UIKit;
using Foundation;
using CoreBluetooth;

namespace aFLOAT.iOS
{
    public class MainController : UIViewController
    {
        MainView contentView;

        NSTimer listRefreshTimer;

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            contentView = new MainView ();

            View = contentView;
        }

        public override void ViewDidAppear (bool animated)
        {
            base.ViewDidAppear (animated);

            (contentView.DeviceTable.Source as DeviceTableSource).DeviceSelected += OnDeviceSelected;

            BleClient.Connected += OnPeripheralConnected;
            BleClient.Disconnected += OnPeripheralDisconnected;
            BleClient.DeviceDiscovered += OnDeviceDiscovered;

            if (BleClient.IsConnected) {
                OnPeripheralConnected (this, EventArgs.Empty);
            } else {
                OnPeripheralDisconnected (this, EventArgs.Empty);
            }

            listRefreshTimer = NSTimer.CreateRepeatingScheduledTimer (1, (t) => contentView.DeviceTable.ReloadData ());
        }

        public override void ViewWillDisappear (bool animated)
        {
            base.ViewWillDisappear (animated);

            (contentView.DeviceTable.Source as DeviceTableSource).DeviceSelected -= OnDeviceSelected;

            BleClient.Connected -= OnPeripheralConnected;
            BleClient.Disconnected -= OnPeripheralDisconnected;
            BleClient.DeviceDiscovered -= OnDeviceDiscovered;
        }

        void OnDeviceSelected (object sender, DeviceEventArgs e)
        {
            listRefreshTimer.Invalidate ();
            listRefreshTimer = null;

            BleClient.Connect (e.Device.Peripheral);

            contentView.HideDeviceTable ();
        }

        void OnPeripheralConnected (object sender, EventArgs e)
        {
        }

        void OnPeripheralDisconnected (object sender, EventArgs e)
        {
        }

        void OnDeviceDiscovered (object sender, CBDiscoveredPeripheralEventArgs e)
        {
            Device d = new Device ();
            d.Peripheral = e.Peripheral;
            d.RSSI = e.RSSI?.Int32Value ?? -127;

            (contentView.DeviceTable.Source as DeviceTableSource).AddDevice (d);
        }
    }
}