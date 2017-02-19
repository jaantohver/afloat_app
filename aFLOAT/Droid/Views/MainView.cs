using System;

using Android.App;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Graphics;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System.Threading;

namespace aFLOAT.Droid
{
    public class MainView : RelativeLayout, IOnMapReadyCallback
    {
        MainActivity activity;

        GoogleMap googleMap;

        public readonly Button StartReadButton, EndReadButton, LedOnButton, LedOffButton, ResetButton, DisconnectButton;
        public readonly MapView Map;

        public MainView (MainActivity activity) : base (activity)
        {
            this.activity = activity;

            BtReceiver.NoFish += NoFish;
            BtReceiver.YesFish += YesFish;
            BtReceiver.LocationChanged += LocationChanged;

            SetBackgroundColor (Color.Yellow);

            StartReadButton = new Button (activity);
            StartReadButton.Text = "Start read";

            EndReadButton = new Button (activity);
            EndReadButton.Text = "End read";

            LedOnButton = new Button (activity);
            LedOnButton.Text = "+";

            LedOffButton = new Button (activity);
            LedOffButton.Text = "-";

            ResetButton = new Button (activity);
            ResetButton.Text = "R";

            DisconnectButton = new Button (activity);
            DisconnectButton.Text = "Disconnect";

            Map = new MapView (activity);
            Map.GetMapAsync (this);

            LinearLayout container = new LinearLayout (activity);
            container.Orientation = Orientation.Vertical;

            //container.AddView (StartReadButton);
            //container.AddView (EndReadButton);
            //container.AddView (LedOnButton);
            //container.AddView (LedOffButton);
            //container.AddView (ResetButton);
            //container.AddView (DisconnectButton);
            container.AddView (Map);

            //AddView (Map, new ViewGroup.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            AddView (container, new ViewGroup.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
        }

        bool fish;
        bool infoVisible = true;
        bool ledOn;
        Marker m;

        public void OnMapReady (GoogleMap googleMap)
        {
            this.googleMap = googleMap;

            MarkerOptions opts = new MarkerOptions ();
            opts.SetPosition (new LatLng (58.3793627, 26.7114551));
            opts.SetTitle ("58.3793627, 26.7114551");
            opts.SetSnippet ("No fish");

            m = googleMap.AddMarker (opts);
            m.ShowInfoWindow ();

            googleMap.MoveCamera (CameraUpdateFactory.NewLatLngZoom (new LatLng (58.3793627, 26.7114551), 18.5f));
            googleMap.MarkerClick += (sender, e) => {
                if (infoVisible) {
                    e.Marker.HideInfoWindow ();
                } else {
                    e.Marker.ShowInfoWindow ();
                }

                infoVisible = !infoVisible;
            };
            googleMap.InfoWindowClick += (sender, e) => {
                if (ledOn) {
                    new Thread (() => {
                        BtReceiver.LedOff ();
                    }).Start ();
                } else {
                    new Thread (() => {
                        BtReceiver.LedOn ();
                    }).Start ();
                }

                ledOn = !ledOn;
            };
        }

        void NoFish (object sender, EventArgs e)
        {
            activity.RunOnUiThread (delegate {
                fish = false;

                LatLng ll = RandomLatLng ();

                MarkerOptions opts = new MarkerOptions ();
                opts.SetPosition (RandomLatLng ());
                opts.SetTitle (ll.Latitude + ", " + ll.Longitude);
                if (fish) {
                    opts.SetSnippet ("Fish caught");
                } else {
                    opts.SetSnippet ("No fish");
                }

                m.Remove ();

                m = googleMap.AddMarker (opts);

                if (infoVisible) {
                    m.ShowInfoWindow ();
                }

                //googleMap.MoveCamera (CameraUpdateFactory.NewLatLng (ll));
            });
        }

        void YesFish (object sender, EventArgs e)
        {
            activity.RunOnUiThread (delegate {
                fish = true;

                LatLng ll = RandomLatLng ();

                MarkerOptions opts = new MarkerOptions ();
                opts.SetPosition (RandomLatLng ());
                opts.SetTitle (ll.Latitude + ", " + ll.Longitude);
                if (fish) {
                    opts.SetSnippet ("Fish caught");
                } else {
                    opts.SetSnippet ("No fish");
                }

                m.Remove ();

                m = googleMap.AddMarker (opts);

                if (infoVisible) {
                    m.ShowInfoWindow ();
                }

                //googleMap.MoveCamera (CameraUpdateFactory.NewLatLng (ll));

                activity.Vibrate ();
            });
        }

        void LocationChanged (object sender, LocationEventArgs e)
        {
            activity.RunOnUiThread (delegate {
                LatLng ll = RandomLatLng ();

                MarkerOptions opts = new MarkerOptions ();
                opts.SetPosition (RandomLatLng ());
                opts.SetTitle (ll.Latitude + ", " + ll.Longitude);
                if (fish) {
                    opts.SetSnippet ("Fish caught");
                } else {
                    opts.SetSnippet ("No fish");
                }

                m.Remove ();

                m = googleMap.AddMarker (opts);

                if (infoVisible) {
                    m.ShowInfoWindow ();
                }

                //googleMap.MoveCamera (CameraUpdateFactory.NewLatLng (new LatLng (e.Lat, e.Lon)));
            });
        }

        Random r = new Random ();

        LatLng RandomLatLng ()
        {
            double lat = Math.Round (r.NextDouble () * (58.3793700 - 58.3793500) + 58.3793500, 5);
            double lon = Math.Round (r.NextDouble () * (26.7114600 - 26.7114400) + 26.7114400, 5);

            return new LatLng (lat, lon);
        }
    }
}