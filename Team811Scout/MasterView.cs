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
    [Activity(Label = "MasterView")]
    public class MasterView: Activity
    {


        Spinner receiveDataSpinner;
        Button b1p1;
        Button b1p2;
        Button b2p1;
        Button b2p2;
        Button b3p1;
        Button b3p2;
        Button b4p1;
        Button b4p2;
        Button b5p1;
        Button b5p2;
        EventDatabase eData;
        List<SpannableString> eventNames;

        Button bCompile;


     



        protected override void OnCreate(Bundle savedInstanceState)
        {
            eData = new EventDatabase();
            eventNames = new List<SpannableString>();



            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.master_view);

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

            eventNames = eData.GetEventDisplayList();

            ArrayAdapter selectAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, eventNames);
            receiveDataSpinner.Adapter = selectAdapt;

            receiveDataSpinner.ItemSelected += SpinnerClick;

            MobileBarcodeScanner.Initialize(Application);
        }

        Button clickedButton;
        async void ButtonClicked(object sender, EventArgs e)
        {
            clickedButton = sender as Button;
            var scanner = new MobileBarcodeScanner();
            var opt = new MobileBarcodeScanningOptions();
            opt.PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE };

            scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
            scanner.BottomText = "Wait for the barcode to automatically scan!";

            //This will start scanning
            ZXing.Result result = await scanner.Scan();

            //Show the result returned.
            HandleResult(result);

        }

        string concatedQR = null;
        void HandleResult(ZXing.Result result)
        {
            var msg = "Failed, Please try again";


            if (result != null && result.BarcodeFormat == BarcodeFormat.QR_CODE)
            {
                clickedButton.Text = "Success";
                clickedButton.SetBackgroundColor(Color.Rgb(121, 234, 144));
                concatedQR += result.Text;
                msg = result.Text;

            }

            AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog missingDetails = dialog.Create();
            missingDetails.SetTitle("Alert");
            missingDetails.SetMessage(msg);
            missingDetails.SetButton("OK", (c, ev) =>
            {

            });
            missingDetails.Show();

        }

        Event currentEvent;
        void compileData(object sender, EventArgs e)
        {

            try

            {
                bool isDuplicate = false;

                IEnumerable<CompiledScoutData> currentData = eData.GetCompiledScoutData();

                foreach (CompiledScoutData q in currentData)
                {
                    if (q.cID == currentEvent.eventID)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                ScoutData[] scoutListArray = eData.GetDataForEvent(currentEvent.eventID).ToArray();

                for (int i = 0; i < scoutListArray.Length; i++)
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

                if (concatedQR != null)
                {
                    if (!isDuplicate)
                    {
                       
                        CompiledScoutData newCompilation = new CompiledScoutData(currentEvent.eventName, currentEvent.startDate, currentEvent.endDate, concatedQR, false, currentEvent.eventID);
                        eData.AddCompiledScoutData(newCompilation);

                        AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                        AlertDialog missingDetails = dialog.Create();
                        missingDetails.SetTitle("Alert");
                        missingDetails.SetMessage("Successfully generated data for event '" + currentEvent.eventName+"'.");
                        ;
                        missingDetails.SetButton("OK", (c, ev) =>
                        {
                            Finish();                            
                        });
                        missingDetails.Show();

                    }
                    else
                    {
                        AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                        AlertDialog missingDetails = dialog.Create();
                        missingDetails.SetTitle("Alert");
                        missingDetails.SetMessage("Data for this event has already been generated on this device. Please delete it in 'View Data' from the home screen first if you want to generate new data");
                        ;
                        missingDetails.SetButton("OK", (c, ev) =>
                        {

                        });
                        missingDetails.Show();
                        this.Recreate();
                    }
                }
                else
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog missingDetails = dialog.Create();
                    missingDetails.SetTitle("Alert");
                    missingDetails.SetMessage("No data collected, please start over");
                    ;
                    missingDetails.SetButton("OK", (c, ev) =>
                    {

                    });
                    missingDetails.Show();
                    this.Recreate();

                }
            }
            catch
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog missingDetails = dialog.Create();
                missingDetails.SetTitle("Alert");
                missingDetails.SetMessage("Honestly idk man but you managed to generate quite the error. Refreshing this page");
                ;
                missingDetails.SetButton("OK", (c, ev) =>
                {

                });
                missingDetails.Show();
                this.Recreate();
            }

        }

        int spinnerIndex;
        private void SpinnerClick(object sender, ItemSelectedEventArgs e)
        {
            spinnerIndex = e.Position;
            currentEvent = eData.GetEvent(eData.EventIDList()[spinnerIndex]);
        }





    }
}