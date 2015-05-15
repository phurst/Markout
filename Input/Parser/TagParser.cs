using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Markout.Common.DataModel.Enumerations;
using Markout.Input.Interfaces;
using Markout.Input.Tags;
using Markout.Input.Tags.TagFactories;

namespace Markout.Input.Parser {

    public class TagParser : ParserBase {

        private static readonly Dictionary<string, ITagFactory> Recognizers = new Dictionary<string, ITagFactory> {
            {"b", new UnqualifiedTagFactory()  { TextAttributeType = TextAttributeTypeEnum.Bold }},
            {"i", new UnqualifiedTagFactory()  { TextAttributeType = TextAttributeTypeEnum.Italic }},
            {"u", new UnqualifiedTagFactory()  { TextAttributeType = TextAttributeTypeEnum.Underline }},
            {"0", new UnqualifiedTagFactory()  { TextAttributeType = TextAttributeTypeEnum.Zero }},
            {"f", new FontTagFactory()  { TextAttributeType = TextAttributeTypeEnum.Font }},
            {"font", new FontTagFactory()  { TextAttributeType = TextAttributeTypeEnum.Font }},
            {"c", new ColorTagFactory()  { TextAttributeType = TextAttributeTypeEnum.Color }},
            {"color", new ColorTagFactory()  { TextAttributeType = TextAttributeTypeEnum.Color }},
            {"colour", new ColorTagFactory()  { TextAttributeType = TextAttributeTypeEnum.Color }},
            {"a", new AnchorTagFactory()  { TextAttributeType = TextAttributeTypeEnum.Anchor }},
            {"anchor", new AnchorTagFactory()  { TextAttributeType = TextAttributeTypeEnum.Anchor }},
            {"hyperlink", new AnchorTagFactory()  { TextAttributeType = TextAttributeTypeEnum.Anchor }},
        };

        /// <summary>
        /// Find the Tags in the specified text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IEnumerable<Tag> Parse(string text) {
            List<Tag> rv = new List<Tag>();
            Regex regex = new Regex(RegexHead + string.Join("|", Recognizers.Keys) + RegexTail);
            MatchCollection matches = regex.Matches(text);
            foreach (Match match in matches) {
                if (match.Success) {
                    Group tagGroup = match.Groups["tag"];
                    if (tagGroup != null) {
                        ITagFactory tagFactory;
                        if (Recognizers.TryGetValue(tagGroup.Value, out tagFactory)) {
                            rv.Add(tagFactory.CreateTagFromMatch(match));
                        } else {
                            throw new ApplicationException(string.Format("Can't find tag factory for tag '{0}'", tagGroup.Value));
                        } 
                    }
                }
            }

            return rv.AsEnumerable();
        }
    }
}
