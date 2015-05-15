using System;
using System.Drawing;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Markout.Common.Tests.Attribute {

    [TestClass]
    public class TextAttributeFontTests {

        [TestMethod]
        public void BasicTest() {
            TextAttributeFont ta = new TextAttributeFont { Font = new Font(FontFamily.GenericSansSerif, 12.0f) };
            Assert.IsFalse(ta.IsClosed);
            Assert.AreEqual(TextAttributeTypeEnum.Font, ta.TextAttributeType);
            Assert.AreEqual("[Font: Name=Microsoft Sans Serif, Size=12, Units=3, GdiCharSet=1, GdiVerticalFont=False]", ta.Font.ToString());
        }
    }
}
