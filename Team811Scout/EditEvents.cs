using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    /*This activity is started when "edit events" is clicked in instructions/settings.
     * It lists events and allows the user to edit an incorrect event id or delete an event
     * and its associated matches"*/

    [Activity(Label = "editevents", Theme = "@style/AppTheme", MainLauncher = false)]
    public class EditEvents: Activity
    {
        //declare objects that will refer to controls
        private ListView eventList;

        private Button bDeleteEvent;
        private Button bEditID;
        private Button bRefresh;

        //selected Index of the ListView
        private int selectedIndex;

        //placeholder to refer to selected event
        private Event selectedEvent;

        //get database instance
        private EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Edit_Event);
            //get controls/assign event handlers
            eventList = FindViewById<ListView>(Resource.Id.eventList);
            eventList.ItemClick += ListViewClick;
            bDeleteEvent = FindViewById<Button>(Resource.Id.bDeleteEvent);
            bDeleteEvent.Click += ButtonClicked;
            bEditID = FindViewById<Button>(Resource.Id.bEditID);
            bEditID.Click += ButtonClicked;
            bRefresh = FindViewById<Button>(Resource.Id.bRefreshEvents);
            bRefresh.Click += ButtonClicked;
            //use an ArrayAdapter to convert the string to ListView format
            var eventAdapter = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, eData.GetEventDisplayList());
            eventList.Adapter = eventAdapter;
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            try
            {
                //decide which button was pressed
                if ((sender as Button) == bDeleteEvent)
                {
                    Popup.Double("Alert", "Are you sure you want to delete the event '" +
                        selectedEvent.eventName + "' AND all associated matches?", "Yes", "CANCEL", this, Delete);
                    //if user presses delete
                    void Delete()
                    {
                        eData.DeleteEvent(selectedEvent.eventID);
                        eData.DeleteMatchDataForEvent(selectedEvent.eventID);
                        Popup.Single("Alert", "Event Deleted", "OK", this);
                        this.Recreate();
                    }
                }
                else if ((sender as Button) == bEditID)
                {
                    //set current event so it can be accessed by the next activity
                    eData.SetCurrentEvent(selectedEvent.eventID);
                    Finish();
                    StartActivity(typeof(EventID));
                }
                else if ((sender as Button) == bRefresh)
                {
                    //refresh
                    Recreate();
                }
            }
            //if no event is selected, selected event will be null and throw an exception
            catch
            {
                Popup.Single("Alert", "Please select an event to edit", "OK", this);
            }
        }

        private void ListViewClick(object sender, ItemClickEventArgs e)
        {
            selectedIndex = e.Position;
            //get a list of current event ids and have them correspond to index of ListView
            selectedEvent = eData.GetEvent(eData.EventIDList()[selectedIndex]);
        }
    }
}