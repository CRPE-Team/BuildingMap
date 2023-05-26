namespace BuildingMap.UI.Visual.Components.ViewModel
{
	public class ToolBarViewModel : ObservableObject
	{
		public ToolBarViewModel(TemplateListViewModel templateMenuViewModel)
		{
			TemplateListViewModel = templateMenuViewModel;
		}

		public TemplateListViewModel TemplateListViewModel { get; }
	}
}
