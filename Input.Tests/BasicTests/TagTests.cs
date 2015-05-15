using System;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;
using Markout.Input.Tags;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Markout.Input.Tests.BasicTests {
    [TestClass]
    public class TagTests {

        [TestMethod]
        public void TagContiguityTest1() {
            Tag tag0 = new Tag {
                Attribute = new TextAttribute { TextAttributeType = TextAttributeTypeEnum.Bold },
                StartIndex = 0,
                TrailIndex = 3,
            };
            Tag tag1 = new Tag {
                Attribute = new TextAttribute { TextAttributeType = TextAttributeTypeEnum.Italic },
                StartIndex = 3,
                TrailIndex = 6,
            };
            Tag tag2 = new Tag {
                Attribute = new TextAttribute { TextAttributeType = TextAttributeTypeEnum.Underline },
                StartIndex = 6,
                TrailIndex = 9,
            };
            Assert.IsTrue(tag0.IsContiguousWith(tag1));
            Assert.IsTrue(tag1.IsContiguousWith(tag0));
            Assert.IsTrue(tag1.IsContiguousWith(tag2));
            Assert.IsTrue(tag2.IsContiguousWith(tag1));
            Assert.IsFalse(tag0.IsContiguousWith(tag2));
            Assert.IsFalse(tag2.IsContiguousWith(tag0));
        }

        [TestMethod]
        public void TagContiguityTest2() {
            Tag tag0 = new Tag {
                Attribute = new TextAttribute { TextAttributeType = TextAttributeTypeEnum.Bold },
                StartIndex = 0,
                TrailIndex = 3,
            };
            Tag tag1 = new Tag {
                Attribute = new TextAttribute { TextAttributeType = TextAttributeTypeEnum.Italic },
                StartIndex = 4,
                TrailIndex = 7,
            };
            Assert.IsFalse(tag0.IsContiguousWith(tag1));
            Assert.IsFalse(tag1.IsContiguousWith(tag0));
        }
    }
}
