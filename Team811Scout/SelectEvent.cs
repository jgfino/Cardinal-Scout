using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    /*This class is for selecting which event to Scout for after clicking "Scout"*/

    [Activity(Label = "SelectEvent", Theme = "@style/AppTheme", MainLauncher = false)]
    public class SelectEvent: Activity
    {
        //declare objects for controls
        ListView eventList;        
        Button bSelect;

        //get database instance        
        EventDatabase eData = new EventDatabase();        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);           
            SetContentView(Resource.Layout.select_event);

            //get controls from layout and assign event handlers
            eventList = FindViewById<ListView>(Resource.Id.chooseList);
            eventList.ItemClick += ListViewClick;
            bSelect = FindViewById<Button>(Resource.Id.bSelect);
            bSelect.Click += ButtonClicked;

            //get a display list of events and adapt them to a ListView            
            var eventAdapter = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, eData.GetEventDisplayList());
            eventList.Adapter = eventAdapter;          

        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            try
            {
                //decide which button was pressed
                if ((sender as Button) == bSelect)
                {
                    //set the current event and go to the scouting page
                    eData.SetCurrentEvent(selectedEvent.eventID);
                    StartActivity(typeof(ScoutLandingPage));
                }
            }
            //if no event is selected, it will throw an exception
            catch
            {
                Popup.Single("Alert", "Please select an event to scout", "OK", this);
            }

        }

        Event selectedEvent;
        int selectedIndex;
        private void ListViewClick(object sender, ItemClickEventArgs e)
        {
            selectedIndex = e.Position;
            //get the selected event based on the position in the event id list
            selectedEvent = eData.GetEvent(eData.EventIDList()[selectedIndex]);
        }


    }
}