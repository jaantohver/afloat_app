using System;

using Android.OS;
using Android.App;
using Android.Content;
using Android.Bluetooth;

namespace aFLOAT.Droid
{
    [Activity (MainLauncher = true, Label = "aFLOAT", Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        MainView contentView;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            SetContentView (contentView = new MainView (this));

            IntentFilter filter = new IntentFilter ();
            filter.AddAction (BluetoothDevice.ActionAclConnected);
            filter.AddAction (BluetoothDevice.ActionAclDisconnectRequested);
            filter.AddAction (BluetoothDevice.ActionAclDisconnected);
            RegisterReceiver (new BtReceiver (), filter);

            contentView.Map.OnCreate (savedInstanceState);
        }

        protected override void OnStart ()
        {
            base.OnStart ();

            contentView.Map.OnStart ();
        }

        protected override void OnResume ()
        {
            base.OnResume ();

            contentView.button.Click += OnButtonClick;

            BtClient.Scanning += OnScanning;
            BtClient.Found += OnDeviceFound;
            BtClient.NotFound += OnDeviceNotFound;
            BtClient.Connecting += OnConencting;
            BtClient.Connected += OnConnected;
            BtClient.Disconnecting += OnDisconnecting;
            BtClient.Disconnected += OnDisconnected;

            BtClient.Init ();

            contentView.Map.OnResume ();
        }

        protected override void OnPause ()
        {
            base.OnPause ();

            contentView.Map.OnPause ();
        }

        protected override void OnStop ()
        {
            base.OnStop ();

            contentView.Map.OnStop ();
        }

        protected override void OnDestroy ()
        {
            base.OnDestroy ();

            contentView.Map.OnDestroy ();
        }

        protected override void OnSaveInstanceState (Bundle outState)
        {
            base.OnSaveInstanceState (outState);

            contentView.Map.OnSaveInstanceState (outState);
        }

        public override void OnLowMemory ()
        {
            base.OnLowMemory ();

            contentView.Map.OnLowMemory ();
        }

        void OnButtonClick (object sender, EventArgs e)
        {
            BtReceiver.WriteToSocket ();
        }

        void OnScanning (object sender, EventArgs e)
        {
            RunOnUiThread (delegate {
                contentView.SetStatusText ("Scanning");
            });
        }

        void OnDeviceFound (object sender, EventArgs e)
        {
            RunOnUiThread (delegate {
                contentView.SetStatusText ("Device found");

                BtClient.Connect (this);
            });
        }

        void OnDeviceNotFound (object sender, EventArgs e)
        {
            RunOnUiThread (delegate {
                contentView.SetStatusText ("Device not found");
            });
        }

        void OnConencting (object sender, EventArgs e)
        {
            RunOnUiThread (delegate {
                contentView.SetStatusText ("Connecting");
            });
        }

        void OnConnected (object sender, EventArgs e)
        {
            RunOnUiThread (delegate {
                contentView.SetStatusText ("Connected");
            });
        }

        void OnDisconnecting (object sender, EventArgs e)
        {
            RunOnUiThread (delegate {
                contentView.SetStatusText ("Disconnecting");
            });
        }

        void OnDisconnected (object sender, EventArgs e)
        {
            RunOnUiThread (delegate {
                contentView.SetStatusText ("Disconnected");
            });
        }
    }
}