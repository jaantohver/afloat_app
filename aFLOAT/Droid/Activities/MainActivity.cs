using System;
using System.Threading;

using Android.OS;
using Android.App;
using Android.Content;
using Android.Bluetooth;

namespace aFLOAT.Droid
{
    [Activity (MainLauncher = true, Label = "aFloat", Icon = "@mipmap/icon")]
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

            Timer t = new Timer (delegate {
                BtReceiver.StartRead ();
            }, null, 5000, int.MaxValue);
        }

        protected override void OnStart ()
        {
            base.OnStart ();

            contentView.Map.OnStart ();
        }

        protected override void OnResume ()
        {
            base.OnResume ();

            contentView.StartReadButton.Click += OnButtonClick;
            contentView.EndReadButton.Click += EndRead;
            contentView.LedOnButton.Click += delegate {
                new Thread (() => {
                    BtReceiver.LedOn ();
                }).Start ();
            };
            contentView.LedOffButton.Click += delegate {
                new Thread (() => {
                    BtReceiver.LedOff ();
                }).Start ();
            };
            contentView.ResetButton.Click += delegate {
                new Thread (() => {
                    BtReceiver.Reset ();
                }).Start ();
            };

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
            BtReceiver.StartRead ();
        }

        void EndRead (object sender, EventArgs e)
        {
            BtReceiver.EndRead ();
        }

        void OnScanning (object sender, EventArgs e)
        {
        }

        void OnDeviceFound (object sender, EventArgs e)
        {
            RunOnUiThread (delegate {
                BtClient.Connect (this);
            });
        }

        void OnDeviceNotFound (object sender, EventArgs e)
        {
        }

        void OnConencting (object sender, EventArgs e)
        {
        }

        void OnConnected (object sender, EventArgs e)
        {
        }

        void OnDisconnecting (object sender, EventArgs e)
        {
        }

        void OnDisconnected (object sender, EventArgs e)
        {
        }

        AlertDialog a;

        public void Vibrate ()
        {
            Vibrator mVibrator = (Vibrator)GetSystemService (Context.VibratorService);

            // Vibrate for 300 milliseconds
            mVibrator.Vibrate (500);

            if (a == null || !a.IsShowing) {
                AlertDialog.Builder b = new AlertDialog.Builder (this, AlertDialog.ThemeDeviceDefaultLight);
                b.SetTitle ("Fish caught!");
                b.SetNeutralButton ("Nice", delegate {
                    BtReceiver.Reset ();
                });

                RunOnUiThread (delegate {
                    a = b.Show ();
                });
            }
        }
    }
}