using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
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
            textTitle.TextFormatted = TextUtils.ConcatFormatted(FormatString.setNormal("Viewing Stats for Team: '"), FormatString.setBold(currentTeam.ToString()), FormatString.setNormal("'"));

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

            //first two rows
            
            List<SpannableString> statsDisp = new List<SpannableString>()
            {
                FormatString.setNormal("Cargo / Hatch"),
                FormatString.setNormal("Climb Level 2 / Level 3"),
                FormatString.setNormal("Reccomendation %"),
                FormatString.setBold(cargoPerc.ToString()+"% / "+hatchPerc.ToString()+"%"),
                FormatString.setBold(climbPerc2.ToString()+"% / "+climbPerc3.ToString()+"%"),
                FormatString.setBold(recPerc.ToString()+"%"),
            };

            //third row (decide)

            //cargo or hatch bot
            if (cargoPerc >= Constants.hatch_cargoMin && hatchPerc > Constants.hatch_cargoMin)
            {
                statsDisp.Add(FormatString.setColorBold("Both", Constants.appGreen));
            }
            else if (cargoPerc >= Constants.hatch_cargoMin)
            {
                statsDisp.Add(FormatString.setColorBold("Cargo", Constants.appGreen));
            }
            else if (hatchPerc >= Constants.hatch_cargoMin)
            {
                statsDisp.Add(FormatString.setColorBold("Hatch", Constants.appGreen));
            }
            else
            {
                statsDisp.Add(FormatString.setColorBold("Neither", Constants.appRed));
            }

            //what kind of climber
            if (climbPerc2 >= Constants.climb2Min && climbPerc3 >= Constants.climb3Min)
            {
                statsDisp.Add(FormatString.setColorBold("Level 2/3 Climber", Constants.appGreen));
            }
            else if (climbPerc3 >= Constants.climb3Min)
            {
                statsDisp.Add(FormatString.setColorBold("Level 3 Climber", Constants.appGreen));
            }
            else if (climbPerc2 >= Constants.climb2Min)
            {
                statsDisp.Add(FormatString.setColorBold("Level 2 Climber", Constants.appGreen));
            }
            else
            {
                statsDisp.Add(FormatString.setColorBold("No reliable climber", Constants.appRed));
            }

            //recommendation
            if (recPerc >= Constants.recommendThreshHigh)
            {
                statsDisp.Add(FormatString.setColorBold("Recommended", Constants.appGreen));
            }
            else if (recPerc <= Constants.recommendThreshLow)
            {
                statsDisp.Add(FormatString.setColorBold("Not Recommended", Constants.appRed));
            }
            else
            {
                statsDisp.Add(FormatString.setColorBold("Possible Recommend", Constants.appYellow));
            }

            //display general stats in first grid box
            ArrayAdapter gridStatsAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, statsDisp);
            gridStats.Adapter = gridStatsAdapt;

            //figure out which sandstorm mode they use the most often

            SpannableString primaryMode;
            if (autoPerc > teleopPerc && autoPerc > nothingPerc)
            {
                primaryMode = FormatString.setColorBold("Auto", Constants.appGreen);
            }
            else if (teleopPerc > autoPerc && teleopPerc > nothingPerc)
            {
                primaryMode = FormatString.setColorBold("Teleop w/Camera", Constants.appGreen);
            }
            else
            {
                primaryMode = FormatString.setColorBold("Nothing", Constants.appRed);
            }

            SpannableString cargo = FormatString.setColorBold("NO (" + cargoSandstormPerc.ToString() + "%) ", Constants.appRed);
            SpannableString hatch = FormatString.setColorBold("NO (" + hatchSandstormPerc.ToString() + "%) ", Constants.appRed);
            SpannableString hab = FormatString.setColorBold("NO (" + habPerc.ToString() + "%) ", Constants.appRed);

            if (cargoSandstormPerc >= Constants.sandstorm_Hatch_CargoThresh)
            {
                cargo = FormatString.setColorBold("YES (" + cargoSandstormPerc.ToString() + "%) ", Constants.appGreen);
            }


            if (hatchSandstormPerc >= Constants.sandstorm_Hatch_CargoThresh)
            {
                hatch = FormatString.setColorBold("YES (" + hatchSandstormPerc.ToString() + "%) ", Constants.appGreen);
            }

            if (habPerc >= Constants.sandstorm_Hatch_CargoThresh)
            {
                hab = FormatString.setColorBold("YES (" + habPerc.ToString() + "%) ", Constants.appGreen);
            }


            List<SpannableString> sandstormDisp = new List<SpannableString>()
            {
                FormatString.setBold("Primary Sandstorm Mode: "),
                primaryMode,

                FormatString.setBold("Cargo? "),
                cargo,

                FormatString.setBold("Hatch? "),
                hatch,

                FormatString.setBold("Crossed Hab Line? "),
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
            List<SpannableString> display = new List<SpannableString>();

            //rows            
            {
                display.Add(FormatString.setBold("Match Number:"));
                for (int i = 0; i < compiled.Count; i++)
                {
                    display.Add(FormatString.setBold(compiled[i].matchNumber.ToString()));
                }

                int p = 0;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].result == 0)
                    {
                        display.Add(FormatString.setColor(compiled[j].getResult(), Constants.appGreen));
                    }
                    else if(compiled[j].result==1)
                    {
                        display.Add(FormatString.setColor(compiled[j].getResult(), Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].getResult(),Constants.appYellow));
                    }
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if(compiled[j].position>=3)
                    {
                        display.Add(FormatString.setColor(compiled[j].getPosition(),Constants.appBlue));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].getPosition(), Constants.appOrange));
                    }
                    
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if(compiled[j].isTable)
                    {
                       display.Add(FormatString.setColorBold(compiled[j].isTable.ToString().ToUpper(),Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].isTable.ToString().ToUpper(), Constants.appGreen));
                    }
                    
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].sandstormStartLevel == 1)
                    {
                        display.Add(FormatString.setNormal("Level " + compiled[j].sandstormStartLevel.ToString()));
                    }
                    else
                    {
                        display.Add(FormatString.setColor("Level " + compiled[j].sandstormStartLevel.ToString(),Constants.appGreen));
                    }
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].sandstormMode == 0 || compiled[j].sandstormMode==1)
                    {
                        display.Add(FormatString.setColor(compiled[j].getSandstormMode(),Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].getSandstormMode(),Constants.appRed));
                    }                    
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if(compiled[j].sandstormHatch)
                    {
                        display.Add(FormatString.setColor(compiled[j].sandstormHatch.ToString(),Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].sandstormHatch.ToString(), Constants.appRed));
                    }
                    
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].sandstormCargo)
                    {
                        display.Add(FormatString.setColor(compiled[j].sandstormCargo.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].sandstormCargo.ToString(), Constants.appRed));
                    }
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].sandstormLine)
                    {
                        display.Add(FormatString.setColor(compiled[j].sandstormLine.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].sandstormLine.ToString(), Constants.appRed));
                    }
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if(compiled[j].cargo)
                    {
                        display.Add(FormatString.setColor(compiled[j].cargo.ToString(),Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].cargo.ToString(), Constants.appRed));
                    }
                    
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if(compiled[j].cargoWell)
                    {
                        display.Add(FormatString.setColor(compiled[j].cargoWell.ToString(),Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].cargoWell.ToString(),Constants.appRed));
                    }                    
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].cargoBarely)
                    {
                        display.Add(FormatString.setColor(compiled[j].cargoBarely.ToString(), Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].cargoBarely.ToString(), Constants.appGreen));
                    }
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].hatch)
                    {
                        display.Add(FormatString.setColor(compiled[j].hatch.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].hatch.ToString(), Constants.appRed));
                    }
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].hatchWell)
                    {
                        display.Add(FormatString.setColor(compiled[j].hatchWell.ToString(), Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].hatchWell.ToString(),Constants.appRed));
                    }
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if (compiled[j].hatchBarely)
                    {
                        display.Add(FormatString.setColor(compiled[j].hatchBarely.ToString(), Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].hatchBarely.ToString(),Constants.appGreen));
                    }
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if(compiled[j].climb == 3)
                    {
                        display.Add(FormatString.setColorBold(compiled[j].getClimb(),Constants.appGreen));
                    }
                    else if(compiled[j].climb==2)
                    {
                        display.Add(FormatString.setColor(compiled[j].getClimb(),Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].getClimb(),Constants.appRed));
                    }
                    
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if(compiled[j].goodDrivers)
                    {
                        display.Add(FormatString.setColor(compiled[j].goodDrivers.ToString(),Constants.appGreen));
                    }
                    else
                    {
                        display.Add(FormatString.setColor(compiled[j].goodDrivers.ToString(), Constants.appRed));
                    }
                    
                }
                p++;
                display.Add(FormatString.setBold(properties[p]));
                for (int j = 0; j < compiled.Count; j++)
                {
                    if(compiled[j].wouldRecommend==0)
                    {
                        display.Add(FormatString.setColorBold(compiled[j].getRecommendation(),Constants.appGreen));
                    }
                    else if(compiled[j].wouldRecommend==1)
                    {
                        display.Add(FormatString.setColorBold(compiled[j].getRecommendation(), Constants.appRed));
                    }
                    else
                    {
                        display.Add(FormatString.setColorBold(compiled[j].getRecommendation(), Constants.appYellow));
                    }
                    
                }
            }

            //put matches in the third grid
            ArrayAdapter gridMatchesAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridMatches.Adapter = gridMatchesAdapt;
        }
    }
}