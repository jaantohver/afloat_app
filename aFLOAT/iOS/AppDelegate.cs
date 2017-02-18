using UIKit;
using Foundation;

namespace aFLOAT.iOS
{
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window {
            get;
            set;
        }

        public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow (UIScreen.MainScreen.Bounds);
            Window.RootViewController = new MainController ();
            Window.MakeKeyAndVisible ();

            BleClient.Init ();

            return true;
        }
    }
}