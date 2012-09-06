using System;
using System.ComponentModel.DataAnnotations;
using Dongle.Web.ModelAttributes;

namespace Dongle.Web.Tests.Tools
{
    public class Foo
    {
        [WEmail]
        public string Name { get; set; }

        [Display(Name="Idade")]
        public int Age { get; set; }

        public DateTime CreatedAt { get; set; }

        public double Price { get; set; }

        [WSwitchButton(LabelNo = "Não!", LabelYes = "Sim!")]
        public bool Enabled { get; set; }

        public Foo Parent { get; set; }
    }
}