using System.Windows.Controls;
using BuildingMap.UI.Pages.ViewModel;

namespace BuildingMap.UI.Pages.View
{
    /// <summary>
    /// Логика взаимодействия для MapPage.xaml
    /// </summary>
    public partial class MapPageView : Page
    {
        public MapPageView(MapPageViewModel viewModel)
        {
			DataContext = viewModel;

            InitializeComponent();
        }
    }
}
