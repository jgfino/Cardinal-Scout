using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Team811Scout
{
    public class CompiledScoutData: ScoutData
    {

        public CompiledScoutData()
        {

        }

        public CompiledScoutData(string officialname, string start, string end, string qrdata, bool isactive, int id)
        {
            officialName = officialname;
            rawData = qrdata;
            cID = id;
            startDate = start;
            endDate = end;
            ID = cID.ToString();
            dateMod = DateTime.Now.ToString("MM/dd/yyyy");
            timeMod = DateTime.Now.ToShortTimeString();
            compileData();
           

        }

        public string officialName { get; set; }
        public string rawData { get; set; }
        public bool isActive { get; set; }
        public string dateMod { get; set; }
        public string timeMod { get; set; }

        public int cID { get; set; }
        
        public List<List<CompiledScoutData>> compileData()
        {
            const int dataLength = 17;
            CompiledScoutData placeholder = new CompiledScoutData();

            string matchData = rawData;

            int substringStart = 0;

            List<CompiledScoutData> preOrder = new List<CompiledScoutData>();

            while (matchData.Length > 1)
            {
                int startIndex = 0;

                placeholder = new CompiledScoutData();

                placeholder.officialName = officialName;
                placeholder.cID = cID;

                List<int> matchCommas = AllIndexesOf(matchData, ",").ToList<int>();

                placeholder.teamNumber = int.Parse(matchData.Substring(0, matchCommas[0]));
                placeholder.matchNumber = int.Parse(matchData.Substring(matchCommas[0] + 1, matchCommas[1] - matchCommas[0] - 1));

                matchData = matchData.Substring(matchCommas[1] + 1, dataLength);
                placeholder.isActive = false;

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
                substringStart += dataLength + matchCommas[1] + 1;
                preOrder.Add(placeholder);

                matchData = rawData.Substring(substringStart);
            }

            CompiledScoutData[] compiledData = new CompiledScoutData[preOrder.Count];
            CompiledScoutData[] dataSort = preOrder.ToArray();

            Array.Sort(dataSort, delegate (CompiledScoutData data1, CompiledScoutData data2)
            {
                return data1.teamNumber.CompareTo(data2.teamNumber);
            });

            List<int> uniqueTeams = new List<int>();

            int j = 0;
            foreach (CompiledScoutData data in dataSort)
            {
                compiledData[j] = data;

                if (!uniqueTeams.Contains(data.teamNumber))
                {
                    uniqueTeams.Add(data.teamNumber);
                }
                j++;
            }

            double loopCount = compiledData.Length / uniqueTeams.Count;
            List<List<CompiledScoutData>> groupedData = new List<List<CompiledScoutData>>();
            groupedData.Add(new List<CompiledScoutData>());

            int c = 0;
            foreach (CompiledScoutData data in compiledData)
            {

                if (data.teamNumber == uniqueTeams[c])
                {

                    groupedData[c].Add(data);

                }
                else
                {
                    c++;
                    groupedData.Add(new List<CompiledScoutData>());
                    groupedData[c].Add(data);

                }


            }


            return groupedData;


        }

        private IEnumerable<int> AllIndexesOf(string str, string searchstring)
        {
            int minIndex = str.IndexOf(searchstring);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
            }
        }


        //calcs
        public List<int> getRecPercentArray(List<List<CompiledScoutData>> c)
        {
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

        public List<string> getWinRecord(List<List<CompiledScoutData>> c)
        {
            List<string> recsPerTeam = new List<string>();

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

        public List<int> getWinPercentArray(List<List<CompiledScoutData>> c)
        {
            List<int> recsPerTeam = new List<int>();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(c[i][j].wouldRecommend);
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

        public List<int> getCargoPercent(List<List<CompiledScoutData>> c)
        {
            List<int> recsPerTeam = new List<int>();

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

        public List<int> getHatchPercent(List<List<CompiledScoutData>> c)
        {
            List<int> recsPerTeam = new List<int>();

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

        public List<int> getClimb2Percent(List<List<CompiledScoutData>> c)
        {
            List<int> recsPerTeam = new List<int>();

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

        public List<int> getClimb3Percent(List<List<CompiledScoutData>> c)
        {
            List<int> recsPerTeam = new List<int>();

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

        public List<int> getDriversPercent(List<List<CompiledScoutData>> c)
        {
            List<int> recsPerTeam = new List<int>();

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

        public List<int> getTeamNumbers(List<List<CompiledScoutData>> c)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < c.Count; i++)
            {
                result.Add(c[i][0].teamNumber);
            }

            return result;
        }

        public List<int> getTablePercent(List<List<CompiledScoutData>> c)
        {
            List<int> recsPerTeam = new List<int>();

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

    }

}