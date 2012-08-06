﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebUtils.System;

namespace WebUtilsTest.System
{
    [TestClass]
    public class DateTimeExtensionsTest
    {
        private static DateTime _now = new DateTime(2011, 8, 15, 12, 30, 15);

        [TestMethod]
        public void TestToFriendlyString()
        {
            AssertFriendlyString("14 ago (Ontem)", "Ontem", _now.AddDays(-1));
            AssertFriendlyString("10 ago (5 dias atrás)", "5 dias atrás", _now.AddDays(-5));
            AssertFriendlyString("05 ago", "10 dias atrás", _now.AddDays(-10));

            AssertFriendlyString("15/08/2010 12:30:15", "365 dias atrás", _now.AddYears(-1));

            AssertFriendlyString("11:30 (1 hora atrás)", "1 hora atrás", _now.AddHours(-1));
            AssertFriendlyString("07:30 (5 horas atrás)", "5 horas atrás", _now.AddHours(-5));

            AssertFriendlyString("12:29 (Poucos segundos atrás)", "Poucos segundos atrás", _now.AddSeconds(-30));
            AssertFriendlyString("12:29 (1 minuto atrás)", "1 minuto atrás", _now.AddMinutes(-1));
            AssertFriendlyString("11:40 (50 minutos atrás)", "50 minutos atrás", _now.AddMinutes(-50));
        }

        [TestMethod]
        public void TestWorkDays()
        {
            Assert.AreEqual(5, _now.WorkDays(_now.AddDays(7)));
            Assert.AreEqual(0, _now.WorkDays(_now));
            Assert.AreEqual(1, _now.WorkDays(_now.AddDays(1)));
            Assert.AreEqual(5, _now.WorkDays(_now.AddDays(-7)));
        }
        
        [TestMethod]
        public void TestToRssTime()
        {
            Assert.AreEqual("Mon, 15 Aug 2011 15:30:15 GMT", _now.ToRssTime());
        }
        
        [TestMethod]
        public void TestToTimeStamp()
        {
            var timestamp = _now.ToTimeStamp();
            Assert.AreEqual(1313422215000, timestamp);
            Assert.AreEqual(_now, timestamp.FromTimeStampToDateTime());
        }

        [TestMethod]
        public void TestFromLogDateToDateTime()
        {
            Assert.AreEqual(_now, "15/Aug/2011:12:30:15".FromLogDateToDateTime());
            Assert.AreEqual(_now, "15/ago/2011:12:30:15".FromLogDateToDateTime());
        }
        
        [TestMethod]
        public void AssertFriendlyStringWithoutNow()
        {
            Assert.AreNotEqual(null, DateTime.Now.ToFriendlyString());
        }

        private void AssertFriendlyString(string dateTimeExpected, string timeSpanExpected, DateTime dateTime)
        {
            Assert.AreEqual(dateTimeExpected, (dateTime).ToFriendlyString(_now));
            Assert.AreEqual(timeSpanExpected, (_now - dateTime).ToFriendlyString());
        }


    }
}
