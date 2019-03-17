using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;

using System.IO;
using System.Collections.Generic;
using System.Linq;

//using Xamarin.Forms;


namespace Team811Scout
{

    [Activity(Label = "Team 811 Scouting", Theme = "@style/AppTheme", MainLauncher = true)]

    public class MainActivity : AppCompatActivity
    {
        //declare buttons globally
        Android.Widget.Button bViewPrev;
        Android.Widget.Button bSync;
        Android.Widget.Button bNewEvent;
        Android.Widget.Button bContinue;
        Button bInstruct;
        ImageView teamPhoto;

        EventDatabase eData;
        
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //uncomment this to reset database on launch
            //var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "scoutdb.db3");
            //this.DeleteDatabase(path);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //get buttons from XAML file
            bViewPrev = FindViewById<Android.Widget.Button>(Resource.Id.bViewPrev);
            bViewPrev.Click += ButtonClicked;
            bSync = FindViewById<Android.Widget.Button>(Resource.Id.bSync);
            bSync.Click += ButtonClicked;
            bNewEvent = FindViewById<Android.Widget.Button>(Resource.Id.bNewEvent);
            bNewEvent.Click += ButtonClicked;
            bContinue = FindViewById<Android.Widget.Button>(Resource.Id.bContinue);
            bContinue.Click += ButtonClicked;
            bInstruct = FindViewById<Button>(Resource.Id.bInstruct);
            bInstruct.Click += ButtonClicked;
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            teamPhoto = FindViewById<ImageView>(Resource.Id.teamPhoto);
            teamPhoto.SetImageResource(Resource.Drawable.DSC_0947);
            eData = new EventDatabase();

        }


        private void ButtonClicked(object sender, EventArgs e)
        {          

            if ((sender as Android.Widget.Button) == bViewPrev)
            {
                StartActivity(typeof(SelectEventView));
                
            }
            else if ((sender as Android.Widget.Button) == bSync)
            {
                StartActivity(typeof(MasterSlaveSelect));
            }
            else if ((sender as Android.Widget.Button) == bNewEvent)
            {
                StartActivity(typeof(CreateEvent));
            }
            else if ((sender as Button)==bInstruct)
            {
                StartActivity(typeof(Settings));

            }
            else if ((sender as Android.Widget.Button) == bContinue)
            {
                StartActivity(typeof(SelectEvent));               
            }          
                
        }
        

        

      
       
    }
}