using Android.App;
using Android.OS;
using Android.Widget;

using System;
using System.IO;

namespace Team811Scout
{
    [Activity(Label = "InstructSettings", Theme = "@style/AppTheme", MainLauncher = false)]
    public class Settings : Activity
    {
        Button bDelete;
        Button bEditEvent;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.instructions_settings);

            bDelete = FindViewById<Button>(Resource.Id.bDelete);
            bDelete.Click += ButtonClicked;
            bEditEvent = FindViewById<Button>(Resource.Id.bEditEvent);
            bEditEvent.Click += ButtonClicked;


        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            if ((sender as Button) == bDelete)
            {
                var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "scoutdb.db3");

                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog confirmDelete = dialog.Create();
                confirmDelete.SetTitle("WARNING");
                confirmDelete.SetMessage("Are you SURE you want to delete the ENTIRE database (all events, matches, files, etc?");

                confirmDelete.SetButton("Yes", (c, ev) =>
                {
                    this.DeleteDatabase(path);
                    StartActivity(typeof(MainActivity));
                });
                confirmDelete.SetButton2("CANCEL", (c, ev) => { });
                confirmDelete.Show();
            }
            else if ((sender as Button) == bEditEvent)
            {
                StartActivity(typeof(EditEvents));
            }
        }
    }
}