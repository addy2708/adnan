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
    public class DocList : AppCompatActivity
    {
        ListView LView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.emr_list);
            LView = FindViewById<ListView>(Resource.Id.emr_listView);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dtoolbar6);
            SetSupportActionBar(toolbar);
            titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            t2.DocApprovalListCompleted += T2_DocApprovalListCompleted;
            t2.DocApprovalListAsync();
        }

        private void T2_DocApprovalListCompleted(object sender, titaniumref.DocApprovalListCompletedEventArgs e)
        {
            List<dTableItem> tb = new List<dTableItem>();
            //string final = e.Result.nlist.Trim().Substring(1, (e.Result.nlist.Length) - 2);
            string s = e.Result.nlist.Replace(@"\", string.Empty);
            List<dTableItem> lc = JsonConvert.DeserializeObject<List<dTableItem>>(s, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            if (lc == null)
            {
                var callDialog = new Android.App.AlertDialog.Builder(this);
                callDialog.SetTitle("Notify");
                callDialog.SetMessage("No Data Present");
                callDialog.SetNeutralButton("Ok", delegate {
                    StartActivity(typeof(AdminType));
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

        public class HomeScreenAdapter : BaseAdapter<dTableItem>
        {
            List<dTableItem> items;
            Activity context;
            public HomeScreenAdapter(Activity context, List<dTableItem> items)
                : base()
            {
                this.context = context;
                this.items = items;
            }
            public override long GetItemId(int position)
            {
                return position;
            }
            public override dTableItem this[int position]
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
                view.FindViewById<TextView>(Resource.Id.txtname).Text = "Dr. " + item.name;
                view.FindViewById<TextView>(Resource.Id.txtemrno).Text = item.regno;
                view.Click += (object sender, EventArgs e) => {
                    Toast.MakeText(parent.Context, "Clicked " + item.name, ToastLength.Long).Show();
                    Globals.dregno = item.regno;
                    parent.Context.StartActivity(typeof(Doctor_Verify));
                };
                return view;

            }


        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(AdminType));
        }
    }
}