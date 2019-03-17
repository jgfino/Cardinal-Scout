using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    [Activity(Label = "SelectEvent", Theme = "@style/AppTheme", MainLauncher = false)]
    public class SelectEvent: Activity
    {
        ListView eventList;
        EventDatabase eData;
        List<string> eventNames;
        Button bSelect;
        int selectedIndex;
        Event selectedEvent;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            eData = new EventDatabase();
            SetContentView(Resource.Layout.select_event);
            eventList = FindViewById<ListView>(Resource.Id.chooseList);
            bSelect = FindViewById<Button>(Resource.Id.bSelect);
            bSelect.Click += ButtonClicked;

            eventNames = eData.GetEventDisplayList();

            var eventAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, eventNames);
            eventList.Adapter = eventAdapter;

            eventList.ItemClick += ListViewClick;

        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if ((sender as Button) == bSelect)
                {

                    eData.SetCurrentEvent(selectedEvent.eventID);

                    StartActivity(typeof(ScoutLandingPage));
                }
            }
            catch
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog missingDetails = dialog.Create();
                missingDetails.SetTitle("Alert");
                missingDetails.SetMessage("Please select an event to scout");
                missingDetails.SetButton("OK", (c, ev) =>
                {

                });
                missingDetails.Show();
            }


        }

        private void ListViewClick(object sender, ItemClickEventArgs e)
        {
            selectedIndex = e.Position;
            selectedEvent = eData.GetEvent(eData.EventIDList()[selectedIndex]);
        }


    }
}