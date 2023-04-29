using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BuildingMap.UI.Components
{
    public class ImageBackground : BuildingGridItem
    {
        private ImageBrush _image;

        private string _imageSourcePath;

        private readonly ScaleTransform _scaleTransform;

        public string ImageSourcePath { get => _imageSourcePath; set => UpdateImage(value); }

        public ImageSource ImageSource { get => _image.ImageSource; set => _image.ImageSource = value; }

        public double Scale { get => _scaleTransform.ScaleX; set => _scaleTransform.ScaleX = _scaleTransform.ScaleY = Math.Max(0.01, Math.Min(3, value)); }

        public bool CanDrag => Grid?.AllowEdit ?? false;

        public ImageBackground()
        {
            Background = _image = new ImageBrush();
            _image.Stretch = Stretch.Uniform;

            TransformGroup.Children.Add(_scaleTransform = new ScaleTransform());

            RenderTransformOrigin = new Point(0.5, 0.5);
        }

        protected override void UpdatePosition(Vector _)
        {
            var position = (Position.ToVector() + (Grid.RenderOffset / 2).FloorInt() * 2 - Grid.RenderOffset + Grid.Offset) * Grid.Grid.GridSize;
            base.UpdatePosition(position / Scale);
        }

        private void UpdateImage(string path)
        {
            if (!File.Exists(path))
            {
                _imageSourcePath = null;
                _image.ImageSource = null;
                return;
            }

            _imageSourcePath = path;
            _image.ImageSource = new BitmapImage(new Uri(_imageSourcePath, UriKind.RelativeOrAbsolute));
        }

        public void Move(Vector offset)
        {
            Position += offset / Grid.RenderGridSize;
        }
    }
}
