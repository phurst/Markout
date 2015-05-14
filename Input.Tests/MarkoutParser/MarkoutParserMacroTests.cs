using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Elements;
using Markout.Common.DataModel.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Markout.Input.Tests.MarkoutParser {

    [TestClass]
    public class MarkoutParserMacroTests {

        [TestMethod]
        public void PaseTextWithMacros() {
            Parser.MacroDefinitionParser parser = new Parser.MacroDefinitionParser();
            string macroInputText = "bolditalic {b}{i}\n" +
                                    "heading {c:Blue}{f:Franklin Gothic Medium:16:b}\n" +
                                    "\theadingoff\t{c}{f}\t\n" +
                                    "";
            Dictionary<string, string> macros = parser.ParseMacroDefinitionsFromText(macroInputText);
            string markupInputText = "{heading}Heading1{headingoff}\n" +
                                     "a{bolditalic}b{0}c";
            Parser.MarkoutParser markoutParser = new Parser.MarkoutParser {Macros = macros};
            List<TextRun> textRuns = markoutParser.Parse(markupInputText).ToList();

            Assert.AreEqual(4, textRuns.Count);

            TextRun tr0 = textRuns[0];
            Assert.AreEqual("Heading1", tr0.Text);
            TextAttributeColor tac = tr0.Attributes.FirstOrDefault(a => a.TextAttributeType == TextAttributeTypeEnum.Color) as TextAttributeColor;
            Assert.IsNotNull(tac);
            Assert.AreEqual("Color [Blue]", tac.Color.ToString());
            TextAttributeFont taf = tr0.Attributes.FirstOrDefault(a => a.TextAttributeType == TextAttributeTypeEnum.Font) as TextAttributeFont;
            Assert.IsNotNull(taf);
            Assert.AreEqual("[Font: Name=Franklin Gothic Medium, Size=16, Units=3, GdiCharSet=1, GdiVerticalFont=False]", taf.Font.ToString());

            TextRun tr1 = textRuns[1];
            Assert.AreEqual("\na", tr1.Text);
            Assert.AreEqual(0, tr1.Attributes.Count());

            TextRun tr2 = textRuns[2];
            Assert.AreEqual("b", tr2.Text);
            Assert.IsTrue(tr2.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Bold));
            Assert.IsTrue(tr2.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Italic));

            TextRun tr3 = textRuns[3];
            Assert.AreEqual("c", tr3.Text);
            Assert.AreEqual(0, tr3.Attributes.Count());
        }
    }
}
