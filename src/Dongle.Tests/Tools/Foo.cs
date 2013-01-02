using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Dongle.Serialization.Attributes;

namespace Dongle.Tests.Tools
{
    public class Foo
    {
        [FixedWidth(30)]
        public string Name { get; set; }

        [Display(Name="Idade")]
        [FixedWidth(10)]
        public int Age { get; set; }

        [FixedWidth(20)]
        public DateTime CreatedAt { get; set; }

        [FixedWidth(10)]
        public double Price { get; set; }

        public bool Enabled { get; set; }

        [Ignore]
        public Foo Parent { get; set; }

        [Ignore]
        public static Foo[] FooArray
        {
            get
            {
                return new[]
                    {
                        new Foo
                            {
                                Age = 82,
                                CreatedAt = new DateTime(1930, 12, 12),
                                Enabled = true,
                                Name = "Silvio Santos",
                                Price = 1.72,
                                Parent = null
                            },
                        new Foo
                            {
                                Age = 83,
                                CreatedAt = new DateTime(1929, 03, 08, 12, 0, 0),
                                Enabled = false,
                                Name = "Hebe Camargo",
                                Price = 1.60,
                                Parent = null
                            }
                    };
            }
        }

        [Ignore]
        public static List<Dictionary<string, object>> FooDictionary
        {
            get
            {
                return new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                            {
                                { "Name", "Silvio Santos" },
                                { "Age", 82 },
                                { "CreatedAt", new DateTime(1930, 12, 12) },
                                { "Price", 1.72 },
                                { "Enabled", true }
                            },
                        new Dictionary<string, object>
                            {
                                { "Name", "Hebe Camargo" },
                                { "Age", 83 },
                                { "CreatedAt", new DateTime(1929, 03, 08, 12, 0, 0) },
                                { "Price", 1.60 },
                                { "Enabled", false }
                            }
                    };
            }
        }
    }
}