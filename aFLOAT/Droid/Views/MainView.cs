using System;

using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Graphics;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace aFLOAT.Droid
{
    public class MainView : RelativeLayout, IOnMapReadyCallback
    {
        readonly TextView statusLabel;
        public readonly Button button, DisconnectButton;
        public readonly MapView Map;

        public MainView (Context context) : base (context)
        {
            SetBackgroundColor (Color.Yellow);

            statusLabel = new TextView (context);
            statusLabel.Text = "State = Initializing";
            statusLabel.SetTextColor (Color.Black);
            statusLabel.TextSize = 25;

            button = new Button (context);
            button.Text = "Send data";

            DisconnectButton = new Button (context);
            DisconnectButton.Text = "Disconnect";

            Map = new MapView (context);
            Map.GetMapAsync (this);

            LinearLayout container = new LinearLayout (context);
            container.Orientation = Orientation.Vertical;

            container.AddView (statusLabel);
            container.AddView (button);
            container.AddView (DisconnectButton);

            AddView (Map, new ViewGroup.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            AddView (container, new ViewGroup.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
        }

        public void SetStatusText (string text)
        {
            statusLabel.Text = "State = " + text;
        }

        bool infoVisible;

        public void OnMapReady (GoogleMap googleMap)
        {
            MarkerOptions opts = new MarkerOptions ();
            opts.SetPosition (new LatLng (58.3793627, 26.7114551));
            opts.SetTitle ("Title here");
            opts.SetSnippet ("Snippet here");

            Marker m = googleMap.AddMarker (opts);

            googleMap.MoveCamera (CameraUpdateFactory.NewLatLngZoom (new LatLng (58.3793627, 26.7114551), 19.0f));
            googleMap.MarkerClick += (sender, e) => {
                if (infoVisible) {
                    e.Marker.HideInfoWindow ();
                } else {
                    e.Marker.ShowInfoWindow ();
                }

                infoVisible = !infoVisible;
            };
            googleMap.InfoWindowClick += (sender, e) => {
                e.Marker.HideInfoWindow ();

                infoVisible = false;
            };
        }
    }
}