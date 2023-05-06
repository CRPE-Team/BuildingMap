using System.Windows.Controls;
using BuildingMap.UI.Visual.Pages.ViewModel;

namespace BuildingMap.UI.Visual.Pages.View
{
    /// <summary>
    /// Логика взаимодействия для MapPage.xaml
    /// </summary>
    public partial class MapPageView : Page
    {
        public MapPageView(MapViewModel viewModel)
        {
			DataContext = viewModel;

            InitializeComponent();
        }
    }
}
