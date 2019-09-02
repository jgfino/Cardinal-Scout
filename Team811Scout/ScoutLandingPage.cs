using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    /*This activity displays a list of recently scouted matches which can be selected to view
     * more details. It also allows a user to scout a new match*/

    [Activity(Label = "scoutLandingPage", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ScoutLandingPage: Activity
    {
        //declare objects for controls
        private Button bAddTeam;

        private ListView recentMatches;
        private TextView textTitle;
        private Button bRefresh;
        private Button bViewEvent;

        //placeholder for current event being scouted for
        private Event currentEvent;

        //get database instance
        private EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Scout_Landing_Page);
            //get controls from layout and assign event handlers
            bAddTeam = FindViewById<Button>(Resource.Id.bAddTeam);
            bAddTeam.Click += ButtonClicked;
            recentMatches = FindViewById<ListView>(Resource.Id.recentMatches);
            recentMatches.ItemClick += ListViewClick;
            textTitle = FindViewById<TextView>(Resource.Id.textTitle);
            bRefresh = FindViewById<Button>(Resource.Id.bRefreshMatches);
            bRefresh.Click += ButtonClicked;
            bViewEvent = FindViewById<Button>(Resource.Id.bViewData);
            bViewEvent.Click += ButtonClicked;
            //get the current event from the database
            currentEvent = eData.GetCurrentEvent();
            //display recent matches in ListView
            var matchAdapter = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, eData.GetMatchDisplayList(currentEvent.eventID));
            recentMatches.Adapter = matchAdapter;
            //set title text based on current event
            textTitle.Text += currentEvent.eventName + "'";
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            //decide which button was clicked
            if ((sender as Button) == bRefresh)
            {
                this.Recreate();
            }
            else if ((sender as Button) == bAddTeam)
            {
                //each event can have a max of 80 matches; more makes reading QR codes difficult
                if (eData.GetMatchDataForEvent(currentEvent.eventID).Count > 79)
                {
                    Popup.Single("Alert", "Max 80 matches per event reached", "OK", this);
                }
                //if ok, go to scout form
                else
                {
                    StartActivity(typeof(ScoutForm));
                    Finish();
                }
            }
            else if ((sender as Button) == bViewEvent)
            {
                try
                {
                    //set the current match to view to the selected match
                    eData.SetCurrentMatch(eData.GetMatchDataForEvent(currentEvent.eventID)[selectedIndex].ID);
                    StartActivity(typeof(ViewMatchData));
                }
                //if no event is selected, it throws an exception
                catch
                {
                    Popup.Single("Alert", "Please select a match to view", "OK", this);
                }
            }
        }

        //handle selecting matches in the ListView
        private int selectedIndex;

        private void ListViewClick(object sender, ItemClickEventArgs e)
        {
            selectedIndex = e.Position;
            bViewEvent.Text = "Details for Match " + eData.GetMatchDataForEvent(currentEvent.eventID)[selectedIndex].matchNumber.ToString();
        }
    }
}