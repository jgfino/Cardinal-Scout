using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

//using Xamarin.Forms;
using Xamarin.Android;


namespace Team811Scout
{
    [Activity(Label = "CreateEvent", Theme = "@style/AppTheme", MainLauncher = false)]
    public class CreateEvent : Activity
    {
        Button bStart;
        DatePicker startDate;
        string getStartDate = null;
        DatePicker endDate;
        string getEndDate = null;
        EditText txtEventName;
        EditText txtEventID;
        string eventName = null;
        Event newEvent;

        EventDatabase eData;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.new_event);
            bStart = FindViewById<Button>(Resource.Id.bStart);
            bStart.Click += ButtonClicked;

            startDate = FindViewById<DatePicker>(Resource.Id.startDate);
            endDate = FindViewById<DatePicker>(Resource.Id.endDate);
            txtEventName = FindViewById<EditText>(Resource.Id.txtEventName);
            txtEventID = FindViewById<EditText>(Resource.Id.txtEventID);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            eData = new EventDatabase();

        }

        private void ButtonClicked(object sender, EventArgs e)
        {
       
            if ((sender as Button) == bStart)
            {
                getStartDate = startDate.DateTime.ToString("MM/dd/yyyy");
                getEndDate = endDate.DateTime.ToString("MM/dd/yyyy");
                eventName = txtEventName.Text;

                try
                {
                    if (eventName != null && eventName != "")
                    {
                        newEvent = new Event(getStartDate, getEndDate, eventName, int.Parse(txtEventID.Text), false);

                        eData.AddEvent(newEvent);
                        StartActivity(typeof(MainActivity));
                        Finish();

                    }
                    else
                    {
                        AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                        AlertDialog missingDetails = dialog.Create();
                        missingDetails.SetTitle("Alert");
                        missingDetails.SetMessage("Please Enter Event Details");
                        missingDetails.SetButton("OK", (c, ev) =>
                        {

                        });
                        missingDetails.Show();

                    }
                }
                catch
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog missingDetails = dialog.Create();
                    missingDetails.SetTitle("Alert");
                    missingDetails.SetMessage("Duplicate Event ID Detected");
                    missingDetails.SetButton("OK", (c, ev) =>
                    {

                    });
                    missingDetails.Show();
                }

                            

            }
        }

      
        
     
    }
}