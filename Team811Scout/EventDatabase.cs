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
            _connection.CreateTable<MatchData>();
            _connection.CreateTable<Event>();
            _connection.CreateTable<CompiledEventData>();
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
            //reverse so that biggest event ID (newest event) is first
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
            foreach (MatchData s in _connection.Table<MatchData>())
            {
                MatchData placeholder = s;
                string id = s.ID;
                if (s.eventID == oldid)
                {
                    //MatchData IDs are strings (eventID,matchNumber)
                    placeholder.ID = newid.ToString() + "," + s.ID.Substring(s.ID.IndexOf(",") + 1);
                    placeholder.eventID = newid;
                    _connection.Insert(placeholder);
                    _connection.Delete<MatchData>(id);
                }
            }
        }

        //Database for Matches
        public MatchData GetMatchData(string id)
        {
            return _connection.Table<MatchData>().FirstOrDefault(t => t.ID == id);
        }

        public void DeleteMatchData(string id)
        {
            _connection.Delete<MatchData>(id);
        }

        public void AddMatchData(MatchData putMatchData)
        {
            _connection.Insert(putMatchData);
        }

        public MatchData GetCurrentMatch()
        {
            foreach (MatchData s in _connection.Table<MatchData>())
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
            foreach (MatchData s in _connection.Table<MatchData>())
            {
                MatchData placeholder = s;
                //set all to false first
                placeholder.isCurrentMatch = false;
                _connection.Delete<MatchData>(s.ID);
                _connection.Insert(placeholder);
                if (s.ID == mid)
                {
                    placeholder.isCurrentMatch = true;
                    _connection.Delete<MatchData>(mid);
                    _connection.Insert(placeholder);
                }
            }
        }

        //delete matches associated with an event
        public void DeleteMatchDataForEvent(int eid)
        {
            List<MatchData> result = new List<MatchData>();
            foreach (MatchData s in _connection.Table<MatchData>())
            {
                if (eid == s.eventID)
                {
                    _connection.Delete<MatchData>(s.ID);
                }
            }
        }

        //get matches associated with an event
        public List<MatchData> GetMatchDataForEvent(int eid)
        {
            List<MatchData> result = new List<MatchData>();
            foreach (MatchData s in _connection.Table<MatchData>())
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
            foreach (MatchData s in _connection.Table<MatchData>())
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

        //Database for Compiled event data
        public CompiledEventData GetCompiledEventData(int id)
        {
            return _connection.Table<CompiledEventData>().FirstOrDefault(t => t.cID == id);
        }

        public void DeleteCompiledEventData(int id)
        {
            _connection.Delete<CompiledEventData>(id);
        }

        public void AddCompiledEventData(CompiledEventData putEventData)
        {
            _connection.Insert(putEventData);
        }

        //get a list of all current compiled event data
        public IEnumerable<CompiledEventData> GetCompiledEventData()
        {
            return (from t in _connection.Table<CompiledEventData>()
                    select t).ToList();
        }

        public List<SpannableString> GetCompiledDisplayList()
        {
            SpannableString[] result;
            List<SpannableString> final = new List<SpannableString>();
            foreach (CompiledEventData e in _connection.Table<CompiledEventData>())
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

        //get compiled data for a single team based off of index in multi dimensional CompiledEventData list for an event
        public List<CompiledEventData> GetCompiledEventDataForIndex(int index)
        {
            CompiledEventData placeholder = new CompiledEventData();
            List<CompiledEventData> result = new List<CompiledEventData>();
            foreach (CompiledEventData c in _connection.Table<CompiledEventData>())
            {
                if (c.isActive)
                {
                    placeholder = c;
                }
            }
            foreach (CompiledEventData c in placeholder.compileData()[index])
            {
                result.Add(c);
            }
            result.Reverse();
            return result;
        }

        //get a list of ids for current compiled event data entries (for use with indexing in ListView, etc)
        public List<int> CompiledIDList()
        {
            List<int> ids = new List<int>();
            foreach (CompiledEventData e in _connection.Table<CompiledEventData>())
            {
                ids.Add(e.cID);
            }
            ids.Reverse();
            return ids;
        }

        public CompiledEventData GetCurrentCompiled()
        {
            foreach (CompiledEventData e in _connection.Table<CompiledEventData>())
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
            foreach (CompiledEventData e in _connection.Table<CompiledEventData>())
            {
                CompiledEventData placeholder = e;
                //set to false first
                placeholder.isActive = false;
                _connection.Delete<CompiledEventData>(e.cID);
                _connection.Insert(placeholder);
                if (e.cID == id)
                {
                    placeholder.isActive = true;
                    _connection.Delete<CompiledEventData>(id);
                    _connection.Insert(placeholder);
                }
            }
        }

        /*Compiled event data index - workaround for SQLite foreign keys; the index of the current
        *team in a compiled event data.*/

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