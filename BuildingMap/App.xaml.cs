using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace BuildingMap
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IUnityContainer Container { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            SetupContainer();

            InitializeMainWindow();
        }

        private void SetupContainer()
        {
            Container = new UnityContainer();

            Container.RegisterInstance(this);

            Container.AddNewExtension<UI.ContainerExtension>();
        }

        private void InitializeMainWindow()
        {
            Container.Resolve<UI.View.MainWindow>().Show();
        }
    }
}
