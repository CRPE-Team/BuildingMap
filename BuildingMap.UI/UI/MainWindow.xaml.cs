using BuildingMap.UI.Pages;
using System.Windows;

namespace BuildingMap.UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		private readonly PageNavigator _navigator;

		public MainWindow(PageNavigator navigator)
		{
			_navigator = navigator;

			InitializeComponent();

			MainFrame.Content = navigator.GetDefaultPage();
		}
    }
}
