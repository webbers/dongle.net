using System;
using System.Linq;
using System.Text;
using Dongle.System;
using Dongle.System.IO;
using Dongle.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dongle.Tests.System
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void ToBytes()
        {
            var bytes = "123\045".ToBytes();
            Assert.AreEqual(6, bytes.Length);
            Assert.AreEqual("123", bytes.FromBytesToString());

            bytes = "12345".ToBytes();
            Assert.AreEqual(5, bytes.Length);
            Assert.AreEqual("12345", bytes.FromBytesToString());

            bytes = "a".ToBytes();
            Assert.AreEqual(97, bytes[0]);
            Assert.AreEqual(1, bytes.Length);
            Assert.AreEqual("a", bytes.FromBytesToString());

            bytes = "1".ToBytes();
            Assert.AreEqual(49, bytes[0]);
            Assert.AreEqual(1, bytes.Length);
            Assert.AreEqual("1", bytes.FromBytesToString());

            //Acentuação
            bytes = "á".ToBytes();
            Assert.AreEqual(1, bytes.Length);
            Assert.AreEqual(225, bytes[0]);
            Assert.AreEqual("á", bytes.FromBytesToString());

            //Unicode
            bytes = "®".ToBytes();
            Assert.AreEqual(1, bytes.Length);
            Assert.AreEqual(174, bytes[0]);
            Assert.AreEqual("®", bytes.FromBytesToString());
        }

        [TestMethod]
        public void ToMd5()
        {
            Assert.AreEqual("827ccb0eea8a706c4c34a16891f84e7b", "12345".ToMd5());

            //Acentuação
            Assert.AreEqual("2eece4376cee1433d0e9f200deb75408", "á".ToMd5());

            //Unicode
            Assert.AreEqual("d1457b72c3fb323a2671125aef3eab5d", "❤".ToMd5());
        }

        [TestMethod]
        public void RemoveAccents()
        {
            Assert.AreEqual("aeiouyaoaeiouaeiouaeiouy", "áéíóúýãõàèìòùâêîôûäëïöüÿ".RemoveAccents());
        }

        [TestMethod]
        public void RemovePunctuation()
        {
            Assert.AreEqual("", ".!,;?".RemovePunctuation());
            Assert.AreEqual("abcde", "a.b!c,d;e?".RemovePunctuation());
        }

        [TestMethod]
        public void FromBase64ToString()
        {
            var base64 = "12345".ToBase64();
            Assert.AreEqual("MTIzNDU=", base64);
            Assert.AreEqual("12345", base64.FromBase64ToString());

            base64 = "á".ToBase64();
            Assert.AreEqual("4Q==", base64);
            Assert.AreEqual("á", base64.FromBase64ToString());

            base64 = "®".ToBase64();
            Assert.AreEqual("rg==", base64);
            Assert.AreEqual("®", base64.FromBase64ToString());
            
        }

        [TestMethod]
        public void Capitalize()
        {
            Assert.AreEqual("", "".Capitalize());
            Assert.AreEqual("O Rato Roeu", "o rato roeu".Capitalize());
            Assert.AreEqual(" O Rato Roeu", " o rato roeu".Capitalize());
            Assert.AreEqual("Mal-humorado", "mal-humorado".Capitalize());
            Assert.AreEqual("Mal.hu.mo.ra.do", "mal.hu.mo.ra.do".Capitalize());
            Assert.AreEqual("Vamos Corrigir As Palavras", "vamos corrigir as palavras".Capitalize());
        }

        [TestMethod]
        public void OnlyNumbers()
        {
            Assert.AreEqual("12345", "1a2b3c4d5e".OnlyNumbers());
            Assert.AreEqual("12345678900", "123.456.789-00".OnlyNumbers());
        }

        [TestMethod]
        public void ToSlug()
        {
            //Máximo de caracteres
            Assert.AreEqual("casa", "casa-branca".ToSlug(4));

            //Slug zerado
            Assert.AreEqual("", "casa-branca".ToSlug(0));

            //UrlEncoded
            Assert.AreEqual("casa-branca", "casa%20branca".ToSlug());

            //Acentos
            Assert.AreEqual("casa-branca", "cása-brânca".ToSlug());

            //Espaços
            Assert.AreEqual("casa-branca", "casa branca".ToSlug());

            //Espaços e hifens juntos
            Assert.AreEqual("casa-branca", "casa - branca".ToSlug());

            //Múltiplos hifens
            Assert.AreEqual("casa-branca", "casa--- - ---branca".ToSlug());

            //Caracteres diversos
            Assert.AreEqual("casa-branca", "cása!❤?❤!brânca".ToSlug());
        }

        [TestMethod]
        public void StripTags()
        {
            Assert.AreEqual("casa", "<a>casa</a>".StripTags());
            Assert.AreEqual("casa", "<a href=\"abc\">casa</a>".StripTags());
            Assert.AreEqual("casa", "<a href=\"abc\"><b>casa</b></a>".StripTags());
            Assert.AreEqual("casa", "<a href=\"abc\"><b>casa</a>".StripTags());
        }

        [TestMethod]
        public void Take()
        {
            Assert.AreEqual("abc", "abc".Take(0));
            Assert.AreEqual("a", "abc".Take(1));
            Assert.AreEqual("abc", "abc".Take(3));
            Assert.AreEqual("abc", "abc".Take(4));
        }

        [TestMethod]
        public void IndexOfAllTest()
        {
            var search = "123.456.789.012";
            var indexes = search.IndexOfAll(".").ToList();
            Assert.AreEqual(3, indexes.Count());
            Assert.AreEqual(3, indexes[0]);
            Assert.AreEqual(7, indexes[1]);
            Assert.AreEqual(11, indexes[2]);

            search = "123---456---789---012";
            indexes = search.IndexOfAll("---").ToList();
            Assert.AreEqual(3, indexes.Count());
            Assert.AreEqual(3, indexes[0]);
            Assert.AreEqual(9, indexes[1]);
            Assert.AreEqual(15, indexes[2]);
        }

        [TestMethod]
        public void Base64Methods()
        {
            var inputUtf7 = Encoding.UTF7.GetString(Encoding.UTF7.GetBytes("Alo Mundo Imundo!! áéç^íóú"));
            var inputUtf8 = DongleEncoding.Default.GetString(Encoding.Default.GetBytes("Alo Mundo Imundo!! áéç^íóú"));
            const string outputUtf7 = "QWxvIE11bmRvIEltdW5kbytBQ0VBSVEtICtBT0VBNlFEbkFGNEE3UUR6QVBvLQ==";
            const string outputUtf8 = "QWxvIE11bmRvIEltdW5kbyEhIOHp517t8/o=";

            Assert.AreEqual(outputUtf8, inputUtf8.ToBase64());
            Assert.AreEqual(outputUtf7, inputUtf7.ToBase64(Encoding.UTF7));

            Assert.AreEqual(inputUtf8, outputUtf8.FromBase64ToString());
            Assert.AreEqual(inputUtf7, outputUtf7.FromBase64ToString(Encoding.UTF7));
        }

        [TestMethod]
        public void RemoveSpecialChars()
        {
            const string input = "ZYÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç ?#\"'\\/:<>|*-+";
            const string output = "ZYAAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc_";
            Assert.AreEqual(output, input.RemoveSpecialChars());
        }

        [TestMethod]
        public void Reverse()
        {
            const string input = "áBÇDefgHi";
            const string output = "iHgfeDÇBá";
            Assert.AreEqual(output, input.Reverse());
        }

        [TestMethod]
        public void ToCrc32()
        {
            const string input = "abcdefghij12234567";
            const int output = 1366991586;
            Assert.AreEqual(output, input.ToCrc32());
        }

        [TestMethod]
        public void FromHexToInt32()
        {
            Assert.AreEqual(999999999, "3B9AC9FF".FromHexToInt32());
            Assert.AreEqual(0, "Vai dar zero!".FromHexToInt32());
        }

        [TestMethod]
        public void FromHexToInt64()
        {
            Assert.AreEqual(48358587860608905, "ABCDE123456789".FromHexToInt64());
            Assert.AreEqual(0, "Vai dar zero!".FromHexToInt64());
        }

        [TestMethod]
        public void TestToMd5Safe()
        {
            Assert.AreEqual("63807B7DE13A99416BAC5C2BEBCA7782", "Alô Mundo Imundo!!".ToMd5Safe());
            Assert.AreEqual("9CB10DB7CEEF37FDD2F3955D05832108", DongleEncoding.Default.GetString(DongleEncoding.Default.GetBytes("Alo Mundo Imundo!!")).ToMd5Safe(DongleEncoding.Default));
        }
    }
}
