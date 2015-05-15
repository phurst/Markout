using System;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Markout.Common.Tests.Attribute {

    [TestClass]
    public class TextAttributeTests {

        [TestMethod]
        public void BasicTest() {
            TextAttribute ta = new TextAttribute { TextAttributeType = TextAttributeTypeEnum.Bold };
            Assert.IsFalse(ta.IsClosed);
            Assert.AreEqual(TextAttributeTypeEnum.Bold, ta.TextAttributeType);
        }
    }
}
