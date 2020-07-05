using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public class AdminLogin : AppCompatActivity
    {
        EditText txtauser;
        EditText txtapwd;
        Button btnalogin;
        private int progressBarStatus;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.admin_login);
            txtapwd = FindViewById<EditText>(Resource.Id.txtapassword);
            txtauser = FindViewById<EditText>(Resource.Id.txtausername);
            btnalogin = FindViewById<Button>(Resource.Id.btnaLogin);
            btnalogin.Click += Btnalogin_Click;
        }

        private void Btnalogin_Click(object sender, EventArgs e)
        {
            titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            t2.Timeout = -1;
            t2.ALoginCompleted += T2_ALoginCompleted;
            t2.ALoginAsync(txtauser.Text, txtapwd.Text);
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

        private void T2_ALoginCompleted(object sender, titaniumref.ALoginCompletedEventArgs e)
        {
            var calldialog = new Android.App.AlertDialog.Builder(this);
            calldialog.SetTitle("Notify");
            calldialog.SetCancelable(false);
            calldialog.SetMessage(e.Result.Message);
            if (e.Result.Message == "Username or Password is incorrect")
            {
                calldialog.SetNeutralButton("OK", delegate {

                });
            }
            else
            {
                Context mContext = Android.App.Application.Context;
                AppPreferences ap = new AppPreferences(mContext);
                ap.AdsaveAccessKey(txtauser.Text, txtapwd.Text, "Yes");
                Globals.docid = e.Result.ndocid;
                calldialog.SetNeutralButton("OK", delegate {
                    StartActivity(typeof(AdminType));
                });
            }
            calldialog.Show();
        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(MainActivity));
        }
    }
}