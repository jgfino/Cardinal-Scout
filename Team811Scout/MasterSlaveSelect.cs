using System;
using Android.App;
using Android.OS;
using Android.Widget;

namespace Team811Scout
{
    /*this activity simple decides if a device will be sending or receiving during the data transfer process*/
    [Activity(Label = "MasterSlaveSelect")]
    public class MasterSlaveSelect: Activity
    {
        //declare objects for controls
        Button bMaster;
        Button bSlave;
       

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.master_slave_select);

            //get controls from layout and assign event handlers
            bMaster = FindViewById<Button>(Resource.Id.bSetMaster);
            bMaster.Click += ButtonClicked;
            bSlave = FindViewById<Button>(Resource.Id.bSetSlave);
            bSlave.Click += ButtonClicked;             
            
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            //decide which button was clicked and start appropriate activity
            if((sender as Button)==bMaster)
            {
                StartActivity(typeof(MasterView));
            }
            else if((sender as Button)==bSlave)
            {
                StartActivity(typeof(SlaveView));
            }
        }
    }

}