using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ColoringWithWPF
{
    class CommonSource
    {
        static public Image SelectedImage { get; } = new Image();

        static public void Init() {
            SelectedImage.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Icons\\Selected.png"));
        }
    }
}
