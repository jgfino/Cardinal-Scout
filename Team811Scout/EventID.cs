using Android.App;
using Android.OS;
using Android.Widget;
using System;

namespace Team811Scout
{
    /*this activity is used to reassign event ids to events if for whatever reason the ids dont match
     * across the 6 devices being used for scouting*/

    [Activity(Label = "EventID", Theme = "@style/AppTheme", MainLauncher = false)]
    public class EventID: Activity
    {
        //declare objects for controls
        private Button bConfirm;

        private EditText newID;
        private TextView title;
        private Event currentEvent;

        //get database instance
        private EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Event_ID);
            //get controls from layout and assign event handlers
            bConfirm = FindViewById<Button>(Resource.Id.bConfirmID);
            bConfirm.Click += ButtonClicked;
            newID = FindViewById<EditText>(Resource.Id.textID);
            title = FindViewById<TextView>(Resource.Id.titleID);
            //get event we will be editing
            currentEvent = eData.GetCurrentEvent();
            //display current event name at the top
            title.Text += "'" + currentEvent.eventName + "'";
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            //decide which button was clicked
            if ((sender as Button) == bConfirm)
            {
                try
                {
                    //change the id based on entered value
                    eData.ChangeEventID(currentEvent.eventID, int.Parse(newID.Text));
                    Finish();
                }
                //if the database has a duplicate id, it will throw an exception
                catch
                {
                    Popup.Single("Alert", "Please enter a new ID not used by an existing event", "OK", this);
                }
            }
        }
    }
}