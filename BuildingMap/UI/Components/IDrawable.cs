namespace BuildingMap.UI.Components
{
    public interface IDrawable
    {
        public void StartDraw();
        public void Draw();
        public bool EndDraw();
    }
}
