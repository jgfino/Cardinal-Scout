using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;




namespace Team811Scout
{
    [Activity(Label = "MasterSlaveSelect")]
    public class MasterSlaveSelect: Activity
    {

        Button bMaster;
        Button bSlave;
       

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.master_slave_select);
            bMaster = FindViewById<Button>(Resource.Id.bSetMaster);
            bMaster.Click += ButtonClicked;
            bSlave = FindViewById<Button>(Resource.Id.bSetSlave);
            bSlave.Click += ButtonClicked; 
            
            
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
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