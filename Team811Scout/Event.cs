using SQLite;

namespace Team811Scout
{
    public class Event
    {
        /*This class stores an event and its properties: name, start date, end date, and id*/

        public Event()
        {
        }

        public Event(string start, string end, string name, int ID, bool isCurrent)
        {
            startDate = start;
            endDate = end;
            eventName = name;
            eventID = ID;
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

        //events are identified by their Event ID in the SQL Database; set this as the Primary Key
        [PrimaryKey]
        public int eventID { get; set; }
    }
}