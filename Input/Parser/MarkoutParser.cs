using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Elements;
using Markout.Common.DataModel.Enumerations;
using Markout.Input.Tags;

namespace Markout.Input.Parser {
    
    public class MarkoutParser {

        public Dictionary<string, string> Macros { get; set; }

        public IEnumerable<TextRun> Parse(string text) {
            List<TextRun> textRuns = new List<TextRun>();

            if (Macros != null && Macros.Any()) {
                MacroExpander macroExpander = new MacroExpander(Macros);
                text = macroExpander.ExpandMacros(text);
            }

            TextRun currentRun = new TextRun {Text = String.Empty};
            textRuns.Add(currentRun);
            Tag prevTag = new Tag {TextAttributeType = TextAttributeTypeEnum.None,};
            TagParser tagParser = new TagParser {Macros = Macros};
            Queue<Tag> tags = new Queue<Tag>(tagParser.Parse(text));
            while (tags.Any()) {
                Tag tag = tags.Dequeue();
                if (tag.Attribute.IsClosed) {
                    currentRun.Text = StripEscapes(text.Substring(prevTag.TrailIndex, tag.StartIndex - prevTag.TrailIndex));
                    currentRun = new TextRun { Text = String.Empty, };
                    currentRun.Attributes = new BaseTextAttribute[] { tag.Attribute };
                    textRuns.Add(currentRun);
                    Tag nextTag = tags.Any() ? tags.Peek() : null;
                    if (nextTag != null) {
                        currentRun.Text = text.Substring(tag.TrailIndex, nextTag.StartIndex - tag.TrailIndex);
                        if (nextTag.TextAttributeType == tag.TextAttributeType) {
                            tag = tags.Dequeue();
                            currentRun = new TextRun { Text = String.Empty, };
                            textRuns.Add(currentRun);
                        }
                    } else {
                        currentRun.Text = text.Substring(tag.TrailIndex, text.Length - tag.TrailIndex);
                    }
                } else {
                    if (!prevTag.IsContiguousWith(tag)) {
                        currentRun.Text = StripEscapes(text.Substring(prevTag.TrailIndex, tag.StartIndex - prevTag.TrailIndex));
                        TextRun prevRun = currentRun;
                        currentRun = new TextRun {Text = String.Empty,};
                        currentRun.GetAttributesFrom(prevRun);
                        currentRun.GetAttribute(tag.Attribute);
                        textRuns.Add(currentRun);
                    } else {
                        currentRun.GetAttribute(tag.Attribute);
                    }
                }
                prevTag = tag;
            }
            if (prevTag.TrailIndex < text.Length) {
                currentRun.Text = StripEscapes(text.Substring(prevTag.TrailIndex, text.Length - prevTag.TrailIndex));
            }

            return textRuns.AsEnumerable();
        }

        private string StripEscapes(string s) {
            return s.Replace("{{", "{");
        }
    }
}
