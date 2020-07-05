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
using Newtonsoft.Json;

namespace Hospitab
{
    [Activity(Label = "@string/app_name", Theme = "@style/NAppTheme")]
    public class Docptdt : AppCompatActivity
    {
        ListView LView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.emr_list);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dtoolbar6);
            SetSupportActionBar(toolbar);
            LView = FindViewById<ListView>(Resource.Id.emr_listView);
            titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            t2.DocPtListCompleted += T2_DocPtListCompleted;
            t2.DocPtListAsync(Globals.fromdate, Globals.todate, Globals.docid);
        }

        private void T2_DocPtListCompleted(object sender, titaniumref.DocPtListCompletedEventArgs e)
        {
            List<TableItem> tb = new List<TableItem>();
            //string final = e.Result.nlist.Trim().Substring(1, (e.Result.nlist.Length) - 2);
            string s = e.Result.nlist.Replace(@"\", string.Empty);
            List<TableItem> lc = JsonConvert.DeserializeObject<List<TableItem>>(s, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            if (lc == null)
            {
                var callDialog = new Android.App.AlertDialog.Builder(this);
                callDialog.SetTitle("Notify");
                callDialog.SetMessage("No Data Present");
                callDialog.SetNeutralButton("Ok", delegate {
                    StartActivity(typeof(DocPtFilter));
                });
                callDialog.Show();
            }
            else
            {
                foreach (var data in lc)
                {
                    tb.Add(data);
                }
                if (tb.Count != 0)
                {
                    LView.Adapter = new HomeScreenAdapter(this, tb);
                }
            }
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

        public class HomeScreenAdapter : BaseAdapter<TableItem>
        {
            List<TableItem> items;
            Activity context;
            public HomeScreenAdapter(Activity context, List<TableItem> items)
                : base()
            {
                this.context = context;
                this.items = items;
            }
            public override long GetItemId(int position)
            {
                return position;
            }
            public override TableItem this[int position]
            {
                get { return items[position]; }
            }
            public override int Count
            {
                get { return items.Count; }
            }
            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var item = items[position];
                View view = convertView;
                if (view == null) // no view to re-use, create new
                    view = context.LayoutInflater.Inflate(Resource.Layout.Ptdesign, null);
                view.FindViewById<TextView>(Resource.Id.txtname).Text = item.name;
                view.FindViewById<TextView>(Resource.Id.txtemrno).Text = item.emrno;
                view.Click += (object sender, EventArgs e) => {
                    Toast.MakeText(parent.Context, "Clicked " + item.name, ToastLength.Long).Show();
                    Globals.gemrno = item.emrno;
                    parent.Context.StartActivity(typeof(EyePtDetails));
                };
                return view;

            }


        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(DocPtFilter));
        }
    }
}