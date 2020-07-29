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
    public class Skinentry : AppCompatActivity
    {
        Spinner cmbpru;
        Spinner cmbsca;
        Spinner cmbery;
        TextView Name;
        TextView Age;
        TextView Visitno;
        Button BtnSave;
        Button btnImg;
        Button BtniAdd;
        ImageView imgpresc;
        MultiAutoCompleteTextView txtcfeat;
        MultiAutoCompleteTextView txtdiag;
        MultiAutoCompleteTextView txttreat;
        AutoCompleteTextView txtmed;
        Spinner cmbtime;
        EditText txtdose;
        AutoCompleteTextView txtadvice;
        EditText NRemarks;
        string[] cfeat;
        string[] diag;
        string[] treat;
        string[] meds;
        string[] adv;
        private int progressBarStatus;
        private static readonly Int32 REQUEST_CAMERA = 0;
        private static readonly Int32 SELECT_FILE = 1;
        ListView LView;
        List<Medlist> tb = new List<Medlist>();
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Skin_Entry);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dtoolbar4);
            SetSupportActionBar(toolbar);
            Name = (TextView)FindViewById(Resource.Id.lblname);
            Visitno = (TextView)FindViewById(Resource.Id.lblvisit);
            string regno = Intent.GetStringExtra("Regno") ?? string.Empty;
            string etype = Intent.GetStringExtra("etype") ?? string.Empty;
            string pname = Intent.GetStringExtra("Name") ?? string.Empty;
            Name.Text = pname;
            Age = (TextView)FindViewById(Resource.Id.lblage);
            string page = Intent.GetStringExtra("Age") ?? string.Empty;
            Age.Text = page;
            cmbpru = (Spinner)FindViewById(Resource.Id.cmbpru);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.yn_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cmbpru.Adapter = adapter;
            cmbsca = FindViewById<Spinner>(Resource.Id.cmbsca);
            var adapter1 = ArrayAdapter.CreateFromResource(this, Resource.Array.yn_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cmbsca.Adapter = adapter1;
            cmbery = (Spinner)FindViewById(Resource.Id.cmbery);
            var adapter2 = ArrayAdapter.CreateFromResource(this, Resource.Array.yn_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cmbery.Adapter = adapter2;
            cmbtime = (Spinner)FindViewById(Resource.Id.cmbtime);
            var adapter3 = ArrayAdapter.CreateFromResource(this, Resource.Array.time_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cmbtime.Adapter = adapter3;
            LoadVisit(regno,Globals.docid.ToString(),etype);
            LoadFeat();
            LoadDiag();
            LoadTreat();
            LoadMed();
            LoadAdv();
            txtcfeat = FindViewById<MultiAutoCompleteTextView>(Resource.Id.txtcfeat);
            txtdiag = FindViewById<MultiAutoCompleteTextView>(Resource.Id.txtdiag);
            txttreat = FindViewById<MultiAutoCompleteTextView>(Resource.Id.txttreat);
            txtmed = FindViewById<AutoCompleteTextView>(Resource.Id.txtmed);
            txtdose = FindViewById<EditText>(Resource.Id.txtdose);
            txtadvice = FindViewById<AutoCompleteTextView>(Resource.Id.txtadvice);
            NRemarks = FindViewById<EditText>(Resource.Id.txtremarks);
            BtnSave = FindViewById<Button>(Resource.Id.btnSave);
            btnImg = FindViewById<Button>(Resource.Id.btnimg);
            BtniAdd = FindViewById<Button>(Resource.Id.btnAdd);
            imgpresc = FindViewById<ImageView>(Resource.Id.imgpower);
            LView = FindViewById<ListView>(Resource.Id.presc_listView);
            btnImg.Click += BtnImg_Click;
            BtnSave.Click += BtnSave_Click;
            BtniAdd.Click += BtniAdd_Click;
        }

        private void BtniAdd_Click(object sender, EventArgs e)
        {
            Medlist m = new Medlist();
            m.medname = txtmed.Text;
            m.dosage = txtdose.Text;
            m.advice = txtadvice.Text;
            m.time = cmbtime.GetItemAtPosition(cmbtime.SelectedItemPosition).ToString();
            tb.Add(m);
            LView.Adapter = new SkinAdapter(this, tb);
            Utility.setListViewHeightBasedOnChildren(LView);
        }

        public class SkinAdapter : BaseAdapter<Medlist>
        {
            List<Medlist> items;
            Activity context;
            public SkinAdapter(Activity context, List<Medlist> items)
                : base()
            {
                this.context = context;
                this.items = items;
            }
            public override long GetItemId(int position)
            {
                return position;
            }
            public override Medlist this[int position]
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
                    view = context.LayoutInflater.Inflate(Resource.Layout.prescdetails, null);
                view.FindViewById<TextView>(Resource.Id.lblmed).Text = item.medname;
                view.FindViewById<TextView>(Resource.Id.lbldose).Text = item.dosage;
                view.FindViewById<TextView>(Resource.Id.lbladvice).Text = item.advice;
                view.FindViewById<TextView>(Resource.Id.lbltime).Text = item.time;
                //view.Click += (object sender, EventArgs e) => {
                //    Toast.MakeText(parent.Context, "Clicked " + item.name, ToastLength.Long).Show();
                //    Globals.gemrno = item.emrno;
                //    parent.Context.StartActivity(typeof(EyePtDetails));
                //};
                return view;
            }
        }

        private void TempSave(string mval,string stype)
        {
            titaniumdoc.WebServiceDB t3 = new titaniumdoc.WebServiceDB();
            t3.TemplateSaveCompleted += T3_TemplateSaveCompleted;
            t3.TemplateSaveAsync(mval, stype);
        }

        private void T3_TemplateSaveCompleted(object sender, titaniumdoc.TemplateSaveCompletedEventArgs e)
        {
            
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
                    imgpresc.SetImageBitmap(bitmap);
                }
                else if (requestCode == SELECT_FILE)
                {
                    imgpresc.SetImageURI(data.Data);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string sval = txtcfeat.Text;
            string[] eval = sval.Split(",");
            foreach (string m in eval)
            {
                string nval = m.Trim();
                TempSave(nval, "Feat");
            }

            string sval1 = txtdiag.Text;
            string[] eval1 = sval1.Split(",");
            foreach (string m in eval1)
            {
                string nval = m.Trim();
                TempSave(nval, "Diag");
            }

            string sval2 = txttreat.Text;
            string[] eval2 = sval2.Split(",");
            foreach (string m in eval2)
            {
                string nval = m.Trim();
                TempSave(nval, "Treat");
            }

            string pru = cmbpru.GetItemAtPosition(cmbpru.SelectedItemPosition).ToString();
            string sca = cmbsca.GetItemAtPosition(cmbsca.SelectedItemPosition).ToString();
            string ery = cmbery.GetItemAtPosition(cmbery.SelectedItemPosition).ToString();
            string ncfeat = txtcfeat.Text.Trim();
            string ndiag = txtdiag.Text.Trim();
            string ntreat = txttreat.Text.Trim();
            string tremarks = NRemarks.Text;
            List<Medlist> presc = new List<Medlist>();
            presc = tb;
            Medlist[] MyArray = presc.ToArray();
            BitmapDrawable bd = (BitmapDrawable)imgpresc.Drawable;
            string gpresc = JsonConvert.SerializeObject(presc);
            Bitmap bitmap = bd.Bitmap;
            MemoryStream stream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 20, stream);
            byte[] ba = stream.ToArray();
            string bal = Convert.ToBase64String(ba);
            titaniumdoc.WebServiceDB t3 = new titaniumdoc.WebServiceDB();
            t3.SkinCompleted += T3_SkinCompleted;
            t3.SkinAsync(Globals.docid, Globals.gyear, Globals.gmonth, Globals.gday, Globals.gageflag, Globals.regno,pru,sca,ery,ncfeat.TrimEnd(','),ndiag.TrimEnd(','), ntreat.TrimEnd(','), tremarks,bal,"0",gpresc,"Dermatology" );
            //titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
            //t2.Timeout = -1;
            //t2.OpthalCompleted += T2_OpthalCompleted;
            //t2.OpthalAsync(Globals.docid, Globals.gyear, Globals.gmonth, Globals.gday, Globals.gageflag, Globals.regno,rsph,lsph,rcyl,lcyl,rvis,lvis,raxis,laxis,nrsph,nlsph,nrcyl,nlcyl,nrvis,nlvis,nraxis,nlaxis,arsph,alsph,centre,remarks,bal);
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

        private void T3_SkinCompleted(object sender, titaniumdoc.SkinCompletedEventArgs e)
        {
            var calldialog = new Android.App.AlertDialog.Builder(this);
            calldialog.SetTitle("Notify");
            calldialog.SetCancelable(false);
            calldialog.SetMessage(e.Result.Message);
            calldialog.SetNeutralButton("OK", delegate {
                var activity = new Intent(this, typeof(PatReg));
                activity.PutExtra("etype", "Dermatology");
                StartActivity(activity);
            });
            calldialog.Show();
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

        private void LoadMed()
        {
            titaniumdoc.WebServiceDB t3 = new titaniumdoc.WebServiceDB();
            t3.MedicineCompleted += T3_MedicineCompleted;
            t3.MedicineAsync();
        }

        private void T3_MedicineCompleted(object sender, titaniumdoc.MedicineCompletedEventArgs e)
        {
            meds = e.Result.nmedicines;
            ArrayAdapter adapter1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, meds);
            txtmed.Threshold = 3;
            txtmed.Adapter = adapter1;
        }

        private void LoadVisit(string regno, string docid, string etype)
        {
            titaniumdoc.WebServiceDB t3 = new titaniumdoc.WebServiceDB();
            t3.GetVisitCompleted += T3_GetVisitCompleted;
            t3.GetVisitAsync(regno, docid, etype);
        }

        private void T3_GetVisitCompleted(object sender, titaniumdoc.GetVisitCompletedEventArgs e)
        {
            Visitno.Text = "Visit No."+ e.Result.visit;
        }

        private void LoadAdv()
        {
            titaniumdoc.WebServiceDB t3 = new titaniumdoc.WebServiceDB();
            t3.MedAdviceCompleted += T3_MedAdviceCompleted;
            t3.MedAdviceAsync();
        }

        private void T3_MedAdviceCompleted(object sender, titaniumdoc.MedAdviceCompletedEventArgs e)
        {
            adv = e.Result.nadvice;
            ArrayAdapter adapter1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, adv);
            txtadvice.Threshold = 3;
            txtadvice.Adapter = adapter1;
        }

        private void LoadFeat()
        {
            titaniumdoc.WebServiceDB t3 = new titaniumdoc.WebServiceDB();
            t3.CfeatCompleted += T3_CfeatCompleted;
            t3.CfeatAsync();
        }

        private void T3_CfeatCompleted(object sender, titaniumdoc.CfeatCompletedEventArgs e)
        {
            cfeat = e.Result.nfeat;
            ArrayAdapter adapter1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, cfeat);
            txtcfeat.Adapter = adapter1;
            txtcfeat.SetTokenizer(new MultiAutoCompleteTextView.CommaTokenizer());
        }

        private void LoadDiag()
        {
            titaniumdoc.WebServiceDB t3 = new titaniumdoc.WebServiceDB();
            t3.DiagnosisCompleted += T3_DiagnosisCompleted;
            t3.DiagnosisAsync();
        }

        private void T3_DiagnosisCompleted(object sender, titaniumdoc.DiagnosisCompletedEventArgs e)
        {
            diag = e.Result.ndiag;
            ArrayAdapter adapter1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, diag);
            txtdiag.Adapter = adapter1;
            txtdiag.SetTokenizer(new MultiAutoCompleteTextView.CommaTokenizer());
        }

        private void LoadTreat()
        {
            titaniumdoc.WebServiceDB t3 = new titaniumdoc.WebServiceDB();
            t3.TreatmentCompleted += T3_TreatmentCompleted;
            t3.TreatmentAsync();
        }

        private void T3_TreatmentCompleted(object sender, titaniumdoc.TreatmentCompletedEventArgs e)
        {
            treat = e.Result.ntreat;
            ArrayAdapter adapter1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, treat);
            txttreat.Adapter = adapter1;
            txttreat.SetTokenizer(new MultiAutoCompleteTextView.CommaTokenizer());
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

                    String toNumber = "919324781366"; // Replace with mobile phone number without +Sign or leading zeros, but with country code
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