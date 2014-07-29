using Dongle.Reflection;
using NUnit.Framework;
using Ploeh.SemanticComparison;

namespace Dongle.Tests.Reflection
{
    [TestFixture]
    public class ObjectFillerTest
    {
        internal class Foo
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }

        [Test]
        public void FillerWithSameObjectTest()
        {
            var f1 = new Foo
                         {
                             Id = 1,
                             Name = "Silvio"
                         };

            var f2 = new Foo();

            ObjectFiller<Foo, Foo>.Fill(f1, f2);

            var lf2 = new Likeness<Foo, Foo>(f2);

            Assert.AreEqual(lf2, f1);
        }

        internal class Foo1Property
        {
            public string Name { get; set; }
        }

        [Test]
        public void FillerWithDifferentObjectsTest()
        {
            var f1 = new Foo
            {
                Id = 1,
                Name = "Silvio"
            };

            var f2 = new Foo1Property();
            ObjectFiller<Foo, Foo1Property>.Fill(f1, f2);
            Assert.AreEqual(f1.Name, f2.Name);
        }

        internal class Foo3
        {
            public string Name { get; set; }
        }

        [Test]
        public void FillerWithLessPropertiesTest()
        {
            var f1 = new Foo
            {
                Name = "Silvio",
                Id = 1
            };

            var f2 = new Foo3();
            ObjectFiller<Foo, Foo3>.Fill(f1, f2);
            Assert.AreEqual(f1.Name, f2.Name);
        }

        internal class Foo3Property
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public string Password { get; set; }
        }

        [Test]
        public void FillerWithMorePropertiesTest()
        {
            var f1 = new Foo
            {
                Name = "Silvio",
                Id = 1,
            };

            var f2 = new Foo3Property();

            ObjectFiller<Foo, Foo3Property>.Fill(f1, f2);

            Assert.AreEqual(f1.Name, f2.Name);
            Assert.AreEqual(f1.Id, f2.Id);
            Assert.AreEqual(null, f2.Password);
        }

        internal class ComplexFoo
        {
            public ComplexFoo Parent { get; set; }
            public string Name { get; set; }
        }

        [Test]
        public void FillerWithComplexTypesTest()
        {
            var f1 = new ComplexFoo
            {
                Name = "Dad",
                Parent = new ComplexFoo
                             {
                                 Name = "Son"
                             }
            };

            var f2 = new ComplexFoo();

            ObjectFiller<ComplexFoo, ComplexFoo>.Fill(f1, f2);
            Assert.AreEqual(f1.Name, f2.Name);
            Assert.AreEqual(f1.Parent.Name, f2.Parent.Name);
        }

        internal class FooNameReadOnly
        {
            public string Name { get { return "foo"; } }
            public int Id { get; set; }
        }

        [Test]
        public void FillerWithNoSetterTest()
        {
            var f1 = new Foo
            {
                Id = 1,
                Name = "Silvio"
            };

            var f2 = new FooNameReadOnly();

            ObjectFiller<Foo, FooNameReadOnly>.Fill(f1, f2);
            Assert.AreEqual("foo", f2.Name);
            Assert.AreEqual(f1.Id, f2.Id);
        }

        [Test]
        public void FillerOneParameter()
        {
            var f1 = new Foo1Property
            {
                Name = "Silvio"
            };

            var f2 = ObjectFiller<Foo1Property, Foo1Property>.Fill(f1);
            Assert.AreEqual(f1.Name, f2.Name);
        }

        [Test]
        public void FillerNonReplaceNullSourceProperty()
        {
            var f1 = new Foo
            {
                Id = 3,
            };

            var f2 = new Foo
            {
                Name = "Silvio",
                Id = 1
            };

            ObjectFiller<Foo, Foo>.Fill(f1, f2);
            Assert.AreEqual(null, f2.Name);
            Assert.AreEqual(3, f2.Id);
        }

        [Test]
        public void FillerMerge()
        {
            var f1 = new Foo
            {
                Id = 3,
            };

            var f2 = new Foo
            {
                Name = "Silvio",
                Id = 1
            };

            ObjectFiller<Foo, Foo>.Merge(f1, f2);
            Assert.AreEqual("Silvio", f2.Name);
            Assert.AreEqual(3, f2.Id);
        }

        internal class FooInt
        {
            public int Enabled { get; set; }
        }

        internal class FooBoolean
        {
            public bool Enabled { get; set; }
        }

        internal class FooNullableBoolean
        {
            public bool? Enabled { get; set; }
        }

        internal class FooNullableInt
        {
            public int? Enabled { get; set; }
        }

        internal class FooString
        {
            public string Enabled { get; set; }
        }

        [Test]
        public void FillerDoesntFillDifferentTypes()
        {
            var f1 = new FooInt
            {
                Enabled = 1,
            };

            var f2 = ObjectFiller<FooInt, FooString>.Fill(f1);
            Assert.AreEqual(null, f2.Enabled);
        }

        [Test]
        public void FillerFillsIfDifferentTypesAreIntAndBoolean()
        {
            var f1 = new FooInt
            {
                Enabled = 1,
            };

            var f2 = ObjectFiller<FooInt, FooBoolean>.Fill(f1);
            Assert.AreEqual(true, f2.Enabled);
        }

        [Test]
        public void FillerFillsIfDifferentTypesAreBooleanAndInt()
        {
            var f1 = new FooBoolean
            {
                Enabled = true,
            };

            var f2 = ObjectFiller<FooBoolean, FooInt>.Fill(f1);
            Assert.AreEqual(1, f2.Enabled);
        }

        [Test]
        public void FillerFillsNullableIntoNotNullable()
        {
            var f1 = new FooNullableBoolean
            {
                Enabled = true,
            };

            var f2 = ObjectFiller<FooNullableBoolean, FooBoolean>.Fill(f1);
            Assert.AreEqual(true, f2.Enabled);
        }

        [Test]
        public void FillerDontFillNullableIntoNotNullableDifferentType()
        {
            var f1 = new FooNullableBoolean
            {
                Enabled = true,
            };

            var f2 = ObjectFiller<FooNullableBoolean, FooInt>.Fill(f1);
            Assert.AreEqual(0, f2.Enabled);
        }

        [Test]
        public void FillerFillsWithDefaultValueNotNullableFromNullNullable()
        {
            var sourceObj = new FooNullableInt
                            {
                                Enabled = null,
                            };

            var destObj = new FooInt
                          {
                              Enabled = 123
                          };

            ObjectFiller<FooNullableInt, FooInt>.Fill(sourceObj, destObj);
            Assert.AreEqual(0, destObj.Enabled);
        }

        [Test]
        public void FillerFillsNotNullableIntoNullable()
        {
            var f1 = new FooBoolean
            {
                Enabled = true,
            };

            var f2 = ObjectFiller<FooBoolean, FooNullableBoolean>.Fill(f1);
            Assert.AreEqual(true, f2.Enabled);
        }
    }
}
