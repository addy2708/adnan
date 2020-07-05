using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;

namespace Hospitab
{
    [Activity(Label = "@string/app_name", Theme = "@style/NAppTheme")]

    public class DocSignUp : AppCompatActivity
    {
        private static readonly Int32 REQUEST_CAMERA = 0;
        private static readonly Int32 SELECT_FILE = 1;
        private string imgtype = "";
        Spinner cbogender;
        Button btnimgdoc;
        ImageView imgdoc;
        Button btnimgdeg;
        ImageView imgdeg;
        Button btnimgproof;
        ImageView imgproof;
        Button btnsignup;
        EditText docname;
        EditText docregno;
        EditText docdegree;
        EditText docphone;
        EditText docemail;
        EditText docpwd;
        EditText doccentre;
        EditText docusername;
        private int progressBarStatus;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.drsignup);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.dtoolbar7);
            SetSupportActionBar(toolbar);
            cbogender = FindViewById<Spinner>(Resource.Id.cmbgender);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.sex_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            cbogender.Adapter = adapter;
            btnimgdoc = FindViewById<Button>(Resource.Id.btnidoc);
            imgdoc = FindViewById<ImageView>(Resource.Id.imgdoc);
            btnimgdeg = FindViewById<Button>(Resource.Id.btnideg);
            imgdeg = FindViewById<ImageView>(Resource.Id.imgdegree);
            btnimgproof = FindViewById<Button>(Resource.Id.btniproof);
            imgproof = FindViewById<ImageView>(Resource.Id.imgidproof);
            btnsignup = FindViewById<Button>(Resource.Id.btnsignup);
            docname = FindViewById<EditText>(Resource.Id.txtname);
            docdegree = FindViewById<EditText>(Resource.Id.txtdegree);
            docregno = FindViewById<EditText>(Resource.Id.txtregno);
            docphone = FindViewById<EditText>(Resource.Id.txtphone);
            docemail = FindViewById<EditText>(Resource.Id.txtemail);
            docpwd = FindViewById<EditText>(Resource.Id.txtpassword);
            doccentre = FindViewById<EditText>(Resource.Id.txtcentre);
            docusername = FindViewById<EditText>(Resource.Id.txtuser);
            btnimgdoc.Click += Btnimgdoc_Click;
            btnimgdeg.Click += Btnimgdeg_Click;
            btnimgproof.Click += Btnimgproof_Click;
            btnsignup.Click += Btnsignup_Click;
        }

        private void Btnsignup_Click(object sender, EventArgs e)
        {
            if (docregno.Text == "" || docname.Text == "" || docdegree.Text == "" || docphone.Text == "" || docemail.Text == "" || docpwd.Text == "" || doccentre.Text == "" || docphone.Text.Length < 10 || docusername.Text == "")
            {
                var calldialog = new Android.App.AlertDialog.Builder(this);
                calldialog.SetTitle("Notify");
                calldialog.SetCancelable(false);
                calldialog.SetMessage("Fill All the Details!!!");
                calldialog.SetNeutralButton("OK", delegate {
                    
                });
                calldialog.Show();
            }
            else
            {
                int gender = cbogender.SelectedItemPosition;
                BitmapDrawable bd = (BitmapDrawable)imgdoc.Drawable;
                Bitmap bitmap = bd.Bitmap;
                MemoryStream stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 20, stream);
                byte[] ba = stream.ToArray();
                string bal = Convert.ToBase64String(ba);

                BitmapDrawable bd1 = (BitmapDrawable)imgdeg.Drawable;
                Bitmap bitmap1 = bd1.Bitmap;
                MemoryStream stream1 = new MemoryStream();
                bitmap1.Compress(Bitmap.CompressFormat.Jpeg, 20, stream1);
                byte[] ba1 = stream1.ToArray();
                string bal1 = Convert.ToBase64String(ba1);

                BitmapDrawable bd2 = (BitmapDrawable)imgdeg.Drawable;
                Bitmap bitmap2 = bd2.Bitmap;
                MemoryStream stream2 = new MemoryStream();
                bitmap2.Compress(Bitmap.CompressFormat.Jpeg, 20, stream2);
                byte[] ba2 = stream2.ToArray();
                string bal2 = Convert.ToBase64String(ba2);
                titaniumref.WebServiceDB t2 = new titaniumref.WebServiceDB();
                t2.Timeout = -1;
                t2.DocVerifyCompleted += T2_DocVerifyCompleted;
                t2.DocVerifyAsync(docname.Text, docdegree.Text, docregno.Text, docphone.Text, docemail.Text, gender, docpwd.Text, doccentre.Text, bal, bal1, bal2,docusername.Text);
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

        private void T2_DocVerifyCompleted(object sender, titaniumref.DocVerifyCompletedEventArgs e)
        {
            var calldialog = new Android.App.AlertDialog.Builder(this);
            calldialog.SetTitle("Notify");
            calldialog.SetCancelable(false);
            calldialog.SetMessage(e.Result.Message);
            calldialog.SetNeutralButton("OK", delegate {
                StartActivity(typeof(MainActivity));
            });
            calldialog.Show();
        }

        private void Btnimgproof_Click(object sender, EventArgs e)
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
                        imgtype = "proof";
                    }
                    //Choose from gallery
                    else if (args.Which == 1)
                    {
                        var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                        intent.SetType("image/*");
                        this.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), SELECT_FILE);
                        imgtype = "proof";
                    }
                });

                dialogBuilder.Show();
            }
        }

        private void Btnimgdeg_Click(object sender, EventArgs e)
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
                        imgtype = "degree";
                    }
                    //Choose from gallery
                    else if (args.Which == 1)
                    {
                        var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                        intent.SetType("image/*");
                        this.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), SELECT_FILE);
                        imgtype = "degree";
                    }
                });

                dialogBuilder.Show();
            }
        }

        private void Btnimgdoc_Click(object sender, EventArgs e)
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
                        imgtype = "doctor";
                    }
                    //Choose from gallery
                    else if (args.Which == 1)
                    {
                        var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                        intent.SetType("image/*");
                        this.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), SELECT_FILE);
                        imgtype = "doctor";
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
                    if (imgtype == "proof")
                    {
                        imgproof.SetImageBitmap(bitmap);
                        
                    }
                    else if (imgtype == "degree")
                    {
                        imgdeg.SetImageBitmap(bitmap);
                    }
                    else if (imgtype == "doctor")
                    {
                        imgdoc.SetImageBitmap(bitmap);
                    }
                }
                else if (requestCode == SELECT_FILE)
                {
                    if (imgtype == "proof")
                    {
                        imgproof.SetImageURI(data.Data);
                    }
                    else if (imgtype == "degree")
                    {
                        imgdeg.SetImageURI(data.Data);
                        
                    }
                    else if (imgtype == "doctor")
                    {
                        imgdoc.SetImageURI(data.Data);
                    }
                }
            }
        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(DrLoginActivity));
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