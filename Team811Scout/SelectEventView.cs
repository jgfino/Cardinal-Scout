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

    [Activity(Label = "SelectEventView", Theme = "@style/AppTheme", MainLauncher = false)]
    public class SelectEventView: Activity
    {
        //declare objects for controls
        ListView eventList;        
        Button bSelect;        

        //placeholder for selected compiled scout data
        CompiledScoutData selectedCompiled;

        //get database instance
        EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            SetContentView(Resource.Layout.select_event_view);

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
                    //set current compiled scout data to the selected one
                    eData.SetCurrentCompiled(selectedCompiled.cID);
                    Finish();
                    //go to general display page for event
                    StartActivity(typeof(ViewData));
                }
            }
            //if no event is selected, it throws an exception
            catch
            {
                Popup.Single("Alert", "Please select an event to view data for", "OK", this);
            }
        }

        //decide which event was clicked based off of the compiled id list
        int selectedIndex;
        private void ListViewClick(object sender, ItemClickEventArgs e)
        {
            selectedIndex = e.Position;
            selectedCompiled = eData.GetCompiledScoutData(eData.CompiledIDList()[selectedIndex]);
        }

    }
}