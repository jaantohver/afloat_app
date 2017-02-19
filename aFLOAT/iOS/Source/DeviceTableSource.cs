using System;
using System.Collections.Generic;

using UIKit;
using Foundation;

namespace aFLOAT.iOS
{
    public class DeviceTableSource : UITableViewSource
    {
        readonly List<Device> devices;

        public event EventHandler<DeviceEventArgs> DeviceSelected;

        public DeviceTableSource ()
        {
            devices = new List<Device> ();
        }

        public override nint RowsInSection (UITableView tableview, nint section)
        {
            return devices.Count;
        }

        public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
        {
            Device device = devices [indexPath.Row];

            UITableViewCell cell = new UITableViewCell (UITableViewCellStyle.Subtitle, string.Empty);
            cell.TextLabel.Text = device.Peripheral.Name ?? "N/A";
            cell.DetailTextLabel.Text = "RSSI: " + device.RSSI;
            cell.BackgroundColor = UIColor.White;

            return cell;
        }

        public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
        {
            DeviceEventArgs args = new DeviceEventArgs ();
            args.Device = devices [indexPath.Row];

            DeviceSelected?.Invoke (this, args);
        }

        public void AddDevice (Device device)
        {
            if (!devices.Contains (device)) {
                devices.Add (device);
            } else {
                foreach (Device d in devices) {
                    if (d.Equals (device)) {
                        d.RSSI = device.RSSI;

                        break;
                    }
                }
            }

            devices.Sort ((x, y) => y.RSSI.CompareTo (x.RSSI));
        }

        public void ClearDevices ()
        {
            devices.Clear ();
        }
    }
}