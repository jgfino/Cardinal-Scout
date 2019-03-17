using SQLite;

namespace Team811Scout
{
    public class Event
    {
        
        

        public Event()
        {

        }

        public Event(string start, string end, string name, int id, bool isCurrent)
        {
            startDate = start;
            endDate = end;
            eventName = name;
            eventID = id;
            isCurrentEvent = isCurrent;          

        }

        public string startDate
        {
            get; set;
        }
        public string endDate
        {
            get; set;
        }
        public string eventName
        {
            get; set;
        }

        public bool isCurrentEvent
        {
            get; set;
        }

        [PrimaryKey]
        public int eventID { get; set; }
    }
}