using System;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Markout.Common.Tests.Attribute {

    [TestClass]
    public class TextAttributeAnchorTests {

        [TestMethod]
        public void BasicTest() {
            TextAttributeAnchor ta = new TextAttributeAnchor { AnchorInfo = "A", ActionName = "B", Uri = new Uri("http://google.com") };
            Assert.IsTrue(ta.IsClosed);
            Assert.AreEqual(TextAttributeTypeEnum.Anchor, ta.TextAttributeType);
            Assert.AreEqual("A", ta.AnchorInfo);
            Assert.AreEqual("B", ta.ActionName);
            Assert.AreEqual("http://google.com/", ta.Uri.ToString());
        }
    }
}
