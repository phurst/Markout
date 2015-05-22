using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Common.DataModel.Attribute {

    public class TextAttributeParagraph : BaseTextAttribute {

        public TextAttributeParagraph() {
            TextAttributeType = TextAttributeTypeEnum.Paragraph;
            Margins = new Margins();
        }

        public Margins Margins { get; set; }

        public override string ToString() {
            return string.Format("{0}({1},{2},{3},{4})", base.ToString(), Margins.Left, Margins.Top, Margins.Right, Margins.Bottom);
        }
    }
}