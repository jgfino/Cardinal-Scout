using System;
using System.Collections.Generic;
using System.Linq;

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

        }

        public string officialName { get; set; }
        public string rawData { get; set; }
        public bool isActive { get; set; }


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

        public List<string> getRecPercentArray(List<List<CompiledScoutData>> c)
        {
            List<string> recsPerTeam = new List<string>();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(c[i][j].wouldRecommend);              
                }

                int countYes = recs.Where(x => x.Equals(0)).Count();
                int countNo = recs.Where(x => x.Equals(1)).Count();
                int countMaybe = recs.Where(x => x.Equals(2)).Count();

                double percent = (countYes + (0.5 * countMaybe)) / (countYes + countMaybe + countNo)*100;
                recsPerTeam.Add(((int)Math.Round(percent)).ToString()+"%");

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
                
                recsPerTeam.Add(countWin.ToString()+"-"+countLoss.ToString()+"-"+countTie.ToString());

            }

            return recsPerTeam;
        }

        public List<string> getCargoPercent(List<List<CompiledScoutData>> c)
        {
            List<string> recsPerTeam = new List<string>();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(Convert.ToByte(c[i][j].cargo));
                }

                int countYes = recs.Where(x => x.Equals(0)).Count();
                int countNo = recs.Where(x => x.Equals(1)).Count();

                double percent = (countYes) / (countYes + countNo) * 100;
                recsPerTeam.Add(Math.Round(percent).ToString()+"%");

            }

            return recsPerTeam;
        }

        public List<string> getHatchPercent(List<List<CompiledScoutData>> c)
        {
            List<string> recsPerTeam = new List<string>();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(Convert.ToByte(c[i][j].hatch));
                }

                int countYes = recs.Where(x => x.Equals(0)).Count();
                int countNo = recs.Where(x => x.Equals(1)).Count();

                double percent = (countYes) / (countYes + countNo) * 100;
                recsPerTeam.Add(Math.Round(percent).ToString()+"%");

            }

            return recsPerTeam;
        }

        public List<string> getClimbPercent(List<List<CompiledScoutData>> c)
        {
            List<string> recsPerTeam = new List<string>();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(c[i][j].climb);
                }

                int count2 = recs.Where(x => x.Equals(2)).Count();
                int count3 = recs.Where(x => x.Equals(3)).Count();
                int countNone = recs.Where(x => x.Equals(0)).Count();

                double twoPercent = (count2) / (count2 + count3 + countNone) * 100;
                double threePercent = (count3) / (count2 + count3 + countNone) * 100;

                recsPerTeam.Add(Math.Round(threePercent).ToString()+"% / "+ Math.Round(twoPercent).ToString()+"%");

            }

            return recsPerTeam;
        }

        public List<string> getDriversPercent(List<List<CompiledScoutData>> c)
        {
            List<string> recsPerTeam = new List<string>();

            for (int i = 0; i < c.Count; i++)
            {
                List<int> recs = new List<int>();

                for (int j = 0; j < c[i].Count; j++)
                {
                    recs.Add(Convert.ToByte(c[i][j].goodDrivers));
                }

                int countYes = recs.Where(x => x.Equals(0)).Count();
                int countNo = recs.Where(x => x.Equals(1)).Count();

                double percent = (countYes) / (countYes + countNo) * 100;
                recsPerTeam.Add(Math.Round(percent).ToString() + "%");

            }

            return recsPerTeam;
        }

        public List<string> getTeamNumbers(List<List<CompiledScoutData>> c)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < c.Count; i++)
            {
                result.Add(c[i][0].teamNumber.ToString());
            }

            return result;
        }

    }

}