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
    public class Ptfilter : AppCompatActivity
    {
        Spinner cmbdoc;
        EditText fdate;
        EditText tdate;
        Button btnfilter;
        string[] names;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.filter_admin);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dtoolbar3);
            SetSupportActionBar(toolbar);
            fdate = FindViewById<EditText>(Resource.Id.txtfrom);
            tdate = FindViewById<EditText>(Resource.Id.txtto);
            fdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            tdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            cmbdoc = FindViewById<Spinner>(Resource.Id.cbodoc);
            btnfilter = FindViewById<Button>(Resource.Id.btnSearch);
            LoadDoc();

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

            btnfilter.Click += Btnfilter_Click;
        }

        private void Btnfilter_Click(object sender, EventArgs e)
        {
            Globals.fromdate = fdate.Text;
            Globals.todate = tdate.Text;
            Globals.docname = cmbdoc.GetItemAtPosition(cmbdoc.SelectedItemPosition).ToString();
            StartActivity(typeof(Opptdt));
        }


        private void LoadDoc()
        {
            titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            t2.DoctorCompleted += T2_DoctorCompleted;
            t2.DoctorAsync();
        }

        private void T2_DoctorCompleted(object sender, titaniumref.DoctorCompletedEventArgs e)
        {
            names = e.Result.ndoctors;
            ArrayAdapter adapter7 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, names);
            adapter7.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cmbdoc.Adapter = adapter7;
        }

        void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            fdate.Text = e.Date.ToString("yyyy-MM-dd");
        }

        void OnDateSetT(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            tdate.Text = e.Date.ToString("yyyy-MM-dd");
        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(AdminType));
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