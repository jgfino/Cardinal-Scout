using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using ZXing;
using ZXing.Common;
using ZXing.Mobile;
using ZXing.QrCode;
using static Android.Widget.AdapterView;

namespace Team811Scout
{
    [Activity(Label = "SlaveView")]
    public class SlaveView: Activity
    {
        ImageView QR1;
        ImageView QR2;
        Button bGenerate;
        Spinner selectEvent;

        string QRdata1;
        string QRdata2;

        Writer writer;
        EventDatabase eData;
        int spinnerIndex;
        Event selectedEvent;

        List<ScoutData> scoutList;
        ScoutData[] scoutListArray;
        ScoutData[] scoutArray1;
        ScoutData[] scoutArray2;


        protected override void OnCreate(Bundle savedInstanceState)
        {

            QRdata1 = "";
            QRdata2 = "";

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.slave_view);
            QR1 = FindViewById<ImageView>(Resource.Id.imgQR1);
            QR2 = FindViewById<ImageView>(Resource.Id.imgQR2);

            bGenerate = FindViewById<Button>(Resource.Id.bGenerateQR);
            bGenerate.Click += ButtonClicked;
            selectEvent = FindViewById<Spinner>(Resource.Id.sendDataChooser);
            eData = new EventDatabase();
            writer = new QRCodeWriter();


            selectEvent = FindViewById<Spinner>(Resource.Id.sendDataChooser);
            selectEvent.ItemSelected += SpinnerClick;

            scoutList = new List<ScoutData>();

            List<string> eventNames = new List<string>();
            eventNames = eData.GetEventDisplayList();

            ArrayAdapter selectAdapt = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, eventNames);
            selectEvent.Adapter = selectAdapt;

            spinnerIndex = -1;



        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            if ((sender as Button) == bGenerate)
            {

                try
                {
                    scoutListArray = eData.GetDataForEvent(selectedEvent.eventID).ToArray();
                    scoutArray1 = new ScoutData[40];
                    scoutArray2 = new ScoutData[40];



                    for (int i = 0; i < scoutListArray.Length; i++)
                    {

                        if (i < Math.Round((double)scoutListArray.Length / 2))
                        {
                            QRdata1 += scoutListArray[i].teamNumber.ToString() + "," +
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
                        else
                        {
                            QRdata2 += scoutListArray[i].teamNumber.ToString() + "," +
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

                    }

                    try
                    {
                        BitMatrix bm1 = writer.encode(QRdata1, BarcodeFormat.QR_CODE, 900, 900);
                        BitmapRenderer bit1 = new BitmapRenderer();
                        QR1.SetImageBitmap(bit1.Render(bm1, BarcodeFormat.QR_CODE, QRdata1));

                        AlertDialog.Builder dialog2 = new AlertDialog.Builder(this);
                        AlertDialog generated = dialog2.Create();
                        generated.SetTitle("Alert");
                        generated.SetMessage("Data for Event: '" + selectedEvent.eventName + "' Event ID: " + selectedEvent.eventID.ToString() + " Generated. Please make sure that the receiving device has the correct event selected and that the event ids match");
                        generated.SetButton("OK", (c, ev) =>
                        {

                        });
                        generated.Show();
                    }
                    catch
                    {
                        AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                        AlertDialog missingDetails = dialog.Create();
                        missingDetails.SetTitle("Alert");
                        missingDetails.SetMessage("No data for this match!");
                        missingDetails.SetButton("OK", (c, ev) =>
                        {

                        });
                        missingDetails.Show();
                    }

                    try
                    {
                        BitMatrix bm2 = writer.encode(QRdata2, BarcodeFormat.QR_CODE, 900, 900);
                        BitmapRenderer bit2 = new BitmapRenderer();
                        QR2.SetImageBitmap(bit2.Render(bm2, BarcodeFormat.QR_CODE, QRdata2));
                    }
                    catch
                    {

                    }

                   

                }
                catch
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog missingDetails = dialog.Create();
                    missingDetails.SetTitle("Alert");
                    missingDetails.SetMessage("Please select an event to generate data for");
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

            selectedEvent = eData.GetEvent(eData.EventIDList()[spinnerIndex]);
            eData.SetCurrentEvent(selectedEvent.eventID);

            QR1.SetImageBitmap(null);
            QR2.SetImageBitmap(null);

            AlertDialog.Builder dialog2 = new AlertDialog.Builder(this);
            AlertDialog generated = dialog2.Create();
            generated.SetTitle("Alert");
            generated.SetMessage("Selected event Changed. Please click generate again to update the data");
            generated.SetButton("OK", (c, ev) =>
            {

            });
            generated.Show();


        }
    }
}