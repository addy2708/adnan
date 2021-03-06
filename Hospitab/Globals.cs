﻿using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Hospitab
{
    public static class Globals
    {
        public static int Guserid = 0;
        public static int docid = 0;
        public static string ageval;
        public static string genderid;
        public static string billid;
        public static string advno;
        public static string ageflag;
        public static string regno;
        public static string fromdate;
        public static string todate;
        public static int gyear = 0;
        public static int gmonth = 0;
        public static int gday = 0;
        public static string gageflag;
        public static string docname;
        public static string gemrno;
        public static string gphone;
        public static string dregno;
    }

    public class Utility
    {
        public static void setListViewHeightBasedOnChildren(ListView listView)
        {
            if (listView.Adapter == null)
            {
                // pre-condition
                return;
            }

            int totalHeight = listView.PaddingTop + listView.PaddingBottom;
            for (int i = 0; i < listView.Count; i++)
            {
                View listItem = listView.Adapter.GetView(i, null, listView);
                if (listItem.GetType() == typeof(ViewGroup))
                {
                    listItem.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                }
                listItem.Measure(0, 0);
                totalHeight += listItem.MeasuredHeight;
            }

            listView.LayoutParameters.Height = totalHeight + (listView.DividerHeight * (listView.Count - 1));
        }
    }
}