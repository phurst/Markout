using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Markout.Input.Tests {

    [TestClass]
    public class RegexTests {

        [TestMethod]
        public void MatchSingleTag() {
            const string regexString = @"(?<=^|[^\{])\{\s*(?<tag>b)\s*(?(:):\s*(?<qualifier>[^\}]*)|)\}";
            Regex regex = new Regex(regexString);
            const string input = "1{b}2";
            MatchCollection matches = regex.Matches(input);
            Assert.AreEqual(1, matches.Count);
            Match match = matches[0];
            Assert.AreEqual("{b}", match.Value);
        }

        [TestMethod]
        public void MatchAdjacentTags() {
            const string regexString = @"(?<=^|[^\{])\{\s*(?<tag>b)\s*(?(:):\s*(?<qualifier>[^\}]*)|)\}";
            Regex regex = new Regex(regexString);
            const string input = "1{b}{b}2";
            MatchCollection matches = regex.Matches(input);
            Assert.IsTrue(matches.Count > 0);
            Match match0 = matches[0];
            Assert.AreEqual("{b}", match0.Value);
            Assert.IsTrue(matches.Count > 1);
            Match match1 = matches[1];
            Assert.AreEqual("{b}", match1.Value);
        }

        [TestMethod]
        public void SimpleAdjacentTags() {
            const string regexString = @"(\{\S\})";
            Regex regex = new Regex(regexString);
            const string input = "1{a}{b}2";
            MatchCollection matches = regex.Matches(input);
            Assert.AreEqual(2, matches.Count);
            Assert.AreEqual("{a}", matches[0].Value);
            Assert.AreEqual("{b}", matches[1].Value);
        }

        [TestMethod]
        public void SimpleAdjacentTags2() {
            const string regexString = @"(?<=[^\{])(\{\S\})";
            Regex regex = new Regex(regexString);
            const string input = "1{a}{b}2";
            MatchCollection matches = regex.Matches(input);
            Assert.AreEqual(2, matches.Count);
            Assert.AreEqual("{a}", matches[0].Value);
            Assert.AreEqual("{b}", matches[1].Value);
        }

        [TestMethod]
        public void SimpleAdjacentTags3() {
            const string regexString = @"(?<=[^\{])(\{\S\})";
            Regex regex = new Regex(regexString);
            const string input = "1{a}{{b}2";
            MatchCollection matches = regex.Matches(input);
            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual("{a}", matches[0].Value);
        }
    }
}
