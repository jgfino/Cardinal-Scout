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

        GridView gridStats;
        GridView gridSandstorm;
        GridView gridMatches;
        TextView textTitle;
        int currentIndex;

        CompiledScoutData currentCompiled;
        EventDatabase eData;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.team_details);

            gridStats = FindViewById<GridView>(Resource.Id.gridViewStats);
            gridSandstorm = FindViewById<GridView>(Resource.Id.gridViewSandstorm);
            gridMatches = FindViewById<GridView>(Resource.Id.gridViewMatches);

            eData = new EventDatabase();

            textTitle = FindViewById<TextView>(Resource.Id.textTitle);















            currentCompiled = eData.GetCurrentCompiled();


            List<CompiledScoutData> compiled = new List<CompiledScoutData>();

            foreach (CompiledScoutData c in currentCompiled.compileData()[eData.getTeamIndex().ID])
            {
                compiled.Add(c);
            }

            compiled.Reverse();

            int recPerc = currentCompiled.getRecPercent(compiled);
            string record = currentCompiled.getWinRecord(compiled);
            int cargoPerc = currentCompiled.getCargoPercent(compiled);
            int hatchPerc = currentCompiled.getHatchPercent(compiled);
            int climbPerc2 = currentCompiled.getClimb2Percent(compiled);
            int climbPerc3 = currentCompiled.getClimb3Percent(compiled);
            int driversPerc = currentCompiled.getDriversPercent(compiled);
            int tablePerc = currentCompiled.getTablePercent(compiled);
            int winPerc = currentCompiled.getWinPercent(compiled);

            int cargoSandstormPerc = currentCompiled.getCargoSandstormPercent(compiled);
            int hatchSandstormPerc = currentCompiled.getHabSandstormPercent(compiled);
            int habPerc = currentCompiled.getHabSandstormPercent(compiled);
            int autoPerc = currentCompiled.getAutoPercent(compiled);
            int teleopPerc = currentCompiled.getTeleopPercent(compiled);
            int nothingPerc = currentCompiled.getNothingPercent(compiled);

            List<SpannableString> statsDisp = new List<SpannableString>()
            {
                new FormatString("Cargo/Hatch%").getNormal(),
                new FormatString("Climb Level 2% / Level 3%").getNormal(),
                new FormatString("Reccomendation %").getNormal(),
                new FormatString(cargoPerc.ToString()+"% / "+hatchPerc.ToString()+"%").getBold(),
                new FormatString(climbPerc2.ToString()+"% / "+climbPerc3.ToString()+"%").getBold(),
                new FormatString(recPerc.ToString()+"%").getBold(),

            };

            if (cargoPerc > 49 && hatchPerc > 49)
            {
                if (Math.Abs(cargoPerc - hatchPerc) < 16)
                {
                    statsDisp.Add(new FormatString("Both").setColorBold(0, 137, 9));
                }
                else if (cargoPerc > hatchPerc)
                {
                    statsDisp.Add(new FormatString("Cargo").setColorBold(0, 137, 9));
                }
                else
                {
                    statsDisp.Add(new FormatString("Hatch").setColorBold(0, 137, 9));
                }
            }
            else
            {
                statsDisp.Add(new FormatString("Neither").setColorBold(255, 0, 0));
            }



            ArrayAdapter gridStatsAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, statsDisp);
            gridStats.Adapter = gridStatsAdapt;

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

            SpannableString cargo = new FormatString("NO (" + cargoSandstormPerc.ToString() + ") %").setColorBold(255,0,0);
            SpannableString hatch = new FormatString("NO (" + hatchSandstormPerc.ToString() + ") %").setColorBold(255,0,0);
            SpannableString hab = new FormatString("NO (" + habPerc.ToString() + ") %").setColorBold(255,0,0);

            if (cargoSandstormPerc>49)
            {
                cargo = new FormatString("YES (" + cargoSandstormPerc.ToString() + ") %").setColorBold(0, 137, 9);
            }
            

            if(hatchSandstormPerc>49)
            {
                hatch = new FormatString("YES (" + hatchSandstormPerc.ToString() + ") %").setColorBold(0, 137, 9);
            }
            
            if(habPerc>49)
            {
                hab = new FormatString("YES (" + habPerc.ToString() + ") %").setColorBold(0, 137, 9);
            }


            List<SpannableString> sandstormDisp = new List<SpannableString>()
            {
                new FormatString("Primary Sandstorm Mode: ").getBold(),
                new FormatString(primaryMode).getBold(),


                new FormatString("Cargo? ").getNormal(),
                cargo,
                

                new FormatString("Hatch? ").getNormal(),
                hatch,
               

                new FormatString("Crossed Hab Line? ").getNormal(),
                hab,
                

            };

            ArrayAdapter gridSandstormAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, sandstormDisp);
            gridSandstorm.Adapter = gridSandstormAdapt;

            //matches list

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

            
            ArrayAdapter gridMatchesAdapt = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, display);
            gridMatches.Adapter = gridMatchesAdapt;
        }
    }
}