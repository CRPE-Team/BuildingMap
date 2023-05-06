using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace BuildingMap.UI.Visual
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

		protected void UiSync(Action callback)
		{
			Application.Current.Dispatcher.Invoke(callback);
		}

		protected T UiSync<T>(Func<T> callback)
		{
			return Application.Current.Dispatcher.Invoke(callback);
		}

		protected DispatcherOperation UiAsync(Action callback)
		{
			return Application.Current.Dispatcher.InvokeAsync(callback);
		}

		protected DispatcherOperation<T> UiAsync<T>(Func<T> callback)
		{
			return Application.Current.Dispatcher.InvokeAsync(callback);
		}
	}
}
