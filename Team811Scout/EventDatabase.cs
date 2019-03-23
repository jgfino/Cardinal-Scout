using Android.Text;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
namespace Team811Scout
{

    /*This class contains the SQLite database used by the app to store data. Each type of data has its own
     * "table" in the databse. In each table, each instance of a type has its properties stored in "columns"
     * Each type of data has a "PrimaryKey" which is how it can be put in/taken out of the database*/

    public class EventDatabase
    {
        //get a connection
        private SQLiteConnection _connection = DependencyService.Get<ISQLite>().GetConnection();

        public EventDatabase()
        {
            //create tables for types of data that will be in the database
            _connection.CreateTable<ScoutData>();
            _connection.CreateTable<Event>();
            _connection.CreateTable<CompiledScoutData>();
            _connection.CreateTable<CompiledTeamIndex>();

        }

        //Database for Events
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
        
        //get a formatted list of events and properties that can be put in a ListView/DropDown
        public List<SpannableString> GetEventDisplayList()
        {
            SpannableString[] result;
            List<SpannableString> final = new List<SpannableString>();

            foreach (Event e in _connection.Table<Event>())
            {

                result = new SpannableString[]
                {
                    FormatString.setBold("'"+e.eventName+"'"),
                    FormatString.setBold(" | "),
                    FormatString.setNormal("("+e.startDate+" - "+e.endDate+")"),                    
                    FormatString.setBold(" | "),
                    FormatString.setBold("ID: " + e.eventID),
                };

                final.Add(new SpannableString(TextUtils.ConcatFormatted(result)));
            }

            //reverse so that newest event is first
            final.Reverse();
            return final;
        }
        public Event GetCurrentEvent()
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
                //set all other events to not current
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
        //get a list of event ids in order (used for relating to indices in ListView/DropDowns
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
            //change id for event
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
            //change id for matches associated with the event
            foreach (ScoutData s in _connection.Table<ScoutData>())
            {
                ScoutData placeholder = s;
                string id = s.ID;
                
                if (s.eventID==oldid)
                {
                    //ScoutData IDs are strings (eventID,matchNumber)
                    placeholder.ID = newid.ToString() + "," + s.ID.Substring(s.ID.IndexOf(",") + 1);
                    placeholder.eventID = newid;
                    _connection.Insert(placeholder);
                    _connection.Delete<ScoutData>(id);
                }               
                
            }
        }

        //Database for ScoutData
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
        public ScoutData GetCurrentMatch()
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
                //set all to false first
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
        //delete matches associated with an event
        public void DeleteScoutDataForEvent(int eid)
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
        //get matches associated with an event
        public List<ScoutData> GetScoutDataForEvent(int eid)
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
        //get formatted list of matches for use in ListView, etc
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
                        FormatString.setNormal("Match "),
                        FormatString.setBold(s.matchNumber.ToString()),
                        FormatString.setNormal(" (Team: "),
                        FormatString.setBold(s.teamNumber.ToString()),
                        FormatString.setNormal(")"),
                    };

                    result.Add(new SpannableString(TextUtils.ConcatFormatted(disp)));
                }
            }
            //put newest match first
            result.Reverse();
            return result;
        }

        //Database for Compiled Scout Data
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
        //get a list of all current compiled scout data
        public IEnumerable<CompiledScoutData> GetCompiledScoutData()
        {
            return (from t in _connection.Table<CompiledScoutData>()
                    select t).ToList();
        }
        public List<SpannableString> GetCompiledDisplayList()
        {
            SpannableString[] result;
            List<SpannableString> final = new List<SpannableString>();

            foreach (CompiledScoutData e in _connection.Table<CompiledScoutData>())
            {

                result = new SpannableString[]
                {
                    FormatString.setBold("'"+e.officialName+"'"),
                    FormatString.setBold(" | "),                   
                    FormatString.setNormal("Date Modified: " + e.dateMod + " at "),
                    FormatString.setBold(e.timeMod),
                    FormatString.setBold(" | "),
                    FormatString.setBold("ID: " + e.cID),
                };

                final.Add(new SpannableString(TextUtils.ConcatFormatted(result)));

            }

            //newest first
            final.Reverse();
            return final;

        }

        public List<CompiledScoutData> GetCompiledScoutDataForIndex(int index)
        {
           CompiledScoutData placeholder = new CompiledScoutData();
           List<CompiledScoutData> result = new List<CompiledScoutData>();
           foreach(CompiledScoutData c in _connection.Table<CompiledScoutData>())
            {
                if(c.isActive)
                {
                    placeholder = c;
                }
            }

            foreach (CompiledScoutData c in placeholder.compileData()[index])
            {
                result.Add(c);
            }

            result.Reverse();
            return result;
        }

        //get a list of ids for current compiled scout data entries (for use with indexing in ListView, etc)
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
    public CompiledScoutData GetCurrentCompiled()
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
            //set to false first
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

    /*Compiled Scout Data index - workaround for SQLite foreign keys; the index of the current
    *team in a compiled scout data.*/
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