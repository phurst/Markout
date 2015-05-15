using System;
using System.Drawing;
using System.Linq;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Common.DataModel.Attribute {

    public class TextAttributeFont : BaseTextAttribute {

        public TextAttributeFont() {
            TextAttributeType = TextAttributeTypeEnum.Font;
        }

        public Font Font { get; set; }

        public override string ToString() {
            return string.Format("{0}({1})", base.ToString(), Font);
        }
    }
}