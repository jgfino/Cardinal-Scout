using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;
using ZXing;
using ZXing.Mobile;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    /*This activity assigns the current device as "master" and collects data from the other 5 devices by scanning their generated QR codes
     * After gathering the data, it creates a new instance of the CompiledEventData class which contains the raw text from the QR codes*/

    [Activity(Label = "MasterView", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MasterView: Activity
    {
        //declare objects for controls
        private Spinner receiveDataSpinner;

        private Button b1p1;
        private Button b1p2;
        private Button b2p1;
        private Button b2p2;
        private Button b3p1;
        private Button b3p2;
        private Button b4p1;
        private Button b4p2;
        private Button b5p1;
        private Button b5p2;
        private Button bCompile;

        //get database instance
        private EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Master_View);
            //get controls and assign event handlers
            b1p1 = FindViewById<Button>(Resource.Id.b1p1);
            b1p1.Click += ButtonClicked;
            b1p2 = FindViewById<Button>(Resource.Id.b1p2);
            b1p2.Click += ButtonClicked;
            b2p1 = FindViewById<Button>(Resource.Id.b2p1);
            b2p1.Click += ButtonClicked;
            b2p2 = FindViewById<Button>(Resource.Id.b2p2);
            b2p2.Click += ButtonClicked;
            b3p1 = FindViewById<Button>(Resource.Id.b3p1);
            b3p1.Click += ButtonClicked;
            b3p2 = FindViewById<Button>(Resource.Id.b3p2);
            b3p2.Click += ButtonClicked;
            b4p1 = FindViewById<Button>(Resource.Id.b4p1);
            b4p1.Click += ButtonClicked;
            b4p2 = FindViewById<Button>(Resource.Id.b4p2);
            b4p2.Click += ButtonClicked;
            b5p1 = FindViewById<Button>(Resource.Id.b5p1);
            b5p1.Click += ButtonClicked;
            b5p2 = FindViewById<Button>(Resource.Id.b5p2);
            b5p2.Click += ButtonClicked;
            bCompile = FindViewById<Button>(Resource.Id.bCompile2);
            bCompile.Click += compileData;
            receiveDataSpinner = FindViewById<Spinner>(Resource.Id.receiveDataChooser);
            receiveDataSpinner.ItemSelected += SpinnerClick;
            //create an adapter for the DropDown picker with event names to choose from
            ArrayAdapter selectAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, eData.GetEventDisplayList());
            receiveDataSpinner.Adapter = selectAdapt;
            //initialize QR scanner class
            MobileBarcodeScanner.Initialize(Application);
        }

        //placeholder button
        private Button clickedButton;

        private async void ButtonClicked(object sender, EventArgs e)
        {
            //remember which button was pressed
            clickedButton = sender as Button;
            var scanner = new MobileBarcodeScanner();
            var opt = new MobileBarcodeScanningOptions();
            //make sure the scanner only looks for QR codes
            opt.PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE };
            scanner.TopText = "Hold the camera up to the QR Code";
            scanner.BottomText = "Wait for the QR Code to automatically scan!";
            //This will start scanning
            ZXing.Result result = await scanner.Scan();
            //Show the result returned.
            HandleResult(result);
        }

        //create a string to store collected values
        private string concatedQR = null;

        private void HandleResult(ZXing.Result result)
        {
            //default message
            var msg = "Failed, Please try again";
            //make sure the scanner detected a valid QR code
            if (result != null && result.BarcodeFormat == BarcodeFormat.QR_CODE)
            {
                clickedButton.Text = "Success";
                clickedButton.SetBackgroundColor(Color.Rgb(121, 234, 144));
                //add result to the combined result string
                concatedQR += result.Text;
                msg = result.Text;
            }
            //debug qr
            //Popup.Single("Alert", msg, "OK", this);
        }

        //create a new CompiledEventData
        private void compileData(object sender, EventArgs e)
        {
            try
            {
                bool isDuplicate = false;
                //get a list of already existing CompiledEventData items
                IEnumerable<CompiledEventData> currentData = eData.GetCompiledEventData();
                foreach (CompiledEventData q in currentData)
                {
                    if (q.cID == selectedEvent.eventID)
                    {
                        isDuplicate = true;
                        break;
                    }
                }
                //add data from the master device
                List<MatchData> scoutListArray = eData.GetMatchDataForEvent(selectedEvent.eventID);
                for (int i = 0; i < scoutListArray.Count; i++)
                {
                    concatedQR += scoutListArray[i].teamNumber.ToString() + "," +
                      scoutListArray[i].matchNumber.ToString() + "," +
                      scoutListArray[i].result.ToString() +
                      scoutListArray[i].position.ToString() +
                      Convert.ToByte(scoutListArray[i].isTable).ToString() +
                      scoutListArray[i].sandstormStartLevel.ToString() +
                      scoutListArray[i].sandstormMode.ToString() +
                      Convert.ToByte(scoutListArray[i].sandstormHatch).ToString() +
                      Convert.ToByte(scoutListArray[i].sandstormCargo).ToString() +
                      Convert.ToByte(scoutListArray[i].sandstormLine).ToString() +
                      Convert.ToByte(scoutListArray[i].cargo).ToString() +
                      Convert.ToByte(scoutListArray[i].cargoWell).ToString() +
                      Convert.ToByte(scoutListArray[i].cargoBarely).ToString() +
                      Convert.ToByte(scoutListArray[i].hatch).ToString() +
                      Convert.ToByte(scoutListArray[i].hatchWell).ToString() +
                      Convert.ToByte(scoutListArray[i].hatchBarely).ToString() +
                      scoutListArray[i].climb.ToString() +
                      Convert.ToByte(scoutListArray[i].goodDrivers).ToString() +
                      scoutListArray[i].wouldRecommend.ToString();
                }
                //make sure there is some data
                if (concatedQR != null)
                {
                    //make sure it isnt a duplicate
                    if (!isDuplicate)
                    {
                        //create a new compiled event data and add it to the database
                        CompiledEventData newCompilation = new CompiledEventData(selectedEvent.eventName, selectedEvent.startDate, selectedEvent.endDate, concatedQR, false, selectedEvent.eventID);
                        eData.AddCompiledEventData(newCompilation);
                        Popup.Single("Alert", "Successfully generated data for event '" + selectedEvent.eventName + "'.", "OK", this);
                        //StartActivity(typeof(MainActivity));
                        //Finish();
                    }
                    //if it is a duplicate
                    else
                    {
                        Popup.Single("Alert", "Data for this event has already been generated on this device. " +
                            "Please delete it in 'View Data' from the home screen first if you want to generate new data", "OK", this);
                        //reset, clear QR data, etc
                        //this.Recreate();
                        concatedQR = null;
                    }
                }
                //if the QR data is completely blank
                else
                {
                    Popup.Single("Alert", "No data collected, please start over", "OK", this);
                    //reset
                    //this.Recreate();
                    concatedQR = null;
                }
            }
            catch
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog missingDetails = dialog.Create();
                missingDetails.SetTitle("Alert");
                missingDetails.SetMessage("No event selected");
                ;
                missingDetails.SetButton("OK", (c, ev) =>
                {
                });
                missingDetails.Show();
                
            }
        }

        //determine which event was selected and put it in the current event placeholder
        private int spinnerIndex;

        private Event selectedEvent;

        private void SpinnerClick(object sender, ItemSelectedEventArgs e)
        {
            spinnerIndex = e.Position;
            selectedEvent = eData.GetEvent(eData.EventIDList()[spinnerIndex]);
        }
    }
}