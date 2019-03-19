using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    [Activity(Label = "scoutLandingPage", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ScoutLandingPage: Activity
    {
        Button bAddTeam;
        ListView recentMatches;
        TextView textTitle;
        Event currentEvent;
        EventDatabase eData;
        List<SpannableString> displayMatches;
        List<ScoutData> matches;
        List<int> scoutIds;
        Button bRefresh;
        Button bViewEvent;

       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.scoutLandingPage);

            bAddTeam = FindViewById<Button>(Resource.Id.bAddTeam);
            recentMatches = FindViewById<ListView>(Resource.Id.recentMatches);
            textTitle = FindViewById<TextView>(Resource.Id.textTitle);
            eData = new EventDatabase();
            scoutIds = new List<int>();
            bRefresh = FindViewById<Button>(Resource.Id.bRefreshMatches);
            bRefresh.Click += ButtonClicked;
            bViewEvent = FindViewById<Button>(Resource.Id.bViewData);
            bViewEvent.Click += ButtonClicked;

            bAddTeam.Click += ButtonClicked;

            currentEvent = eData.CurrentEvent();
            matches = eData.GetDataForEvent(currentEvent.eventID);
            

            var matchAdapter = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, eData.GetMatchDisplayList(currentEvent.eventID));
            recentMatches.Adapter = matchAdapter;

            recentMatches.ItemClick += ListViewClick;

            textTitle.Text += currentEvent.eventName+"'";
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            if ((sender as Button) == bRefresh)
            {
                this.Recreate();
            }
            else if((sender as Button)==bAddTeam)
            {
                if (matches.Count > 79)
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog missingDetails = dialog.Create();
                    missingDetails.SetTitle("Alert");
                    missingDetails.SetMessage("Max 80 matches per event reached");
                    missingDetails.SetButton("OK", (c, ev) =>
                    {

                    });
                    missingDetails.Show();
                }
                else
                {
                    StartActivity(typeof(ScoutForm));
                    Finish();
                }
            }
            else if((sender as Button)==bViewEvent)
            {
                try
                {
                    eData.SetCurrentMatch(matches[selectedIndex].ID);
                    Finish();
                    StartActivity(typeof(RecentData));
                }
                catch
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog missingDetails = dialog.Create();
                    missingDetails.SetTitle("Alert");
                    missingDetails.SetMessage("Please select a match to view");
                    missingDetails.SetButton("OK", (c, ev) =>
                    {

                    });
                    missingDetails.Show();
                }
            }
        }
        
        int selectedIndex;

        private void ListViewClick(object sender, ItemClickEventArgs e)
        {
            
            selectedIndex = e.Position;
            bViewEvent.Text = "View Data for Match " + matches[selectedIndex].matchNumber.ToString();
           

        }
    }
}