using System.Windows;
using Markout.Output.Inlines.TestApp.ViewModel;

namespace Markout.Output.Inlines.TestApp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Loaded += (sender, args) => {
                MainView.DataContext = new MainViewModel();
            };
        }
    }
}
