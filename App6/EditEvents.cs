using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    [Activity(Label = "editevents", Theme = "@style/AppTheme", MainLauncher = false)]
    public class EditEvents: Activity
    {
        ListView eventList;
        EventDatabase eData;
        List<string> eventNames;
        Button bDeleteEvent;
        Button bEditID;
        int selectedIndex;
        Event selectedEvent;
        int deletedMatches;
        Button bRefresh;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            deletedMatches = 0;

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.edit_events);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            eData = new EventDatabase();
            eventList = FindViewById<ListView>(Resource.Id.eventList);
            bDeleteEvent = FindViewById<Button>(Resource.Id.bDeleteEvent);
            bDeleteEvent.Click += ButtonClicked;

            bEditID = FindViewById<Button>(Resource.Id.bEditID);
            bEditID.Click += ButtonClicked;

            bRefresh = FindViewById<Button>(Resource.Id.bRefreshEvents);
            bRefresh.Click += ButtonClicked;

            eventNames = eData.GetEventDisplayList();           

            var eventAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, eventNames);
            eventList.Adapter = eventAdapter;

            eventList.ItemClick += ListViewClick;

        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            try

            {
                if ((sender as Button) == bDeleteEvent)
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog confirmDelete = dialog.Create();
                    confirmDelete.SetTitle("Alert");
                    confirmDelete.SetMessage("Are you sure you want to delete the event '" + selectedEvent.eventName + "' AND all associated matches?");

                    confirmDelete.SetButton("Yes", (c, ev) =>
                    {

                        eData.DeleteEvent(selectedEvent.eventID);
                        eData.DeleteDataForEvent(selectedEvent.eventID);

                        this.Recreate();


                    });
                    confirmDelete.SetButton2("CANCEL", (c, ev) => { });
                    confirmDelete.Show();
                }

                else if ((sender as Button) == bEditID)
                {

                    eData.SetCurrentEvent(selectedEvent.eventID);
                    Finish();
                    StartActivity(typeof(EventID));

                }
                else if ((sender as Button) == bRefresh)
                {
                    this.Recreate();
                }

            }
            catch
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog missingDetails = dialog.Create();
                missingDetails.SetTitle("Alert");
                missingDetails.SetMessage("Please select an event to edit");
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