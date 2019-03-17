using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;

namespace Team811Scout
{
    [Activity(Label = "RecentData", Theme = "@style/AppTheme", MainLauncher = false)]
    public class RecentData: Activity
    {


        ScoutData currentMatch;
        EventDatabase eData;
        TextView textRecent;
        string[] data;
        string[] properties;
        Button bDeleteMatch;

        FormatString[] propertiesFormat;
        SpannableString[] display;

        GridView gridRecent;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.recent_data);
            eData = new EventDatabase();

            bDeleteMatch = FindViewById<Button>(Resource.Id.bDeleteMatch);
            bDeleteMatch.Click += ButtonClicked;

            gridRecent = FindViewById<GridView>(Resource.Id.gridRecent);

            textRecent = FindViewById<TextView>(Resource.Id.textRecent);

            currentMatch = eData.CurrentMatch();

            textRecent.Text += currentMatch.matchNumber.ToString() + " Team: " + currentMatch.teamNumber.ToString();

            properties = new string[]
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

            data = new string[]
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

            propertiesFormat = new FormatString[properties.Length];


            for (int i = 0; i < propertiesFormat.Length; i++)
            {
                propertiesFormat[i] = new FormatString(properties[i]);

            }

            display = new SpannableString[propertiesFormat.Length + data.Length];

            int j = 0;
            for (int i = 0; i < display.Length; i += 2)
            {
                display[i] = propertiesFormat[j].getBold();
                display[i + 1] = new SpannableString(data[j]);
                j++;
            }

            ArrayAdapter gridAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridRecent.Adapter = gridAdapt;

        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            if ((sender as Button) == bDeleteMatch)
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog confirmDelete = dialog.Create();
                confirmDelete.SetTitle("Alert");
                confirmDelete.SetMessage("Are you sure you want to delete match " + currentMatch.matchNumber);

                confirmDelete.SetButton("Yes", (c, ev) =>
                {

                    eData.DeleteScoutData(currentMatch.ID);
                    StartActivity(typeof(ScoutLandingPage));
                    Finish();


                });
                confirmDelete.SetButton2("CANCEL", (c, ev) => { });
                confirmDelete.Show();
            }


        }





    }
}