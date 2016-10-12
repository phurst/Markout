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
    public class ExternalTagParserTests {

        [TestMethod]
        public void TagParserParseExternalOnTag() {
            string input = "123{x:rats:dogs}456";
            string posit = "0123456789|123456789|123456789|123456789|123456789";
            Parser.TagParser tagParser = new Parser.TagParser();
            List<Tag> tags = tagParser.ParseExternalTags(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagA = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.External);
            Assert.IsNotNull(tagA);
            Assert.AreEqual(3, tagA.StartIndex);
            Assert.AreEqual(16, tagA.TrailIndex);

            Assert.IsNotNull(tagA.Attribute);
            TextAttributeExternal taa = tagA.Attribute as TextAttributeExternal;
            Assert.IsNotNull(taa);
            Assert.IsNotNull(taa.Name);
            Assert.AreEqual("rats", taa.Name);
            Assert.IsNotNull(taa.Parameter);
            Assert.AreEqual("dogs", taa.Parameter);
        }

        [TestMethod]
        public void TagParserParseExternalOnTagSansParameter() {
            string input = "123{x:rats}456";
            string posit = "0123456789|123456789|123456789|123456789|123456789";
            Parser.TagParser tagParser = new Parser.TagParser();
            List<Tag> tags = tagParser.ParseExternalTags(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagA = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.External);
            Assert.IsNotNull(tagA);
            Assert.AreEqual(3, tagA.StartIndex);
            Assert.AreEqual(11, tagA.TrailIndex);

            Assert.IsNotNull(tagA.Attribute);
            TextAttributeExternal taa = tagA.Attribute as TextAttributeExternal;
            Assert.IsNotNull(taa);
            Assert.IsNotNull(taa.Name);
            Assert.AreEqual("rats", taa.Name);
            Assert.IsNull(taa.Parameter);
        }

        [TestMethod]
        public void TagParserParseExternalOnTagWithSpaces() {
            string input = "123{external: rats : }456";
            string posit = "0123456789|123456789|123456789|123456789|123456789";
            Parser.TagParser tagParser = new Parser.TagParser();
            List<Tag> tags = tagParser.ParseExternalTags(input).ToList();
            Assert.IsTrue(tags.Any());
            tags.ForEach(t => Console.WriteLine("\t" + t.GetDescription()));

            Tag tagA = tags.FirstOrDefault(t => t.TextAttributeType == TextAttributeTypeEnum.External);
            Assert.IsNotNull(tagA);
            Assert.AreEqual(3, tagA.StartIndex);
            Assert.AreEqual(22, tagA.TrailIndex);

            Assert.IsNotNull(tagA.Attribute);
            TextAttributeExternal taa = tagA.Attribute as TextAttributeExternal;
            Assert.IsNotNull(taa);
            Assert.IsNotNull(taa.Name);
            Assert.AreEqual("rats", taa.Name);
            Assert.IsNull(taa.Parameter);
        }
    }
}
