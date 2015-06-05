using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Common.DataModel.Elements {

    public class TextRun {

        private List<BaseTextAttribute> _attributes = new List<BaseTextAttribute>();

        public IEnumerable<BaseTextAttribute> Attributes {
            get { return _attributes.AsEnumerable(); }
            set { _attributes = value.ToList(); }
        }

        public string Text { get; set; }

        /// <summary>
        /// Get all the attributes from other by calling GetAttribute for each one.
        /// </summary>
        /// <param name="other"></param>
        public void GetAttributesFrom(TextRun other) {
            GetAttributesFrom(other.Attributes);
        }

        /// <summary>
        /// Get the atributes by calling GetAttribute for each one. 
        /// </summary>
        /// <param name="attributes"></param>
        public void GetAttributesFrom(IEnumerable<BaseTextAttribute> attributes) {
            attributes.ToList().ForEach(GetAttributeFromOtherRun);
        }

        public void GetAttributeFromOtherRun(BaseTextAttribute attribute) {
            if (attribute.IsCopiedToFollowingRun) {
                GetAttribute(attribute);
            }
        }

        /// <summary>
        /// <para>
        /// If this TextRun does not have an attribute with the same TextAttributeType as attribute, add
        /// attribute to the _attributes collection, otherwise remove the existing attribute of that type.
        /// </para>
        /// </summary>
        /// <param name="attribute"></param>
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
    }
}
