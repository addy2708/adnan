using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Hospitab
{
    [Activity(Label = "@string/app_name", Theme = "@style/NAppTheme")]
    public class Doctor_Verify : AppCompatActivity
    {
        TextView txtdname;
        TextView txtddegree;
        TextView txtdregno;
        TextView txtdphone;
        TextView txtdemail;
        TextView txtdgender;
        TextView txtdcentre;
        ImageView imgdoc;
        ImageView imgdeg;
        ImageView imgproof;
        Button btndphone;
        Button btndapprove;
        Button btnddiscard;
        string docregno = "";
        EditText docremarks;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DocDetails);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dtoolbar10);
            SetSupportActionBar(toolbar);
            txtdname = FindViewById<TextView>(Resource.Id.txtdname);
            txtddegree = FindViewById<TextView>(Resource.Id.txtddegree);
            txtdregno = FindViewById<TextView>(Resource.Id.txtdregno);
            docremarks = FindViewById<EditText>(Resource.Id.txtdocremarks);
            txtdemail = FindViewById<TextView>(Resource.Id.txtdemail);
            txtdphone = FindViewById<TextView>(Resource.Id.txtdphone);
            txtdgender = FindViewById<TextView>(Resource.Id.txtdgender);
            txtdcentre = FindViewById<TextView>(Resource.Id.txtdcentre);
            imgdeg = FindViewById<ImageView>(Resource.Id.imgddeg);
            imgdoc = FindViewById<ImageView>(Resource.Id.imgddoc);
            imgproof = FindViewById<ImageView>(Resource.Id.imgdproof);
            btndphone = FindViewById<Button>(Resource.Id.btndPhone);
            btndapprove = FindViewById<Button>(Resource.Id.btndApprove);
            btnddiscard = FindViewById<Button>(Resource.Id.btndDiscard);
            titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            t2.DocDtCompleted += T2_DocDtCompleted;
            t2.DocDtAsync(Globals.dregno);
            btndphone.Click += Btndphone_Click;
            btndapprove.Click += Btndapprove_Click;
            btnddiscard.Click += Btnddiscard_Click;
        }

        private void Btnddiscard_Click(object sender, EventArgs e)
        {
            if (docremarks.Text == "")
            {
                var calldialog = new Android.App.AlertDialog.Builder(this);
                calldialog.SetTitle("Notify");
                calldialog.SetCancelable(false);
                calldialog.SetMessage("Please fill in the remarks for disapproval!!!");
                calldialog.SetNeutralButton("OK", delegate {

                });
                calldialog.Show();
            }
            else
            {
                titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
                t2.Timeout = -1;
                t2.DocDiscardCompleted += T2_DocDiscardCompleted;
                t2.DocDiscardAsync(docregno, docremarks.Text);
            }
        }

        private void T2_DocDiscardCompleted(object sender, titaniumref.DocDiscardCompletedEventArgs e)
        {
            var callDialog = new Android.App.AlertDialog.Builder(this);
            callDialog.SetTitle("Notify");
            callDialog.SetMessage(e.Result.Message);
            callDialog.SetNeutralButton("Ok", delegate {
                StartActivity(typeof(AdminType));
            });
            callDialog.Show();
        }

        private void Btndapprove_Click(object sender, EventArgs e)
        {
            titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            t2.Timeout = -1;
            t2.DocApproveCompleted += T2_DocApproveCompleted;
            t2.DocApproveAsync(docregno);
        }

        private void T2_DocApproveCompleted(object sender, titaniumref.DocApproveCompletedEventArgs e)
        {
            var callDialog = new Android.App.AlertDialog.Builder(this);
            callDialog.SetTitle("Notify");
            callDialog.SetMessage(e.Result.Message);
            callDialog.SetNeutralButton("Ok", delegate {
                StartActivity(typeof(AdminType));
            });
            callDialog.Show();
        }

        private void Btndphone_Click(object sender, EventArgs e)
        {
            var uri = Android.Net.Uri.Parse("tel:" + Globals.gphone);
            var intent = new Intent(Intent.ActionDial, uri);
            StartActivity(intent);
        }

        private void T2_DocDtCompleted(object sender, titaniumref.DocDtCompletedEventArgs e)
        {
            docregno = e.Result.dregno;
            txtdname.Text = "Name: " + e.Result.dname;
            txtddegree.Text = "Degree: " + e.Result.ddegree;
            txtdemail.Text = "Email: " + e.Result.demail;
            txtdphone.Text = "Phone: " + e.Result.dphone;
            txtdregno.Text = "Regno: " + e.Result.dregno;
            txtdcentre.Text = "Centre: " + e.Result.dcentre;
            txtdgender.Text = "Gender: " + e.Result.dgender;
            byte[] decByte = System.Convert.FromBase64String(e.Result.idoc);
            Bitmap myIcon = Bytes2Bimap(decByte);
            imgdoc.SetImageBitmap(myIcon);
            byte[] decByte1 = System.Convert.FromBase64String(e.Result.ideg);
            Bitmap myIcon1 = Bytes2Bimap(decByte1);
            imgdeg.SetImageBitmap(myIcon1);
            byte[] decByte2 = System.Convert.FromBase64String(e.Result.iproof);
            Bitmap myIcon2 = Bytes2Bimap(decByte2);
            imgproof.SetImageBitmap(myIcon2);
            if (e.Result.dphone == "")
            {
                Globals.gphone = "0";
            }
            else
            {
                Globals.gphone = e.Result.dphone;
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ad_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.nav_adtype)
            {
                StartActivity(typeof(AdminType));
            }
            else if (id == Resource.Id.nav_ptlist)
            {
                StartActivity(typeof(Ptfilter));
            }
            else if (id == Resource.Id.nav_docapp)
            {
                StartActivity(typeof(DocList));
            }
            else if (id == Resource.Id.nav_adlog)
            {
                Context mContext = Android.App.Application.Context;
                AppPreferences ap = new AppPreferences(mContext);
                ap.AdsaveAccessKey("", "", "No");
                StartActivity(typeof(MainActivity));
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}