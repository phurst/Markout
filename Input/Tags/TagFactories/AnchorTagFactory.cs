using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Input.Tags.TagFactories {

    public class AnchorTagFactory : BaseTagFactory {

        public override Tag CreateTagFromMatch(Match match) {
            Group tagGroup = match.Groups["tag"];
            if (tagGroup == null) {
                throw new ApplicationException(string.Format("The AnchorTagFactory cannot find the tag group in the match"));
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
                throw new ApplicationException(string.Format("The AnchorTagFactory qualifer is empty"));
            }
            Uri uri = null;
            string anchorInfo = null;
            string actionName = null;
            string[] parts = qualifier.Split(':');
            if (parts.Length > 1 && parts[0].Length > 0 && parts[1].StartsWith("//")) {
                // This must be a Url of the form "scheme://host...", so undo the first split
                parts = (new string[] { parts[0] + ":" + parts[1] }).Concat(parts.Skip(2)).ToArray();
            }
            if (parts.Length > 0) {
                try {
                    UriBuilder uriBuilder = new UriBuilder(parts[0]);
                    uri = uriBuilder.Uri;
                } catch (UriFormatException) {
                    if (!string.IsNullOrWhiteSpace(parts[0])) {
                        anchorInfo = parts[0].Trim();
                    }
                }
            }
            if (parts.Length > 1) {
                actionName = parts[1].Trim();
            }
            return new TextAttributeAnchor { Uri = uri, AnchorInfo = anchorInfo, ActionName = actionName, };
        }
    }
}