using BuildingMap.UI.Visual.Pages.View;
using System.Windows.Controls;
using Unity;

namespace BuildingMap.UI.Visual.Pages
{
    public class PageNavigator
    {
        public Page GetDefaultPage()
        {
            return GetPage<MapPageView>();
        }

        private T GetPage<T>() where T : Page
        {
            return App.Container.Resolve<T>();
        }
    }
}
