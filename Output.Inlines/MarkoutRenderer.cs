using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Elements;
using Markout.Common.DataModel.Enumerations;

namespace Markout.Output.Inlines {

    public class MarkoutRenderer {

        private Queue<TextRun> _textRuns;
        private readonly List<Inline> _inlines = new List<Inline>();
        private Inline _currInline = null;
        private Dictionary<string, Action<TextAttributeAnchor, string>> _anchorActions = null;

        public IEnumerable<Inline> Render(
            IEnumerable<TextRun> textRuns,
            Dictionary<string, Action<TextAttributeAnchor, string>> anchorActions = null
            ) {
                _textRuns = new Queue<TextRun>(textRuns);
            _inlines.Clear();
            _currInline = null;
            _anchorActions = anchorActions;
            ProcessRuns();
            return _inlines.AsEnumerable();
        }

        public void ProcessRuns() {
            while (_textRuns.Any()) {
                ProcessNextRun(_textRuns.Dequeue());
            }
        }

        private void ProcessNextRun(TextRun textRun) {
            if (textRun.Attributes.Any(a => a.TextAttributeType == TextAttributeTypeEnum.Anchor)) {
                _currInline = ProcessAnchorTextRun(textRun);
                _inlines.Add(_currInline);
            } else if (!string.IsNullOrEmpty(textRun.Text)) {
                _currInline = ProcessAttributes(textRun.Attributes, new Run(textRun.Text));
                _inlines.Add(_currInline);
            }
        }

        private Inline ProcessAnchorTextRun(TextRun textRun) {
            TextAttributeAnchor taa = textRun.Attributes.FirstOrDefault(ta => ta.TextAttributeType == TextAttributeTypeEnum.Anchor) as TextAttributeAnchor;
            if (taa == null) {
                throw new ApplicationException("ProcessAnchorTextRun cannot process a runs lacking an Anchor attribute");
            }
            Hyperlink hyperlink = new Hyperlink(new Run(textRun.Text));
            hyperlink.NavigateUri = taa.Uri;
            if (!string.IsNullOrWhiteSpace(taa.ActionName) && _anchorActions != null) {
                Action<TextAttributeAnchor, string> action;
                if (_anchorActions.TryGetValue(taa.ActionName.Trim(), out action)) {
                    hyperlink.Click += (sender, args) => action(taa, textRun.Text);
                }
            }
            hyperlink.Foreground = new SolidColorBrush(Colors.Blue);
            hyperlink.TextDecorations = TextDecorations.Underline;
            if (textRun.Attributes.Count() > 1) {
                ProcessAttributes(textRun.Attributes.Except(new BaseTextAttribute[] { taa }), hyperlink);
            }
            return hyperlink;
        }

        private Inline ProcessAttributes(IEnumerable<BaseTextAttribute> attributes, Inline run) {
            attributes.ToList().ForEach(a => {
                switch (a.TextAttributeType) {
                    case TextAttributeTypeEnum.Bold:
                        run.FontWeight = FontWeights.Bold;
                        break;
                    case TextAttributeTypeEnum.Color: {
                            TextAttributeColor taColor = a as TextAttributeColor;
                            if (taColor != null) {
                                run.Foreground = new SolidColorBrush(Color.FromArgb(taColor.Color.A, taColor.Color.R, taColor.Color.G, taColor.Color.B));
                            }
                        } break;
                    case TextAttributeTypeEnum.Font: {
                            TextAttributeFont taFont = a as TextAttributeFont;
                            if (taFont != null) {
                                if (taFont.Font != null) {
                                    TextDecorationCollection textDecorations = new TextDecorationCollection(); // Freezable
                                    run.FontFamily = new FontFamily(taFont.Font.FontFamily.Name);
                                    run.FontSize = taFont.Font.Size;
                                    if (taFont.Font.Style.HasFlag(System.Drawing.FontStyle.Bold)) {
                                        run.FontWeight = FontWeights.Bold;
                                    }
                                    if (taFont.Font.Style.HasFlag(System.Drawing.FontStyle.Italic)) {
                                        run.FontStyle = FontStyles.Italic;
                                    }
                                    if (taFont.Font.Style.HasFlag(System.Drawing.FontStyle.Strikeout)) {
                                        textDecorations.Add(TextDecorations.Strikethrough);
                                    }
                                    if (taFont.Font.Style.HasFlag(System.Drawing.FontStyle.Underline)) {
                                        textDecorations.Add(TextDecorations.Underline);
                                    }
                                    run.TextDecorations = new TextDecorationCollection(textDecorations); // Freezable
                                } else {
                                    run.FontFamily = new FontFamily();
                                    run.FontSize = 12;
                                    run.FontStyle = FontStyles.Normal;
                                    run.FontWeight = FontWeights.Normal;
                                }
                            }
                        } break;
                    case TextAttributeTypeEnum.Italic:
                        run.FontStyle = FontStyles.Italic;
                        break;
                    case TextAttributeTypeEnum.Underline:
                        run.TextDecorations = TextDecorations.Underline;
                        break;
                    case TextAttributeTypeEnum.Zero:
                        break;
                    case TextAttributeTypeEnum.None:
                        break;
                }
            });
            return run;
        }
    }
}
