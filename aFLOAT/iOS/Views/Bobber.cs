using System;

using UIKit;
using Foundation;
using CoreGraphics;
using CoreAnimation;

namespace aFLOAT.iOS
{
    public class Bobber : UIView
    {
        bool upsideDown;

        UIView redPart, whitePart;

        public Bobber ()
        {
            ClipsToBounds = true;
            Layer.BorderWidth = 5;
            Layer.BorderColor = UIColor.Red.CGColor;

            redPart = new UIView ();
            redPart.BackgroundColor = UIColor.Red;

            whitePart = new UIView ();
            whitePart.BackgroundColor = UIColor.White;

            AddSubviews (
                redPart,
                whitePart
            );
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();

            Layer.CornerRadius = Frame.Width / 2;

            redPart.Frame = new CGRect (
                0,
                0,
                Frame.Width,
                Frame.Height / 2
            );

            whitePart.Frame = new CGRect (
                0,
                redPart.Frame.Bottom,
                Frame.Width,
                Frame.Height - redPart.Frame.Bottom
            );
        }

        public void Rotate ()
        {
            CASpringAnimation rotationAnimation = new CASpringAnimation ();
            rotationAnimation.KeyPath = "transform.rotation.z";
            rotationAnimation.RepeatCount = 1;
            rotationAnimation.InitialVelocity = 5;
            rotationAnimation.Mass = 1.5f;
            rotationAnimation.RemovedOnCompletion = false;
            rotationAnimation.FillMode = CAFillMode.Forwards;

            if (upsideDown) {
                rotationAnimation.From = new NSNumber (Math.PI);
                rotationAnimation.To = new NSNumber (Math.PI * 2);
            } else {
                rotationAnimation.From = new NSNumber (0);
                rotationAnimation.To = new NSNumber (Math.PI);
            }
            rotationAnimation.Duration = rotationAnimation.SettlingDuration;

            Layer.AddAnimation (rotationAnimation, nameof (rotationAnimation));

            upsideDown = !upsideDown;
        }
    }
}