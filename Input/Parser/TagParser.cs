using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Markout.Input.Interfaces;
using Markout.Input.Tags;
using Markout.Input.Tags.TagFactories;

namespace Markout.Input.Parser {

    public class TagParser : ParserBase {

        private static readonly ITagFactory[] TagFactories = {
            new UnqualifiedTagFactory()  { TagRecognizer = "b" },
            new UnqualifiedTagFactory()  { TagRecognizer = "i" },
            new UnqualifiedTagFactory()  { TagRecognizer = "u" },
            new FontTagFactory()  { TagRecognizer = "f" },
            new FontTagFactory()  { TagRecognizer = "font" },
            new ColorTagFactory()  { TagRecognizer = "c" },
            new ColorTagFactory()  { TagRecognizer = "color" },
            new ColorTagFactory()  { TagRecognizer = "colour" },
            new AnchorTagFactory()  { TagRecognizer = "a" },
            new AnchorTagFactory()  { TagRecognizer = "anchor" },
        };

        public Dictionary<string, string> Macros { get; set; }

        public IEnumerable<Tag> Parse(string text) {
            List<Tag> rv = new List<Tag>();

            Dictionary<string, ITagFactory> recognizers = new Dictionary<string, ITagFactory>();
            TagFactories.ToList().ForEach(tf => recognizers.Add(tf.TagRecognizer, tf));

            Regex regex = new Regex(RegexHead + string.Join("|", recognizers.Keys) + RegexTail);
            MatchCollection matches = regex.Matches(text);
            foreach (Match match in matches) {
                if (match.Success) {
                    Group tagGroup = match.Groups["tag"];
                    if (tagGroup != null) {
                        ITagFactory tagFactory;
                        if (recognizers.TryGetValue(tagGroup.Value, out tagFactory)) {
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
