using System;
using System.Collections.Generic;
using Dongle.Algorithms;
using Dongle.Tests.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Tests.Algorithms
{
    [TestClass]
    public class LevensteinDistanceTest
    {
        [TestMethod]
        public void Similarity()
        {
            Assert.AreEqual(0, LevensteinDistance.Similarity("abcd", "AB"));
            Assert.AreEqual(0.5, LevensteinDistance.Similarity("abcd", "AB", StringComparison.CurrentCultureIgnoreCase));
            Assert.AreEqual(0.5, LevensteinDistance.Similarity("abcd", "ab"));
            Assert.AreEqual(1.0, LevensteinDistance.Similarity("abcd", "ABCD", StringComparison.CurrentCultureIgnoreCase));
            Assert.AreEqual(0, LevensteinDistance.Similarity("abcd", ""));
            Assert.AreEqual(0, LevensteinDistance.Similarity("", ""));
        }

        [TestMethod]
        public void Calculate()
        {
            Assert.AreEqual(2, LevensteinDistance.Calculate("abcd", "ab"));
            Assert.AreEqual(2, LevensteinDistance.Calculate("", "ab"));
            Assert.AreEqual(4, LevensteinDistance.Calculate("abcd", ""));
        }

        [TestMethod]
        public void GetMostSimilarTo()
        {
            var silvio = new Foo {Name = "Silvio"};
            var sauro = new Foo {Name = "Sauro"};
            var listFoo = new List<Foo>
                              {
                                  silvio,
                                  new Foo {Name = "Santos"},
                                  new Foo {Name = "Silva"},
                                  sauro
                              };

            Assert.AreEqual(silvio, listFoo.GetMostSimilarTo(f=>f.Name, "silvio").Item);
            Assert.AreEqual(1, listFoo.GetMostSimilarTo(f=>f.Name, "silvio", StringComparison.CurrentCultureIgnoreCase).Rate);
            Assert.AreEqual(sauro, listFoo.GetMostSimilarTo(f => f.Name, "saur").Item);

            Assert.IsNull(new List<string>().GetMostSimilarTo(f=>f, "bla"));

            Assert.AreEqual("bla", new List<string> { "bla" }.GetMostSimilarTo("bla").Item);
            Assert.IsNull(new List<string>().GetMostSimilarTo("bla"));
        }
    }
}
