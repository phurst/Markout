using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Markout.Input.Tests.MacroDefinitionParser {

    [TestClass]
    public class MacroDefinitionParserTests {

        [TestMethod]
        public void ParsePlainText() {
            Parser.MacroDefinitionParser parser = new Parser.MacroDefinitionParser();
            string inputText = "\n" +
                               " \n" +
                               " \t\n" +
                               " ; A comment\n" +
                               "a {b}{i}\n" +
                               " c { d } \n" +
                               "\te\t{f} {g}\t\n" +
                               "";
            Dictionary<string, string> macros = parser.ParseMacroDefinitionsFromText(inputText);
            Assert.AreEqual(3, macros.Count);
            Assert.AreEqual("{b}{i}", macros["a"]);
            Assert.AreEqual("{ d }", macros["c"]);
            Assert.AreEqual("{f} {g}", macros["e"]);
        }
    }
}
