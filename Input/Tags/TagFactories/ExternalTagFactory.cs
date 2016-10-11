using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Input.Tags.TagFactories {

    public class ExternalTagFactory : BaseTagFactory {

        public override Tag CreateTagFromMatch(Match match) {
            Group tagGroup = match.Groups["tag"];
            if (tagGroup == null) {
                throw new ApplicationException(string.Format("The ExternalTagFactory cannot find the tag group in the match"));
            }
            Group qualifierGroup = match.Groups["qualifier"];
            if (qualifierGroup == null || qualifierGroup.Length == 0) {
                return new Tag {
                    TextAttributeType = TextAttributeType,
                    StartIndex = tagGroup.Index - 1,
                    TrailIndex = tagGroup.Index + tagGroup.Length + 1,
                };
            }
            return new Tag {
                Attribute = GetAnchorAttribute(qualifierGroup.Value),
                StartIndex = tagGroup.Index - 1,
                TrailIndex = qualifierGroup.Index + qualifierGroup.Length + 1,
            };
        }

        private BaseTextAttribute GetAnchorAttribute(string qualifier) {
            if (string.IsNullOrWhiteSpace(qualifier)) {
                throw new ApplicationException(string.Format("The ExternalTagFactory qualifer is empty"));
            }
            string name = null;
            string parameter = null;
            string[] parts = qualifier.Split(':');
            if (parts.Length > 0 && parts[0].Trim().Length > 0) {
                name = parts[0].Trim();
            }
            if (parts.Length > 1 && parts[1].Trim().Length > 0) {
                parameter = parts[1].Trim();
            }
            return new TextAttributeExternal { Name = name, Parameter = parameter, };
        }
    }
}