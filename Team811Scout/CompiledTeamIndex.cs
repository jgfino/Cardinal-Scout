using SQLite;

namespace Team811Scout
{
    /*This class stores the index of a current teams in a CompiledEventData multidimensional list*/

    public class CompiledTeamIndex
    {
        public CompiledTeamIndex()
        {
        }

        public CompiledTeamIndex(int id)
        {
            ID = id;
        }

        //set key for SQL
        [PrimaryKey]
        public int ID { get; set; }
    }
}