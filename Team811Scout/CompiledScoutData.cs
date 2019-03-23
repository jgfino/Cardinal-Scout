using System;
using System.Collections.Generic;
using System.Linq;

namespace Team811Scout
{
    /*The CompiledScoutData class extends the ScoutData class. A CompiledScoutData can have all the same properties as
     * a normal ScoutData, but it also can perform calculations and figure out percentages based on multiple
     * other CompiledScoutDatas*/

    public class CompiledScoutData: ScoutData
    {
        public CompiledScoutData()
        {
        }


        public CompiledScoutData(string officialname, string start, string end, string qrdata, bool isactive, int id)
        {
            //make the compiled scout data mimic the event details
            officialName = officialname;
            startDate = start;
            endDate = end;

            //CompiledScoutData specific data
            rawData = qrdata;
            cID = id;
            ID = cID.ToString();
            isActive = isactive;

            //note the date and time modified
            dateMod = DateTime.Now.ToString("MM/dd/yyyy");
            timeMod = DateTime.Now.ToShortTimeString();

        }

        public string officialName { get; set; }

        //string of data collected from the QR codes
        public string rawData { get; set; }

        public bool isActive { get; set; }
        public string dateMod { get; set; }
        public string timeMod { get; set; }

        //the official primary key for this class is a string, but cID is used in the database anyway
        //the cID matches the event id for the compiled scout data
        public int cID { get; set; }

        //compile the data and return it in a multidimensional list grouped by team number
        public List<List<CompiledScoutData>> compileData()
        {
            //how many properties are we looking for:
            const int dataLength = 17;

            CompiledScoutData placeholder = new CompiledScoutData();

            //list before putting it in team order
            List<CompiledScoutData> preOrder = new List<CompiledScoutData>();

            //get a substring for the raw data
            string matchData = rawData;
            int substringStart = 0;
            while (matchData.Length > 1)
            {
                int startIndex = 0;

                //give default values to the placeholder
                placeholder = new CompiledScoutData();
                placeholder.officialName = officialName;
                placeholder.cID = cID;
                placeholder.isActive = false;

                //set values which appear at the beginning of the QR string separated by commas
                List<int> matchCommas = AllIndexesOf(matchData, ",").ToList<int>();
                placeholder.teamNumber = int.Parse(matchData.Substring(0, matchCommas[0]));
                placeholder.matchNumber = int.Parse(matchData.Substring(matchCommas[0] + 1, matchCommas[1] - matchCommas[0] - 1));

                //create substring of data after the commas
                matchData = matchData.Substring(matchCommas[1] + 1, dataLength);

                //interpret numerical values
                placeholder.result = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;
                placeholder.position = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;
                placeholder.isTable = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.sandstormStartLevel = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;
                placeholder.sandstormMode = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;
                placeholder.sandstormHatch = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.sandstormCargo = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.sandstormLine = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.cargo = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.cargoWell = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.cargoBarely = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.hatch = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.hatchWell = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.hatchBarely = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.climb = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;
                placeholder.goodDrivers = Convert.ToBoolean(int.Parse(matchData.Substring(startIndex, 1)));
                startIndex++;
                placeholder.wouldRecommend = int.Parse(matchData.Substring(startIndex, 1));
                startIndex++;

                //add the CompiledScoutData
                preOrder.Add(placeholder);

                //move to the next team in the string
                substringStart += dataLength + matchCommas[1] + 1;
                matchData = matchData.Substring(substringStart);
            }

            //convert to an array to sort by team number
            CompiledScoutData[] dataSorted = preOrder.ToArray();
            Array.Sort(dataSorted, delegate (CompiledScoutData data1, CompiledScoutData data2)
            {
                return data1.teamNumber.CompareTo(data2.teamNumber);
            });

            //create a list of the teams contained in the raw data
            List<int> uniqueTeams = new List<int>();
            foreach (CompiledScoutData data in dataSorted)
            {
                if (!uniqueTeams.Contains(data.teamNumber))
                {
                    uniqueTeams.Add(data.teamNumber);
                }

            }

            //create a multidimensional list to group that data
            List<List<CompiledScoutData>> groupedData = new List<List<CompiledScoutData>>();
            groupedData.Add(new List<CompiledScoutData>());

            //starting first index
            int c = 0;
            foreach (CompiledScoutData data in dataSorted)
            {
                //add all matches by one team to one index of the list
                if (data.teamNumber == uniqueTeams[c])
                {
                    groupedData[c].Add(data);
                }
                //go to the next index after all matches by that team are added
                else
                {
                    c++;
                    groupedData.Add(new List<CompiledScoutData>());
                    groupedData[c].Add(data);
                }
            }

            //return the grouped data list
            return groupedData;
        }

        //used to find where commas are in the rawData in order to split it up into teams/matches
        private IEnumerable<int> AllIndexesOf(string str, string searchstring)
        {
            int minIndex = str.IndexOf(searchstring);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
            }
        }

        //Calculations with ScoutData

        //Group calculations (arrays of percents/values for all teams in the data
        public List<int> getRecPercentArray()
        {
            List<List<CompiledScoutData>> c = compileData();
            List<int> recsPerTeam = new List<int>();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(c[i][j].wouldRecommend);
                }

                double countYes = recs.Where(x => x.Equals(0)).Count();
                double countNo = recs.Where(x => x.Equals(1)).Count();
                double countMaybe = recs.Where(x => x.Equals(2)).Count();

                double percent = (countYes + (0.5 * countMaybe)) / (countYes + countMaybe + countNo) * 100;
                recsPerTeam.Add((int)Math.Round(percent));

            }

            return recsPerTeam;
        }

        public List<string> getWinRecordArray()
        {
            List<string> recsPerTeam = new List<string>();
            List<List<CompiledScoutData>> c = compileData();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(c[i][j].result);
                }

                int countWin = recs.Where(x => x.Equals(0)).Count();
                int countLoss = recs.Where(x => x.Equals(1)).Count();
                int countTie = recs.Where(x => x.Equals(2)).Count();

                recsPerTeam.Add(countWin.ToString() + "-" + countLoss.ToString() + "-" + countTie.ToString());

            }

            return recsPerTeam;
        }

        public List<int> getWinPercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<List<CompiledScoutData>> c = compileData();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(c[i][j].result);
                }

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(c[i][j].result);
                }

                double countWin = recs.Where(x => x.Equals(0)).Count();
                double countLoss = recs.Where(x => x.Equals(1)).Count();
                double countTie = recs.Where(x => x.Equals(2)).Count();

                double percent = countWin / (countLoss + countTie) * 100;
                recsPerTeam.Add((int)Math.Round(percent));

            }
            return recsPerTeam;
        }

        public List<int> getCargoPercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<List<CompiledScoutData>> c = compileData();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(Convert.ToByte(c[i][j].cargo));
                }

                double countYes = recs.Where(x => x.Equals(1)).Count();
                double countNo = recs.Where(x => x.Equals(0)).Count();

                double percent = (countYes) / (countYes + countNo) * 100;
                recsPerTeam.Add((int)Math.Round(percent));

            }

            return recsPerTeam;
        }

        public List<int> getHatchPercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<List<CompiledScoutData>> c = compileData();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(Convert.ToByte(c[i][j].hatch));
                }

                double countYes = recs.Where(x => x.Equals(1)).Count();
                double countNo = recs.Where(x => x.Equals(0)).Count();

                double percent = (countYes) / (countYes + countNo) * 100;
                recsPerTeam.Add((int)Math.Round(percent));

            }

            return recsPerTeam;
        }

        public List<int> getClimb2PercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<List<CompiledScoutData>> c = compileData();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(c[i][j].climb);
                }

                double count2 = recs.Where(x => x.Equals(2)).Count();
                double count3 = recs.Where(x => x.Equals(3)).Count();
                double countNone = recs.Where(x => x.Equals(0)).Count();

                double twoPercent = (count2) / (count2 + count3 + countNone) * 100;


                recsPerTeam.Add((int)Math.Round(twoPercent));

            }

            return recsPerTeam;
        }

        public List<int> getClimb3PercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<List<CompiledScoutData>> c = compileData();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(c[i][j].climb);
                }

                double count2 = recs.Where(x => x.Equals(2)).Count();
                double count3 = recs.Where(x => x.Equals(3)).Count();
                double countNone = recs.Where(x => x.Equals(0)).Count();

                double threePercent = (count3) / (count2 + count3 + countNone) * 100;


                recsPerTeam.Add((int)Math.Round(threePercent));

            }

            return recsPerTeam;
        }

        public List<int> getDriversPercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<List<CompiledScoutData>> c = compileData();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(Convert.ToByte(c[i][j].goodDrivers));
                }

                double countYes = recs.Where(x => x.Equals(1)).Count();
                double countNo = recs.Where(x => x.Equals(0)).Count();

                double percent = ((countYes) / (countYes + countNo)) * 100;
                recsPerTeam.Add((int)Math.Round(percent));

            }

            return recsPerTeam;
        }

        public List<int> getTeamNumbersArray()
        {
            List<List<CompiledScoutData>> c = compileData();
            List<int> result = new List<int>();

            for (int i = 0; i < c.Count; i++)
            {
                result.Add(c[i][0].teamNumber);
            }

            return result;
        }

        public List<int> getTablePercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<List<CompiledScoutData>> c = compileData();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(Convert.ToByte(c[i][j].isTable));
                }

                double countYes = recs.Where(x => x.Equals(1)).Count();
                double countNo = recs.Where(x => x.Equals(0)).Count();

                double percent = (countYes) / (countYes + countNo) * 100;
                recsPerTeam.Add((int)Math.Round(percent));

            }

            return recsPerTeam;
        }

        //calculations for an individual team

        //get a list of the scoutdata for one team
        private List<CompiledScoutData> dataForTeam(int team)
        {
            int index = -1;
            for (int i = 0; i < compileData().Count; i++)
            {
                if (compileData()[i][0].teamNumber == team)
                {
                    index = i;
                    break;
                }
            }

            List<CompiledScoutData> c = compileData()[index];
            return c;
        }

        public int getRecPercentForTeam(int team)
        {
            List<int> recs = new List<int>();            

            List<CompiledScoutData> c = dataForTeam(team);
            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(c[i].wouldRecommend);
            }

            double countYes = recs.Where(x => x.Equals(0)).Count();
            double countNo = recs.Where(x => x.Equals(1)).Count();
            double countMaybe = recs.Where(x => x.Equals(2)).Count();

            double percent = (countYes + (0.5 * countMaybe)) / (countYes + countMaybe + countNo) * 100;

            return (int)percent;
        }

        public string getWinRecordForTeam(int team)
        {
            List<int> recs = new List<int>();

            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(c[i].result);
            }

            int countWin = recs.Where(x => x.Equals(0)).Count();
            int countLoss = recs.Where(x => x.Equals(1)).Count();
            int countTie = recs.Where(x => x.Equals(2)).Count();

            return (countWin.ToString() + "-" + countLoss.ToString() + "-" + countTie.ToString());

        }

        public int getWinPercentForTeam(int team)
        {
            List<int> recs = new List<int>();

            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(c[i].result);
            }

            double countWin = recs.Where(x => x.Equals(0)).Count();
            double countLoss = recs.Where(x => x.Equals(1)).Count();
            double countTie = recs.Where(x => x.Equals(2)).Count();

            double percent = countWin / (countLoss + countTie) * 100;
            return (int)percent;

        }

        public int getCargoPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {

                recs.Add(Convert.ToByte(c[i].cargo));

            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();

            double percent = (countYes) / (countYes + countNo) * 100;
            return (int)percent;
        }

        public int getHatchPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {

                recs.Add(Convert.ToByte(c[i].hatch));
            }

            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();

            double percent = (countYes) / (countYes + countNo) * 100;

            return (int)percent;
        }

        public int getClimb2PercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {


                recs.Add(c[i].climb);

            }
            double count2 = recs.Where(x => x.Equals(2)).Count();
            double count3 = recs.Where(x => x.Equals(3)).Count();
            double countNone = recs.Where(x => x.Equals(0)).Count();

            double twoPercent = (count2) / (count2 + count3 + countNone) * 100;

            return (int)twoPercent;
        }

        public int getClimb3PercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {

                recs.Add(c[i].climb);

            }
            double count2 = recs.Where(x => x.Equals(2)).Count();
            double count3 = recs.Where(x => x.Equals(3)).Count();
            double countNone = recs.Where(x => x.Equals(0)).Count();

            double threePercent = (count3) / (count2 + count3 + countNone) * 100;

            return (int)threePercent;
        }

        public int getDriversPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {

                recs.Add(Convert.ToByte(c[i].goodDrivers));
            }

            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();

            double percent = ((countYes) / (countYes + countNo)) * 100;

            return (int)percent;
        }

        public int getTablePercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {
                recs.Add(Convert.ToByte(c[i].isTable));
            }

            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();

            double percent = (countYes) / (countYes + countNo) * 100;

            return (int)percent;
        }

        public int getCargoSandstormPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {

                recs.Add(Convert.ToByte(c[i].sandstormCargo));

            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();

            double percent = (countYes) / (countYes + countNo) * 100;
            return (int)percent;
        }
        public int getHatchSandstormPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {

                recs.Add(Convert.ToByte(c[i].sandstormHatch));

            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();

            double percent = (countYes) / (countYes + countNo) * 100;
            return (int)percent;
        }
        public int getHabSandstormPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {

                recs.Add(Convert.ToByte(c[i].sandstormLine));

            }
            double countYes = recs.Where(x => x.Equals(1)).Count();
            double countNo = recs.Where(x => x.Equals(0)).Count();

            double percent = (countYes) / (countYes + countNo) * 100;
            return (int)percent;
        }
        public int getAutoPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {

                recs.Add(c[i].sandstormMode);

            }
            double auto = recs.Where(x => x.Equals(0)).Count();
            double teleop = recs.Where(x => x.Equals(1)).Count();
            double nothing = recs.Where(x => x.Equals(2)).Count();

            double percent = (auto) / (teleop + auto + nothing) * 100;

            return (int)percent;
        }
        public int getTeleopPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {

                recs.Add(c[i].sandstormMode);

            }
            double auto = recs.Where(x => x.Equals(0)).Count();
            double teleop = recs.Where(x => x.Equals(1)).Count();
            double nothing = recs.Where(x => x.Equals(2)).Count();

            double percent = (teleop) / (teleop + auto + nothing) * 100;

            return (int)percent;
        }
        public int getNothingPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledScoutData> c = dataForTeam(team);

            for (int i = 0; i < c.Count; i++)
            {

                recs.Add(c[i].sandstormMode);

            }
            double auto = recs.Where(x => x.Equals(0)).Count();
            double teleop = recs.Where(x => x.Equals(1)).Count();
            double nothing = recs.Where(x => x.Equals(2)).Count();

            double percent = (nothing) / (teleop + auto + nothing) * 100;
            return (int)percent;
        }
    }

}