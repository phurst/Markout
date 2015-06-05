using Markout.Common.DataModel.Enumerations;

namespace Markout.Common.DataModel.Attribute {

    public abstract class BaseTextAttribute {

        public BaseTextAttribute() {
            IsCopiedToFollowingRun = true;
        }

        public TextAttributeTypeEnum TextAttributeType { get; set; }

        /// <summary>
        /// A closed attribute is not copied to the following run.
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// A closed attribute is not copied to the following run.
        /// </summary>
        public bool IsCopiedToFollowingRun { get; set; }

        public override string ToString() {
            return TextAttributeType.ToString();
        }
    }
}