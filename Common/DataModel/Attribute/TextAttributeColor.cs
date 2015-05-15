using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Common.DataModel.Attribute {

    public class TextAttributeColor : BaseTextAttribute {

        public TextAttributeColor() {
            TextAttributeType = TextAttributeTypeEnum.Color;
            Color = Color.FromKnownColor(KnownColor.Black);
        }

        public Color Color { get; set; }

        public override string ToString() {
            return string.Format("{0}({1})", base.ToString(), Color.IsNamedColor ? Color.Name : Color.ToArgb().ToString("X"));
        }
    }
}