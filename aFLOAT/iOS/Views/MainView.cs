using UIKit;
using CoreGraphics;

namespace aFLOAT.iOS
{
    public class MainView : UIView
    {
        readonly UIView deviceTableContainer;
        public readonly UITableView DeviceTable;

        public MainView ()
        {
            BackgroundColor = Colours.Background;

            deviceTableContainer = new UIView ();
            deviceTableContainer.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 100);

            DeviceTable = new UITableView ();
            DeviceTable.Source = new DeviceTableSource ();
            DeviceTable.BackgroundColor = UIColor.Clear;
            DeviceTable.TableFooterView = new UIView (CGRect.Empty);

            deviceTableContainer.AddSubviews (
                DeviceTable
            );

            AddSubviews (
                deviceTableContainer
            );
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();

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