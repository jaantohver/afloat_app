using System.IO;

using Android.Content;
using Android.Bluetooth;

using static System.Console;

namespace aFLOAT.Droid
{
    [BroadcastReceiver]
    public class BtReceiver : BroadcastReceiver
    {
        static BluetoothDevice device;
        static BluetoothSocket socket;

        public BtReceiver ()
        {
            BtClient.Found += delegate {
                device = BtClient.Device;
            };
        }

        public override void OnReceive (Context context, Intent intent)
        {
            string action = intent.Action;
            BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra (BluetoothDevice.ExtraDevice);

            if (BluetoothDevice.ActionFound == (action)) {
                //... //Device found
                WriteLine ("!!!Device found");
            } else if (BluetoothDevice.ActionAclConnected == (action)) {
                //... //Device is now connected
                WriteLine ("!!!Device connected");

                socket = device.CreateRfcommSocketToServiceRecord (Java.Util.UUID.FromString ("00001101-0000-1000-8000-00805F9B34FB"));
                socket.Connect ();

                WriteLine (socket.IsConnected);
                WriteLine (socket.ConnectionType);

                Stream inStream = socket.InputStream;
                Stream outStream = socket.OutputStream;

                WriteLine (inStream.CanRead);
                WriteLine (outStream.CanWrite);

                WriteLine ("---");
            } else if (BluetoothAdapter.ActionDiscoveryFinished == (action)) {
                //... //Done searching
                WriteLine ("!!!Discovery finished");
            } else if (BluetoothDevice.ActionAclDisconnectRequested == (action)) {
                //... //Device is about to disconnect
                WriteLine ("!!!About to disconnect");
            } else if (BluetoothDevice.ActionAclDisconnected == (action)) {
                //... //Device has disconnected
                WriteLine ("!!!Disconnected");
            }
        }

        public static void WriteToSocket ()
        {
            if (socket == null) {
                socket = device.CreateRfcommSocketToServiceRecord (Java.Util.UUID.FromString ("00001101-0000-1000-8000-00805F9B34FB"));
                socket.Connect ();

                WriteLine (socket.IsConnected);
                WriteLine (socket.ConnectionType);

                Stream inStream = socket.InputStream;
                Stream outStream = socket.OutputStream;

                WriteLine (inStream.CanRead);
                WriteLine (outStream.CanWrite);

                WriteLine ("---");
            }

            if (socket.IsConnected && socket.OutputStream.CanWrite) {
                //byte [] buf = new System.Text.ASCIIEncoding ().GetBytes ("TEST");

                //socket.OutputStream.Write (buf, 0, buf.Length);


                if (socket.InputStream.CanRead) {
                    byte [] buffer = new byte [20];

                    var result = socket.InputStream.BeginRead (buffer, 0, 20, delegate {
                        WriteLine (string.Join (",", buffer));
                    }, null);

                    WriteLine (result);
                }
            } else {
                throw new System.Exception ("Fail");
            }
        }
    }
}