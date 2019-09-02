using Android.App;
using Android.OS;
using Android.Widget;
using System;
using Team811Scout.Data;

namespace Team811Scout
{
    /*this activity contains instructions for using the app and an option to factory reset it,
     * which deletes the entire database (everything). It also has a button to edit existing events
     * to delete individual events or change event ids*/

    [Activity(Label = "InstructSettings", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class Settings: Activity
    {
        //declare objects for controls
        private Button bDelete;

        private Button bEditEvent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Instructions_Settings);
            //get controls from layout and assign event handlers
            bDelete = FindViewById<Button>(Resource.Id.bDelete);
            bDelete.Click += ButtonClicked;
            bEditEvent = FindViewById<Button>(Resource.Id.bEditEvent);
            bEditEvent.Click += ButtonClicked;
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            //decide which button was pressed
            if ((sender as Button) == bDelete)
            {
                //double check to make sure the user wants to delete
                Popup.Double("WARNING", "Are you SURE you want to delete the ENTIRE database (all events, matches, files, etc?", "Yes", "CANCEL", this, yes1);
                void yes1()
                {
                    Popup.Double("WARNING", "Are you ABSOLUTELY sure?", "Yes, I'm sure", "CANCEL", this, yes2);
                    void yes2()
                    {
                        //delete database and go back to the main page
                        DeleteDatabase(SQLite_android.getDatabasePath());
                        StartActivity(typeof(MainActivity));
                        Finish();
                    }
                }
            }
            //go to editevents page
            else if ((sender as Button) == bEditEvent)
            {
                StartActivity(typeof(EditEvents));
            }
        }
    }
}