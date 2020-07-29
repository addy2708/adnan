using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class AdminType : AppCompatActivity
    {
        Button btndoclist;
        Button btnptlist;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.admin_type);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dtoolbar12);
            SetSupportActionBar(toolbar);
            btndoclist = FindViewById<Button>(Resource.Id.btnDocList);
            btnptlist = FindViewById<Button>(Resource.Id.btnPtList);
            btndoclist.Click += Btndoclist_Click;
            btnptlist.Click += Btnptlist_Click;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ad_menu, menu);
            return true;
        }

        private void Btnptlist_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Ptfilter));
        }

        private void Btndoclist_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(DocList));
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

        public Task<bool> AlertAsync(Context context, string title, string message, string positiveButton, string negativeButton)
        {
            var tcs = new TaskCompletionSource<bool>();

            using (var db = new Android.App.AlertDialog.Builder(context))
            {
                db.SetTitle(title);
                db.SetMessage(message);
                db.SetPositiveButton(positiveButton, (sender, args) => { tcs.TrySetResult(true); });
                db.SetNegativeButton(negativeButton, (sender, args) => { tcs.TrySetResult(false); });
                db.Show();
            }

            return tcs.Task;
        }

        public override void OnBackPressed()
        {
            RunOnUiThread(
            async () =>
            {
                var isCloseApp = await AlertAsync(this, "MEDI-SWASTHA", "Do you want to close this app?", "Yes", "No");

                if (isCloseApp)
                {
                    this.FinishAffinity();
                    Finish();
                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                }
            });
        }
    }
}