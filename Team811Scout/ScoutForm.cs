using Android.App;
using Android.OS;
using Android.Widget;
using System;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    /*this activity creates a new ScoutData based off of information inputted by users while scouting a match*/
    [Activity(Label = "ScoutForm", Theme = "@style/AppTheme", MainLauncher = false)]
    class ScoutForm: Activity
    {
        //declare objects that wil refer to controls        
        
        TextView textTitle;
        EditText vMatchNumber;
        EditText vTeamNumber;
        Spinner position;
        CheckBox table;
        RadioGroup radioLevel;
        RadioGroup radioSandstorm;
        CheckBox sandCargo;
        CheckBox sandHatch;
        CheckBox sandHab;
        CheckBox cargo;
        CheckBox cargoWell;
        CheckBox cargoBarely;
        CheckBox hatch;
        CheckBox hatchWell;
        CheckBox hatchBarely;
        RadioGroup radioClimb;
        RadioGroup radioDrivers;
        RadioGroup radioRecommend;
        RadioGroup radioResult;
        MultiAutoCompleteTextView comments;
        Button bFinish;

        int sandstormMode;        
        int sandLevel;
        int climb = 0;    
        bool goodDrivers;      
        int wouldRecommend;       
        int result;       

        //placeholder for new ScoutData and current event
        Event currentEvent;
        ScoutData scoutData;

        //get database instance
        EventDatabase eData = new EventDatabase();  

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.scout_form);          

            //get controls from layout and assign event handlers
            textTitle = FindViewById<TextView>(Resource.Id.textEventTitle);
            vMatchNumber = FindViewById<EditText>(Resource.Id.vMatchNumber);
            vTeamNumber = FindViewById<EditText>(Resource.Id.vTeamNumber);
            position = FindViewById<Spinner>(Resource.Id.vPosition);
            position.ItemSelected+=SpinnerClick;
            table = FindViewById<CheckBox>(Resource.Id.table);
            radioLevel = FindViewById<RadioGroup>(Resource.Id.radioLevel);
            radioLevel.CheckedChange += RadioClicked;
            radioSandstorm = FindViewById<RadioGroup>(Resource.Id.radioSandstorm);
            radioSandstorm.CheckedChange += RadioClicked;
            sandCargo = FindViewById<CheckBox>(Resource.Id.sandCargo);
            sandHatch = FindViewById<CheckBox>(Resource.Id.sandHatch);
            sandHab = FindViewById<CheckBox>(Resource.Id.sandHab);
            cargo = FindViewById<CheckBox>(Resource.Id.cargo);
            cargoWell = FindViewById<CheckBox>(Resource.Id.cargoWell);
            cargoBarely = FindViewById<CheckBox>(Resource.Id.cargoBarely);
            hatch = FindViewById<CheckBox>(Resource.Id.hatch);
            hatchWell = FindViewById<CheckBox>(Resource.Id.hatchWell);
            hatchBarely = FindViewById<CheckBox>(Resource.Id.hatchBarely);
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
            textTitle.Text += currentEvent.eventName+"'";

        }       

        private void ButtonClicked(object sender, EventArgs e)
        {            
            //decide which button was clicked 
            if ((sender as Button) == bFinish)
            {
                try
                {
                    string scoutID = currentEvent.eventID.ToString() + ","+ vMatchNumber.Text;                    

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
                    false,
                    currentEvent.eventID);

                    //add the new match
                    eData.AddScoutData(scoutData);

                    //go back to the main scouting page
                    StartActivity(typeof(ScoutLandingPage));
                    Finish();
                }

                //not putting a match or team number will throw an exception; so will a duplicate match number since the ScoutData id
                //is based off of match number
                catch
                {
                    Popup.Single("Alert", "Please Enter At Least the Match and Team Number//Possible Duplicate Match Number", "OK", this);
                }
            }
        }

        //get driverstation position
        int spinnerIndex;
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