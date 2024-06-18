using System.Windows;
using Practice.ViewModel;

namespace Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.current.services.GetService(typeof(MainWindowViewModel));
        }
    }
}