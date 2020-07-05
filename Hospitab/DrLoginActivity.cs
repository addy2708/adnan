using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Windows;
using System.Net.Http.Headers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Hospitab;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using System.Threading;

namespace Hospitab
{
    [Activity(Label = "DrLoginActivity", Theme = "@style/NAppTheme")]
    public class DrLoginActivity : AppCompatActivity
    {
        EditText username;
        EditText password;
        Button btnsubmit;
        Button btnSignup;
        Button btnstatus;
        private int progressBarStatus;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.dr_signin);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            username = FindViewById<EditText>(Resource.Id.txtusername);
            password = FindViewById<EditText>(Resource.Id.txtpassword);
            btnsubmit = FindViewById<Button>(Resource.Id.btnLogin);
            btnSignup = FindViewById<Button>(Resource.Id.btnSignup);
            btnstatus = FindViewById<Button>(Resource.Id.btnStatus);
            btnsubmit.Click += Btnsubmit_Click;
            btnSignup.Click += BtnSignup_Click;
            btnstatus.Click += Btnstatus_Click;

        }

        private void Btnstatus_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(DocStatus));
        }

        private void BtnSignup_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(DocSignUp));
        }

        private void Btnsubmit_Click(object sender, EventArgs e)
        {
            //HttpClient client = new HttpClient();
            //var uri = new Uri(string.Format("http://192.168.0.52:84/api/Login?username=" + username.Text + "&password=" + password.Text));
            //HttpResponseMessage response;
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //response = await client.GetAsync(uri);
            //if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            //{
            //    var errorMessage1 = response.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
            //    {
            //    '"'
            //    });
            //    Toast.MakeText(this, errorMessage1, ToastLength.Long).Show();
            //    StartActivity(typeof(PatReg));
            //}
            //else
            //{
            //    var errorMessage1 = response.Content.ReadAsStringAsync().Result.Replace("\\", "").Trim(new char[1]
            //    {
            //    '"'
            //    });
            //    Toast.MakeText(this, errorMessage1, ToastLength.Long).Show();
            //}
            string name = username.Text;
            string pwd = password.Text;
            titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            t2.Timeout = -1;
            t2.LoginCompleted += T2_LoginCompleted;
            t2.LoginAsync(name, pwd);
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

        private void T2_LoginCompleted(object sender, titaniumref.LoginCompletedEventArgs e)
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
                ap.saveAccessKey(username.Text, password.Text, "Yes");
                Globals.docid = e.Result.ndocid;
                calldialog.SetNeutralButton("OK", delegate {
                    StartActivity(typeof(DoctorType));
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
