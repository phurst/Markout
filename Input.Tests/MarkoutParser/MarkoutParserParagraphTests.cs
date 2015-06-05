using System;
using System.Collections.Generic;
using System.Linq;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Elements;
using Markout.Common.DataModel.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Markout.Input.Tests.MarkoutParser {

    [TestClass]
    public class MarkoutParserParagraphTests {

        [TestMethod]
        public void MarkoutParserParseParagraphTags() {
            string input = "0{p}1{i}2{p}3{0}4";
            Parser.MarkoutParser markoutParser = new Parser.MarkoutParser();
            List<TextRun> textRuns = markoutParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(5, textRuns.Count);

            TextRun tr0 = textRuns[0] as TextRun;
            Assert.IsNotNull(tr0);
            Assert.AreEqual("0", tr0.Text);
            Assert.AreEqual(0, tr0.Attributes.Count());

            TextRun tr1 = textRuns[1] as TextRun;
            Assert.IsNotNull(tr1);
            Assert.AreEqual("1", tr1.Text);
            Assert.AreEqual(1, tr1.Attributes.Count());
            Assert.AreEqual(TextAttributeTypeEnum.Bold, tr1.Attributes.First().TextAttributeType);

            TextRun tr2 = textRuns[2] as TextRun;
            Assert.IsNotNull(tr2);
            Assert.AreEqual("2", tr2.Text);
            Assert.AreEqual(2, tr2.Attributes.Count());
            Assert.IsTrue(tr2.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Bold));
            Assert.IsTrue(tr2.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Italic));

            TextRun tr3 = textRuns[3] as TextRun;
            Assert.IsNotNull(tr3);
            Assert.AreEqual("3", tr3.Text);
            Assert.AreEqual(3, tr3.Attributes.Count());
            Assert.IsTrue(tr3.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Bold));
            Assert.IsTrue(tr3.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Italic));
            Assert.IsTrue(tr3.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Underline));

            TextRun tr4 = textRuns[4] as TextRun;
            Assert.IsNotNull(tr4);
            Assert.AreEqual("4", tr4.Text);
            Assert.AreEqual(0, tr4.Attributes.Count());
        }
    }
}
