using System;
using System.Drawing;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Markout.Common.Tests.Attribute {

    [TestClass]
    public class TextAttributeColorTests {

        [TestMethod]
        public void BasicTest() {
            TextAttributeColor ta = new TextAttributeColor { Color = Color.AliceBlue };
            Assert.IsFalse(ta.IsClosed);
            Assert.AreEqual(TextAttributeTypeEnum.Color, ta.TextAttributeType);
            Assert.AreEqual(Color.AliceBlue, ta.Color);
        }
    }
}
