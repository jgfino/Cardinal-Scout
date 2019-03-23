using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;
using static Android.Widget.AdapterView;

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
        GridView gridTeams;
        List<List<CompiledScoutData>> compiled;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.view_data);
            eData = new EventDatabase();

            bDeleteData = FindViewById<Button>(Resource.Id.bDeleteData);
            bDeleteData.Click += ButtonClicked;

            gridTeams = FindViewById<GridView>(Resource.Id.gridViewTeam);
            gridTeams.ItemClick += gridClicked;

            textRecent = FindViewById<TextView>(Resource.Id.textEvent);

            currentCompiled = eData.GetCurrentCompiled();
            compiled = currentCompiled.compileData();

           //eData.DeleteCompiledScoutData(currentCompiled.cID);

            SpannableString[] textDisp = new SpannableString[]
            {
                new FormatString("Viewing Data for Event: \n").getNormal(),
                new FormatString("'"+currentCompiled.officialName+"'").getBold(),
                new FormatString(" as of ").getNormal(),
                new FormatString(currentCompiled.dateMod).getBold(),
                new FormatString(" at ").getNormal(),
                new FormatString(currentCompiled.timeMod).getBold()

            };

            textRecent.TextFormatted = TextUtils.ConcatFormatted(textDisp);

            properties = new string[]
            {
                "Team",
                "Recommend %",
                "Record",
                "Cargo/Hatch %",
                "        Climb?\n(Lvl 2%/Lvl 3%)",
                "Good Drivers (%)",
                "'Table' %"

            };

           
            List<int> teamNumbers = currentCompiled.getTeamNumbersArray(compiled);
            List<int> recPerc = currentCompiled.getRecPercentArray(compiled);
            List<string> record = currentCompiled.getWinRecordArray(compiled);
            List<int> cargoPerc = currentCompiled.getCargoPercentArray(compiled);
            List<int> hatchPerc = currentCompiled.getHatchPercentArray(compiled);
            List<int> climbPerc2 = currentCompiled.getClimb2PercentArray(compiled);
            List<int> climbPerc3 = currentCompiled.getClimb3PercentArray(compiled);
            List<int> driversPerc = currentCompiled.getDriversPercentArray(compiled);
            List<int> tablePerc = currentCompiled.getTablePercentArray(compiled);

            List<int> winPerc = currentCompiled.getWinPercentArray(compiled);

            List<SpannableString> display = new List<SpannableString>();

            for (int i = 0; i < properties.Length; i++)
            {
                display.Add(new FormatString(properties[i]).getBold());
            }

            for (int i = 0; i < compiled.Count; i++)
            {

                display.Add(new FormatString(teamNumbers[i].ToString()).getBold());

                if (recPerc[i] > Constants.recommendThresh)
                {
                    display.Add(new FormatString(recPerc[i].ToString() + "%").setColorBold(0, 137, 9));
                }
                else
                {
                    display.Add(new FormatString(recPerc[i].ToString() + "%").setColorBold(255, 0, 0));
                }

                if (winPerc[i] > 74)
                {
                    display.Add(new FormatString(record[i]).setColorBold(0, 137, 9));
                }
                else
                {
                    display.Add(new FormatString(record[i]).getNormal());
                }

                display.Add(new FormatString(cargoPerc[i].ToString() + "% / " + hatchPerc[i].ToString() + "%").getNormal());

                if (climbPerc3[i] > 49)
                {
                    SpannableString[] disp = new SpannableString[]
                    {
                        new FormatString(climbPerc2[i].ToString() + "% / ").getNormal(),
                        new FormatString(climbPerc3[i].ToString() + "%").setColorBold(0,137,9)
                    };

                    display.Add(new SpannableString(TextUtils.ConcatFormatted(disp)));
                }
                else
                {
                    display.Add(new FormatString(climbPerc2[i].ToString() + "% / " + climbPerc3[i].ToString() + "%").getNormal());
                }

                if (driversPerc[i] < 34)
                {
                    display.Add(new FormatString(driversPerc[i].ToString() + "%").setColorBold(255,0,0));
                }
                else if (driversPerc[i] > 79)
                {
                    display.Add(new FormatString(driversPerc[i].ToString() + "%").setColorBold(0,137,9));
                }
                else
                {
                    display.Add(new FormatString(driversPerc[i].ToString() + "%").getNormal());
                }

                if (tablePerc[i] > 32)
                {
                    display.Add(new FormatString(tablePerc[i].ToString() + "%").setColorBold(255, 0, 0));
                }
                else
                {
                    display.Add(new FormatString(tablePerc[i].ToString() + "%").setColorBold(0, 137, 9));
                }

            }


            ArrayAdapter gridAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridTeams.Adapter = gridAdapt;

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

        private void gridClicked(object sender, ItemClickEventArgs e)
        {
            
            if(e.Position%7==0&&e.Position!=0)
            {
                CompiledTeamIndex currentIndex = new CompiledTeamIndex((e.Position / 7) - 1);

                eData.setTeamIndex(currentIndex);

                string name = compiled[eData.getTeamIndex().ID][0].teamNumber.ToString();

                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog missingDetails = dialog.Create();
                missingDetails.SetTitle("Alert");
                missingDetails.SetMessage(name);
                ;
                missingDetails.SetButton("OK", (c, ev) =>
                {

                });
                missingDetails.Show();

                StartActivity(typeof(TeamDetails));
            }
            else
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog missingDetails = dialog.Create();
                missingDetails.SetTitle("Alert");
                missingDetails.SetMessage("Please click a team number to view detailed data");
                ;
                missingDetails.SetButton("OK", (c, ev) =>
                {

                });
                missingDetails.Show();
                this.Recreate();
            }
        }

    }





}
