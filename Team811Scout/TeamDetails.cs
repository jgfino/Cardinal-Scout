using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace Team811Scout
{
    [Activity(Label = "TeamDetails")]
    public class TeamDetails: Activity
    {

        //declare objects for controls
        GridView gridStats;
        GridView gridSandstorm;
        GridView gridMatches;
        TextView textTitle;
       
        //placeholder for the current compiled data
        CompiledScoutData currentCompiled;
        EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.team_details);

            //get controls from layout
            gridStats = FindViewById<GridView>(Resource.Id.gridViewStats);
            gridSandstorm = FindViewById<GridView>(Resource.Id.gridViewSandstorm);
            gridMatches = FindViewById<GridView>(Resource.Id.gridViewMatches);
            textTitle = FindViewById<TextView>(Resource.Id.textTeam);

            //get current compiled data
            currentCompiled = eData.GetCurrentCompiled();            

            List<CompiledScoutData> compiled = eData.GetCompiledScoutDataForIndex(eData.getTeamIndex().ID);
            int currentTeam = compiled[0].teamNumber;

            //display current team in the title
            textTitle.TextFormatted = TextUtils.ConcatFormatted(FormatString.setNormal("Viewing Stats for Team: "), FormatString.setBold(currentTeam.ToString()));

            int recPerc = currentCompiled.getRecPercentForTeam(currentTeam);
            string record = currentCompiled.getWinRecordForTeam(currentTeam);
            int cargoPerc = currentCompiled.getCargoPercentForTeam(currentTeam);
            int hatchPerc = currentCompiled.getHatchPercentForTeam(currentTeam);
            int climbPerc2 = currentCompiled.getClimb2PercentForTeam(currentTeam);
            int climbPerc3 = currentCompiled.getClimb3PercentForTeam(currentTeam);
            int driversPerc = currentCompiled.getDriversPercentForTeam(currentTeam);
            int tablePerc = currentCompiled.getTablePercentForTeam(currentTeam);
            int winPerc = currentCompiled.getWinPercentForTeam(currentTeam);
            int cargoSandstormPerc = currentCompiled.getCargoSandstormPercentForTeam(currentTeam);
            int hatchSandstormPerc = currentCompiled.getHabSandstormPercentForTeam(currentTeam);
            int habPerc = currentCompiled.getHabSandstormPercentForTeam(currentTeam);
            int autoPerc = currentCompiled.getAutoPercentForTeam(currentTeam);
            int teleopPerc = currentCompiled.getTeleopPercentForTeam(currentTeam);
            int nothingPerc = currentCompiled.getNothingPercentForTeam(currentTeam);

            List<SpannableString> statsDisp = new List<SpannableString>()
            {
                FormatString.setNormal("Cargo/Hatch%"),
                FormatString.setNormal("Climb Level 2% / Level 3%"),
                FormatString.setNormal("Reccomendation %"),
                FormatString.setBold(cargoPerc.ToString()+"% / "+hatchPerc.ToString()+"%"),
                FormatString.setBold(climbPerc2.ToString()+"% / "+climbPerc3.ToString()+"%"),
                FormatString.setBold(recPerc.ToString()+"%"),

            };

            if (cargoPerc > 49 && hatchPerc > 49)
            {
                if (Math.Abs(cargoPerc - hatchPerc) < 16)
                {
                    statsDisp.Add(FormatString.setColorBold("Both",Constants.appGreen));
                }
                else if (cargoPerc > hatchPerc)
                {
                    statsDisp.Add(FormatString.setColorBold("Cargo",Constants.appRed));
                }
                else
                {
                    statsDisp.Add(FormatString.setColorBold("Hatch",Constants.appGreen));
                }
            }
            else
            {
                statsDisp.Add(FormatString.setColorBold("Neither",Constants.appRed));
            }

            //display general stats in first grid box
            ArrayAdapter gridStatsAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, statsDisp);
            gridStats.Adapter = gridStatsAdapt;

            //figure out which sandstorm mode they use the most often
            string primaryMode;
            if(autoPerc>teleopPerc&&autoPerc>nothingPerc)
            {
                primaryMode = "Auto";
            }
            else if(teleopPerc>autoPerc&&teleopPerc>nothingPerc)
            {
                primaryMode = "Teleop w/Camera";
            }
            else
            {
                primaryMode = "Nothing";
            }

            SpannableString cargo = FormatString.setColorBold("NO (" + cargoSandstormPerc.ToString() + ") %",Constants.appRed);
            SpannableString hatch = FormatString.setColorBold("NO (" + hatchSandstormPerc.ToString() + ") %",Constants.appRed);
            SpannableString hab = FormatString.setColorBold("NO (" + habPerc.ToString() + ") %",Constants.appRed);

            if (cargoSandstormPerc>49)
            {
                cargo = FormatString.setColorBold("YES (" + cargoSandstormPerc.ToString() + ") %",Constants.appGreen);
            }
            

            if(hatchSandstormPerc>49)
            {
                hatch = FormatString.setColorBold("YES (" + hatchSandstormPerc.ToString() + ") %",Constants.appGreen);
            }
            
            if(habPerc>49)
            {
                hab = FormatString.setColorBold("YES (" + habPerc.ToString() + ") %",Constants.appGreen);
            }


            List<SpannableString> sandstormDisp = new List<SpannableString>()
            {
                FormatString.setNormal("Primary Sandstorm Mode: "),
                FormatString.setBold(primaryMode),


                FormatString.setNormal("Cargo? "),
                cargo,
                

                FormatString.setNormal("Hatch? "),
                hatch,
               

                FormatString.setNormal("Crossed Hab Line? "),
                hab,                

            };

            //display sandstorm stats in the second grid
            ArrayAdapter gridSandstormAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, sandstormDisp);
            gridSandstorm.Adapter = gridSandstormAdapt;

            //display a list of matches the team was in and the details from each one
            string[] properties = new string[]
            {

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
                "Recommended"
            };

            gridMatches.NumColumns = compiled.Count + 1;
            List<string> display = new List<string>();

            //rows
            display.Add("Match Number:");
            {
                for (int i = 0; i < compiled.Count; i++)
                {
                    display.Add(compiled[i].matchNumber.ToString());
                }

                int p = 0;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].getResult());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].getPosition());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].isTable.ToString().ToUpper());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add("Level " + compiled[j].sandstormStartLevel.ToString());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].getSandstormMode());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].sandstormHatch.ToString());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].sandstormCargo.ToString());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].sandstormLine.ToString());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].cargo.ToString());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].cargoWell.ToString());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].cargoBarely.ToString());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].hatch.ToString());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].hatchWell.ToString());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].hatchBarely.ToString());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].getClimb());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].goodDrivers.ToString());
                }
                p++;
                display.Add(properties[p]);
                for (int j = 0; j < compiled.Count; j++)
                {

                    display.Add(compiled[j].getRecommendation());
                }
            }
            
            //put matches in the third grid
            ArrayAdapter gridMatchesAdapt = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridMatches.Adapter = gridMatchesAdapt;
        }
    }
}