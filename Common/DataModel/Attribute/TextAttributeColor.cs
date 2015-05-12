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

        public void SetColor(string colorString) {
            if (colorString == null) throw new ArgumentNullException("colorString");

            KnownColor color;
            string s = colorString;
            if(s.Length > 0 && Char.IsLower(s.First())) {
                s = Char.ToUpper(s.First()) + s.Substring(1);
            }
            if (KnownColor.TryParse(s, out color)) {
                Color = Color.FromName(color.ToString());
                return;
            }

            int n;
            if (int.TryParse(colorString, NumberStyles.HexNumber | NumberStyles.AllowHexSpecifier | NumberStyles.AllowLeadingWhite, new CultureInfo("en-US"), out n)) {
                Color = Color.FromArgb(n);
                return;
            }

            throw new ApplicationException(string.Format("TextAttributeColor can't parse a color from string '{0}'", colorString));
        }

        public override string ToString() {
            return string.Format("{0}({1})", base.ToString(), Color.IsNamedColor ? Color.Name : Color.ToArgb().ToString("X"));
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TextAttributeColor)obj);
        }

        public bool Equals(TextAttributeColor other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Color == other.Color;
        }

        public override int GetHashCode() {
            unchecked {
                return ((base.GetHashCode() * 397) ^ Color.GetHashCode());
            }
        }
    }
}