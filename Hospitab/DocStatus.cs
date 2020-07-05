using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Hospitab
{
    [Activity(Label = "@string/app_name", Theme = "@style/NAppTheme")]
    public class DocStatus : AppCompatActivity
    {
        EditText txtregno;
        EditText txtphone;
        Button btncheck;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Doctor_status);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dtoolbar9);
            SetSupportActionBar(toolbar);
            txtregno = FindViewById<EditText>(Resource.Id.txtreg);
            txtphone = FindViewById<EditText>(Resource.Id.txtphn);
            btncheck = FindViewById<Button>(Resource.Id.btnCheck);
            btncheck.Click += Btncheck_Click;
        }

        private void Btncheck_Click(object sender, EventArgs e)
        {
            titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            t2.DocStatusCompleted += T2_DocStatusCompleted;
            t2.DocStatusAsync(txtregno.Text, txtphone.Text);
        }

        private void T2_DocStatusCompleted(object sender, titaniumref.DocStatusCompletedEventArgs e)
        {
            var callDialog = new Android.App.AlertDialog.Builder(this);
            callDialog.SetTitle("Notify");
            callDialog.SetMessage(e.Result.Message);
            callDialog.SetNeutralButton("Ok", delegate {
                StartActivity(typeof(MainActivity));
            });
            callDialog.Show();
        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(DrLoginActivity));
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.dr_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.nav_presc)
            {
                StartActivity(typeof(PatReg));
            }
            else if (id == Resource.Id.nav_drptlist)
            {
                StartActivity(typeof(DocPtFilter));
            }
            else if (id == Resource.Id.nav_status)
            {
                StartActivity(typeof(DocStatus));
            }
            else if (id == Resource.Id.nav_log)
            {
                Context mContext = Android.App.Application.Context;
                AppPreferences ap = new AppPreferences(mContext);
                ap.saveAccessKey("", "", "No");
                StartActivity(typeof(MainActivity));
            }
            else if (id == Resource.Id.nav_adchat)
            {
                try
                {
                    String text = "";// Replace with your message.

                    String toNumber = "919324439699"; // Replace with mobile phone number without +Sign or leading zeros, but with country code
                                                      //Suppose your country is India and your phone number is “xxxxxxxxxx”, then you need to send “91xxxxxxxxxx”.


                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetData(Android.Net.Uri.Parse("http://api.whatsapp.com/send?phone=" + toNumber + "&text=" + text));
                    StartActivity(intent);
                }
                catch (Exception ex)
                {
                    string mess = ex.Message;
                }
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}