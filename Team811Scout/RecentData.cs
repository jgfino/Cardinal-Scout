using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace Team811Scout
{
    /*This class displays the data for a scouted match in a list*/

    [Activity(Label = "RecentData", Theme = "@style/AppTheme", MainLauncher = false)]
    public class RecentData: Activity
    {
        //get database instance
        EventDatabase eData = new EventDatabase();
        ScoutData currentMatch;
        
        //declare objects for controls
        TextView textRecent;        
        Button bDeleteMatch;
        GridView gridRecent;        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.recent_data);
            
            //get controls from layout and assign event handlers
            bDeleteMatch = FindViewById<Button>(Resource.Id.bDeleteMatch);
            bDeleteMatch.Click += ButtonClicked;
            gridRecent = FindViewById<GridView>(Resource.Id.gridRecent);
            textRecent = FindViewById<TextView>(Resource.Id.textRecent);

            //get the current match to display data for
            currentMatch = eData.GetCurrentMatch();

            SpannableString[] textDisp = new SpannableString[]
            {
                FormatString.setNormal("Viewing Data For - Match: "),
                FormatString.setBold(currentMatch.matchNumber.ToString()),
                FormatString.setNormal(" /// Team: "),
                FormatString.setBold(currentMatch.teamNumber.ToString())
            };

            //set title text
            textRecent.TextFormatted = new SpannableString(TextUtils.ConcatFormatted(textDisp));

            //make display lists
            List<SpannableString> data = new List<SpannableString>();
            List<SpannableString> properties = new List<SpannableString>();
            List<SpannableString> display = new List<SpannableString>();

            string[]propertiesPre = new string[]
            {
                "Match Number",
               "Team Number",
                "Result of Team's Alliance",
                "Position",
               "Table",
                "Started Off Level",
                "Sandstorm Mode",
                "Hatch in Sandstorm",
                "Cargo in Sandstorm",
                "Crossed Hab Line",
                "Can do Cargo",
                "Does Cargo Well",
                "Barely Does Cargo",
                "Can do Hatches",
                "Does Hatches Well",
                "Barely Does Hatches",
                "Climbing Level",
                "Good Driveteam",
                "Recommended",
                "Additional Comments",
            };

            //format the properties
            for(int i = 0; i<propertiesPre.Length;i++)
            {
                properties.Add(FormatString.setBold(propertiesPre[i]));
            }

            string[] dataPre = new string[]
            {
               currentMatch.matchNumber.ToString(),
               currentMatch.teamNumber.ToString(),
               currentMatch.getResult(),
               currentMatch.getPosition(),
               currentMatch.isTable.ToString().ToUpper(),
               "Level " + currentMatch.sandstormStartLevel.ToString(),
               currentMatch.getSandstormMode(),
               currentMatch.sandstormHatch.ToString(),
               currentMatch.sandstormCargo.ToString(),
               currentMatch.sandstormLine.ToString(),
               currentMatch.cargo.ToString(),
               currentMatch.cargoWell.ToString(),
               currentMatch.cargoBarely.ToString(),
               currentMatch.hatch.ToString(),
               currentMatch.hatchWell.ToString(),
               currentMatch.hatchBarely.ToString(),
               currentMatch.getClimb(),
               currentMatch.goodDrivers.ToString(),
               currentMatch.getRecommendation(),
               currentMatch.additionalComments,
            };

            //format the data
            for (int i = 0; i<dataPre.Length;i++)
            {
                data.Add(FormatString.setNormal(dataPre[i]));
            }
            
            //combine properties and data
            for (int i = 0; i < properties.Count; i += 2)
            {
                display.Add(properties[i]);
                display.Add(data[i]);
            }

            //adapt the lists to be displayed in the grid
            ArrayAdapter gridAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridRecent.Adapter = gridAdapt;

        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            //decide which button was pressed
            if ((sender as Button) == bDeleteMatch)
            {
                Popup.Double("Alert", "Are you sure you want to delete match " + currentMatch.matchNumber + "?", "Yes", "CANCEL", this, Delete);
                
                void Delete()
                {
                    eData.DeleteScoutData(currentMatch.ID);
                    StartActivity(typeof(ScoutLandingPage));
                    Finish();
                }
            }


        }





    }
}