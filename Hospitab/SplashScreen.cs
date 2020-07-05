using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Hospitab
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Splash", MainLauncher = true)]
    //[Activity(Label = "SplashScreen")]
    public class SplashScreen : Activity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //string drloggedin = "";
            //string adloggedin = "";
            base.OnCreate(savedInstanceState);
            Thread.Sleep(4000);
            Finish();
            Context mContext = Android.App.Application.Context;
            AppPreferences ap = new AppPreferences(mContext);
            //drloggedin = ap.getDrisLogIn();
            //adloggedin = ap.getAdisLogIn();
            ap.AdsaveAccessKey("", "", "No");
            ap.saveAccessKey("", "", "No");
            StartActivity(typeof(MainActivity));
            
        }
    }
}