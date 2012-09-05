using System;
using System.ComponentModel.DataAnnotations;

namespace Dongle.Tests.Tools
{
    public class Foo
    {
        public string Name { get; set; }

        [Display(Name="Idade")]
        public int Age { get; set; }

        public DateTime CreatedAt { get; set; }

        public double Price { get; set; }

        public bool Enabled { get; set; }

        public Foo Parent { get; set; }
    }
}