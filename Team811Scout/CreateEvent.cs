using Android.App;
using Android.OS;
using Android.Widget;
using System;

namespace Team811Scout
{

    /*This activity is started when "Create Event" is clicked on the home screen. It gathers the data from user input and creates a new instance
    * of the "Event" class with the properties given*/

    [Activity(Label = "CreateEvent", Theme = "@style/AppTheme", MainLauncher = false)]
    public class CreateEvent: Activity
    {
        //declare objects that will refer to controls in the app
        Button bStart;
        DatePicker startDate;
        DatePicker endDate;
        EditText txtEventName;
        EditText txtEventID;

        string getStartDate = null;
        string getEndDate = null;
        string eventName = null;

        //create a new "Event" instance to be assigned to later
        Event newEvent;

        //create an instance of our database
        EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //get layout for new event input
            SetContentView(Resource.Layout.new_event);

            //get controls from layout and assign neccessary event handlers
            bStart = FindViewById<Button>(Resource.Id.bStart);
            bStart.Click += ButtonClicked;
            startDate = FindViewById<DatePicker>(Resource.Id.startDate);
            endDate = FindViewById<DatePicker>(Resource.Id.endDate);
            txtEventName = FindViewById<EditText>(Resource.Id.txtEventName);
            txtEventID = FindViewById<EditText>(Resource.Id.txtEventID);

        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            //decide which button on the form was clicked
            if ((sender as Button) == bStart)
            {
                //convert DateTime format to a readable string
                getStartDate = startDate.DateTime.ToString("MM/dd/yyyy");
                getEndDate = endDate.DateTime.ToString("MM/dd/yyyy");
                eventName = txtEventName.Text;

                try
                {
                    //make sure user inputted an event name and id
                    if (eventName != null && eventName != ""&&txtEventID.Text!=null&&txtEventID.Text!="")
                    {
                        //create new event and add it to the databse
                        newEvent = new Event(getStartDate, getEndDate, eventName, int.Parse(txtEventID.Text), false);
                        eData.AddEvent(newEvent);

                        //go back to the home screen and finish this activity
                        StartActivity(typeof(MainActivity));
                        Finish();

                    }
                    else
                    {
                        Popup.Single("Alert", "Please Enter Event Details", "OK", this);
                    }
                }

                //if the database has a duplicate event id, it will throw an exception not allowing the new one to be added
                catch
                {
                    Popup.Single("Alert", "Duplicate Event ID Detected.", "OK", this);
                }
            }
        }
    }
}