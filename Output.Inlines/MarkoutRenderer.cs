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
        private Dictionary<string, Action<TextAttributeAnchor>> _anchorActions = null;

        public IEnumerable<Inline> Render(
            IEnumerable<TextRun> textRuns,
            Dictionary<string, Action<TextAttributeAnchor>> anchorActions = null
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
            if (!string.IsNullOrEmpty(textRun.Text)) {
                _currInline = new Run(textRun.Text);
                textRun.Attributes.ToList().ForEach(a => {
                    switch (a.TextAttributeType) {
                        case TextAttributeTypeEnum.Anchor: {
                            TextAttributeAnchor taa = a as TextAttributeAnchor;
                            Hyperlink hyperlink = new Hyperlink(new Run(textRun.Text));
                            hyperlink.NavigateUri = taa.Uri;
                            hyperlink.Foreground = new SolidColorBrush(Colors.Blue);
                            hyperlink.TextDecorations = TextDecorations.Underline;
                            if (!string.IsNullOrWhiteSpace(taa.ActionName) && _anchorActions != null) {
                                Action<TextAttributeAnchor> action;
                                if (_anchorActions.TryGetValue(taa.ActionName.Trim(), out action)) {
                                    hyperlink.Click += (sender, args) => action(taa);
                                }
                            }
                            _currInline = hyperlink;
                        } break;
                        case TextAttributeTypeEnum.Bold:
                            _currInline.FontWeight = FontWeights.Bold;
                            break;
                        case TextAttributeTypeEnum.Color: {
                                TextAttributeColor taColor = a as TextAttributeColor;
                                if (taColor != null) {
                                    _currInline.Foreground = new SolidColorBrush(Color.FromArgb(taColor.Color.A, taColor.Color.R, taColor.Color.G, taColor.Color.B));
                                }
                            } break;
                        case TextAttributeTypeEnum.Font: {
                                TextAttributeFont taFont = a as TextAttributeFont;
                                if (taFont != null) {
                                    if(taFont.Font != null) {
                                        _currInline.FontFamily = new FontFamily(taFont.Font.FontFamily.Name);
                                        _currInline.FontSize = taFont.Font.Size;
                                        if(taFont.Font.Style.HasFlag(System.Drawing.FontStyle.Bold)) {
                                            _currInline.FontWeight = FontWeights.Bold;
                                        }
                                        if(taFont.Font.Style.HasFlag(System.Drawing.FontStyle.Italic)) {
                                            _currInline.FontStyle = FontStyles.Italic;
                                        }
                                        if(taFont.Font.Style.HasFlag(System.Drawing.FontStyle.Strikeout)) {
                                            _currInline.TextDecorations.Add(TextDecorations.Strikethrough);
                                        }
                                        if(taFont.Font.Style.HasFlag(System.Drawing.FontStyle.Underline)) {
                                            _currInline.TextDecorations.Add(TextDecorations.Underline);
                                        }
                                    } else {
                                        _currInline.FontFamily = new FontFamily();
                                        _currInline.FontSize = 12;
                                        _currInline.FontStyle = FontStyles.Normal;
                                        _currInline.FontWeight = FontWeights.Normal;
                                    }
                                }
                            } break;
                        case TextAttributeTypeEnum.Italic:
                            _currInline.FontStyle = FontStyles.Italic;
                            break;
                        case TextAttributeTypeEnum.Underline:
                            _currInline.TextDecorations = TextDecorations.Underline;
                            break;
                        case TextAttributeTypeEnum.Zero:
                            break;
                        case TextAttributeTypeEnum.None:
                            break;
                    }
                });
                _inlines.Add(_currInline);
            }
        }
    }
}
