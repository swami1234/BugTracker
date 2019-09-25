using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ChartViewModel
{
    public class ChartJsBarData
    {
        public List<string> Labels { get; set; }
        public List<int> Values { get; set; }

        public ChartJsBarData()
        {
            Labels = new List<string>();
            Values = new List<int>();
        }
    }
}