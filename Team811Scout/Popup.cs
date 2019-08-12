using Android.App;
using Android.Content;
using System;

namespace Team811Scout
{
    /*this is a custom class to make using AlertDialogs easier*/

    public static class Popup
    {
        //dialog with one button
        public static void Single(string title, string message, string button, Context context)
        {
            AlertDialog.Builder dialog = new AlertDialog.Builder(context);
            AlertDialog popup = dialog.Create();
            popup.SetTitle(title);
            popup.SetMessage(message);
            popup.SetButton(button, (c, ev) =>
            {
            });
            popup.Show();
        }

        //dialog with yes/no
        public static void Double(string title, string message, string button1, string button2, Context context, Action ifYes)
        {
            AlertDialog.Builder dialog = new AlertDialog.Builder(context);
            AlertDialog popup = dialog.Create();
            popup.SetTitle(title);
            popup.SetMessage(message);
            popup.SetButton(button1, (c, ev) =>
            {
                ifYes();
            });
            popup.SetButton2(button2, (c, ev) => { });
            popup.Show();
        }
    }
}