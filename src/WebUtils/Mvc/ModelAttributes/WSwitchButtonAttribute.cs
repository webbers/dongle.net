using System;

namespace WebUtils.Mvc.ModelAttributes
{
    public class WSwitchButtonAttribute : Attribute
    {
        public string LabelYes { get; set; }
        public string LabelNo { get; set; }
    }
}