using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System;
using Team811Scout.Data;

namespace Team811Scout
{
    /*This is the home page for the app. It shows up first when the app is launched and contains buttons
     * to go to other areas in the app*/

    [Activity(Label = "Team 811 Scouting", Theme = "@style/AppTheme", MainLauncher = true)]

    public class MainActivity : AppCompatActivity
    {
        //declare objects to refer to controls
        Button bViewPrev;
        Button bSync;
        Button bNewEvent;
        Button bContinue;
        Button bInstruct;
        ImageView teamPhoto;        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //uncomment this to reset database on launch
            //DeleteDatabase(SQLite_android.getDatabasePath());

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //get controls and assign event handlers
            bViewPrev = FindViewById<Button>(Resource.Id.bViewPrev);
            bViewPrev.Click += ButtonClicked;
            bSync = FindViewById<Button>(Resource.Id.bSync);
            bSync.Click += ButtonClicked;
            bNewEvent = FindViewById<Button>(Resource.Id.bNewEvent);
            bNewEvent.Click += ButtonClicked;
            bContinue = FindViewById<Button>(Resource.Id.bContinue);
            bContinue.Click += ButtonClicked;
            bInstruct = FindViewById<Button>(Resource.Id.bInstruct);
            bInstruct.Click += ButtonClicked;
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            teamPhoto = FindViewById<ImageView>(Resource.Id.teamPhoto);
            teamPhoto.SetImageResource(Resource.Drawable.DSC_0947);

        }

        //handle button clicks
        private void ButtonClicked(object sender, EventArgs e)
        {          
            //decide which button was pressed and start appropriate activity
            if ((sender as Button) == bViewPrev)
            {
                StartActivity(typeof(SelectEventView));
                
            }
            else if ((sender as Button) == bSync)
            {
                StartActivity(typeof(MasterSlaveSelect));
            }
            else if ((sender as Button) == bNewEvent)
            {
                StartActivity(typeof(CreateEvent));
            }
            else if ((sender as Button)==bInstruct)
            {
                StartActivity(typeof(Settings));

            }
            else if ((sender as Button) == bContinue)
            {
                StartActivity(typeof(SelectEvent));               
            }          
                
        }
        

        

      
       
    }
}