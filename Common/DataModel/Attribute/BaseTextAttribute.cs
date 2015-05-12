using Markout.Common.DataModel.Enumerations;

namespace Markout.Common.DataModel.Attribute {

    public abstract class BaseTextAttribute {

        public TextAttributeTypeEnum TextAttributeType { get; set; }

        /// <summary>
        /// A closed attribute is not copied to the following run.
        /// </summary>
        public bool IsClosed { get; set; }

        public override string ToString() {
            return TextAttributeType.ToString();
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BaseTextAttribute) obj);
        }

        public bool Equals(BaseTextAttribute other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TextAttributeType == other.TextAttributeType && IsClosed == other.IsClosed;
        }

        public override int GetHashCode() {
            unchecked {
                return ((TextAttributeType.GetHashCode()*397) ^ IsClosed.GetHashCode());
            }
        }
    }
}