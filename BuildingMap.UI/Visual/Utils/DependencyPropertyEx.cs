﻿using System.Runtime.CompilerServices;
using System.Windows;

namespace BuildingMap.UI.Visual.Utils
{
	public delegate void PropertyChangedCallback<T>(T d, DependencyPropertyChangedEventArgs e) where T : DependencyObject;
	public delegate object CoerceValueCallback<T, TValue>(T d, TValue e) where T : DependencyObject;
    public delegate bool ValidateValueCallback<TValue>(TValue value);

	public static class DependencyPropertyEx
	{
		private const string PropertySuffix = "Property";

		public static DependencyProperty Register<TProperty, TOwner>(TProperty defaultValue = default, [CallerMemberName] string name = "") where TOwner : DependencyObject
		{
			return DependencyProperty.Register(
				ResolvePropertyName(name),
				typeof(TProperty),
				typeof(TOwner),
				new FrameworkPropertyMetadata(defaultValue));
		}

		public static DependencyProperty Register<TProperty, TOwner>(
			FrameworkPropertyMetadataOptions flags, 
			TProperty defaultValue = default, 
			[CallerMemberName] string name = "") where TOwner : DependencyObject
		{
			return DependencyProperty.Register(
				ResolvePropertyName(name),
				typeof(TProperty),
				typeof(TOwner),
				new FrameworkPropertyMetadata(defaultValue, flags));
		}

		public static DependencyProperty Register<TProperty, TOwner>(
			PropertyChangedCallback<TOwner> propertyChangedCallback, 
			TProperty defaultValue = default, 
			[CallerMemberName] string name = "") where TOwner : DependencyObject
		{
			return DependencyProperty.Register(
				ResolvePropertyName(name), 
				typeof(TProperty), typeof(TOwner), 
				new FrameworkPropertyMetadata(defaultValue) 
				{ 
					PropertyChangedCallback = new PropertyChangedCallback((d, args) => propertyChangedCallback((TOwner) d, args))
				});
		}

		public static DependencyProperty Register<TProperty, TOwner>(
			PropertyChangedCallback<TOwner> propertyChangedCallback,
			CoerceValueCallback<TOwner, TProperty> coerceValueCallback,
			TProperty defaultValue = default,
			[CallerMemberName] string name = "") where TOwner : DependencyObject
		{
			return DependencyProperty.Register(
				ResolvePropertyName(name),
				typeof(TProperty), typeof(TOwner),
				new FrameworkPropertyMetadata(defaultValue)
				{
					PropertyChangedCallback = new PropertyChangedCallback((d, args) => propertyChangedCallback((TOwner) d, args)),
					CoerceValueCallback = new CoerceValueCallback((d, value) => coerceValueCallback((TOwner) d, (TProperty) value))
				});
		}

		public static DependencyProperty Register<TProperty, TOwner>(
			PropertyChangedCallback<TOwner> propertyChangedCallback,
			FrameworkPropertyMetadataOptions flags,
			TProperty defaultValue = default,
			[CallerMemberName] string name = "") where TOwner : DependencyObject
		{
			return DependencyProperty.Register(
				ResolvePropertyName(name),
				typeof(TProperty), typeof(TOwner),
				new FrameworkPropertyMetadata(defaultValue, flags)
				{
					PropertyChangedCallback = new PropertyChangedCallback((d, args) => propertyChangedCallback((TOwner) d, args))
				});
		}

		public static DependencyProperty Register<TProperty, TOwner>(
			PropertyChangedCallback<TOwner> propertyChangedCallback,
			CoerceValueCallback<TOwner, TProperty> coerceValueCallback,
			FrameworkPropertyMetadataOptions flags,
			TProperty defaultValue = default,
			[CallerMemberName] string name = "") where TOwner : DependencyObject
		{
			return DependencyProperty.Register(
				ResolvePropertyName(name),
				typeof(TProperty), typeof(TOwner),
				new FrameworkPropertyMetadata(defaultValue, flags)
				{
					PropertyChangedCallback = new PropertyChangedCallback((d, args) => propertyChangedCallback((TOwner) d, args)),
					CoerceValueCallback = new CoerceValueCallback((d, value) => coerceValueCallback((TOwner) d, (TProperty) value))
				});
		}

		public static DependencyProperty Register<TProperty, TOwner>(
			PropertyChangedCallback<TOwner> propertyChangedCallback,
			ValidateValueCallback<TProperty> validateValueCallback,
			TProperty defaultValue = default,
			[CallerMemberName] string name = "") where TOwner : DependencyObject
		{
			return DependencyProperty.Register(
				ResolvePropertyName(name),
				typeof(TProperty), typeof(TOwner),
				new FrameworkPropertyMetadata(defaultValue)
				{
					PropertyChangedCallback = new PropertyChangedCallback((d, args) => propertyChangedCallback((TOwner) d, args))
				},
				new ValidateValueCallback(value => validateValueCallback((TProperty) value)));
		}

		private static string ResolvePropertyName(string name)
		{
			if (name.EndsWith(PropertySuffix))
			{
				return name.Substring(0, name.Length - PropertySuffix.Length);
			}

			return name;
		}
	}
}
