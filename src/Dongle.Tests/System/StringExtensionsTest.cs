using System.Linq;
using Dongle.System;
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
            Assert.AreEqual(2, bytes.Length);
            Assert.AreEqual(195, bytes[0]);
            Assert.AreEqual(161, bytes[1]);
            Assert.AreEqual("á", bytes.FromBytesToString());

            //Unicode
            bytes = "❤".ToBytes();
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(226, bytes[0]);
            Assert.AreEqual(157, bytes[1]);
            Assert.AreEqual(164, bytes[2]);
            Assert.AreEqual("❤", bytes.FromBytesToString());
        }

        [TestMethod]
        public void ToMd5()
        {
            Assert.AreEqual("827ccb0eea8a706c4c34a16891f84e7b", "12345".ToMd5());

            //Acentuação
            Assert.AreEqual("36b7148acc1b607c473a15a47fa17706", "á".ToMd5());

            //Unicode
            Assert.AreEqual("7aba075adb50a589f94c87fb569882dc", "❤".ToMd5());
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
            Assert.AreEqual("w6E=", base64);
            Assert.AreEqual("á", base64.FromBase64ToString());

            base64 = "❤".ToBase64();
            Assert.AreEqual("4p2k", base64);
            Assert.AreEqual("❤", base64.FromBase64ToString());
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
            Assert.AreEqual("", "abc".Take(0));
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
    }
}
