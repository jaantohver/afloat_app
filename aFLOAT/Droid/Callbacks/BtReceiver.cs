using System;
using System.IO;
using System.Text;

using Android.Content;
using Android.Bluetooth;

using static System.Console;

namespace aFLOAT.Droid
{
    public class LocationEventArgs : EventArgs
    {
        public double Lat { get; set; }

        public double Lon { get; set; }
    }

    [BroadcastReceiver]
    public class BtReceiver : BroadcastReceiver
    {
        public static event EventHandler NoFish, YesFish;
        public static event EventHandler<LocationEventArgs> LocationChanged;

        static bool reading, writing;

        static readonly ASCIIEncoding encoder = new ASCIIEncoding ();

        static BluetoothDevice device;

        static BluetoothSocket socket;
        static BluetoothSocket Socket {
            get {
                if (socket == null) {
                    try {
                        socket = device.CreateInsecureRfcommSocketToServiceRecord (Java.Util.UUID.FromString ("00001101-0000-1000-8000-00805F9B34FB"));
                        socket.Connect ();

                        WriteLine (Socket.IsConnected);
                        WriteLine (Socket.ConnectionType);
                        WriteLine (socket.InputStream.CanRead);
                        WriteLine (socket.OutputStream.CanWrite);

                        WriteLine ("---");
                    } catch (Exception e) {
                        WriteLine (e.GetType ());
                    }
                }

                return socket;
            }
        }

        public BtReceiver ()
        {
            BtClient.Found += delegate {
                device = BtClient.Device;
            };
        }

        public override void OnReceive (Context context, Intent intent)
        {
            string action = intent.Action;
            device = (BluetoothDevice)intent.GetParcelableExtra (BluetoothDevice.ExtraDevice);
            switch ((action)) {
            case BluetoothDevice.ActionFound:
                //... //Device found
                WriteLine ("!!!Device found");
                break;
            case BluetoothDevice.ActionAclConnected:
                //... //Device is now connected
                WriteLine ("!!!Device connected");
                break;
            case BluetoothAdapter.ActionDiscoveryFinished:
                //... //Done searching
                WriteLine ("!!!Discovery finished");
                break;
            case BluetoothDevice.ActionAclDisconnectRequested:
                //... //Device is about to disconnect
                WriteLine ("!!!About to disconnect");
                break;
            case BluetoothDevice.ActionAclDisconnected:
                //... //Device has disconnected
                WriteLine ("!!!Disconnected");
                break;
            }
        }

        public static void StartRead ()
        {
            if (reading) {
                return;
            }

            if (Socket == null) {
                WriteLine ("Socket null");

                return;
            }

            if (Socket.IsConnected && Socket.InputStream.CanRead) {
                reading = true;

                ReadNextBytes (0);
            }
        }

        public static void EndRead ()
        {
            if (Socket == null) {
                WriteLine ("Socket null");

                return;
            }

            reading = false;

            Socket.InputStream.EndRead (null);
        }

        const int bufSize = 100;

        static void ReadNextBytes (int offset)
        {
            byte [] buffer = new byte [bufSize];

            var result = Socket.InputStream.BeginRead (buffer, offset, bufSize, delegate {
                string raw = string.Join (", ", buffer);
                string ascii = encoder.GetString (buffer);

                WriteLine ("RAW:\t" + raw);
                WriteLine ("ASCII:\t" + ascii);
                WriteLine ("----");

                if (ascii.Contains ("STATE") && ascii.Contains ("0")) {
                    NoFish?.Invoke (null, EventArgs.Empty);
                } else if (ascii.Contains ("STATE") && ascii.Contains ("1")) {
                    YesFish?.Invoke (null, EventArgs.Empty);
                }

                //$GPGLL,5821.95899,N,02641.45598,e,085306.00,a,a * 6f

                if (ascii.Contains ("GPGLL")) {
                    string [] pieces = ascii.Split (',');

                    double lat;
                    double lon;

                    try {
                        double.TryParse (pieces [1], out lat);
                        double.TryParse (pieces [3], out lon);

                        if (lat > 0 && lon > 0) {
                            LocationEventArgs args = new LocationEventArgs ();
                            args.Lat = lat / 100;
                            args.Lon = lon / 100;

                            LocationChanged?.Invoke (null, args);
                        }
                    } catch { }
                }

                if (!reading) {
                    return;
                }

                if (buffer [buffer.Length - 1] == 0) {
                    ReadNextBytes (0);
                } else {
                    ReadNextBytes (bufSize);
                }
            }, null);
        }

        public static void LedOn ()
        {
            if (Socket == null) {
                WriteLine ("Socket null");

                return;
            }

            if (writing) {
                return;
            }

            writing = true;

            byte [] buf = encoder.GetBytes ("+");

            try {
                Socket.OutputStream.Write (buf, 0, buf.Length);
            } catch (Exception e) {
                WriteLine (e.GetType ());
            }

            writing = false;
        }

        public static void LedOff ()
        {
            if (Socket == null) {
                WriteLine ("Socket null");

                return;
            }

            if (writing) {
                return;
            }

            writing = true;

            byte [] buf = encoder.GetBytes ("-");

            try {
                Socket.OutputStream.Write (buf, 0, buf.Length);
            } catch (Exception e) {
                WriteLine (e.GetType ());
            }

            writing = false;
        }

        public static void Reset ()
        {
            if (Socket == null) {
                WriteLine ("Socket null");

                return;
            }

            if (writing) {
                return;
            }

            writing = true;

            byte [] buf = encoder.GetBytes ("R");

            try {
                Socket.OutputStream.Write (buf, 0, buf.Length);
            } catch (Exception e) {
                WriteLine (e.GetType ());
            }

            writing = false;
        }
    }
}