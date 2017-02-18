using Android.OS;
using Android.App;

namespace aFLOAT.Droid
{
    [Activity (MainLauncher = true, Label = "aFLOAT", Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
        }
    }
}