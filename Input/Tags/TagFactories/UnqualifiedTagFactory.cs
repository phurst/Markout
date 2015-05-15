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

            return new Tag {
                TextAttributeType = TextAttributeType,
                StartIndex = tagGroup.Index - 1,
                TrailIndex = tagGroup.Index + tagGroup.Length + 1,
            };
        }
    }
}