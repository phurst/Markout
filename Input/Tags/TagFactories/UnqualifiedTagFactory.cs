using System;
using System.Text.RegularExpressions;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Input.Tags.TagFactories {

    public class UnqualifiedTagFactory : BaseTagFactory {

        public override Tag CreateTagFromMatch(Match match) {
            Group tagGroup = match.Groups["tag"];
            if (tagGroup == null) {
                throw new ApplicationException(string.Format("The SimpleTagFactory cannot find the tag group in the match"));
            }
            TextAttributeTypeEnum textAttributeType = TextAttributeTypeEnum.None;
            switch (TagRecognizer) {
                case "b":
                    textAttributeType = TextAttributeTypeEnum.Bold;
                    break;
                case "i":
                    textAttributeType = TextAttributeTypeEnum.Italic;
                    break;
                case "u":
                    textAttributeType = TextAttributeTypeEnum.Underline;
                    break;
                default:
                    throw new ApplicationException(string.Format("The SimpleTagFactory was asked to create a Tag from a match of '{0}'", TagRecognizer));
            }
            return new Tag {
                TextAttributeType = textAttributeType,
                StartIndex = tagGroup.Index - 1,
                TrailIndex = tagGroup.Index + tagGroup.Length + 1,
            };
        }
    }
}