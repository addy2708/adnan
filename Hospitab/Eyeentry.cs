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
using System.IO;
using Android.Graphics.Drawables;
using Android.Support.V7.App;
using System.Threading;

namespace Hospitab
{
    [Activity(Label = "@string/app_name", Theme = "@style/NAppTheme")]
    public class Eyeentry : AppCompatActivity
    {
        Spinner cborsph;
        Spinner cbolsph;
        Spinner cborcyl;
        Spinner cbolcyl;
        Spinner cboraxis;
        Spinner cbolaxis;
        Spinner cborvis;
        Spinner cbolvis;
        Spinner cboradd;
        Spinner cboladd;
        TextView Name;
        TextView Age;
        Button BtnSave;
        Button btnImg;
        ImageView imgpower;
        AutoCompleteTextView NCentre;
        EditText NRemarks;
        string[] names;
        private int progressBarStatus;
        private static readonly Int32 REQUEST_CAMERA = 0;
        private static readonly Int32 SELECT_FILE = 1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EyeEntry);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dtoolbar4);
            SetSupportActionBar(toolbar);
            Name = (TextView)FindViewById(Resource.Id.lblname);
            string pname = Intent.GetStringExtra("Name") ?? string.Empty;
            Name.Text = pname;
            Age = (TextView)FindViewById(Resource.Id.lblage);
            string page = Intent.GetStringExtra("Age") ?? string.Empty;
            Age.Text = page;
            cborsph = (Spinner)FindViewById(Resource.Id.cborsph);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.power_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cborsph.Adapter = adapter;
            cbolsph = FindViewById<Spinner>(Resource.Id.cbolsph);
            var adapter1 = ArrayAdapter.CreateFromResource(this, Resource.Array.power_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cbolsph.Adapter = adapter1;
            cborcyl = (Spinner)FindViewById(Resource.Id.cborcyl);
            var adapter2 = ArrayAdapter.CreateFromResource(this, Resource.Array.power_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cborcyl.Adapter = adapter2;
            cbolcyl = FindViewById<Spinner>(Resource.Id.cbolcyl);
            var adapter3 = ArrayAdapter.CreateFromResource(this, Resource.Array.power_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter3.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cbolcyl.Adapter = adapter3;
            cboraxis = (Spinner)FindViewById(Resource.Id.cboraxis);
            var adapter4 = ArrayAdapter.CreateFromResource(this, Resource.Array.axis_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter4.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cboraxis.Adapter = adapter4;
            cbolaxis = FindViewById<Spinner>(Resource.Id.cbolaxis);
            var adapter5 = ArrayAdapter.CreateFromResource(this, Resource.Array.axis_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter5.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cbolaxis.Adapter = adapter5;
            cborvis = (Spinner)FindViewById(Resource.Id.cborvis);
            var adapter6 = ArrayAdapter.CreateFromResource(this, Resource.Array.vision_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter6.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cborvis.Adapter = adapter6;
            cbolvis = FindViewById<Spinner>(Resource.Id.cbolvis);
            var adapter7 = ArrayAdapter.CreateFromResource(this, Resource.Array.vision_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter7.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cbolvis.Adapter = adapter7;
            cboradd = (Spinner)FindViewById(Resource.Id.cboradd);
            var adapter8 = ArrayAdapter.CreateFromResource(this, Resource.Array.apower_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter8.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cboradd.Adapter = adapter8;
            cboladd = FindViewById<Spinner>(Resource.Id.cboladd);
            var adapter9 = ArrayAdapter.CreateFromResource(this, Resource.Array.apower_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter9.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cboladd.Adapter = adapter9;
            NCentre = FindViewById<AutoCompleteTextView>(Resource.Id.txtcentre);
            NRemarks = FindViewById<EditText>(Resource.Id.txtremarks);
            BtnSave = FindViewById<Button>(Resource.Id.btnSave);
            LoadCentre();
            btnImg = FindViewById<Button>(Resource.Id.btnimg);
            imgpower = FindViewById<ImageView>(Resource.Id.imgpower);
            btnImg.Click += BtnImg_Click;
            BtnSave.Click += BtnSave_Click;
        }

        private void BtnImg_Click(object sender, EventArgs e)
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

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                if (requestCode == REQUEST_CAMERA)
                {
                    Bitmap bitmap = (Bitmap)data.Extras.Get("data");
                    imgpower.SetImageBitmap(bitmap);
                }
                else if (requestCode == SELECT_FILE)
                {
                    imgpower.SetImageURI(data.Data);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string rsph = cborsph.GetItemAtPosition(cborsph.SelectedItemPosition).ToString();
            string lsph = cbolsph.GetItemAtPosition(cbolsph.SelectedItemPosition).ToString();
            string rcyl = cborcyl.GetItemAtPosition(cborcyl.SelectedItemPosition).ToString();
            string lcyl = cbolcyl.GetItemAtPosition(cbolcyl.SelectedItemPosition).ToString();
            string raxis = cboraxis.GetItemAtPosition(cboraxis.SelectedItemPosition).ToString();
            string laxis = cbolaxis.GetItemAtPosition(cbolaxis.SelectedItemPosition).ToString();
            string rvis = cborvis.GetItemAtPosition(cborvis.SelectedItemPosition).ToString();
            string lvis = cbolvis.GetItemAtPosition(cbolvis.SelectedItemPosition).ToString();
            string arsph = cboradd.GetItemAtPosition(cboradd.SelectedItemPosition).ToString();
            string alsph = cboladd.GetItemAtPosition(cboladd.SelectedItemPosition).ToString();
            string nrsph = "";
            string nrcyl = "";
            string nraxis = "";
            string nrvis = "";
            if (arsph != "0.00")
            {
                decimal irsph = Convert.ToDecimal(rsph) + Convert.ToDecimal(arsph);
                if (irsph < 0)
                {
                    nrsph = irsph.ToString();
                }
                else
                {
                    nrsph = "+" + irsph.ToString();
                }
                nrcyl = cborcyl.GetItemAtPosition(cborcyl.SelectedItemPosition).ToString();
                nraxis = cboraxis.GetItemAtPosition(cboraxis.SelectedItemPosition).ToString();
                nrvis = cborvis.GetItemAtPosition(cborvis.SelectedItemPosition).ToString();
            }
            string nlsph = "";
            string nlcyl = "";
            string nlaxis = "";
            string nlvis = "";
            if (alsph != "0.00")
            {
                decimal ilsph = Convert.ToDecimal(lsph) + Convert.ToDecimal(alsph);
                if (ilsph < 0)
                {
                    nlsph = ilsph.ToString();
                }
                else
                {
                    nlsph = "+" + ilsph.ToString();
                }
                nlcyl = cbolcyl.GetItemAtPosition(cbolcyl.SelectedItemPosition).ToString();
                nlaxis = cbolaxis.GetItemAtPosition(cbolaxis.SelectedItemPosition).ToString();
                nlvis = cbolvis.GetItemAtPosition(cbolvis.SelectedItemPosition).ToString();
            }
            string centre = NCentre.Text;
            string remarks = NRemarks.Text;
            BitmapDrawable bd = (BitmapDrawable)imgpower.Drawable;

            Bitmap bitmap = bd.Bitmap;
            MemoryStream stream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 20, stream);
            byte[] ba = stream.ToArray();
            string bal = Convert.ToBase64String(ba);
            titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            t2.Timeout = -1;
            t2.OpthalCompleted += T2_OpthalCompleted;
            t2.OpthalAsync(Globals.docid, Globals.gyear, Globals.gmonth, Globals.gday, Globals.gageflag, Globals.regno,rsph,lsph,rcyl,lcyl,rvis,lvis,raxis,laxis,nrsph,nlsph,nrcyl,nlcyl,nrvis,nlvis,nraxis,nlaxis,arsph,alsph,centre,remarks,bal);
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

        private void T2_OpthalCompleted(object sender, titaniumref.OpthalCompletedEventArgs e)
        {
            var calldialog = new Android.App.AlertDialog.Builder(this);
            calldialog.SetTitle("Notify");
            calldialog.SetCancelable(false);
            calldialog.SetMessage(e.Result.Message);
            calldialog.SetNeutralButton("OK", delegate {
                StartActivity(typeof(PatReg));
            });
            calldialog.Show();
        }

        private void LoadCentre()
        {
            titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            t2.CentreCompleted += T2_CentreCompleted;
            t2.CentreAsync(Globals.docid);
        }

        private void T2_CentreCompleted(object sender, titaniumref.CentreCompletedEventArgs e)
        {
            names = e.Result.ncentre;
            ArrayAdapter adapter1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, names);
            NCentre.Adapter = adapter1;
            NCentre.Text = e.Result.ecentre;
        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(PatReg));
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