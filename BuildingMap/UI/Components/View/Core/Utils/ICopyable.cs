namespace BuildingMap.UI.Components.View.Core.Utils
{
    public interface ICopyable
    {
        private static ICopyable _copyCache;

        public static void CopyToCache(ICopyable obj)
        {
            _copyCache = obj;
        }

        public static ICopyable GetCopy() => _copyCache.CreateCopy();

        ICopyable CreateCopy();
    }
}
