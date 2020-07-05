using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Hospitab
{
    [Activity(Label = "@string/app_name", Theme = "@style/NAppTheme")]
    public class EyePtDetails : AppCompatActivity
    {
        TextView rsph;
        TextView lsph;
        TextView rcyl;
        TextView lcyl;
        TextView raxis;
        TextView laxis;
        TextView rvis;
        TextView lvis;
        TextView radd;
        TextView ladd;
        TextView pname;
        TextView emrno;
        TextView visitno;
        TextView remarks;
        Button btnphone;
        ImageView imgpresc;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EyeDetails);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dtoolbar5);
            SetSupportActionBar(toolbar);
            rsph = (TextView)FindViewById(Resource.Id.lblrsph);
            lsph = (TextView)FindViewById(Resource.Id.lbllsph);
            rcyl = (TextView)FindViewById(Resource.Id.lblrcyl);
            lcyl = (TextView)FindViewById(Resource.Id.lbllcyl);
            raxis = (TextView)FindViewById(Resource.Id.lblraxis);
            laxis = (TextView)FindViewById(Resource.Id.lbllaxis);
            rvis = (TextView)FindViewById(Resource.Id.lblrvis);
            lvis = (TextView)FindViewById(Resource.Id.lbllvis);
            radd = (TextView)FindViewById(Resource.Id.lblradd);
            ladd = (TextView)FindViewById(Resource.Id.lblladd);
            remarks = (TextView)FindViewById(Resource.Id.txtremarks);
            pname = (TextView)FindViewById(Resource.Id.txtrname);
            emrno = (TextView)FindViewById(Resource.Id.txtremrno);
            visitno = (TextView)FindViewById(Resource.Id.txtrvisit);
            btnphone = (Button)FindViewById(Resource.Id.btnPhone);
            imgpresc = (ImageView)FindViewById(Resource.Id.imgpresc);
            titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            t2.EyeDtCompleted += T2_EyeDtCompleted;
            t2.EyeDtAsync(Globals.gemrno);
            btnphone.Click += Btnphone_Click;
        }

        private void Btnphone_Click(object sender, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse("tel:" + Globals.gphone);
            var intent = new Intent(Intent.ActionDial, uri);
            StartActivity(intent);
        }

        private void T2_EyeDtCompleted(object sender, titaniumref.EyeDtCompletedEventArgs e)
        {
            rsph.Text = e.Result.trsph;
            lsph.Text = e.Result.tlsph;
            rcyl.Text = e.Result.trcyl;
            lcyl.Text = e.Result.tlcyl;
            raxis.Text = e.Result.traxis;
            laxis.Text = e.Result.tlaxis;
            rvis.Text = e.Result.trvis;
            lvis.Text = e.Result.tlvis;
            radd.Text = e.Result.tradd;
            ladd.Text = e.Result.tladd;
            pname.Text = e.Result.pfname;
            emrno.Text = e.Result.emrno;
            visitno.Text = "Visit No. " + e.Result.visit;
            remarks.Text = "Remarks : " + e.Result.remarks;
            byte[] decByte3 = System.Convert.FromBase64String(e.Result.nimage);
            Bitmap myIcon = Bytes2Bimap(decByte3);
            imgpresc.SetImageBitmap(myIcon);
            if (e.Result.pphone == "")
            {
                Globals.gphone = "0";
            }
            else
            {
                Globals.gphone = e.Result.pphone;
            }
        }

        public Bitmap Bytes2Bimap(byte[] b)
        {
            if (b.Length != 0)
            {
                return BitmapFactory.DecodeByteArray(b, 0, b.Length);
            }
            else
            {
                return null;
            }
        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.ad_menu, menu);
        //    return true;
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    int id = item.ItemId;
        //    if (id == Resource.Id.nav_adtype)
        //    {
        //        StartActivity(typeof(AdminType));
        //    }
        //    else if (id == Resource.Id.nav_ptlist)
        //    {
        //        StartActivity(typeof(Ptfilter));
        //    }
        //    else if (id == Resource.Id.nav_docapp)
        //    {
        //        StartActivity(typeof(DocList));
        //    }
        //    else if (id == Resource.Id.nav_adlog)
        //    {
        //        Context mContext = Android.App.Application.Context;
        //        AppPreferences ap = new AppPreferences(mContext);
        //        ap.AdsaveAccessKey("", "", "No");
        //        StartActivity(typeof(MainActivity));
        //    }
        //    return base.OnOptionsItemSelected(item);
        //}
    }
}