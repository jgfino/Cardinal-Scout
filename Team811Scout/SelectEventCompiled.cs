using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    /*This activity is for selecting which event to view compiled data for after clicking
     * "View Data" on the home page*/

    [Activity(Label = "SelectEventCompiled", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SelectEventCompiled: Activity
    {
        //declare objects for controls
        private ListView eventList;

        private Button bSelect;

        //placeholder for selected compiled event data
        private CompiledEventData selectedCompiled;

        //get database instance
        private EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Select_Compiled);
            //get controls from layout and assign event handlers
            eventList = FindViewById<ListView>(Resource.Id.choosetoView);
            eventList.ItemClick += ListViewClick;
            bSelect = FindViewById<Button>(Resource.Id.bSelectView);
            bSelect.Click += ButtonClicked;
            //display available events with compiled data
            var eventAdapter = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, eData.GetCompiledDisplayList());
            eventList.Adapter = eventAdapter;
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if ((sender as Button) == bSelect)
                {
                    //set current compiled event data to the selected one
                    eData.SetCurrentCompiled(selectedCompiled.cID);
                    Finish();
                    //go to general display page for event
                    StartActivity(typeof(GeneralCompiledData));
                }
            }
            //if no event is selected, it throws an exception
            catch
            {
                Popup.Single("Alert", "Please select an event to view data for", "OK", this);
            }
        }

        //decide which event was clicked based off of the compiled id list
        private int selectedIndex;

        private void ListViewClick(object sender, ItemClickEventArgs e)
        {
            selectedIndex = e.Position;
            selectedCompiled = eData.GetCompiledEventData(eData.CompiledIDList()[selectedIndex]);
        }
    }
}