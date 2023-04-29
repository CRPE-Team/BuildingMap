namespace BuildingMap.UI.Components
{
    public interface ISelectable
    {
        void Select();
        void Unselect();

        public static ISelectable SelectedObject { get; private set; }

        public static void Select(ISelectable obj)
        {
            SelectedObject?.Unselect();
            SelectedObject = obj;
        }

        public static void ResetSelected()
        {
            SelectedObject = null;
        }
    }
}
