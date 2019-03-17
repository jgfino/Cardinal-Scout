using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    [Activity(Label = "EventID", Theme = "@style/AppTheme", MainLauncher = false)]
    public class EventID: Activity
    {
        Button bConfirm;
        EditText newID;
        TextView title;
        Event currentEvent;
        EventDatabase eData;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.event_id);

            bConfirm = FindViewById<Button>(Resource.Id.bConfirmID);
            bConfirm.Click += ButtonClicked;

            newID = FindViewById<EditText>(Resource.Id.textID);

            title = FindViewById<TextView>(Resource.Id.titleID);

            eData = new EventDatabase();

            currentEvent = eData.CurrentEvent();

            title.Text += "'" + currentEvent.eventName + "'";

        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            if ((sender as Button) == bConfirm)
            {
                try
                {
                    eData.ChangeEventID(currentEvent.eventID, int.Parse(newID.Text));

                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog missingDetails = dialog.Create();
                    missingDetails.SetTitle("Alert");
                    missingDetails.SetMessage("Success");
                    missingDetails.SetButton("OK", (c, ev) =>
                    {
                        StartActivity(typeof(EditEvents));
                        Finish();
                    });
                    missingDetails.Show();                  

                 
                }
                catch
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog missingDetails = dialog.Create();
                    missingDetails.SetTitle("Alert");
                    missingDetails.SetMessage("Please Enter a New ID Not Used by an Existing Event");
                    missingDetails.SetButton("OK", (c, ev) =>
                    {

                    });
                    missingDetails.Show();
                }
            }
           
        }
       
    }
}