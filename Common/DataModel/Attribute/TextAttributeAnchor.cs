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
    }
}