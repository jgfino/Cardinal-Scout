using Android.Text;
using Android.Text.Style;

namespace Team811Scout
{
    /*this is a basic custom string formatting class used to set text style/color for display*/

    public static class FormatString
    {
        public static SpannableString setBold(string input)
        {
            SpannableString result = new SpannableString(input);
            result.SetSpan(new StyleSpan(Android.Graphics.TypefaceStyle.Bold),0,input.Length,0);
            return result;
        }

        public static SpannableString setColor(string input, int r, int g, int b)
        {
            SpannableString result = new SpannableString(input);
            result.SetSpan(new ForegroundColorSpan(Android.Graphics.Color.Rgb(r,g,b)), 0, input.Length, 0);
            return result;
        }

        public static SpannableString setColorBold(string input, int r, int g, int b)
        {
            SpannableString result = new SpannableString(input);
            result.SetSpan(new ForegroundColorSpan(Android.Graphics.Color.Rgb(r, g, b)), 0, input.Length, 0);
            result.SetSpan(new StyleSpan(Android.Graphics.TypefaceStyle.Bold), 0, input.Length, 0);
            return result;
        }

        public static SpannableString setNormal(string input)
        {
            SpannableString result = new SpannableString(input);            
            return result;
        }        

    }
}