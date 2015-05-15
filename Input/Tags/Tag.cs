using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Input.Tags {

    public class Tag {

        public Tag() {
            Attribute = new TextAttribute {TextAttributeType = TextAttributeTypeEnum.None,};
            StartIndex = 0;
            TrailIndex = 0;
        }

        public TextAttributeTypeEnum TextAttributeType
        {
            get { return Attribute.TextAttributeType; }
            set { Attribute.TextAttributeType = value; }
        }

        public BaseTextAttribute Attribute { get; set; }
        public int StartIndex { get; set; }
        public int TrailIndex { get; set; } // Index of character following the end of this tag

        public bool IsContiguousWith(Tag other) {
            return StartIndex == other.TrailIndex || other.StartIndex == TrailIndex;
        }

        public string GetDescription() {
            StringBuilder b = new StringBuilder("Tag: ");
            b.AppendFormat("Type='{0}', ", TextAttributeType);
            b.AppendFormat("Start={0}, Trail={1}, ", StartIndex, TrailIndex);
            b.AppendFormat("Attribute={0}", Attribute);
            return b.ToString();
        }
    }
}