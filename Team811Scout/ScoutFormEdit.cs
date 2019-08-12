using Android.App;
using Android.OS;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    /*this activity creates a new MatchData based off of information inputted by users while scouting a match*/

    [Activity(Label = "ScoutFormEdit", Theme = "@style/AppTheme", MainLauncher = false)]
    internal class ScoutFormEdit: Activity
    {
        //declare objects that wil refer to controls
        private TextView textTitle;

        private EditText vMatchNumber;
        private EditText vTeamNumber;
        private Spinner position;
        private CheckBox table;
        private RadioGroup radioLevel;
        private RadioGroup radioSandstorm;
        private CheckBox sandCargo;
        private CheckBox sandHatch;
        private CheckBox sandHab;
        private CheckBox cargo;
        private CheckBox cargoWell;
        private CheckBox cargoBarely;
        private CheckBox hatch;
        private CheckBox hatchWell;
        private CheckBox hatchBarely;
        private RadioGroup radioClimb;
        private RadioGroup radioDrivers;
        private RadioGroup radioRecommend;
        private RadioGroup radioResult;
        private MultiAutoCompleteTextView comments;
        private Button bFinish;
        private int sandstormMode = 2;
        private int sandLevel = 1;
        private int climb = 0;
        private bool goodDrivers = false;
        private int wouldRecommend = 1;
        private int result = 1;

        //placeholder for new MatchData and current event
        private Event currentEvent;

        private MatchData currentMatch;
        private MatchData matchData;

        //get database instance
        private EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Scout_Form_Edit);
            //get controls from layout and assign event handlers
            textTitle = FindViewById<TextView>(Resource.Id.textEventTitle);
            vMatchNumber = FindViewById<EditText>(Resource.Id.vMatchNumber);
            vTeamNumber = FindViewById<EditText>(Resource.Id.vTeamNumber);
            position = FindViewById<Spinner>(Resource.Id.vPosition);
            position.ItemSelected += SpinnerClick;
            table = FindViewById<CheckBox>(Resource.Id.table);
            radioLevel = FindViewById<RadioGroup>(Resource.Id.radioLevel);
            radioLevel.CheckedChange += RadioClicked;
            radioSandstorm = FindViewById<RadioGroup>(Resource.Id.radioSandstorm);
            radioSandstorm.CheckedChange += RadioClicked;
            sandCargo = FindViewById<CheckBox>(Resource.Id.sandCargo);
            sandHatch = FindViewById<CheckBox>(Resource.Id.sandHatch);
            sandHab = FindViewById<CheckBox>(Resource.Id.sandHab);
            cargo = FindViewById<CheckBox>(Resource.Id.cargo);
            cargo.CheckedChange += CheckboxClicked;
            cargoWell = FindViewById<CheckBox>(Resource.Id.cargoWell);
            cargoWell.CheckedChange += CheckboxClicked;
            cargoBarely = FindViewById<CheckBox>(Resource.Id.cargoBarely);
            cargoBarely.CheckedChange += CheckboxClicked;
            hatch = FindViewById<CheckBox>(Resource.Id.hatch);
            hatch.CheckedChange += CheckboxClicked;
            hatchWell = FindViewById<CheckBox>(Resource.Id.hatchWell);
            hatchWell.CheckedChange += CheckboxClicked;
            hatchBarely = FindViewById<CheckBox>(Resource.Id.hatchBarely);
            hatchBarely.CheckedChange += CheckboxClicked;
            radioClimb = FindViewById<RadioGroup>(Resource.Id.radioClimb);
            radioClimb.CheckedChange += RadioClicked;
            radioDrivers = FindViewById<RadioGroup>(Resource.Id.radioDrivers);
            radioDrivers.CheckedChange += RadioClicked;
            radioRecommend = FindViewById<RadioGroup>(Resource.Id.radioRecommend);
            radioRecommend.CheckedChange += RadioClicked;
            radioResult = FindViewById<RadioGroup>(Resource.Id.radioResult);
            radioResult.CheckedChange += RadioClicked;
            comments = FindViewById<MultiAutoCompleteTextView>(Resource.Id.comments);
            bFinish = FindViewById<Button>(Resource.Id.bFinish);
            bFinish.Click += ButtonClicked;
            //put positions into dropdown
            string[] positions = new string[] { "Red 1", "Red 2", "Red 3", "Blue 1", "Blue 2", "Blue 3" };
            ArrayAdapter posAdapt = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, positions);
            position.Adapter = posAdapt;
            //get current event and set title text
            currentEvent = eData.GetCurrentEvent();
            currentMatch = eData.GetCurrentMatch();
            textTitle.Text += currentMatch.matchNumber.ToString() + "'";
            //set options to match properties
            vMatchNumber.Text = currentMatch.matchNumber.ToString();
            vTeamNumber.Text = currentMatch.teamNumber.ToString();
            position.SetSelection(currentMatch.position);
            table.Checked = currentMatch.isTable;
            ((RadioButton)radioLevel.GetChildAt(currentMatch.sandstormStartLevel - 1)).Checked = true;
            ((RadioButton)radioSandstorm.GetChildAt(currentMatch.sandstormMode)).Checked = true;
            sandCargo.Checked = currentMatch.sandstormCargo;
            sandHatch.Checked = currentMatch.sandstormHatch;
            sandHab.Checked = currentMatch.sandstormLine;
            cargo.Checked = currentMatch.cargo;
            cargoWell.Checked = currentMatch.cargoWell;
            cargoBarely.Checked = currentMatch.cargoBarely;
            hatch.Checked = currentMatch.hatch;
            hatchWell.Checked = currentMatch.hatchWell;
            hatchBarely.Checked = currentMatch.hatchBarely;
            try
            {
                ((RadioButton)radioClimb.GetChildAt(currentMatch.climb - 2)).Checked = true;
            }
            catch
            {
                ((RadioButton)radioClimb.GetChildAt(2)).Checked = true;
            }
            ((RadioButton)radioDrivers.GetChildAt(Convert.ToInt16(!currentMatch.goodDrivers))).Checked = true;
            ((RadioButton)radioRecommend.GetChildAt(currentMatch.wouldRecommend)).Checked = true;
            ((RadioButton)radioResult.GetChildAt(currentMatch.result)).Checked = true;
            comments.Text = currentMatch.additionalComments;
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            //decide which button was clicked
            if ((sender as Button) == bFinish)
            {
                try
                {
                    matchData = new MatchData(
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
                    true, //keep it current
                    currentEvent.eventID);
                    if (vMatchNumber.Text != currentMatch.matchNumber.ToString())
                    {
                        //modify if changing match number, check for duplicate first
                        eData.AddMatchData(matchData);
                        eData.DeleteMatchData(currentMatch.ID);
                    }
                    else
                    {
                        //modify the match (without changing ID/match number)
                        eData.DeleteMatchData(currentMatch.ID);
                        eData.AddMatchData(matchData);
                    }
                    //go back to the main scouting page
                    StartActivity(typeof(ViewMatchData));
                    Finish();
                }
                //not putting a match or team number will throw an exception; so will a duplicate match number since the MatchData id
                //is based off of match number
                catch
                {
                    Popup.Single("Alert", "Please Enter At Least the Match and Team Number//Possible Duplicate Match Number", "OK", this);
                }
            }
        }

        //make changes to match
        //get driverstation position
        private int spinnerIndex;

        private void SpinnerClick(object sender, ItemSelectedEventArgs e)
        {
            spinnerIndex = e.Position;
        }

        //handle radio buttons
        private void RadioClicked(object sender, EventArgs e)
        {
            //which group of radio buttons was modified
            RadioGroup selectedGroup = sender as RadioGroup;
            //index of which button was clicked in the group
            int placeHolder;
            placeHolder = selectedGroup.CheckedRadioButtonId;
            RadioButton clickedButton = FindViewById<RadioButton>(placeHolder);
            //check the clicked button
            clickedButton.Checked = true;
            int childIndex = selectedGroup.IndexOfChild(clickedButton);
            //device which group was modified
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
            else if (selectedGroup == radioClimb)
            {
                if (childIndex == 0)
                {
                    climb = 2;
                }
                else if (childIndex == 1)
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

        private void CheckboxClicked(object sender, EventArgs e)
        {
            CheckBox clicked = sender as CheckBox;
            if (clicked == hatchBarely || clicked == hatchWell && clicked.Checked)
            {
                hatch.Checked = true;
            }
            else if (clicked == cargoBarely || clicked == cargoWell && clicked.Checked)
            {
                cargo.Checked = true;
            }
            else if (clicked == cargo && !clicked.Checked)
            {
                cargoBarely.Checked = false;
                cargoWell.Checked = false;
                clicked.Checked = false;
            }
            else if (clicked == hatch && !clicked.Checked)
            {
                hatchBarely.Checked = false;
                hatchWell.Checked = false;
                clicked.Checked = false;
            }
        }
    }
}