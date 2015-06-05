using System;
using System.Collections.Generic;
using System.Linq;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;
using Markout.Input.Tags;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#pragma warning disable 219

namespace Markout.Input.Tests.TagParser {

    [TestClass]
    public class ParagraphTagParserTests {

        [TestMethod]
        public void TagParserParseParagraphTag() {
            string input = "1{p}2{p}3";
            string posit = "0123456789|123456789|123456789|123456789|123456789";
            Parser.TagParser tagParser = new Parser.TagParser();
            List<Tag> tags = tagParser.Parse(input).ToList();
            Assert.AreEqual(2, tags.Count);
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tag0 = tags[0];
            Assert.AreEqual(1, tag0.StartIndex);
            Assert.AreEqual(4, tag0.TrailIndex);

            Tag tag1 = tags[1];
            Assert.AreEqual(5, tag1.StartIndex);
            Assert.AreEqual(8, tag1.TrailIndex);
        }
    }
}
