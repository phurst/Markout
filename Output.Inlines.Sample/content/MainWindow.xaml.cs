using System;
using System.Windows;
using System.Windows.Threading;

namespace Markout.Output.Inlines.Sample {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Loaded += (sender, args) => {
                MainViewModel vm = new MainViewModel();
                DataContext = vm;
                (new DispatcherTimer(
                    new TimeSpan(0,0,0,0,500), 
                    DispatcherPriority.Normal, 
                    (s, e) => {
                        ((DispatcherTimer)s).Stop();
                        vm.Initialize();
                    }, 
                    Dispatcher.CurrentDispatcher)
                    ).Start();
            };
        }
    }
}
