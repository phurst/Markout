using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Input.Tags.TagFactories {

    public class ParagraphTagFactory : BaseTagFactory {

        public override Tag CreateTagFromMatch(Match match) {
            Group tagGroup = match.Groups["tag"];
            if (tagGroup == null) {
                throw new ApplicationException(string.Format("The ParagraphTagFactory cannot find the tag group in the match"));
            }
            Group qualifierGroup = match.Groups["qualifier"];
            if (qualifierGroup == null || qualifierGroup.Length == 0) {
                return new Tag {
                    Attribute = new TextAttributeParagraph(),
                    TextAttributeType = TextAttributeType,
                    StartIndex = tagGroup.Index - 1,
                    TrailIndex = tagGroup.Index + tagGroup.Length + 1,
                };
            }
            return new Tag {
                Attribute = GetParagraphAttribute(qualifierGroup.Value),
                StartIndex = tagGroup.Index - 1,
                TrailIndex = qualifierGroup.Index + qualifierGroup.Length + 1,
            };
        }

        private BaseTextAttribute GetParagraphAttribute(string qualifier) {
            if (string.IsNullOrWhiteSpace(qualifier)) {
                throw new ApplicationException(string.Format("The ParagraphTagFactory qualifer is empty"));
            }
            int leftMargin;
            if (int.TryParse(qualifier, out leftMargin)) {
                return new TextAttributeParagraph { Margins = new Margins(leftMargin, 0, 0, 0), };
            }
            return new TextAttributeParagraph();
        }
    }
}