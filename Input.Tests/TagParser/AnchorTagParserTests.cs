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
    public class AnchorTagParserTests {

        [TestMethod]
        public void TagParserParseAnchorOnTag() {
            string input = "123{a:http://google.com/index.html}456";
            string posit = "0123456789|123456789|123456789|123456789|123456789";
            Parser.TagParser tagParser = new Parser.TagParser();
            List<Tag> tags = tagParser.Parse(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagA = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Anchor);
            Assert.IsNotNull(tagA);
            Assert.AreEqual(3, tagA.StartIndex);
            Assert.AreEqual(35, tagA.TrailIndex);

            Assert.IsNotNull(tagA.Attribute);
            TextAttributeAnchor taa = tagA.Attribute as TextAttributeAnchor;
            Assert.IsNotNull(taa);
            Assert.IsNotNull(taa.Uri);
            Assert.AreEqual("http://google.com/index.html", taa.Uri.ToString());
        }

        [TestMethod]
        public void TagParserParseAnchorOnTagWithSpaces() {
            string input = "123{a: http://google.com/index.html }456";
            string posit = "0123456789|123456789|123456789|123456789|123456789";
            Parser.TagParser tagParser = new Parser.TagParser();
            List<Tag> tags = tagParser.Parse(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagA = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Anchor);
            Assert.IsNotNull(tagA);
            Assert.AreEqual(3, tagA.StartIndex);
            Assert.AreEqual(37, tagA.TrailIndex);

            Assert.IsNotNull(tagA.Attribute);
            TextAttributeAnchor taa = tagA.Attribute as TextAttributeAnchor;
            Assert.IsNotNull(taa);
            Assert.IsNotNull(taa.Uri);
            Assert.AreEqual("http://google.com/index.html", taa.Uri.ToString());
        }

        [TestMethod]
        public void TagParserParseAnchorOffTag() {
            string input = "123{anchor}2";
            string posit = "0123456789|123456789|123456789";
            Parser.TagParser tagParser = new Parser.TagParser();
            List<Tag> tags = tagParser.Parse(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagA = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Anchor);
            Assert.IsNotNull(tagA);
            Assert.AreEqual(3, tagA.StartIndex);
            Assert.AreEqual(11, tagA.TrailIndex);

            Assert.IsNotNull(tagA.Attribute);
        }

        [TestMethod]
        public void TagParserParseWithNonUrlAnchorInfo() {
            string input = "123{a:=BB(\"AA\",\"01/01/2015\"):ActionName}Hello{a}";
            string posit = "0123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789";
            Parser.TagParser tagParser = new Parser.TagParser();
            List<Tag> tags = tagParser.Parse(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagA = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Anchor);
            Assert.IsNotNull(tagA);
            Assert.AreEqual(3, tagA.StartIndex);
            Assert.AreEqual(40, tagA.TrailIndex);

            Assert.IsNotNull(tagA.Attribute);
            TextAttributeAnchor taa = tagA.Attribute as TextAttributeAnchor;
            Assert.IsNotNull(taa);
            Assert.IsNull(taa.Uri);
            Assert.IsNotNull(taa.AnchorInfo);
            Assert.AreEqual("=BB(\"AA\",\"01/01/2015\")", taa.AnchorInfo);
        }
    }
}
