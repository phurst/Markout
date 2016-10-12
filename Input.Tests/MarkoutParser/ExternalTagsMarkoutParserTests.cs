using System;
using System.Collections.Generic;
using System.Linq;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Elements;
using Markout.Common.DataModel.Enumerations;
using Markout.Input.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#pragma warning disable 219

namespace Markout.Input.Tests.MarkoutParser {

    [TestClass]
    public class ExternalTagsMarkoutParserTests {

        private class TestTagResolver : IExternalTagResolver {
            public string Resolve(TextAttributeExternal textAttributeExternal) {
                if (textAttributeExternal.Name == "InsertTextHere") {
                    return textAttributeExternal.Parameter ?? "SomeText";
                } else {
                    return string.Empty;
                }
            }
        }

        private class DayTagResolver : IExternalTagResolver {
            public string Resolve(TextAttributeExternal textAttributeExternal) {
                if (textAttributeExternal.Name == "Day" && !string.IsNullOrWhiteSpace(textAttributeExternal.Parameter)) {
                    switch(textAttributeExternal.Parameter) {
                        case "Today": 
                            return "Monday";
                        case "Tomorrow":
                            return "Tuesday";
                        default:
                            return string.Empty;
                    }
                } else {
                    return string.Empty;
                }
            }
        }

        [TestMethod]
        public void ExternalValueSubstOnly() {
            string input = "0{x:InsertTextHere}1";
            string posit = "0123456789|123456789|123456789|123456789|123456789";
            Parser.MarkoutParser markdownParser = new Parser.MarkoutParser();
            markdownParser.ExternalTagResolvers = new Dictionary<string, IExternalTagResolver> {
                { "InsertTextHere", new TestTagResolver() }
            };
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(1, textRuns.Count);

            Assert.AreEqual("0SomeText1", textRuns[0].Text);
            Assert.AreEqual(0, textRuns[0].Attributes.Count());
        }

        [TestMethod]
        public void ExternalValueSubstComplex() {
            string input = "0{x:InsertTextHere}1{external:Day:Today}{x:Day:Tomorrow}2";
            string posit = "0123456789|123456789|123456789|123456789|123456789";
            Parser.MarkoutParser markdownParser = new Parser.MarkoutParser();
            markdownParser.ExternalTagResolvers = new Dictionary<string, IExternalTagResolver> {
                { "InsertTextHere", new TestTagResolver() },
                { "Day", new DayTagResolver() },
            };
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(1, textRuns.Count);

            Assert.AreEqual("0SomeText1MondayTuesday2", textRuns[0].Text);
            Assert.AreEqual(0, textRuns[0].Attributes.Count());
        }

        [TestMethod]
        public void MarkoutParserExternalPlain() {
            string input = "0{x:InsertTextHere}1";
            string posit = "0123456789|123456789|123456789|123456789|123456789";
            Parser.MarkoutParser markdownParser = new Parser.MarkoutParser();
            markdownParser.ExternalTagResolvers = new Dictionary<string, IExternalTagResolver> {
                { "InsertTextHere", new TestTagResolver() }
            };
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(1, textRuns.Count);

            Assert.AreEqual("0SomeText1", textRuns[0].Text);
            Assert.AreEqual(0, textRuns[0].Attributes.Count());
        }


        [TestMethod]
        public void MarkoutParserExternalLeadingAttribute() {
            string input = "0{b}{x:InsertTextHere}1";
            Parser.MarkoutParser markdownParser = new Parser.MarkoutParser();
            markdownParser.ExternalTagResolvers = new Dictionary<string, IExternalTagResolver> {
                { "InsertTextHere", new TestTagResolver() }
            };
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(2, textRuns.Count);

            Assert.AreEqual("0", textRuns[0].Text);
            Assert.AreEqual(0, textRuns[0].Attributes.Count());

            Assert.AreEqual("SomeText1", textRuns[1].Text);
            Assert.AreEqual(1, textRuns[1].Attributes.Count());
            Assert.AreEqual(TextAttributeTypeEnum.Bold, textRuns[1].Attributes.First().TextAttributeType);
            Assert.IsTrue(textRuns[1].Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Bold));
        }

        [TestMethod]
        public void MarkoutParserExternalTrailingAttribute() {
            string input = "0{b}{x:InsertTextHere}{i}1";
            Parser.MarkoutParser markdownParser = new Parser.MarkoutParser();
            markdownParser.ExternalTagResolvers = new Dictionary<string, IExternalTagResolver> {
                { "InsertTextHere", new TestTagResolver() }
            };
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(3, textRuns.Count);

            Assert.AreEqual("0", textRuns[0].Text);
            Assert.AreEqual(0, textRuns[0].Attributes.Count());

            Assert.AreEqual("SomeText", textRuns[1].Text);
            Assert.AreEqual(1, textRuns[1].Attributes.Count());
            Assert.IsTrue(textRuns[1].Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Bold));

            Assert.AreEqual("1", textRuns[2].Text);
            Assert.AreEqual(2, textRuns[2].Attributes.Count());
            Assert.IsTrue(textRuns[2].Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Bold));
            Assert.IsTrue(textRuns[2].Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Italic));
        }

        [TestMethod]
        public void MarkoutParserExternalTrailingAttributeAndInvalidExternalTag() {
            string input = "0{b}{x:InsertTextHere}{x:NotATagName:foo}{i}1";
            Parser.MarkoutParser markdownParser = new Parser.MarkoutParser();
            markdownParser.ExternalTagResolvers = new Dictionary<string, IExternalTagResolver> {
                { "InsertTextHere", new TestTagResolver() }
            };
            List<TextRun> textRuns = markdownParser.Parse(input).ToList();
            textRuns.ForEach(tr => Console.WriteLine(tr.ToString()));
            Assert.AreEqual(3, textRuns.Count);

            Assert.AreEqual("0", textRuns[0].Text);
            Assert.AreEqual(0, textRuns[0].Attributes.Count());

            Assert.AreEqual("SomeText", textRuns[1].Text);
            Assert.AreEqual(1, textRuns[1].Attributes.Count());
            Assert.IsTrue(textRuns[1].Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Bold));

            Assert.AreEqual("1", textRuns[2].Text);
            Assert.AreEqual(2, textRuns[2].Attributes.Count());
            Assert.IsTrue(textRuns[2].Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Bold));
            Assert.IsTrue(textRuns[2].Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Italic));
        }
    }
}
