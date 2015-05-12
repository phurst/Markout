using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Input.Tags.TagFactories {

    public class FontTagFactory : BaseTagFactory {

        public override Tag CreateTagFromMatch(Match match) {
            Group tagGroup = match.Groups["tag"];
            if (tagGroup == null) {
                throw new ApplicationException(string.Format("The FontTagFactory cannot find the tag group in the match"));
            }
            TextAttributeTypeEnum textAttributeType = TextAttributeTypeEnum.None;
            switch (TagRecognizer) {
                case "f":
                    textAttributeType = TextAttributeTypeEnum.Font;
                    break;
                case "font":
                    textAttributeType = TextAttributeTypeEnum.Font;
                    break;
                default:
                    throw new ApplicationException(string.Format("The FontTagFactory was asked to create a Tag from a match of '{0}'", TagRecognizer));
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
                Attribute = GetFontAttribute(qualifierGroup.Value),
                StartIndex = tagGroup.Index - 1,
                TrailIndex = qualifierGroup.Index + qualifierGroup.Length + 1,
            };
        }

        private BaseTextAttribute GetFontAttribute(string qualifier) {
            if (string.IsNullOrWhiteSpace(qualifier)) {
                throw new ApplicationException(string.Format("The FontTagFactory qualifer is empty"));
            }
            Font font = null;
            string[] parts = qualifier.Split(':');
            if (parts.Length > 0) {
                string fontFamilyName = parts[0].Trim();
                FontFamily fontFamily = new FontFamily(fontFamilyName);
                font = new Font(fontFamily, 12);
            }
            if (parts.Length > 1) {
                string sizeString = parts[1].Trim();
                float emSize;
                if (float.TryParse(sizeString, out emSize)) {
                    font = new Font(font.FontFamily, emSize);
                }
            }
            if (parts.Length > 2) {
                FontStyle fontStyle = new FontStyle();
                string styleString = parts[2].Trim();
                styleString.ToList().ForEach(c => {
                    switch (c) {
                        case 'b':
                            fontStyle = fontStyle | FontStyle.Bold;
                            break;
                        case 'i':
                            fontStyle = fontStyle | FontStyle.Italic;
                            break;
                        case 's':
                            fontStyle = fontStyle | FontStyle.Strikeout;
                            break;
                        case 'u':
                            fontStyle = fontStyle | FontStyle.Underline;
                            break;
                    }
                });
                if (fontStyle != new FontStyle()) {
                    font = new Font(font.FontFamily, font.Size, fontStyle);
                }
            }
            return new TextAttributeFont {Font = font};
        }
    }
}