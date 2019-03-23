using Android.App;
using Android.OS;
using Android.Text;
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
    /*This activity assigns the current device as a slave and generates the QR codes needed
     * to send data to the master device. It splits the QR code into 2 equal parts to make reading
     * it easier for low-quality cameras*/

    [Activity(Label = "SlaveView")]
    public class SlaveView: Activity
    {
        //declare objects for controls
        ImageView QR1;
        ImageView QR2;
        Button bGenerate;
        Spinner selectEvent;        

        //get databse instance
        EventDatabase eData = new EventDatabase();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.slave_view);

            //get controls from layout and assign event handlers
            QR1 = FindViewById<ImageView>(Resource.Id.imgQR1);
            QR2 = FindViewById<ImageView>(Resource.Id.imgQR2);
            bGenerate = FindViewById<Button>(Resource.Id.bGenerateQR);
            bGenerate.Click += ButtonClicked;
            selectEvent = FindViewById<Spinner>(Resource.Id.sendDataChooser);
            selectEvent.ItemSelected += SpinnerClick;                   

            //put events in the dropdown
            ArrayAdapter selectAdapt = new ArrayAdapter<SpannableString>(this, Android.Resource.Layout.SimpleListItem1, eData.GetEventDisplayList());
            selectEvent.Adapter = selectAdapt;

        }
        
        private void ButtonClicked(object sender, EventArgs e)
        {
            if ((sender as Button) == bGenerate)
            {
                try
                {
                    string QRdata1 = null;
                    string QRdata2 = null;

                    //get the scoutdata for the selected event
                    List<ScoutData> scoutList = eData.GetScoutDataForEvent(selectedEvent.eventID);
                    
                    for (int i = 0; i < scoutList.Count; i++)
                    {
                        //split data into two
                        if (i < Math.Round((double)scoutList.Count / 2))
                        {
                            QRdata1 += scoutList[i].teamNumber.ToString() + "," +
                              scoutList[i].matchNumber.ToString() + "," +
                              scoutList[i].result.ToString() +
                              scoutList[i].position.ToString() +
                              Convert.ToByte(scoutList[i].isTable).ToString() +
                              scoutList[i].sandstormStartLevel.ToString() +
                              scoutList[i].sandstormMode.ToString() +
                              Convert.ToByte(scoutList[i].sandstormHatch).ToString() +
                              Convert.ToByte(scoutList[i].sandstormCargo).ToString() +
                              Convert.ToByte(scoutList[i].sandstormLine).ToString() +
                              Convert.ToByte(scoutList[i].cargo).ToString() +
                              Convert.ToByte(scoutList[i].cargoWell).ToString() +
                              Convert.ToByte(scoutList[i].cargoBarely).ToString() +
                              Convert.ToByte(scoutList[i].hatch).ToString() +
                              Convert.ToByte(scoutList[i].hatchWell).ToString() +
                              Convert.ToByte(scoutList[i].hatchBarely).ToString() +
                              scoutList[i].climb.ToString() +
                              Convert.ToByte(scoutList[i].goodDrivers).ToString() +
                              scoutList[i].wouldRecommend.ToString();
                        }
                        else
                        {
                            QRdata2 += scoutList[i].teamNumber.ToString() + "," +
                              scoutList[i].matchNumber.ToString() + "," +
                              scoutList[i].result.ToString() +
                              scoutList[i].position.ToString() +
                              Convert.ToByte(scoutList[i].isTable).ToString() +
                              scoutList[i].sandstormStartLevel.ToString() +
                              scoutList[i].sandstormMode.ToString() +
                              Convert.ToByte(scoutList[i].sandstormHatch).ToString() +
                              Convert.ToByte(scoutList[i].sandstormCargo).ToString() +
                              Convert.ToByte(scoutList[i].sandstormLine).ToString() +
                              Convert.ToByte(scoutList[i].cargo).ToString() +
                              Convert.ToByte(scoutList[i].cargoWell).ToString() +
                              Convert.ToByte(scoutList[i].cargoBarely).ToString() +
                              Convert.ToByte(scoutList[i].hatch).ToString() +
                              Convert.ToByte(scoutList[i].hatchWell).ToString() +
                              Convert.ToByte(scoutList[i].hatchBarely).ToString() +
                              scoutList[i].climb.ToString() +
                              Convert.ToByte(scoutList[i].goodDrivers).ToString() +
                              scoutList[i].wouldRecommend.ToString();
                        }

                    }

                    //get QR code writer
                    Writer writer = new QRCodeWriter();

                    //create the QR codes
                    try
                    {                        
                        BitMatrix bm1 = writer.encode(QRdata1, BarcodeFormat.QR_CODE, 900, 900);
                        BitmapRenderer bit1 = new BitmapRenderer();
                        QR1.SetImageBitmap(bit1.Render(bm1, BarcodeFormat.QR_CODE, QRdata1));

                        Popup.Single("Alert", "Data for Event: '" + selectedEvent.eventName + "' Event ID: " +
                            selectedEvent.eventID.ToString() + " Generated. Please make sure that the receiving " +
                            "device has the correct event selected and that the event ids match", "OK", this);                       
                    }

                    //exception thrown if QR data strings are empty
                    catch
                    {
                        Popup.Single("Alert", "No data for this match", "OK", this);
                    }

                    //try to generate a second QR code
                    try
                    {
                        BitMatrix bm2 = writer.encode(QRdata2, BarcodeFormat.QR_CODE, 900, 900);
                        BitmapRenderer bit2 = new BitmapRenderer();
                        QR2.SetImageBitmap(bit2.Render(bm2, BarcodeFormat.QR_CODE, QRdata2));
                    }

                    //do nothing if there is not enough data for a second QR code
                    catch
                    {

                    }                  

                }

                //if no event is selected, it throws an exception
                catch
                {
                    Popup.Single("Alert", "Please select an event to generate data for", "OK", this);                    
                }

            }

        }

        //placeholders for dropdown index and selected event
        int spinnerIndex;
        Event selectedEvent;
        private void SpinnerClick(object sender, ItemSelectedEventArgs e)
        {
            spinnerIndex = e.Position;
            selectedEvent = eData.GetEvent(eData.EventIDList()[spinnerIndex]);            

            //clear QR codes from previous events
            QR1.SetImageBitmap(null);
            QR2.SetImageBitmap(null);

            Popup.Single("Alert", "Selected event Changed. Please click generate again to update the data", "OK", this);

        }
    }
}