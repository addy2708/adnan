using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Hospitab
{
    public class Ptinfo
    {
        public string Name
        {
            get;
            set;
        }
        public string optbillno
        {
            get;
            set;
        }
    }

    public class Ptdt
    {
        public string Ptname
        {
            get;
            set;
        }
        public string recno
        {
            get;
            set;
        }

        public string age
        {
            get;
            set;
        }
    }

    public class Ptreports
    {
        public string rptname
        {
            get;
            set;
        }
    }

    public class Pttests 
    {
        public int id
        {
            get;
            set;
        }
        public string testname
        {
            get;
            set;
        }

        public string value
        {
            get;
            set;
        }

        public string normalrange
        {
            get;
            set;
        }
    }

    public class Ptdetails
    {
        public string Name { get; set; }

        public string Regno { get; set; }

        public string Age { get; set; }

        public int ageflag { get; set; }

        public int gender { get; set; }

        public string phone { get; set; }

        public string address { get; set; }
    }

    public class TableItem
    {
        public string name;
        public string emrno;
    }

    public class Medlist
    {
        public string medname;
        public string dosage;
        public string advice;
        public string time;
    }

    public class dTableItem
    {
        public string name;
        public string regno;
    }
}