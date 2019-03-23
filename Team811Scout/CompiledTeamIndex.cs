using SQLite;
namespace Team811Scout
{
    /*This class stores the index of the current team in a CompiledScoutData multidimensional list*/

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