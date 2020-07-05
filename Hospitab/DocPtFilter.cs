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
    public class DocPtFilter : AppCompatActivity
    {
        EditText fdate;
        EditText tdate;
        Button btnfilter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Doc_ptlist);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dtoolbar11);
            SetSupportActionBar(toolbar);
            btnfilter = FindViewById<Button>(Resource.Id.btndSearch);
            fdate = FindViewById<EditText>(Resource.Id.txtdfrom);
            tdate = FindViewById<EditText>(Resource.Id.txtdto);
            fdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            tdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            btnfilter.Click += Btnfilter_Click;
            fdate.Click += (sender, e) =>
            {
                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(this, OnDateSet, today.Year, today.Month - 1, today.Day);
                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();


            };

            tdate.Click += (sender, e) =>
            {
                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(this, OnDateSetT, today.Year, today.Month - 1, today.Day);
                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();


            };
        }

        private void Btnfilter_Click(object sender, EventArgs e)
        {
            Globals.fromdate = fdate.Text;
            Globals.todate = tdate.Text;
            StartActivity(typeof(Docptdt));
        }

        void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            fdate.Text = e.Date.ToString("yyyy-MM-dd");
        }

        void OnDateSetT(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            tdate.Text = e.Date.ToString("yyyy-MM-dd");
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

        public override void OnBackPressed()
        {
            StartActivity(typeof(DoctorType));
        }
    }
}