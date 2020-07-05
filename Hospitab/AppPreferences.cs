using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Preferences;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Hospitab
{
    class AppPreferences
    {
        private ISharedPreferences mSharedPrefs;
        private ISharedPreferencesEditor mPrefsEditor;
        private Context mContext;

        private static string druser = "user";
        private static string drpwd = "1234";
        private static string DrIsLoggedIn = "No";
        private static string aduser = "user";
        private static string adpwd = "1234";
        private static string AdIsLoggedIn = "No";

        public AppPreferences(Context context)
        {
            this.mContext = context;
            mSharedPrefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
            mPrefsEditor = mSharedPrefs.Edit();
        }

        public void saveAccessKey(string user,string pwd,string log)
        {
            mPrefsEditor.PutString(druser, user);
            mPrefsEditor.PutString(drpwd, pwd);
            mPrefsEditor.PutString(DrIsLoggedIn, log);
            mPrefsEditor.Commit();
        }

        public void AdsaveAccessKey(string user, string pwd, string log)
        {
            mPrefsEditor.PutString(aduser, user);
            mPrefsEditor.PutString(adpwd, pwd);
            mPrefsEditor.PutString(AdIsLoggedIn, log);
            mPrefsEditor.Commit();
        }

        public string getDrUser()
        {
            return mSharedPrefs.GetString(druser, "");
        }

        public string getDrPass()
        {
            return mSharedPrefs.GetString(drpwd, "");
        }

        public string getDrisLogIn()
        {
            return mSharedPrefs.GetString(DrIsLoggedIn, "No");
        }

        public string getAdUser()
        {
            return mSharedPrefs.GetString(aduser, "");
        }

        public string getAdPass()
        {
            return mSharedPrefs.GetString(adpwd, "");
        }

        public string getAdisLogIn()
        {
            return mSharedPrefs.GetString(AdIsLoggedIn, "No");
        }
    }
}