using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    /*This activity displays general compiled data for an event. It displays the teams involved in the event
     * and their overall percentages based off of performance in matches*/

    [Activity(Label = "ViewData", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ViewData: Activity
    {
        //declare objects for controls
        TextView textRecent;
        string[] properties;
        Button bDeleteData;
        GridView gridTeams;

        //placeholder for current compiled scout data to view
        CompiledScoutData currentCompiled;

        //get database instance
        EventDatabase eData = new EventDatabase();

        List<List<CompiledScoutData>> compiled;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.view_data);

            //get controls from layout
            bDeleteData = FindViewById<Button>(Resource.Id.bDeleteData);
            bDeleteData.Click += ButtonClicked;
            gridTeams = FindViewById<GridView>(Resource.Id.gridViewTeam);
            gridTeams.ItemClick += gridClicked;
            textRecent = FindViewById<TextView>(Resource.Id.textEvent);

            //get current compiled data
            currentCompiled = eData.GetCurrentCompiled();            

            SpannableString[] textDisp = new SpannableString[]
            {
                FormatString.setNormal("Viewing Data for Event: \n"),
                FormatString.setBold("'"+currentCompiled.officialName+"'"),
                FormatString.setNormal(" as of "),
                FormatString.setBold(currentCompiled.dateMod),
                FormatString.setNormal(" at "),
                FormatString.setBold(currentCompiled.timeMod)
            };

            //set title display text
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

            //get general data for all teams
            List<int> teamNumbers = currentCompiled.getTeamNumbersArray();
            List<int> recPerc = currentCompiled.getRecPercentArray();
            List<string> record = currentCompiled.getWinRecordArray();
            List<int> cargoPerc = currentCompiled.getCargoPercentArray();
            List<int> hatchPerc = currentCompiled.getHatchPercentArray();
            List<int> climbPerc2 = currentCompiled.getClimb2PercentArray();
            List<int> climbPerc3 = currentCompiled.getClimb3PercentArray();
            List<int> driversPerc = currentCompiled.getDriversPercentArray();
            List<int> tablePerc = currentCompiled.getTablePercentArray();
            List<int> winPerc = currentCompiled.getWinPercentArray();

            List<SpannableString> display = new List<SpannableString>();


            //format data in the right order for the grid
            //make this nicer
            for (int i = 0; i < properties.Length; i++)
            {
                display.Add(FormatString.setBold(properties[i]));
            }

            for (int i = 0; i < currentCompiled.compileData().Count; i++)
            {

                display.Add(FormatString.setBold(teamNumbers[i].ToString()));

                if (recPerc[i] > Constants.recommendThresh)
                {
                    display.Add(FormatString.setColorBold(recPerc[i].ToString() + "%", Constants.appGreen));
                }
                else
                {
                    display.Add(FormatString.setColorBold(recPerc[i].ToString() + "%", Constants.appRed));
                }

                if (winPerc[i] > 74)
                {
                    display.Add(FormatString.setColorBold(record[i], Constants.appGreen));
                }
                else
                {
                    display.Add(FormatString.setNormal(record[i]));
                }

                display.Add(FormatString.setNormal(cargoPerc[i].ToString() + "% / " + hatchPerc[i].ToString() + "%"));

                if (climbPerc3[i] > 49)
                {
                    SpannableString[] disp = new SpannableString[]
                    {
                        FormatString.setNormal(climbPerc2[i].ToString() + "% / "),
                        FormatString.setColorBold(climbPerc3[i].ToString() + "%",Constants.appGreen)
                    };

                    display.Add(new SpannableString(TextUtils.ConcatFormatted(disp)));
                }
                else
                {
                    display.Add(FormatString.setNormal(climbPerc2[i].ToString() + "% / " + climbPerc3[i].ToString() + "%"));
                }

                if (driversPerc[i] < 34)
                {
                    display.Add(FormatString.setColorBold(driversPerc[i].ToString() + "%",Constants.appRed));
                }
                else if (driversPerc[i] > 79)
                {
                    display.Add(FormatString.setColorBold(driversPerc[i].ToString() + "%",Constants.appRed));
                }
                else
                {
                    display.Add(FormatString.setNormal(driversPerc[i].ToString() + "%"));
                }

                if (tablePerc[i] > 32)
                {
                    display.Add(FormatString.setColorBold(tablePerc[i].ToString() + "%",Constants.appRed));
                }
                else
                {
                    display.Add(FormatString.setColorBold(tablePerc[i].ToString() + "%",Constants.appRed));
                }

            }

            //display data in the grid
            ArrayAdapter gridAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridTeams.Adapter = gridAdapt;

        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            
            //decide which button was clicked
            if ((sender as Button) == bDeleteData)
            {

                Popup.Double("Alert", "Are you sure you want to delete the compiled data for '" + currentCompiled.eventName + "'", "Yes", "CANCEL", this, ifYes);

                void ifYes()
                {
                    eData.DeleteCompiledScoutData(currentCompiled.cID);
                    Finish();
                    StartActivity(typeof(SelectEventView));
                }
               
            }

        }

        private void gridClicked(object sender, ItemClickEventArgs e)
        {
            //make sure a team number on the left was selected
            if (e.Position % 7 == 0 && e.Position != 0)
            {
                CompiledTeamIndex currentIndex = new CompiledTeamIndex((e.Position / 7) - 1);

                //set where the team is in the compiled scout data multidimensional lsit
                eData.setTeamIndex(currentIndex);                

                StartActivity(typeof(TeamDetails));
            }
            else
            {
                Popup.Single("Alert", "Please click a team number to view detailed data", "OK", this);               
            }
        }

    }





}
