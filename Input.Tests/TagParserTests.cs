using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;
using Markout.Input.Parser;
using Markout.Input.Tags;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#pragma warning disable 219

namespace Markout.Input.Tests {

    [TestClass]
    public class TagParserTests {

        [TestMethod]
        public void TagParserParseSimpleTags() {
            string input = "1{b}2{i}3{u}4";
            string posit = "0123456789|123456789";
            TagParser tagParser = new TagParser();
            List<Tag> tags = tagParser.Parse(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagB = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Bold);
            Assert.IsNotNull(tagB);
            Assert.AreEqual(1, tagB.StartIndex);
            Assert.AreEqual(4, tagB.TrailIndex);
            Assert.IsNotNull(tagB.Attribute);
            Assert.AreEqual(TextAttributeTypeEnum.Bold, tagB.Attribute.TextAttributeType);

            Tag tagI = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Italic);
            Assert.IsNotNull(tagI);
            Assert.AreEqual(5, tagI.StartIndex);
            Assert.AreEqual(8, tagI.TrailIndex);
            Assert.IsNotNull(tagI.Attribute);
            Assert.AreEqual(TextAttributeTypeEnum.Italic, tagI.Attribute.TextAttributeType);

            Tag tagU = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Underline);
            Assert.IsNotNull(tagU);
            Assert.AreEqual(9, tagU.StartIndex);
            Assert.AreEqual(12, tagU.TrailIndex);
            Assert.IsNotNull(tagU.Attribute);
            Assert.AreEqual(TextAttributeTypeEnum.Underline, tagU.Attribute.TextAttributeType);
        }

        [TestMethod]
        public void TagParserParseAdjacentSimpleTags() {
            string input = "1{b}{i}2{b}{u}3";
            string posit = "0123456789|123456789";
            TagParser tagParser = new TagParser();
            List<Tag> tags = tagParser.Parse(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagB0 = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Bold);
            Assert.IsNotNull(tagB0);
            Assert.AreEqual(1, tagB0.StartIndex);
            Assert.AreEqual(4, tagB0.TrailIndex);
            Assert.IsNotNull(tagB0.Attribute);
            Assert.AreEqual(TextAttributeTypeEnum.Bold, tagB0.Attribute.TextAttributeType);

            Tag tagI = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Italic);
            Assert.IsNotNull(tagI);
            Assert.AreEqual(4, tagI.StartIndex);
            Assert.AreEqual(7, tagI.TrailIndex);
            Assert.IsNotNull(tagI.Attribute);
            Assert.AreEqual(TextAttributeTypeEnum.Italic, tagI.Attribute.TextAttributeType);

            Tag tagB1 = tags.Skip(1).FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Bold);
            Assert.IsNotNull(tagB1);
            Assert.AreEqual(8, tagB1.StartIndex);
            Assert.AreEqual(11, tagB1.TrailIndex);
            Assert.IsNotNull(tagB1.Attribute);
            Assert.AreEqual(TextAttributeTypeEnum.Bold, tagB1.Attribute.TextAttributeType);

            Tag tagU = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Underline);
            Assert.IsNotNull(tagU);
            Assert.AreEqual(11, tagU.StartIndex);
            Assert.AreEqual(14, tagU.TrailIndex);
            Assert.IsNotNull(tagU.Attribute);
            Assert.AreEqual(TextAttributeTypeEnum.Underline, tagU.Attribute.TextAttributeType);
        }

        [TestMethod]
        public void TagParserParseFontOnTag() {
            string input = "12{font:Times New Roman:10:b}2";
            string posit = "0123456789|123456789|123456789";
            TagParser tagParser = new TagParser();
            List<Tag> tags = tagParser.Parse(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagF = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Font);
            Assert.IsNotNull(tagF);
            Assert.AreEqual(2, tagF.StartIndex);
            Assert.AreEqual(29, tagF.TrailIndex);

            Assert.IsNotNull(tagF.Attribute);
            TextAttributeFont taf = tagF.Attribute as TextAttributeFont;
            Assert.IsNotNull(taf);
            Assert.IsNotNull(taf.Font);
            Assert.AreEqual("Times New Roman", taf.Font.Name);
            Assert.AreEqual(10, taf.Font.Size);
            Assert.IsTrue(taf.Font.Bold);
            Assert.IsFalse(taf.Font.Italic);
            Assert.IsFalse(taf.Font.Underline);
        }

        [TestMethod]
        public void TagParserParseFontOffTag() {
            string input = "1{f}2";
            string posit = "0123456789|123456789|123456789";
            TagParser tagParser = new TagParser();
            List<Tag> tags = tagParser.Parse(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagF = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Font);
            Assert.IsNotNull(tagF);
            Assert.AreEqual(1, tagF.StartIndex);
            Assert.AreEqual(4, tagF.TrailIndex);

            Assert.IsNotNull(tagF.Attribute);
        }

        [TestMethod]
        public void TagParserParseColorOnTag() {
            string input = "123{c:Red}2";
            string posit = "0123456789|123456789|123456789";
            TagParser tagParser = new TagParser();
            List<Tag> tags = tagParser.Parse(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagC = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Color);
            Assert.IsNotNull(tagC);
            Assert.AreEqual(3, tagC.StartIndex);
            Assert.AreEqual(10, tagC.TrailIndex);

            Assert.IsNotNull(tagC.Attribute);
            TextAttributeColor tac = tagC.Attribute as TextAttributeColor;
            Assert.IsNotNull(tac);
            Assert.IsNotNull(tac.Color);
            Assert.AreEqual(Color.FromName(KnownColor.Red.ToString()), tac.Color);
        }

        [TestMethod]
        public void TagParserParseColorOffTag() {
            string input = "123{colour}2";
            string posit = "0123456789|123456789|123456789";
            TagParser tagParser = new TagParser();
            List<Tag> tags = tagParser.Parse(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagC = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Color);
            Assert.IsNotNull(tagC);
            Assert.AreEqual(3, tagC.StartIndex);
            Assert.AreEqual(11, tagC.TrailIndex);

            Assert.IsNotNull(tagC.Attribute);
        }

        [TestMethod]
        public void TagParserParseAnchorOnTag() {
            string input = "123{a:http://google.com/index.html}456";
            string posit = "0123456789|123456789|123456789|123456789|123456789";
            TagParser tagParser = new TagParser();
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
        public void TagParserParseAnchorOffTag() {
            string input = "123{anchor}2";
            string posit = "0123456789|123456789|123456789";
            TagParser tagParser = new TagParser();
            List<Tag> tags = tagParser.Parse(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagA = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.Anchor);
            Assert.IsNotNull(tagA);
            Assert.AreEqual(3, tagA.StartIndex);
            Assert.AreEqual(11, tagA.TrailIndex);

            Assert.IsNotNull(tagA.Attribute);
        }
    }
}
