using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace Team811Scout
{
    public class CompiledTeamIndex
    {
        public CompiledTeamIndex()
        {

        }

        public CompiledTeamIndex(int id)
        {
            ID = id;
        }

        [PrimaryKey]
        public int ID { get; set; }
    }
}