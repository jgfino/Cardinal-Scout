using Android.App;
using Android.OS;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    [Activity(Label = "ScoutForm", Theme = "@style/AppTheme", MainLauncher = false)]
    class ScoutForm: Activity
    {
        EventDatabase eData;
        Event currentEvent;
        TextView textTitle;
        EditText vMatchNumber;
        EditText vTeamNumber;
        Spinner position;

        RadioGroup radioSandstorm;
        int sandstormMode;

        RadioGroup radioLevel;
        int sandLevel;

        CheckBox sandCargo;
        CheckBox sandHatch;
        CheckBox sandHab;

        CheckBox cargo;
        CheckBox cargoWell;
        CheckBox cargoBarely;
        CheckBox hatch;
        CheckBox hatchWell;
        CheckBox hatchBarely;

        int climb = 0;
        RadioGroup radioClimb;

        CheckBox table;

        RadioGroup radioDrivers;
        bool goodDrivers;

        RadioGroup radioRecommend;
        int wouldRecommend;

        RadioGroup radioResult;
        int result;

        MultiAutoCompleteTextView comments;
        Button bFinish;

        ScoutData scoutData;

        int spinnerIndex;
        string spinnerItem;


        


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.scout_form);
            eData = new EventDatabase();
           

            textTitle = FindViewById<TextView>(Resource.Id.textEventTitle);
            vMatchNumber = FindViewById<EditText>(Resource.Id.vMatchNumber);
            vTeamNumber = FindViewById<EditText>(Resource.Id.vTeamNumber);
            position = FindViewById<Spinner>(Resource.Id.vPosition);
            position.ItemSelected+=SpinnerClick;

            string[] positions = new string[] { "Red 1", "Red 2", "Red 3", "Blue 1", "Blue 2", "Blue 3" };
            ArrayAdapter posAdapt = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, positions);
            position.Adapter = posAdapt;
        

            radioSandstorm = FindViewById<RadioGroup>(Resource.Id.radioSandstorm);
            radioSandstorm.CheckedChange += RadioClicked;
            sandCargo = FindViewById<CheckBox>(Resource.Id.sandCargo);
            sandHatch = FindViewById<CheckBox>(Resource.Id.sandHatch);
            sandHab = FindViewById<CheckBox>(Resource.Id.sandHab);
            radioLevel = FindViewById<RadioGroup>(Resource.Id.radioLevel);
            radioLevel.CheckedChange += RadioClicked;

            cargo = FindViewById<CheckBox>(Resource.Id.cargo);
            cargoWell = FindViewById<CheckBox>(Resource.Id.cargoWell);
            cargoBarely = FindViewById<CheckBox>(Resource.Id.cargoBarely);
            hatch = FindViewById<CheckBox>(Resource.Id.hatch);
            hatchWell = FindViewById<CheckBox>(Resource.Id.hatchWell);
            hatchBarely = FindViewById<CheckBox>(Resource.Id.hatchBarely);

            radioClimb = FindViewById<RadioGroup>(Resource.Id.radioClimb);
            radioClimb.CheckedChange += RadioClicked;
            table = FindViewById<CheckBox>(Resource.Id.table);
            radioDrivers = FindViewById<RadioGroup>(Resource.Id.radioDrivers);
            radioDrivers.CheckedChange += RadioClicked;
            radioRecommend = FindViewById<RadioGroup>(Resource.Id.radioRecommend);
            radioRecommend.CheckedChange += RadioClicked;
            radioResult = FindViewById<RadioGroup>(Resource.Id.radioResult);
            radioResult.CheckedChange += RadioClicked;
            comments = FindViewById<MultiAutoCompleteTextView>(Resource.Id.comments);
            bFinish = FindViewById<Button>(Resource.Id.bFinish);
            bFinish.Click += ButtonClicked;

            //get current event
            currentEvent = eData.CurrentEvent();
            textTitle.Text += currentEvent.eventName+"'";

        }

        int TeamNum;
        int MatchNum;

        private void ButtonClicked(object sender, EventArgs e)
        {
            

            if ((sender as Button) == bFinish)
            {
                try
                {
                    string scoutID = currentEvent.eventID.ToString() + ","+ vMatchNumber.Text;

                    TeamNum = int.Parse(vTeamNumber.Text);
                    MatchNum = int.Parse(vMatchNumber.Text);

                    scoutData = new ScoutData(
                    scoutID,
                    currentEvent.eventName,
                    currentEvent.startDate,
                    currentEvent.endDate,
                    int.Parse(vMatchNumber.Text),
                    int.Parse(vTeamNumber.Text),
                    spinnerIndex,
                    table.Checked,
                    sandstormMode,
                    sandHatch.Checked,
                    sandCargo.Checked,
                    sandHab.Checked,
                    sandLevel,
                    cargo.Checked,
                    cargoWell.Checked,
                    cargoBarely.Checked,
                    hatch.Checked,
                    hatchWell.Checked,
                    hatchBarely.Checked,
                    climb,
                    goodDrivers,
                    wouldRecommend,
                    result,
                    comments.Text,
                    false);

                    eData.AddScoutData(scoutData);

                    StartActivity(typeof(ScoutLandingPage));
                    Finish();
                }
                catch
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog missingDetails = dialog.Create();
                    missingDetails.SetTitle("Alert");
                    missingDetails.SetMessage("Please Enter At Least the Match and Team Number//Possible Duplicate Match Number");
                    missingDetails.SetButton("OK", (c, ev) =>
                    {

                    });
                    missingDetails.Show();
                }

                

            }
        }

        private void SpinnerClick(object sender, ItemSelectedEventArgs e)
        {
            spinnerIndex = e.Position;            
        }

        

        //handle radio buttons
        private void RadioClicked(object sender, EventArgs e)
        {
            int placeHolder;
            RadioGroup selectedGroup = sender as RadioGroup;
            placeHolder = selectedGroup.CheckedRadioButtonId;
            RadioButton clickedButton = FindViewById<RadioButton>(placeHolder);
            clickedButton.Checked = true;

            int childIndex = selectedGroup.IndexOfChild(clickedButton);

            if (selectedGroup == radioSandstorm)
            {
                sandstormMode = selectedGroup.IndexOfChild(clickedButton);
            }
            else if (selectedGroup == radioLevel)
            {
                sandLevel = childIndex + 1;
            }
            else if (selectedGroup == radioDrivers)
            {
                if (childIndex == 0)
                {
                    goodDrivers = true;
                }
                else
                {
                    goodDrivers = false;
                }
            }
            else if(selectedGroup==radioClimb)
            {
                if(childIndex==0)
                {
                    climb = 2;
                }
                else if (childIndex==1)
                {
                    climb = 3;
                }
                else
                {
                    climb = 0;
                }
            }
            else if (selectedGroup == radioRecommend)
            {
                wouldRecommend = childIndex;
            }
            else if (selectedGroup == radioResult)
            {
                result = childIndex;
            }

        }
    }
}