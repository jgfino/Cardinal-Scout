using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace Team811Scout
{
    [Activity(Label = "ViewData", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ViewData: Activity
    {
        CompiledScoutData currentCompiled;
        EventDatabase eData;
        TextView textRecent;     
        string[] properties;
        Button bDeleteData;
        GridView gridRecent;




        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.view_data);
            eData = new EventDatabase();

            bDeleteData = FindViewById<Button>(Resource.Id.bDeleteData);
            bDeleteData.Click += ButtonClicked;

            gridRecent = FindViewById<GridView>(Resource.Id.gridViewEvent);

            textRecent = FindViewById<TextView>(Resource.Id.textEvent);

            currentCompiled = eData.CurrentCompiled();
            List<List<CompiledScoutData>> compiled = currentCompiled.compileData();

            textRecent.Text += currentCompiled.officialName + " Click on a team to view detailed data";

            properties = new string[]
            {
                "Team",
                "Recommend %",
                "Record",
                "Cargo/Hatch %",
                "Climb?/n(Lvl 3 %/Lvl 2%)",
                "Good Drivers (%)",
               
            };

            List<string> teamNumbers = currentCompiled.getTeamNumbers(compiled);
            List<string> recPerc = currentCompiled.getRecPercentArray(compiled);
            List<string> record = currentCompiled.getWinRecord(compiled);
            List<string> cargoPerc = currentCompiled.getCargoPercent(compiled);
            List<string> hatchPerc = currentCompiled.getHatchPercent(compiled);
            List<string> climbPerc = currentCompiled.getClimbPercent(compiled);
            List<string> driversPerc = currentCompiled.getDriversPercent(compiled);

            List<SpannableString> display = new List<SpannableString>();
            
            for(int i = 0; i<properties.Length;i++)
            {
                display.Add(new FormatString(properties[i]).getBold());
            }
            
            for(int i = 0;i<compiled.Count;i++)
            {
                //change this to 75
                display.Add(new FormatString(teamNumbers[i]).getNormal());
                if(int.Parse(recPerc[i].Substring(0,recPerc[i].IndexOf("%")))>75)
                {
                    display.Add(new FormatString(recPerc[i]).setColorBold(121,234,144));
                }
                else
                {
                    display.Add(new FormatString(recPerc[i]).getNormal());
                }
                
                display.Add(new FormatString(record[i]).getNormal());
                display.Add(new FormatString(cargoPerc[i] + " / " + hatchPerc[i]).getNormal());
                display.Add(new FormatString(climbPerc[i]).getNormal());
                display.Add(new FormatString(driversPerc[i]).getNormal());
            }
           

            ArrayAdapter gridAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridRecent.Adapter = gridAdapt;

        }

        private void ButtonClicked(object sender, EventArgs e)
        {

            if ((sender as Button) == bDeleteData)
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog confirmDelete = dialog.Create();
                confirmDelete.SetTitle("Alert");
                confirmDelete.SetMessage("Are you sure you want to delete the compiled data for '" + currentCompiled.eventName + "'");

                confirmDelete.SetButton("Yes", (c, ev) =>
                {

                    eData.DeleteCompiledScoutData(currentCompiled.cID);
                    Finish();
                    StartActivity(typeof(SelectEventView));


                });
                confirmDelete.SetButton2("CANCEL", (c, ev) => { });
                confirmDelete.Show();
            }

        }

    }





}
