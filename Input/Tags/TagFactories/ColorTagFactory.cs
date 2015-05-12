using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Input.Tags.TagFactories {

    public class ColorTagFactory : BaseTagFactory {

        public override Tag CreateTagFromMatch(Match match) {
            Group tagGroup = match.Groups["tag"];
            if (tagGroup == null) {
                throw new ApplicationException(string.Format("The ColorTagFactory cannot find the tag group in the match"));
            }
            TextAttributeTypeEnum textAttributeType = TextAttributeTypeEnum.None;
            switch (TagRecognizer) {
                case "c":
                    textAttributeType = TextAttributeTypeEnum.Color;
                    break;
                case "color":
                    textAttributeType = TextAttributeTypeEnum.Color;
                    break;
                case "colour":
                    textAttributeType = TextAttributeTypeEnum.Color;
                    break;
                default:
                    throw new ApplicationException(string.Format("The ColorTagFactory was asked to create a Tag from a match of '{0}'", TagRecognizer));
            }
            Group qualifierGroup = match.Groups["qualifier"];
            if (qualifierGroup == null || qualifierGroup.Length == 0) {
                return new Tag {
                    TextAttributeType = textAttributeType,
                    StartIndex = tagGroup.Index - 1,
                    TrailIndex = tagGroup.Index + tagGroup.Length + 1,
                };
            }
            return new Tag {
                Attribute = GetColorAttribute(qualifierGroup.Value),
                StartIndex = tagGroup.Index - 1,
                TrailIndex = qualifierGroup.Index + qualifierGroup.Length + 1,
            };
        }

        private BaseTextAttribute GetColorAttribute(string qualifier) {
            if (string.IsNullOrWhiteSpace(qualifier)) {
                throw new ApplicationException(string.Format("The ColorTagFactory qualifer is empty"));
            }

            KnownColor color;
            string s = qualifier;
            if (s.Length > 0 && Char.IsLower(s.First())) {
                s = Char.ToUpper(s.First()) + s.Substring(1);
            }
            if (KnownColor.TryParse(s, out color)) {
                return new TextAttributeColor { Color = Color.FromName(color.ToString()), };
            }

            int n;
            if (int.TryParse(qualifier, NumberStyles.HexNumber | NumberStyles.AllowHexSpecifier | NumberStyles.AllowLeadingWhite, new CultureInfo("en-US"), out n)) {
                return new TextAttributeColor { Color = Color.FromArgb(n), };
            }

            throw new ApplicationException(string.Format("ColorTagFactory can't parse a color from string '{0}'", qualifier));
        }
    }
}