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
using Android.Text;
using Android.Text.Style;

namespace Team811Scout
{
    class FormatString
    {
        private string _input;
        private SpannableString _result;

        public FormatString(string input)
        {
            _input = input;
        }

        public SpannableString getBold()
        {
            _result = new SpannableString(_input);
            _result.SetSpan(new StyleSpan(Android.Graphics.TypefaceStyle.Bold),0,_input.Length,0);
            return _result;
        }

        public SpannableString setColor(int r, int g, int b)
        {
            _result = new SpannableString(_input);
            _result.SetSpan(new ForegroundColorSpan(Android.Graphics.Color.Rgb(r,g,b)), 0, _input.Length, 0);
            return _result;
        }

        public SpannableString setColorBold(int r, int g, int b)
        {
            _result = new SpannableString(_input);
            _result.SetSpan(new ForegroundColorSpan(Android.Graphics.Color.Rgb(r, g, b)), 0, _input.Length, 0);
            _result.SetSpan(new StyleSpan(Android.Graphics.TypefaceStyle.Bold), 0, _input.Length, 0);
            return _result;
        }

        public SpannableString getNormal()
        {
            _result = new SpannableString(_input);            
            return _result;
        }

    }
}