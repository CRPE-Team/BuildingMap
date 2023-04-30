using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BuildingMap.UI
{
	public abstract class ObservableObject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void OnPropertyChanged<T>(ref T obj, T value, [CallerMemberName] string propertyName = "")
		{
			obj = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
