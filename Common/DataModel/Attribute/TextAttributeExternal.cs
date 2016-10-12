using System;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Common.DataModel.Attribute {

    public class TextAttributeExternal : BaseTextAttribute {

        public TextAttributeExternal() {
            TextAttributeType = TextAttributeTypeEnum.External;
            IsClosed = false;
        }

        public string Name { get; set; }
        public string Parameter { get; set; }

        public override string ToString() {
            return string.Format("{0}({1})->({2})", 
                base.ToString(), 
                Name != null ? Name : "<none>",
                Parameter != null ? Parameter : "<none>"
                );
        }
    }
}