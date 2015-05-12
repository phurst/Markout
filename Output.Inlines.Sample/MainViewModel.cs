using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Documents;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Elements;
using Markout.Input.Parser;

namespace Markout.Output.Inlines.Sample {

    public class MainViewModel : INotifyPropertyChanged {

        private string _inputText;
        private readonly MarkoutParser _markoutParser = new MarkoutParser();
        private readonly MarkoutRenderer _renderer = new MarkoutRenderer();

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel() {
            MarkupInlines = new ObservableCollection<Inline>();
        }

        public string InputText {
            get { return _inputText; }
            set {
                _inputText = value;
                UpdateInlines();
                NotifyPropertyChanged("InputText");
            }
        }

        public ObservableCollection<Inline> MarkupInlines { get; set; }

        public void Initialize() {
            InputText = @"" +
                        "plain {b}bold {u}bold underline {b}underline {i}underline italic {0}plain\n" +
                        "{u}underline {u}{b}bold {0}plain\n" +
                        "plain {a:http://google.com:LaunchUrl}Launch Google in Browser{a} plain\n" +
                        "plain {a:SayHello:AnchorAction}Say \"Hello\" to me!{a} plain\n" +
                        "plain {color:Red}Red {b}Red Bold {color}Bold {b}plain\n" +
                        "plain {color:FF008000}Dark Green {b}Dark Green Bold {color}{b}plain\n" +
                        "plain {font:Rockwell Extra Bold:24}This is Rockwell Extra Bold 24em in {color:red}Red{color} and {color:green}Green{color}\n" +
                        "no color{font} plain.\n" +
                        "plain {font:Rockwell Extra Bold:16:su}This is Rockwell Extra Bold 16em with strikeout and underline{font} plain.\n" +
                        "";
        }

        private void UpdateInlines() {
            IEnumerable<TextRun> textRuns = _markoutParser.Parse(InputText);
            IEnumerable<Inline> inlines = _renderer.Render(textRuns, new Dictionary<string, Action<TextAttributeAnchor>> {
                {
                    "LaunchUrl",
                    anchor => Process.Start(anchor.Uri.ToString())
                },
                {
                    "AnchorAction",
                    anchor => MessageBox.Show("Hello there!")
                },
            });
            MarkupInlines.Clear();
            inlines.ToList().ForEach(inline => MarkupInlines.Add(inline));
        }

        private void NotifyPropertyChanged(string propertyName) {
            PropertyChangedEventHandler h = PropertyChanged;
            if (h != null) {
                h(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
