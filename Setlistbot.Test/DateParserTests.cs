using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Setlistbot.Test
{
    [TestClass]
    public class DateParserTests
    {
        [TestMethod]
        public void ParseDatesExpectNoDates()
        {
            string input = "";
            DateParser parser = new DateParser();
            List<DateTime> dates = parser.ParseDates(input);
            Assert.AreEqual(0, dates.Count);
        }

        [TestMethod]
        public void ParseDatesExpectOneDate()
        {
            string input = "12-31-95";
            DateParser parser = new DateParser();
            List<DateTime> dates = parser.ParseDates(input);
            Assert.AreEqual(1, dates.Count);
        }

        [TestMethod]
        public void ParseDatesInCommentExpectOneDate()
        {
            string input = "I think 12-31-95 is the canonical phish show";
            DateParser parser = new DateParser();
            List<DateTime> dates = parser.ParseDates(input);
            Assert.AreEqual(1, dates.Count);
        }

        [TestMethod]
        public void ParseDatesDuplicatesExpectOneDate()
        {
            string input = "12-31-95, 12-31-95";
            DateParser parser = new DateParser();
            List<DateTime> dates = parser.ParseDates(input);
            Assert.AreEqual(1, dates.Count);
        }
    }
}
