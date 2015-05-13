using System;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Common.DataModel.Attribute {

    public class TextAttributeAnchor : BaseTextAttribute {

        public TextAttributeAnchor() {
            TextAttributeType = TextAttributeTypeEnum.Anchor;
            IsClosed = true;
        }

        public Uri Uri { get; set; }
        public string AnchorInfo { get; set; }
        public string ActionName { get; set; }

        public override string ToString() {
            return string.Format("{0}({1})->({2})", 
                base.ToString(), 
                Uri != null ? Uri.ToString() : "<none>",
                ActionName != null ? ActionName.ToString() : "<none>"
                );
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TextAttributeAnchor)obj);
        }

        public bool Equals(TextAttributeAnchor other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Uri == other.Uri && ActionName == other.ActionName;
        }

        public override int GetHashCode() {
            unchecked {
                return (
                    (base.GetHashCode() * 397) ^ 
                    (Uri != null ? Uri.GetHashCode() : 0) ^
                    (ActionName != null ? ActionName.GetHashCode() : 0) 
                    );
            }
        }
    }
}