using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using Markout.Common.DataModel.Attribute;
using Markout.Common.DataModel.Elements;
using Markout.Input.Parser;

namespace Markout.Output.Inlines.TestApp.ViewModel {

    public class MainViewModel : BaseViewModel {

        private string _markupText;
        private readonly MarkoutParser _markupParser = new MarkoutParser();
        private readonly MarkoutRenderer _renderer = new MarkoutRenderer();

        public MainViewModel() {
            UseSelectedMarkup = new Command {ExecuteAction = () => MarkupText = SelectedMarkup != null ? SelectedMarkup.Item2 : string.Empty};
            Markups = new ObservableCollection<Tuple<string, string>>();
            MarkupInlines = new ObservableCollection<Inline>();
            (new DispatcherTimer(
                new TimeSpan(0, 0, 0, 0, 100), 
                DispatcherPriority.Normal, 
                (s, e) => {
                    ((DispatcherTimer)s).Stop();
                    LoadMarkups();
                }, 
                Dispatcher.CurrentDispatcher)).Start();
        }

        public ICommand UseSelectedMarkup { get; set; }

        public ObservableCollection<Tuple<string,string>> Markups { get; set; }

        public Tuple<string, string> SelectedMarkup { get; set; }

        public string MarkupText {
            get { return _markupText; }
            set {
                _markupText = value;
                UpdateInlines();
            }
        }

        public ObservableCollection<Inline> MarkupInlines { get; set; }

        private void UpdateInlines() {
            IEnumerable<TextRun> textRuns = _markupParser.Parse(MarkupText);
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

        private void LoadMarkups() {
            try {
                IEnumerable<Tuple<string, string>> markups = ReadMarkups();
                Markups.Clear();
                markups.ToList().ForEach(mu => Markups.Add(mu));
                SelectedMarkup = Markups.FirstOrDefault();
            } catch(Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private IEnumerable<Tuple<string, string>> ReadMarkups() {
            List<Tuple<string, string>> markups = new List<Tuple<string, string>>();
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            if (codeBase.StartsWith("file:///")) {
                codeBase = codeBase.Substring("file:///".Length);
            }
            string dirPath = Path.Combine(Path.GetDirectoryName(codeBase), "Markups");
            Directory.EnumerateFiles(dirPath, "*.txt").ToList().ForEach(filePath => {
                Debug.WriteLine(string.Format("Reading Markup from file: {0}", filePath));
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                markups.Add(new Tuple<string, string>(Path.GetFileNameWithoutExtension(filePath), File.ReadAllText(filePath)));
            });
            return markups.AsEnumerable();
        }

        private class Command : ICommand {

            public Action ExecuteAction { get; set; }

            public bool CanExecute(object parameter) {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter) {
                ExecuteAction();
            }
        }
    }
}
