using UIKit;
using CoreGraphics;

namespace aFLOAT.iOS
{
    public class Background : UIView
    {
        UIView whitePart, bluePart;

        public Background ()
        {
            whitePart = new UIView ();
            whitePart.BackgroundColor = UIColor.White;

            bluePart = new UIView ();
            bluePart.BackgroundColor = UIColor.Blue;

            AddSubviews (
                whitePart,
                bluePart
            );
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();

            whitePart.Frame = new CGRect (
                0,
                0,
                Frame.Width,
                Frame.Height / 3
            );

            bluePart.Frame = new CGRect (
                0,
                whitePart.Frame.Bottom,
                Frame.Width,
                Frame.Height - whitePart.Frame.Bottom
            );
        }
    }
}