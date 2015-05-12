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

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TextAttributeFont)obj);
        }

        public bool Equals(TextAttributeFont other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Font == other.Font;
        }

        public override int GetHashCode() {
            unchecked {
                return ((base.GetHashCode() * 397) ^ Font.GetHashCode());
            }
        }
    }
}