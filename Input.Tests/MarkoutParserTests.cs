﻿using System;
using System.Collections.Generic;
using System.Linq;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Elements;
using Markout.Common.DataModel.Enumerations;
using Markout.Input.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Markout.Input.Tests {

    [TestClass]
    public class MarkoutParserTests {

        [TestMethod]
        public void MarkoutParserParseSimpleTags() {
            string input = "0{b}1{i}2{u}3";
            MarkoutParser markdownParser = new MarkoutParser();
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(4, textRuns.Count);

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
        }

        [TestMethod]
        public void MarkoutParserOnAndOffSimpleTags() {
            string input = "0{b}1{b}2";
            MarkoutParser markdownParser = new MarkoutParser();
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(3, textRuns.Count);

            TextRun tr0 = textRuns[0] as TextRun;
            Assert.IsNotNull(tr0);
            Assert.AreEqual("0", tr0.Text);
            Assert.AreEqual(0, tr0.Attributes.Count());

            TextRun tr1 = textRuns[1] as TextRun;
            Assert.IsNotNull(tr1);
            Assert.AreEqual("1", tr1.Text);
            Assert.AreEqual(1, tr1.Attributes.Count());
            Assert.IsTrue(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Bold));

            TextRun tr2 = textRuns[2] as TextRun;
            Assert.IsNotNull(tr2);
            Assert.AreEqual("2", tr2.Text);
            Assert.AreEqual(0, tr2.Attributes.Count());
        }

        [TestMethod]
        public void MarkoutParserOnAndOffAdjacentSimpleTags() {
            string input = "0{b}{u}1{b}2{u}{i}3";
            MarkoutParser markdownParser = new MarkoutParser();
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(4, textRuns.Count);

            TextRun tr0 = textRuns[0] as TextRun;
            Assert.IsNotNull(tr0);
            Assert.AreEqual("0", tr0.Text);
            Assert.AreEqual(0, tr0.Attributes.Count());

            TextRun tr1 = textRuns[1] as TextRun;
            Assert.IsNotNull(tr1);
            Assert.AreEqual("1", tr1.Text);
            Assert.AreEqual(2, tr1.Attributes.Count());
            Assert.IsTrue(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Bold));
            Assert.IsTrue(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Underline));

            TextRun tr2 = textRuns[2] as TextRun;
            Assert.IsNotNull(tr2);
            Assert.AreEqual("2", tr2.Text);
            Assert.AreEqual(1, tr2.Attributes.Count());
            Assert.IsTrue(tr2.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Underline));

            TextRun tr3 = textRuns[3] as TextRun;
            Assert.IsNotNull(tr3);
            Assert.AreEqual("3", tr3.Text);
            Assert.AreEqual(1, tr3.Attributes.Count());
            Assert.IsTrue(tr3.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Italic));
        }

        [TestMethod]
        public void MarkoutParserOverlapSimpleTags() {
            string input = "0{b}1{i}2{b}3{u}4{i}5";
            MarkoutParser markdownParser = new MarkoutParser();
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(6, textRuns.Count);

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
            Assert.AreEqual(1, tr3.Attributes.Count());
            Assert.IsTrue(tr3.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Italic));

            TextRun tr4 = textRuns[4] as TextRun;
            Assert.IsNotNull(tr4);
            Assert.AreEqual("4", tr4.Text);
            Assert.AreEqual(2, tr4.Attributes.Count());
            Assert.IsTrue(tr4.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Italic));
            Assert.IsTrue(tr4.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Underline));

            TextRun tr5 = textRuns[5] as TextRun;
            Assert.IsNotNull(tr5);
            Assert.AreEqual("5", tr5.Text);
            Assert.AreEqual(1, tr5.Attributes.Count());
            Assert.IsTrue(tr5.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Underline));
        }

        [TestMethod]
        public void MarkoutParserAnchorTerminated() {
            string input = "0{a:http://www.phurst.com:ProcessUrl}1{a}2";
            MarkoutParser markdownParser = new MarkoutParser();
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
        public void MarkoutParserAnchorUnterminated() {
            string input = "0{a:http://www.phurst.com:ProcessUrl}1";
            MarkoutParser markdownParser = new MarkoutParser();
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
            MarkoutParser markdownParser = new MarkoutParser();
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
            Assert.AreEqual(1, textRuns[2].Attributes.Count());
            Assert.AreEqual(TextAttributeTypeEnum.Bold, textRuns[2].Attributes.First().TextAttributeType);
        }

        [TestMethod]
        public void MarkoutParserParseSimpleTagsWithMacros() {
            string input = "0{bolditalic}1{bolditalic}2{bigredon}3{bigredoff}4";
            MarkoutParser markdownParser = new MarkoutParser();
            markdownParser.Macros = new Dictionary<string, string> {
                {"bolditalic", "{b}{i}"},
                {"bigredon", "{font:Times New Roman:24}{color:red}"},
                {"bigredoff", "{font}{color}"},
            };
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(5, textRuns.Count);

            TextRun tr0 = textRuns[0] as TextRun;
            Assert.IsNotNull(tr0);
            Assert.AreEqual("0", tr0.Text);
            Assert.AreEqual(0, tr0.Attributes.Count());

            TextRun tr1 = textRuns[1] as TextRun;
            Assert.IsNotNull(tr1);
            Assert.AreEqual("1", tr1.Text);
            Assert.AreEqual(2, tr1.Attributes.Count());
            Assert.IsTrue(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Bold));
            Assert.IsTrue(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Italic));

            TextRun tr2 = textRuns[2] as TextRun;
            Assert.IsNotNull(tr2);
            Assert.AreEqual("2", tr2.Text);
            Assert.AreEqual(0, tr2.Attributes.Count());

            TextRun tr3 = textRuns[3] as TextRun;
            Assert.IsNotNull(tr3);
            Assert.AreEqual("3", tr3.Text);
            Assert.AreEqual(2, tr3.Attributes.Count());
            Assert.IsTrue(tr3.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Font));
            Assert.IsTrue(tr3.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Color));

            TextRun tr4 = textRuns[4] as TextRun;
            Assert.IsNotNull(tr4);
            Assert.AreEqual("4", tr4.Text);
            Assert.AreEqual(0, tr4.Attributes.Count());
        }
    }
}
