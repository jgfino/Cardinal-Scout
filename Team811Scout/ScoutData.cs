using SQLite;

namespace Team811Scout
{
    public class ScoutData
    {
        [PrimaryKey]
        public string ID { get; set; }

        public ScoutData()
        {
        }

        public ScoutData(
            string id,
            string name,
            string start,
            string end,

            int matchNum,
            int teamNum,
            int pos,
            bool table,
            int Mode,
            bool sHatch,
            bool sCargo,
            bool sLine,
            int StartLevel,
            bool c,
            bool cWell,
            bool cBarely,
            bool h,
            bool hWell,
            bool hBarely,
            int clim,
            bool Drivers,
            int Recommend,
            int res,
            string comments,
            bool isCurrent)
        {
            ID = id;
            eventName = name;
            startDate = start;
            endDate = end;
            matchNumber = matchNum;
            teamNumber = teamNum;
            position = pos;
            isTable = table;
            sandstormMode = Mode;
            sandstormHatch = sHatch;
            sandstormCargo = sCargo;
            sandstormLine = sLine;
            sandstormStartLevel = StartLevel;
            cargo = c;
            cargoWell = cWell;
            cargoBarely = cBarely;
            hatch = h;
            hatchWell = hWell;
            hatchBarely = hBarely;
            climb = clim;
            goodDrivers = Drivers;
            wouldRecommend = Recommend;
            result = res;
            additionalComments = comments;
            isCurrentMatch = isCurrent;

        }


        public int matchNumber { get; set; }
        public int teamNumber { get; set; }
        public int position { get; set; }
        //1-3: red 1-3; 4-6: blue 1-3

        public string getPosition()
        {
            if(position==0)
            {
                return "Red 1";
            }
            else if(position==1)
            {
                return "Red 2";
            }
            else if(position==2)
            {
                return "Red 3";
            }
            else if(position==3)
            {
                return "Blue 1";
            }
            else if(position==4)
            {
                return "Blue 2";    
            }
            else
            {
                return "Blue 3";
            }
        }

        public bool isTable { get; set; }

        public int sandstormMode { get; set; }
        //0 - auto, 1 - camera, 2 - nothing
        public string getSandstormMode()
        {
            if(sandstormMode==0)
            {
                return "Auto";
            }
            else if(sandstormMode==1)
            {
                return "Teleop w/Camera";
            }
            else
            {
                return "Nothing";
            }
        }
        
        public bool sandstormHatch { get; set; }
        public bool sandstormCargo { get; set; }
        public bool sandstormLine { get; set; }

        public int sandstormStartLevel { get; set; }
        //1,2,3
       
        public bool cargo { get; set; }
        public bool cargoWell { get; set; }
        public bool cargoBarely { get; set; }
        public bool hatch { get; set; }
        public bool hatchWell { get; set; }
        public bool hatchBarely { get; set; }
        public int climb { get; set; }
        public string getClimb()
        {
            if(climb==2)
            {
                return "Level 2";
            }
            else if (climb==3)
            {
                return "Level 3";
            }
            else
            {
                return "No Climb";
            }
        }
        
        public bool goodDrivers { get; set; }
        public int wouldRecommend { get; set; }
        //0 - yes; 1 - no; 2 - maybe
        public string getRecommendation()
        {
            if(wouldRecommend==0)
            {
                return "Yes";
            }
            else if(wouldRecommend==1)
            {
                return "No";
            }
            else
            {
                return "Maybe";
            }
        }


        public int result { get; set; }
        //0 - win; 1 - lose; 2 - tie
        public string getResult()
        {
            if(result==0)
            {
                return "Win";
            }
            else if(result==1)
            {
                return "Loss";
            }
            else
            {
                return "Tie";
            }
        }

        public string additionalComments { get; set; }
        public string eventName { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public bool isCurrentMatch { get; set; }
        
       
    }
}