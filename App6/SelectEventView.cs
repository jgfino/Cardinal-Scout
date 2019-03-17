using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    [Activity(Label = "SelectEventView", Theme = "@style/AppTheme", MainLauncher = false)]
    public class SelectEventView: Activity
    {
        ListView eventList;
        EventDatabase eData;
        List<string> eventNames;
        Button bSelect;
        int selectedIndex;
        CompiledScoutData selectedCompiled;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            eData = new EventDatabase();
            SetContentView(Resource.Layout.select_event_view);
            eventList = FindViewById<ListView>(Resource.Id.choosetoView);
            bSelect = FindViewById<Button>(Resource.Id.bSelectView);
            bSelect.Click += ButtonClicked;

            eventNames = eData.GetCompiledList();

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
                    eData.SetCurrentCompiled(selectedCompiled.cID);
                    Finish();
                    StartActivity(typeof(ViewData));
                }
            }
            catch
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog missingDetails = dialog.Create();
                missingDetails.SetTitle("Alert");
                missingDetails.SetMessage("Please select an event to view data for");
                missingDetails.SetButton("OK", (c, ev) =>
                {

                });
                missingDetails.Show();
            }


        }

        private void ListViewClick(object sender, ItemClickEventArgs e)
        {
            selectedIndex = e.Position;
            selectedCompiled = eData.GetCompiledScoutData(eData.CompiledIDList()[selectedIndex]);
        }


    }
}