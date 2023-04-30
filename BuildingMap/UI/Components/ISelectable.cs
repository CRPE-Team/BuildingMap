namespace BuildingMap.UI.Components
{
    public interface ISelectable
    {
        bool CanSelect { get; }

        void Select();
        void Unselect();

        public static ISelectable SelectedObject { get; private set; }

        public static bool Select(ISelectable obj)
        {
            if (!obj.CanSelect) return false;

            SelectedObject?.Unselect();
            SelectedObject = obj;

            return true;
        }

        public static void ResetSelected()
        {
            SelectedObject = null;
        }
    }
}
