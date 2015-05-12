using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Common.DataModel.Elements {

    public class TextRun : IEquatable<TextRun> {

        private List<BaseTextAttribute> _attributes = new List<BaseTextAttribute>();

        public IEnumerable<BaseTextAttribute> Attributes {
            get { return _attributes.AsEnumerable(); }
            set { _attributes = value.ToList(); }
        }

        public string Text { get; set; }

        public void GetAttributesFrom(TextRun other) {
            other.Attributes.ToList().ForEach(GetAttribute);
        }

        public void GetAttributesFrom(IEnumerable<BaseTextAttribute> attributes) {
            attributes.ToList().ForEach(GetAttribute);
        }

        public void GetAttribute(BaseTextAttribute attribute) {
            BaseTextAttribute extantTextAttribute = _attributes.FirstOrDefault(a => a.TextAttributeType == attribute.TextAttributeType);
            if (extantTextAttribute != null) {
                _attributes.Remove(extantTextAttribute);
            } else if (!attribute.IsClosed) {
                _attributes.Add(attribute);
            }
        }

        public override string ToString() {
            StringBuilder b = new StringBuilder();
            b.AppendFormat("Run Text=[{0}]", Text ?? "<none>");
            b.AppendFormat(" Attributes: {0}", string.Join(", ", Attributes.Select(a => a.ToString())));
            return b.ToString();
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TextRun)obj);
        }

        public bool Equals(TextRun other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            bool attributesEqual = false;
            if (_attributes == null && other.Attributes == null) {
                attributesEqual = true;
            } else if (_attributes != null && other.Attributes != null && _attributes.Count == other._attributes.Count) {
                if (_attributes.Count == 0) {
                    attributesEqual = true;
                } else {
                    attributesEqual = _attributes.All(a => other._attributes.Any(a1 => Equals(a, a1)));
                }
            }
            return attributesEqual && string.Equals(Text, other.Text);
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 0;
                if (_attributes != null) {
                    _attributes.ToList().ForEach(a => hash = (a.GetHashCode()*397) ^ hash);
                }
                return (hash ^ (Text != null ? Text.GetHashCode() : 0));
            }
        }
    }
}
