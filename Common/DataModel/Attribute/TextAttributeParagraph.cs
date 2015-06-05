using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Common.DataModel.Attribute {

    public class TextAttributeParagraph : BaseTextAttribute {

        public TextAttributeParagraph() {
            TextAttributeType = TextAttributeTypeEnum.Paragraph;
            Margins = new Margins(0,0,0,0);
            IsCopiedToFollowingRun = false;
        }

        /// <summary>
        /// If true this attribute requires that its TextRun be contained in a InlineUIContainer, a RichTextBox, and a FlowDocument.
        /// </summary>
        public bool RequiresRichTextContainer
        {
            get { return Margins.Left != 0 || Margins.Right != 0 || Margins.Top != 0 || Margins.Bottom != 0; } 
        }

        public Margins Margins { get; set; }

        public override string ToString() {
            return string.Format("{0}({1},{2},{3},{4})", base.ToString(), Margins.Left, Margins.Top, Margins.Right, Margins.Bottom);
        }
    }
}