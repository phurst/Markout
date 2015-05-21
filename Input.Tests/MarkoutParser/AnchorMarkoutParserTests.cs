using System;
using System.Collections.Generic;
using System.Linq;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Elements;
using Markout.Common.DataModel.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Markout.Input.Tests.MarkoutParser {

    [TestClass]
    public class AnchorMarkoutParserTests {

        [TestMethod]
        public void MarkoutParserAnchorTerminated() {
            string input = "0{a:http://www.phurst.com:ProcessUrl}1{a}2";
            Parser.MarkoutParser markdownParser = new Parser.MarkoutParser();
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(3, textRuns.Count);

            Assert.AreEqual("0", textRuns[0].Text);
            Assert.AreEqual(0, textRuns[0].Attributes.Count());

            Assert.AreEqual("1", textRuns[1].Text);
            Assert.AreEqual(1, textRuns[1].Attributes.Count());
            Assert.AreEqual(TextAttributeTypeEnum.Anchor, textRuns[1].Attributes.First().TextAttributeType);
            TextAttributeAnchor taa = textRuns[1].Attributes.First() as TextAttributeAnchor;
            Assert.IsNotNull(taa);
            Assert.AreEqual("http://www.phurst.com/", taa.Uri.ToString());
            Assert.AreEqual("ProcessUrl", taa.ActionName);

            Assert.AreEqual("2", textRuns[2].Text);
            Assert.AreEqual(0, textRuns[2].Attributes.Count());
        }

        [TestMethod]
        public void MarkoutParserAnchorSansUrlTerminated() {
            string input = "0{a::DoSomething}1{a}2";
            Parser.MarkoutParser markdownParser = new Parser.MarkoutParser();
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(3, textRuns.Count);

            Assert.AreEqual("0", textRuns[0].Text);
            Assert.AreEqual(0, textRuns[0].Attributes.Count());

            Assert.AreEqual("1", textRuns[1].Text);
            Assert.AreEqual(1, textRuns[1].Attributes.Count());
            Assert.AreEqual(TextAttributeTypeEnum.Anchor, textRuns[1].Attributes.First().TextAttributeType);
            TextAttributeAnchor taa = textRuns[1].Attributes.First() as TextAttributeAnchor;
            Assert.IsNotNull(taa);
            Assert.IsNull(taa.Uri);
            Assert.AreEqual("DoSomething", taa.ActionName);

            Assert.AreEqual("2", textRuns[2].Text);
            Assert.AreEqual(0, textRuns[2].Attributes.Count());
        }

        [TestMethod]
        public void MarkoutParserAnchorUnterminated() {
            string input = "0{a:http://www.phurst.com:ProcessUrl}1";
            Parser.MarkoutParser markdownParser = new Parser.MarkoutParser();
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(2, textRuns.Count);

            Assert.AreEqual("0", textRuns[0].Text);
            Assert.AreEqual(0, textRuns[0].Attributes.Count());

            Assert.AreEqual("1", textRuns[1].Text);
            Assert.AreEqual(1, textRuns[1].Attributes.Count());
            Assert.AreEqual(TextAttributeTypeEnum.Anchor, textRuns[1].Attributes.First().TextAttributeType);
            TextAttributeAnchor taa = textRuns[1].Attributes.First() as TextAttributeAnchor;
            Assert.IsNotNull(taa);
            Assert.AreEqual("http://www.phurst.com/", taa.Uri.ToString());
            Assert.AreEqual("ProcessUrl", taa.ActionName);
        }

        [TestMethod]
        public void MarkoutParserAnchorMalterminated() {
            string input = "0{a:http://www.phurst.com:ProcessUrl}1{b}2";
            Parser.MarkoutParser markdownParser = new Parser.MarkoutParser();
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(2, textRuns.Count);

            Assert.AreEqual("0", textRuns[0].Text);
            Assert.AreEqual(0, textRuns[0].Attributes.Count());

            Assert.AreEqual("12", textRuns[1].Text);
            Assert.AreEqual(2, textRuns[1].Attributes.Count());

            TextAttributeAnchor taa = textRuns[1].Attributes.OfType<TextAttributeAnchor>().FirstOrDefault() as TextAttributeAnchor;
            Assert.IsNotNull(taa);
            Assert.AreEqual("http://www.phurst.com/", taa.Uri.ToString());
            Assert.AreEqual("ProcessUrl", taa.ActionName);

            TextAttribute tab = textRuns[1].Attributes.FirstOrDefault(ta => ta.TextAttributeType == TextAttributeTypeEnum.Bold) as TextAttribute;
            Assert.IsNotNull(tab);
        }

        [TestMethod]
        public void MarkoutParserAnchorTerminatedWithFontAndColorSpec() {
            string input = "0{a:http://www.phurst.com:ProcessUrl}{f:Times New Roman:16:ub}{c:Cyan}1{a}2";
            Parser.MarkoutParser markdownParser = new Parser.MarkoutParser();
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(3, textRuns.Count);

            Assert.AreEqual("0", textRuns[0].Text);
            Assert.AreEqual(0, textRuns[0].Attributes.Count());

            Assert.AreEqual("1", textRuns[1].Text);
            Assert.AreEqual(3, textRuns[1].Attributes.Count());

            TextAttributeAnchor taa = textRuns[1].Attributes.OfType<TextAttributeAnchor>().FirstOrDefault() as TextAttributeAnchor;
            Assert.IsNotNull(taa);
            Assert.AreEqual("http://www.phurst.com/", taa.Uri.ToString());
            Assert.AreEqual("ProcessUrl", taa.ActionName);

            TextAttributeFont taf = textRuns[1].Attributes.OfType<TextAttributeFont>().FirstOrDefault() as TextAttributeFont;
            Assert.IsNotNull(taf);
            Assert.AreEqual("[Font: Name=Times New Roman, Size=16, Units=3, GdiCharSet=1, GdiVerticalFont=False]", taf.Font.ToString());

            TextAttributeColor tac = textRuns[1].Attributes.OfType<TextAttributeColor>().FirstOrDefault() as TextAttributeColor;
            Assert.IsNotNull(tac);
            Assert.AreEqual("Color [Cyan]", tac.Color.ToString());

            Assert.AreEqual("2", textRuns[2].Text);
            Assert.AreEqual(0, textRuns[2].Attributes.Count());
        }

    }
}
