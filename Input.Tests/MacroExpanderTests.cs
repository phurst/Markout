using System;
using System.Collections.Generic;
using Markout.Input.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Markout.Input.Tests {
    [TestClass]
    public class MacroExpanderTests {

        [TestMethod]
        public void MacroExpanderTest0() {
            MacroExpander macroExpander = new MacroExpander(
                new Dictionary<string, string> {
                    {"m1", "{v1}"},
                    {"m2", "{v2}"},
                    {"m3", "{v3}"},
                    {"m4", "{v4}"},
                });
            string inputText =
                "0{m1}1{m1}2";
            string outputText = macroExpander.ExpandMacros(inputText);
            string expectedText =
                "0{v1}1{v1}2";
            Console.WriteLine("Output:   [{0}]", outputText);
            Console.WriteLine("Expected: [{0}]", expectedText);
            Assert.AreEqual(expectedText, outputText);
        }

        [TestMethod]
        public void MacroExpanderTest1() {
            MacroExpander macroExpander = new MacroExpander(
                new Dictionary<string, string> {
                    {"m1", "{v1}"},
                    {"m2", "{v2}"},
                    {"m3", "{v3}"},
                    {"m4", "{v4}"},
                });
            string inputText =
                "{m1}All About Macros{m1}\n" +
                "A macro looks like this {m2}{{x} Where x is the macro tag{m2}\n" +
                "NB. {m3}Macros are addictive{m3}.\n" +
                "You {m4}HAVE{m4} been warned!";
            string outputText = macroExpander.ExpandMacros(inputText);
            string expectedText =
                "{v1}All About Macros{v1}\n" +
                "A macro looks like this {v2}{{x} Where x is the macro tag{v2}\n" +
                "NB. {v3}Macros are addictive{v3}.\n" +
                "You {v4}HAVE{v4} been warned!";
            Console.WriteLine("Output:   [{0}]", outputText);
            Console.WriteLine("Expected: [{0}]", expectedText);
            Assert.AreEqual(expectedText, outputText);
        }

        [TestMethod]
        public void MacroExpanderTest2() {
            MacroExpander macroExpander = new MacroExpander(
                new Dictionary<string, string> {
                    {"headeron", "{font::16:bu}"},
                    {"headeroff", "{font}"},
                    {"codeon", "{font:Courier:8}{color:DarkGrey}"},
                    {"codeoff", "{font}{color}"},
                    {"emphasison", "{font::12:b}{u}"},
                    {"emphasisoff", "{font}{u}"},
                    {"warningon", "{color:red}"},
                    {"warningoff", "{color}"},
                });
            string inputText =
                "{headeron}All About Macros{headeroff}\n" +
                "A macro looks like this {codeon}{{x} Where x is the macro tag{codeoff}\n" +
                "NB. {warningon}Macros are addictive{warningoff}.\n" +
                "You {emphasison}HAVE{emphasisoff} been warned!";
            string outputText = macroExpander.ExpandMacros(inputText);
            string expectedText =
                "{font::16:bu}All About Macros{font}\n" +
                "A macro looks like this {font:Courier:8}{color:DarkGrey}{{x} Where x is the macro tag{font}{color}\n" +
                "NB. {color:red}Macros are addictive{color}.\n" +
                "You {font::12:b}{u}HAVE{font}{u} been warned!";
            Console.WriteLine("Output: [{0}]", outputText);
            Console.WriteLine("Expected: [{0}]", expectedText);
            Assert.AreEqual(expectedText, outputText);
        }
    }
}
