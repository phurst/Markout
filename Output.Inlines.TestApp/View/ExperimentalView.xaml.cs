using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Markout.Output.Inlines.TestApp.View {
    /// <summary>
    /// Interaction logic for ExperimentalView.xaml
    /// </summary>
    public partial class ExperimentalView : UserControl {

        private bool _isLoaded = false;

        public ExperimentalView() {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) {
            if (!_isLoaded) {
                _isLoaded = true;
                (new DispatcherTimer(
                    TimeSpan.FromSeconds(1),
                    DispatcherPriority.Normal,
                    (s, e) => {
                        ((DispatcherTimer) s).Stop();
                        DoDelayedStuff();
                    },
                    Dispatcher.CurrentDispatcher)).Start();
            }
        }

        private void DoDelayedStuff() {
            // SayHelloButton.IsEnabled = true;            
        }
    }
}
