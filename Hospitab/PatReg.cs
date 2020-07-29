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
using Newtonsoft.Json.Linq;
using Json.Net;
using Android.Provider;
using Android.Graphics;
using Android.Graphics.Drawables;
using System.IO;
using Android.Support.V7.App;
using System.Threading;

namespace Hospitab
{
    [Activity(Label = "@string/app_name", Theme = "@style/NAppTheme")]
    public class PatReg : AppCompatActivity
    {
        AutoCompleteTextView fname;
        EditText lname;
        EditText age;
        EditText nphone;
        EditText naddress;
        EditText regno;
        Spinner cboageflag;
        Spinner cbotitle;
        Button btnnew;
        Button btnold;
        Button btnimg;
        Spinner cbogender;
        ImageView imgpt;
        string etype;
        string[] names;
        string sageflag = "";
        string sgender = "";
        private int progressBarStatus;
        private static readonly Int32 REQUEST_CAMERA = 0;
        private static readonly Int32 SELECT_FILE = 1;
        public static List<Ptdetails> Ptdtlist = new List<Ptdetails>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.regpat);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dtoolbar1);
            SetSupportActionBar(toolbar);
            etype = Intent.GetStringExtra("etype") ?? string.Empty;
            cboageflag = FindViewById<Spinner>(Resource.Id.cmbageflag);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.age_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cboageflag.Adapter = adapter;
            cbotitle = FindViewById<Spinner>(Resource.Id.cmbtitle);
            var adapter3 = ArrayAdapter.CreateFromResource(this, Resource.Array.title_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter3.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cbotitle.Adapter = adapter3;
            cbogender = FindViewById<Spinner>(Resource.Id.cmbgender);
            var adapter2 = ArrayAdapter.CreateFromResource(this, Resource.Array.sex_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cbogender.Adapter = adapter2;
            LoadPtData();
            fname = FindViewById<AutoCompleteTextView>(Resource.Id.txtname);
            lname = FindViewById<EditText>(Resource.Id.txtlname);
            regno = FindViewById<EditText>(Resource.Id.txtregno);
            nphone = FindViewById<EditText>(Resource.Id.txtphone);
            naddress = FindViewById<EditText>(Resource.Id.txtaddr);
            age = FindViewById<EditText>(Resource.Id.txtage);
            btnnew = FindViewById<Button>(Resource.Id.btnnew);
            btnold = FindViewById<Button>(Resource.Id.btnold);
            btnimg = FindViewById<Button>(Resource.Id.btnimg);
            imgpt = FindViewById<ImageView>(Resource.Id.imgpt);
            fname.ItemClick += Name_ItemClick;
            btnnew.Click += Btnnew_Click;
            btnold.Click += Btnold_Click;
            btnimg.Click += Btnimg_Click;
            regno.Enabled = false;
        }

        private void Btnimg_Click(object sender, EventArgs e)
        {
            String[] items = { "Take Photo", "Choose from Library", "Cancel" };

            using (var dialogBuilder = new Android.App.AlertDialog.Builder(this))
            {
                dialogBuilder.SetTitle("Add Photo");
                dialogBuilder.SetItems(items, (d, args) => {
                    //Take photo
                    if (args.Which == 0)
                    {
                        var intent = new Intent(MediaStore.ActionImageCapture);
                        StartActivityForResult(intent, REQUEST_CAMERA);
                    }
                    //Choose from gallery
                    else if (args.Which == 1)
                    {
                        var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                        intent.SetType("image/*");
                        this.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), SELECT_FILE);
                    }
                });

                dialogBuilder.Show();
            }
        }

        private void Btnold_Click(object sender, EventArgs e)
        {
            if (regno.Text == "")
            {
                var calldialog = new Android.App.AlertDialog.Builder(this);
                calldialog.SetTitle("Notify");
                calldialog.SetCancelable(false);
                calldialog.SetMessage("Please select registered patient for old records!!!");
                calldialog.SetNeutralButton("OK", delegate {
                    
                });
                calldialog.Show();
            }
            else
            {
                Globals.regno = regno.Text;
                StartActivity(typeof(OldPtdt));
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                if (requestCode == REQUEST_CAMERA)
                {
                    Bitmap bitmap = (Bitmap)data.Extras.Get("data");
                    imgpt.SetImageBitmap(bitmap);
                }
                else if (requestCode == SELECT_FILE)
                {
                    imgpt.SetImageURI(data.Data);
                }
            }
        }

        private void Btnnew_Click(object sender, EventArgs e)
        {
            if (regno.Text == "")
            {
                if (fname.Text == "" || lname.Text == "" || age.Text == "" || nphone.Text == "" || naddress.Text == "" || nphone.Text.Length < 10)
                {
                    var calldialog = new Android.App.AlertDialog.Builder(this);
                    calldialog.SetTitle("Notify");
                    calldialog.SetCancelable(false);
                    calldialog.SetMessage("Please fill in First/last names,age,phone and address!!!");
                    calldialog.SetNeutralButton("OK", delegate {
                        
                    });
                    calldialog.Show();
                }
                else
                {
                    string First = fname.Text;
                    string Last = lname.Text;
                    int Age = Convert.ToInt32(age.Text);
                    int title = cbotitle.SelectedItemPosition;
                    int ageflag = cboageflag.SelectedItemPosition;
                    int gender = cbogender.SelectedItemPosition;
                    string phone = nphone.Text;
                    string address = naddress.Text;
                    sageflag = cboageflag.GetItemAtPosition(ageflag).ToString();
                    sgender = cbogender.GetItemAtPosition(gender).ToString();
                    BitmapDrawable bd = (BitmapDrawable)imgpt.Drawable;

                    Bitmap bitmap = bd.Bitmap;
                    MemoryStream stream = new MemoryStream();
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 20, stream);
                    byte[] ba = stream.ToArray();
                    string bal = Convert.ToBase64String(ba);
                    titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
                    t2.Timeout = -1;
                    t2.RegisterCompleted += T2_RegisterCompleted;
                    t2.RegisterAsync(title, First, Last, Age, ageflag, gender, phone, address,bal);
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
                
            }
            else
            {
                int ageflag = cboageflag.SelectedItemPosition;
                int gender = cbogender.SelectedItemPosition;
                sageflag = cboageflag.GetItemAtPosition(ageflag).ToString();
                sgender = cbogender.GetItemAtPosition(gender).ToString();
                if (etype == "Dermatology")
                {
                    var activity = new Intent(this, typeof(Skinentry));
                    activity.PutExtra("Name", fname.Text + " " + lname.Text);
                    activity.PutExtra("Age", age.Text + " " + sageflag + " / " + sgender);
                    activity.PutExtra("Regno", regno.Text);
                    activity.PutExtra("etype", etype);
                    StartActivity(activity);
                }
                else if (etype == "Trichology")
                {
                    var activity = new Intent(this, typeof(HairEntry));
                    activity.PutExtra("Name", fname.Text + " " + lname.Text);
                    activity.PutExtra("Age", age.Text + " " + sageflag + " / " + sgender);
                    activity.PutExtra("Regno", regno.Text);
                    activity.PutExtra("etype", etype);
                    StartActivity(activity);
                }
            }
            
        }

        private void T2_RegisterCompleted(object sender, titaniumref.RegisterCompletedEventArgs e)
        {
            Globals.regno = e.Result.pregno;
            var calldialog = new Android.App.AlertDialog.Builder(this);
            calldialog.SetTitle("Notify");
            calldialog.SetCancelable(false);
            calldialog.SetMessage(e.Result.Message);
            calldialog.SetNeutralButton("OK",delegate{
                newact();
            });
            calldialog.Show();
        }

        private void newact()
        {
            int ageflag = cboageflag.SelectedItemPosition;
            int gender = cbogender.SelectedItemPosition;
            sageflag = cboageflag.GetItemAtPosition(ageflag).ToString();
            sgender = cbogender.GetItemAtPosition(gender).ToString();
            if (etype == "Dermatology")
            {
                var activity = new Intent(this, typeof(Skinentry));
                activity.PutExtra("Name", fname.Text + " " + lname.Text);
                activity.PutExtra("Age", age.Text + " " + sageflag + " / " + sgender);
                activity.PutExtra("etype", etype);
                StartActivity(activity);
            }
            else if (etype == "Trichology")
            {
                var activity = new Intent(this, typeof(HairEntry));
                activity.PutExtra("Name", fname.Text + " " + lname.Text);
                activity.PutExtra("Age", age.Text + " " + sageflag + " / " + sgender);
                activity.PutExtra("etype", etype);
                StartActivity(activity);
            }

        }

        private void Name_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var pname = fname.Text;
            int secondIndex = pname.IndexOf('-', pname.IndexOf('-') + 1);
            pname = pname.Substring(secondIndex + 1);
            pname = pname.Trim();
            LoadData(pname);
        }

        private void LoadPtData()
        {
            titaniumdoc.WebServiceDB t2 = new titaniumdoc.WebServiceDB();
            t2.Timeout = -1;
            t2.PatientCompleted += T2_PatientCompleted;
            t2.PatientAsync();
        }

        private void T2_PatientCompleted(object sender, titaniumdoc.PatientCompletedEventArgs e)
        {
            names = e.Result.npatients;
            ArrayAdapter adapter1 = new ArrayAdapter<string>(this, Resource.Layout.list_item, names);
            //adapter1.Filter.InvokeFilter("");
            fname.Threshold = 3;
            fname.Adapter = adapter1;
        }

        private void LoadData(string ptid)
        {
            titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            t2.PtDetailsCompleted += T2_PtDetailsCompleted;
            t2.PtDetailsAsync(ptid);
        }

        private void T2_PtDetailsCompleted(object sender, titaniumref.PtDetailsCompletedEventArgs e)
        {
            fname.Text = e.Result.pfname;
            lname.Text = e.Result.plname;
            age.Text = e.Result.page;
            nphone.Text = e.Result.pphone;
            naddress.Text = e.Result.paddress;
            regno.Text = e.Result.pregno;
            cboageflag.SetSelection(e.Result.ageflag);
            cbogender.SetSelection(e.Result.gender);
            cbotitle.SetSelection(e.Result.title);
            sageflag = cboageflag.GetItemAtPosition(e.Result.ageflag).ToString();
            sgender = cbogender.GetItemAtPosition(e.Result.gender).ToString();
            byte[] decByte3 = System.Convert.FromBase64String(e.Result.pimg);
            Bitmap myIcon = Bytes2Bimap(decByte3);
            imgpt.SetImageBitmap(myIcon);
            Globals.gyear = e.Result.nyear;
            Globals.gmonth = e.Result.nmonth;
            Globals.gday = e.Result.nday;
            Globals.gageflag = e.Result.nflag;
            Globals.regno = e.Result.pregno;
            fname.Enabled = false;
            lname.Enabled = false;
            age.Enabled = false;
            nphone.Enabled = false;
            naddress.Enabled = false;
        }

        public Bitmap Bytes2Bimap(byte[] b)
        {
            if (b.Length != 0)
            {
                return BitmapFactory.DecodeByteArray(b, 0, b.Length);
            }
            else
            {
                return null;
            }
        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(DoctorType));
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
    }
}