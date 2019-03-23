using Android.Graphics;

namespace Team811Scout
{

    /*contains constants for calculations of the compiled data. Thresholds are the percents which determine between yes/no or true/false
     * Differences are the point at which two values will be considered "the same." For example, if a robot can do cargo 70% of the time and
     * hatches 85% of the time, the difference will be ignored and the robot will display as being able to do both. These values can only be
     * changed here*/

    public static class Constants
    {
        //ViewData.cs
        public static readonly int recommendThresh = 75;
        public static readonly int winThresh = 75;
        public static readonly int climb3Thresh = 50;
        public static readonly int climb2Thresh = 50;
        public static readonly int driversThresh = 40;
        public static readonly int tableTresh = 33;

        //TeamDetails.cs
        public static readonly int hatch_cargoDiff = 15;
        public static readonly int hatch_cargoThresh = 75;
        public static readonly int sandstorm_modeDiff = 10;
        public static readonly int sandstormThresh = 50;

        //colors
        public static readonly Color appGreen = Color.Rgb(0, 137, 9);
        public static readonly Color appRed = Color.Rgb(255, 0, 0);
    }
}