using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using System.Threading;

namespace Hospitab
{
    [Activity(Label = "@string/app_name", Theme = "@style/NAppTheme")]
    public class Opptdt : AppCompatActivity
    {
        ListView LView;
        private int progressBarStatus;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.emr_list);
            LView = FindViewById<ListView>(Resource.Id.emr_listView);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dtoolbar6);
            SetSupportActionBar(toolbar);
            titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            t2.PtListCompleted += T2_PtListCompleted;
            t2.PtListAsync(Globals.fromdate, Globals.todate, Globals.docname);
            ProgressDialog progessBar = new ProgressDialog(this);
            progessBar.SetCancelable(true);
            progessBar.SetMessage("Please Wait......");
            progessBar.SetProgressStyle(ProgressDialogStyle.Horizontal);
            progessBar.Progress = 0;
            progessBar.Max = 100;
            progessBar.Show();
            progressBarStatus = 0;

            new Thread(new ThreadStart(delegate
            {
                while (progressBarStatus < 100)
                {
                    progressBarStatus += 10;
                    progessBar.Progress += progressBarStatus;
                    Thread.Sleep(100);
                }
                RunOnUiThread(() => { progessBar.Hide(); });
            })).Start();
        }
        

        private void T2_PtListCompleted(object sender, titaniumref.PtListCompletedEventArgs e)
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
                    StartActivity(typeof(Ptfilter));
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
            StartActivity(typeof(Ptfilter));
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