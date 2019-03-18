using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
namespace Team811Scout
{
    public class EventDatabase
    {
        private SQLiteConnection _connection;
        public EventDatabase()        {

            _connection = DependencyService.Get<ISQLite>().GetConnection();
            _connection.CreateTable<ScoutData>();
            _connection.CreateTable<Event>();
            _connection.CreateTable<CompiledScoutData>();
        }


        //Event database
        public Event GetEvent(int id)
        {
            return _connection.Table<Event>().FirstOrDefault(t => t.eventID == id);
        }
        public void DeleteEvent(int id)
        {
            _connection.Delete<Event>(id);
        }
        public void AddEvent(Event putEvent)
        {
            _connection.Insert(putEvent);
        }
        public IEnumerable<Event> GetEvents()
        {
            return (from t in _connection.Table<Event>()
                    select t).ToList();
        }
        public List<string> GetEventDisplayList()
        {
            List<string> result = new List<string>();
            foreach (Event e in _connection.Table<Event>())
            {
                result.Add(e.eventName + " (" + e.startDate + " - " + e.endDate + ") ID: " + e.eventID);
            }
            result.Reverse();
            return result;
        }
        public int eventCount()
        {
            return (from t in _connection.Table<Event>()
                    select t).ToList().Count;
        }
        public Event CurrentEvent()
        {
            foreach (Event e in _connection.Table<Event>())
            {
                if (e.isCurrentEvent)
                {
                    return e;
                }
            }
            return null;
        }
        public void SetCurrentEvent(int id)
        {
            foreach (Event e in _connection.Table<Event>())
            {
                Event placeholder = e;

                placeholder.isCurrentEvent = false;
                _connection.Delete<Event>(e.eventID);
                _connection.Insert(placeholder);

                if (e.eventID == id)
                {
                    placeholder.isCurrentEvent = true;
                    _connection.Delete<Event>(id);
                    _connection.Insert(placeholder);
                }
            }
        }
        public List<int> EventIDList()
        {
            List<int> ids = new List<int>();
            foreach (Event e in _connection.Table<Event>())
            {
                ids.Add(e.eventID);
            }
            ids.Reverse();
            return ids;
        }
        public void ChangeEventID(int oldid, int newid)
        {
            foreach (Event e in _connection.Table<Event>())
            {
                Event placeholder = e;
                if (e.eventID == oldid)
                {
                    placeholder.eventID = newid;
                    _connection.Insert(placeholder);
                    _connection.Delete<Event>(oldid);
                }
            }
            foreach (ScoutData s in _connection.Table<ScoutData>())
            {
                _connection.Delete<ScoutData>(s.ID);
                ScoutData placeholder = s;
                placeholder.ID = newid.ToString() + "," + s.ID.Substring(s.ID.IndexOf(",") + 1);
                _connection.Insert(placeholder);
            }
        }


        //ScoutData Database
        public ScoutData GetScoutData(string id)
        {
            return _connection.Table<ScoutData>().FirstOrDefault(t => t.ID == id);
        }
        public void DeleteScoutData(string id)
        {
            _connection.Delete<ScoutData>(id);
        }
        public void AddScoutData(ScoutData putScoutData)
        {
            _connection.Insert(putScoutData);
        }
        public IEnumerable<ScoutData> GetScoutData()
        {
            return (from t in _connection.Table<ScoutData>()
                    select t).ToList();
        }
        public int ScoutDataCount()
        {
            return (from t in _connection.Table<ScoutData>()
                    select t).ToList().Count;
        }
        public ScoutData CurrentMatch()
        {
            foreach (ScoutData s in _connection.Table<ScoutData>())
            {
                if (s.isCurrentMatch)
                {
                    return s;
                }
            }
            return null;
        }
        public void SetCurrentMatch(string mid)
        {
            foreach (ScoutData s in _connection.Table<ScoutData>())
            {
                ScoutData placeholder = s;

                placeholder.isCurrentMatch = false;
                _connection.Delete<ScoutData>(s.ID);
                _connection.Insert(placeholder);

                if (s.ID == mid)
                {
                    placeholder.isCurrentMatch = true;
                    _connection.Delete<ScoutData>(mid);
                    _connection.Insert(placeholder);
                }
            }
        }
        public void DeleteDataForEvent(int eid)
        {
            List<ScoutData> result = new List<ScoutData>();
            foreach (ScoutData s in _connection.Table<ScoutData>())
            {
                int id = int.Parse(s.ID.Substring(0, (s.ID.IndexOf(","))));
                if (eid == id)
                {
                    _connection.Delete<ScoutData>(s.ID);
                }
            }
        }
        public List<ScoutData> GetDataForEvent(int eid)
        {
            List<ScoutData> result = new List<ScoutData>();
            foreach (ScoutData s in _connection.Table<ScoutData>())
            {
                int id = int.Parse(s.ID.Substring(0, (s.ID.IndexOf(","))));
                if (eid == id)
                {
                    result.Add(s);
                }
            }
            result.Reverse();
            return result;
        }
        public List<string> GetMatchDisplayList(int eid)
        {
            List<string> result = new List<string>();
            foreach (ScoutData s in _connection.Table<ScoutData>())
            {
                result.Add("Match: " + s.matchNumber.ToString() + " (Team: " + s.teamNumber.ToString() + ")");
            }
            result.Reverse();
            return result;
        }


        //Compiled data
        public CompiledScoutData GetCompiledScoutData(int id)
        {
            return _connection.Table<CompiledScoutData>().FirstOrDefault(t => t.cID == id);
        }
        public void DeleteCompiledScoutData(int id)
        {
            _connection.Delete<CompiledScoutData>(id);
        }
        public void AddCompiledScoutData(CompiledScoutData putScoutData)
        {
            _connection.Insert(putScoutData);
        }
        public IEnumerable<CompiledScoutData> GetCompiledScoutData()
        {
            return (from t in _connection.Table<CompiledScoutData>()
                    select t).ToList();
        }
        public int CompiledScoutDataCount()
        {
            return (from t in _connection.Table<CompiledScoutData>()
                    select t).ToList().Count;
        }
        public List<string> GetCompiledList()
        {
            List<string> result = new List<string>();
            foreach (CompiledScoutData e in _connection.Table<CompiledScoutData>())
            {
                result.Add(e.officialName + " (" + e.startDate + " - " + e.endDate + ") ID: " + e.cID + " Date Modified: "+ e.dateMod);
            }
            result.Reverse();
            return result;
        }
        public List<int> CompiledIDList()
        {
            List<int> ids = new List<int>();
            foreach (CompiledScoutData e in _connection.Table<CompiledScoutData>())
            {
                ids.Add(e.cID);
            }
            ids.Reverse();
            return ids;
        }
        public CompiledScoutData CurrentCompiled()
        {
            foreach (CompiledScoutData e in _connection.Table<CompiledScoutData>())
            {
                if (e.isActive)
                {
                    return e;
                }
            }
            return null;
        }
        public void SetCurrentCompiled(int id)
        {
            foreach (CompiledScoutData e in _connection.Table<CompiledScoutData>())
            {
                CompiledScoutData placeholder = e;

                placeholder.isActive = false;
                _connection.Delete<CompiledScoutData>(e.cID);
                _connection.Insert(placeholder);

                if (e.cID == id)
                {
                    placeholder.isActive = true;
                    _connection.Delete<CompiledScoutData>(id);
                    _connection.Insert(placeholder);
                }
            }
        }
    }
}