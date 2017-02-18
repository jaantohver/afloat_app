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
            return true;
        }
    }
}