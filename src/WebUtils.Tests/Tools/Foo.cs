using System;
using System.ComponentModel.DataAnnotations;
using WebUtils.Mvc.ModelAttributes;

namespace WebUtilsTest.Tools
{
    public class Foo
    {
        [WEmail]
        public string Name { get; set; }

        [Display(Name="Idade")]
        public int Age { get; set; }

        public DateTime CreatedAt { get; set; }

        public double Price { get; set; }

        [WSwitchButton(LabelNo = "N�o!", LabelYes = "Sim!")]
        public bool Enabled { get; set; }

        public Foo Parent { get; set; }
    }
}