using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Elements;
using Markout.Common.DataModel.Enumerations;
using Markout.Input.Interfaces;
using Markout.Input.Tags;
// ReSharper disable All

namespace Markout.Input.Parser {
    
    public class MarkoutParser {

        /// <summary>
        /// Optional macros to be located and then expanded in the text.
        /// </summary>
        public Dictionary<string, string> Macros { get; set; }
        public Dictionary<string, IExternalTagResolver> ExternalTagResolvers { get; set; }

        public IEnumerable<TextRun> Parse(string text) {
            text = ParseExternalTags(text);
            return ParseMarkout(text);
        }

        /// <summary>
        /// Find the tags in the text and then extract the intervening TextRuns complet with
        /// tag attributes.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private IEnumerable<TextRun> ParseMarkout(string text) {
            List<TextRun> textRuns = new List<TextRun>();

            if (Macros != null && Macros.Any()) {
                MacroExpander macroExpander = new MacroExpander(Macros);
                text = macroExpander.ExpandMacros(text);
            }

            TextRun currentRun = new TextRun {Text = String.Empty};
            textRuns.Add(currentRun);
            Tag prevTag = new Tag {TextAttributeType = TextAttributeTypeEnum.None,};
            TagParser tagParser = new TagParser();
            Queue<Tag> tags = new Queue<Tag>(tagParser.Parse(text));
            while (tags.Any() && prevTag != null) {
                Tag tag = tags.Dequeue();
                if (tag.Attribute.IsClosed) {
                    currentRun.Text = StripEscapes(text.Substring(prevTag.TrailIndex, tag.StartIndex - prevTag.TrailIndex));
                    currentRun = new TextRun { Text = String.Empty, };
                    currentRun.Attributes = new BaseTextAttribute[] { tag.Attribute };
                    textRuns.Add(currentRun);
                    GetClosedRun(text, tags, ref tag, currentRun);
                    if (tag != null) {
                        currentRun = new TextRun {Text = String.Empty,};
                        textRuns.Add(currentRun);
                    }
                } else {
                    if (!prevTag.IsContiguousWith(tag)) {
                        currentRun.Text = StripEscapes(text.Substring(prevTag.TrailIndex, tag.StartIndex - prevTag.TrailIndex));
                        TextRun prevRun = currentRun;
                        currentRun = new TextRun {Text = String.Empty,};
                        if (tag.Attribute.TextAttributeType != TextAttributeTypeEnum.Zero) {
                            currentRun.GetAttributesFrom(prevRun);
                            currentRun.GetAttribute(tag.Attribute);
                        }
                        textRuns.Add(currentRun);
                    } else {
                        currentRun.GetAttribute(tag.Attribute);
                    }
                }
                prevTag = tag;
            }
            if (prevTag != null && prevTag.TrailIndex < text.Length) {
                currentRun.Text = StripEscapes(text.Substring(prevTag.TrailIndex, text.Length - prevTag.TrailIndex));
            }

            return textRuns.AsEnumerable();
        }

        /// <summary>
        /// Find and substitutr any external tags in the text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string ParseExternalTags(string text) {
            StringBuilder rv = new StringBuilder();
            Tag prevTag = new Tag { TextAttributeType = TextAttributeTypeEnum.None, };
            TagParser tagParser = new TagParser();
            Queue<Tag> tags = new Queue<Tag>(tagParser.ParseExternalTags(text));
            while(tags.Any() && prevTag != null) {
                Tag tag = tags.Dequeue();
                TextAttributeExternal textAttributeExternal = tag.Attribute as TextAttributeExternal;
                if(textAttributeExternal != null) {
                    string externalValue = ResolveExternalTag(textAttributeExternal);
                    if(!string.IsNullOrEmpty(externalValue)) {
                        rv.Append(text.Substring(prevTag.TrailIndex, tag.StartIndex - prevTag.TrailIndex));
                        rv.Append(externalValue);
                    }
                }
                prevTag = tag;
            }
            if(prevTag.TrailIndex < text.Length) {
                rv.Append(text.Substring(prevTag.TrailIndex, text.Length - prevTag.TrailIndex));
            }
            return rv.ToString();
        }

        /// <summary>
        /// <para>
        /// A closed run is something like an anchor that is delimited by two tags of the same TextAttributeType, e.g., 
        /// "{a:...}some content{a}". 
        /// </para><para>
        /// Qualifying tags are permitted within the content, so "{a:...}{c:Cyan}some content{a}" is legal and the 
        /// current run would get 2 attributes: anchor and color. The rendering system is free to ignore tags that
        /// caoont be applied, so an HTML renderer would probably ignore a color attribute in an anchor since
        /// the appearance of an anchor is defined by stylinh in HTML.
        /// </para><para>
        /// The content around any qualifying tags is concatenated together, so for "{a:...}some content, {c:Cyan}more content{a}"
        /// the currentRun would have the following text: "some content, more content". Note that the color attribute
        /// applies to he whole run, so it's position is irrelevant.
        /// </para>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="tags"></param>
        /// <param name="prevTag"></param>
        /// <param name="currentRun"></param>
        private void GetClosedRun(string text, Queue<Tag> tags, ref Tag prevTag, TextRun currentRun) {
            if (!prevTag.Attribute.IsClosed) {
                throw new ApplicationException("GetClosedRun was called with a non-closed attribute");
            }
            TextAttributeTypeEnum closingTagType = prevTag.TextAttributeType;
            Tag tag = tags.Any() ? tags.Dequeue() : null;
            while (tag != null) {
                if (!prevTag.IsContiguousWith(tag)) {
                    currentRun.Text += StripEscapes(text.Substring(prevTag.TrailIndex, tag.StartIndex - prevTag.TrailIndex));
                }
                if (tag.Attribute != null && tag.TextAttributeType != closingTagType) {
                    currentRun.GetAttribute(tag.Attribute);
                }
                prevTag = tag;
                if (tag.TextAttributeType == closingTagType) {
                    break;
                }
                tag = tags.Any() ? tags.Dequeue() : null;
            }
            if (tag == null && prevTag != null && prevTag.TrailIndex < text.Length) {
                currentRun.Text += text.Substring(prevTag.TrailIndex);
                prevTag = null;
            }
        }

        private string ResolveExternalTag(TextAttributeExternal textAttributeExternal) {
            string rv = string.Empty;
            try {
                IExternalTagResolver resolver;
                if(ExternalTagResolvers != null && ExternalTagResolvers.TryGetValue(textAttributeExternal.Name, out resolver)) {
                    rv = resolver.Resolve(textAttributeExternal);
                }
            } catch(Exception ex) {
                rv = string.Format("An exception was thrown in an external tag resolver: {0}", ex.Message);
            }
            return rv;
        }

        private string StripEscapes(string s) {
            return s.Replace("{{", "{");
        }
    }
}
