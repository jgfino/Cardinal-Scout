using Android.Text;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
namespace Team811Scout
{
    public class EventDatabase
    {
        private SQLiteConnection _connection;


        public EventDatabase()
        {

            _connection = DependencyService.Get<ISQLite>().GetConnection();
            _connection.CreateTable<ScoutData>();
            _connection.CreateTable<Event>();
            _connection.CreateTable<CompiledScoutData>();
            _connection.CreateTable<CompiledTeamIndex>();

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
        public List<SpannableString> GetEventDisplayList()
        {
            SpannableString[] result;
            List<SpannableString> final = new List<SpannableString>();

            foreach (Event e in _connection.Table<Event>())
            {

                result = new SpannableString[]
                {
                    new FormatString("'"+e.eventName+"'").getBold(),
                    new FormatString(" | ").getBold(),
                    new FormatString("("+e.startDate+" - "+e.endDate+")").getNormal(),                    
                    new FormatString(" | ").getBold(),
                    new FormatString("ID: " + e.eventID).getBold(),
                };

                final.Add(new SpannableString(TextUtils.ConcatFormatted(result)));

            }

            final.Reverse();
            return final;
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
                ScoutData placeholder = s;
                string id = s.ID;
                
                if (s.eventID==oldid)
                {
                    placeholder.ID = newid.ToString() + "," + s.ID.Substring(s.ID.IndexOf(",") + 1);
                    placeholder.eventID = newid;
                    _connection.Insert(placeholder);
                    _connection.Delete<ScoutData>(id);
                }            
                
                
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
                
                if (eid == s.eventID)
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
                
                if (s.eventID == eid)
                {
                    result.Add(s);
                }
            }
            result.Reverse();
            return result;
        }
        public List<SpannableString> GetMatchDisplayList(int eid)
        {
            List<SpannableString> result = new List<SpannableString>();
            SpannableString[] disp;

            foreach (ScoutData s in _connection.Table<ScoutData>())
            {                
                if (s.eventID == eid)
                {
                    disp = new SpannableString[]
                    {
                        new FormatString("Match ").getNormal(),
                        new FormatString(s.matchNumber.ToString()).getBold(),
                        new FormatString(" (Team: ").getNormal(),
                        new FormatString(s.teamNumber.ToString()).getBold(),
                        new FormatString(")").getNormal()
                    };

                    result.Add(new SpannableString(TextUtils.ConcatFormatted(disp)));
                }
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
        public List<SpannableString> GetCompiledList()
        {
            SpannableString[] result;
            List<SpannableString> final = new List<SpannableString>();

            foreach (CompiledScoutData e in _connection.Table<CompiledScoutData>())
            {

                result = new SpannableString[]
                {
                    new FormatString("'"+e.officialName+"'").getBold(),
                    new FormatString(" | ").getBold(),                   
                    new FormatString("Date Modified: " + e.dateMod + " at ").getNormal(),
                    new FormatString(e.timeMod).getBold(),
                    new FormatString(" | ").getBold(),
                    new FormatString("ID: " + e.cID).getBold(),
                };

                final.Add(new SpannableString(TextUtils.ConcatFormatted(result)));

            }

            final.Reverse();
            return final;

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


    //index
    public CompiledTeamIndex getTeamIndex()
    {
        foreach (CompiledTeamIndex c in _connection.Table<CompiledTeamIndex>())
        {
            return c;
        }

        return null;
    }

    public void setTeamIndex(CompiledTeamIndex index)
    {
        foreach (CompiledTeamIndex c in _connection.Table<CompiledTeamIndex>())
        {
            _connection.Delete<CompiledTeamIndex>(c.ID);
        }
        _connection.Insert(index);
    }




}
}