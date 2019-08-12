using System;
using System.Collections.Generic;
using System.Linq;

namespace Team811Scout
{
    /*The CompiledEventData class extends the MatchData class. It contains information for all
     * scouted matches for an event from all devices. Compiled Event Data can have all the same properties as
     * a normal MatchData, but it also can perform calculations and figure out percentages*/

    public class CompiledEventData: MatchData
    {
        public CompiledEventData()
        {
        }

        public CompiledEventData(string officialname, string start, string end, string qrdata, bool isactive, int id)
        {
            //make the compiled event data mimic the event details
            officialName = officialname;
            startDate = start;
            endDate = end;
            //CompiledEventData specific data
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

        //the official primary key for this class is a string, but cID is used in the database because the string
        //is match-specific and this class deals with the entire event
        //the cID matches the event id for the compiled event data
        public int cID { get; set; }

        //read QR data and return it in a multidimensional list of matches grouped by team number
        public List<List<CompiledEventData>> compileData()
        {
            //how many properties are we looking for:
            const int dataLength = 17;
            CompiledEventData placeholder = new CompiledEventData();
            //list before putting it in team order
            List<CompiledEventData> preOrder = new List<CompiledEventData>();
            //get a substring for the raw data
            string matchData = rawData;
            int substringStart = 0;
            while (matchData.Length > 1)
            {
                int startIndex = 0;
                //give default values to the placeholder
                placeholder = new CompiledEventData();
                placeholder.officialName = officialName;
                placeholder.cID = cID;
                placeholder.isActive = false;
                //set values which appear at the beginning of the QR string separated by commas
                List<int> matchCommas = AllIndexesOf(matchData, ",").ToList();
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
                //add the CompiledEventData
                preOrder.Add(placeholder);
                //move to the next team in the string
                substringStart += dataLength + matchCommas[1] + 1;
                matchData = rawData.Substring(substringStart);
            }
            //convert to an array to sort by team number
            CompiledEventData[] dataSorted = preOrder.ToArray();
            Array.Sort(dataSorted, delegate (CompiledEventData data1, CompiledEventData data2)
            {
                return data1.teamNumber.CompareTo(data2.teamNumber);
            });
            //create a list of the teams contained in the raw data
            List<int> uniqueTeams = new List<int>();
            foreach (CompiledEventData data in dataSorted)
            {
                if (!uniqueTeams.Contains(data.teamNumber))
                {
                    uniqueTeams.Add(data.teamNumber);
                }
            }
            //create a multidimensional list to group that data
            List<List<CompiledEventData>> groupedData = new List<List<CompiledEventData>>();
            groupedData.Add(new List<CompiledEventData>());
            //starting first index
            int c = 0;
            foreach (CompiledEventData data in dataSorted)
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
                    groupedData.Add(new List<CompiledEventData>());
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

        //Calculations with MatchData
        //Group calculations (arrays of percents/values for all teams in the data)
        //used for display purposes
        public List<int> getRecPercentArray()
        {
            List<List<CompiledEventData>> c = compileData();
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
            List<List<CompiledEventData>> c = compileData();
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
            List<List<CompiledEventData>> c = compileData();
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
            List<int> recsWellPerTeam = new List<int>();
            List<int> recsBarelyPerTeam = new List<int>();
            List<List<CompiledEventData>> c = compileData();
            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();
                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(Convert.ToByte(c[i][j].cargo));
                    recsWellPerTeam.Add(Convert.ToByte(c[i][j].cargoWell));
                    recsBarelyPerTeam.Add(Convert.ToByte(c[i][j].cargoBarely));
                }
                double countYes = recs.Where(x => x.Equals(1)).Count();
                double countNo = recs.Where(x => x.Equals(0)).Count();
                double countWell = recsWellPerTeam.Where(x => x.Equals(1)).Count();
                double countBarely = recsBarelyPerTeam.Where(x => x.Equals(1)).Count();
                double percent = (countYes + countWell * Constants.well_barelyWeight) / (countYes + countNo + countBarely * Constants.well_barelyWeight) * 100;
                if (percent > 100)
                {
                    percent = 100;
                }
                recsPerTeam.Add((int)Math.Round(percent));
            }
            return recsPerTeam;
        }

        public List<int> getHatchPercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<int> recsWellPerTeam = new List<int>();
            List<int> recsBarelyPerTeam = new List<int>();
            List<List<CompiledEventData>> c = compileData();
            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();
                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(Convert.ToByte(c[i][j].hatch));
                    recsWellPerTeam.Add(Convert.ToByte(c[i][j].hatchWell));
                    recsBarelyPerTeam.Add(Convert.ToByte(c[i][j].hatchBarely));
                }
                double countYes = recs.Where(x => x.Equals(1)).Count();
                double countNo = recs.Where(x => x.Equals(0)).Count();
                double countWell = recsWellPerTeam.Where(x => x.Equals(1)).Count();
                double countBarely = recsBarelyPerTeam.Where(x => x.Equals(1)).Count();
                double percent = (countYes + countWell * Constants.well_barelyWeight) / (countYes + countNo + countBarely * Constants.well_barelyWeight) * 100;
                if (percent > 100)
                {
                    percent = 100;
                }
                recsPerTeam.Add((int)Math.Round(percent));
            }
            return recsPerTeam;
        }

        public List<int> getClimb2PercentArray()
        {
            List<int> recsPerTeam = new List<int>();
            List<List<CompiledEventData>> c = compileData();
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
            List<List<CompiledEventData>> c = compileData();
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
            List<List<CompiledEventData>> c = compileData();
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
            List<List<CompiledEventData>> c = compileData();
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
            List<List<CompiledEventData>> c = compileData();
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

        //calculations for an individual team (for detailed display)
        //get a list of all matchdatas for one team
        private List<CompiledEventData> dataForTeam(int team)
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
            List<CompiledEventData> c = compileData()[index];
            return c;
        }

        public int getRecPercentForTeam(int team)
        {
            List<int> recs = new List<int>();
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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
            List<CompiledEventData> c = dataForTeam(team);
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