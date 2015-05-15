using System;
using System.Linq;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Elements;
using Markout.Common.DataModel.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Markout.Common.Tests.Elements {

    [TestClass]
    public class TextRunTests {

        [TestMethod]
        public void GetAttributesFromOtherTest1() {
            TextRun tr0 = new TextRun {
                Attributes = new[] {
                    new TextAttribute {TextAttributeType = TextAttributeTypeEnum.Bold},
                    new TextAttribute {TextAttributeType = TextAttributeTypeEnum.Italic},
                    new TextAttribute {TextAttributeType = TextAttributeTypeEnum.Underline},
                },
            };
            TextRun tr1 = new TextRun {};
            Assert.AreEqual(3, tr0.Attributes.Count());
            Assert.AreEqual(0, tr1.Attributes.Count());

            tr1.GetAttributesFrom(tr0);
            Assert.AreEqual(3, tr1.Attributes.Count());
            Assert.IsTrue(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Bold));
            Assert.IsTrue(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Italic));
            Assert.IsTrue(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Underline));
        }

        [TestMethod]
        public void GetAttributesFromOtherTest2() {
            TextRun tr0 = new TextRun { 
                Attributes = new [] {
                    new TextAttribute { TextAttributeType = TextAttributeTypeEnum.Bold },
                    new TextAttribute { TextAttributeType = TextAttributeTypeEnum.Italic },
                },
            };
            TextRun tr1 = new TextRun {
                Attributes = new[] {
                    new TextAttribute { TextAttributeType = TextAttributeTypeEnum.Bold },
                    new TextAttribute { TextAttributeType = TextAttributeTypeEnum.Underline },
                },
            };
            Assert.AreEqual(2, tr0.Attributes.Count());
            Assert.AreEqual(2, tr1.Attributes.Count());

            tr1.GetAttributesFrom(tr0);
            Assert.AreEqual(2, tr1.Attributes.Count());
            Assert.IsTrue(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Italic));
            Assert.IsTrue(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Underline));
        }

        [TestMethod]
        public void GetAttributesFromOtherTestWithAnchor() {
            TextRun tr0 = new TextRun {
                Attributes = new BaseTextAttribute[] {
                    new TextAttribute {TextAttributeType = TextAttributeTypeEnum.Bold},
                    new TextAttribute {TextAttributeType = TextAttributeTypeEnum.Italic},
                    new TextAttributeAnchor {
                        ActionName = "DoSomething",
                        AnchorInfo = "AnchorInfo",
                    },
                    new TextAttribute {TextAttributeType = TextAttributeTypeEnum.Underline},
                },
            };
            TextRun tr1 = new TextRun { };
            Assert.AreEqual(4, tr0.Attributes.Count());
            Assert.AreEqual(0, tr1.Attributes.Count());

            tr1.GetAttributesFrom(tr0);
            Assert.AreEqual(3, tr1.Attributes.Count());
            Assert.IsTrue(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Bold));
            Assert.IsTrue(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Italic));
            Assert.IsTrue(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Underline));
            // Closed tags are nor propagated
            Assert.IsFalse(tr1.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Anchor));
        }
    }
}
