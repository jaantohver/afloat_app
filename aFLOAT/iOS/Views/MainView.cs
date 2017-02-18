using System;

using UIKit;
using CoreGraphics;

namespace aFLOAT.iOS
{
    public class MainView : UIView
    {
        readonly Bobber bobber;
        readonly UIView deviceTableContainer;
        readonly Background background;
        public readonly UITableView DeviceTable;

        public MainView ()
        {
            background = new Background ();

            bobber = new Bobber ();

            deviceTableContainer = new UIView ();
            deviceTableContainer.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 100);

            DeviceTable = new UITableView ();
            DeviceTable.Source = new DeviceTableSource ();
            DeviceTable.BackgroundColor = UIColor.Clear;
            DeviceTable.TableFooterView = new UIView (CGRect.Empty);

            deviceTableContainer.AddSubviews (
            //DeviceTable
            );

            AddSubviews (
                background,
                bobber,
                deviceTableContainer
            );

            Foundation.NSTimer.CreateRepeatingScheduledTimer (3, delegate {
                bobber.Rotate ();
            });
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();

            nfloat bobberSize = Frame.Width - 100;

            background.Frame = Bounds;

            bobber.Frame = new CGRect (
                (Frame.Width - bobberSize) / 2,
                Frame.Height / 3 - bobberSize / 2,
                bobberSize,
                bobberSize
            );

            deviceTableContainer.Frame = Bounds;

            DeviceTable.Frame = new CGRect (
                25,
                25,
                deviceTableContainer.Frame.Width - 50,
                deviceTableContainer.Frame.Height - 50
            );
        }

        public void ShowDeviceTable ()
        {
            deviceTableContainer.Hidden = false;
        }

        public void HideDeviceTable ()
        {
            deviceTableContainer.Hidden = true;
        }
    }
}