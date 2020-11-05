using ColoringWithWPF.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColoringWithWPF
{
    /// <summary>
    /// Логика взаимодействия для ColoringWindow.xaml
    /// </summary>
    public partial class ColoringWindow : Window
    {
        private string WINDOW_TITLE = "Раскраска";
        public ColoringWindow(string picPath)
        {
            InitializeComponent();

            //  Настраиваем окно
            Title = WINDOW_TITLE;
            Background = Settings.MainBackground;
            Width = Settings.DefaultWindowWidth;
            Height = Settings.DefaultWindowHeight;

            //  Привяжем каждой кнопке инструментов соответствующий инструмент
            BrushBtn.Tag = Tools.Brush.GetInstance();
            EraserBtn.Tag = Eraser.GetInstance();

            //Инициализируем игровое пространство
            Game.GameInit(picPath, BrushBtn, (Button)ColorsList.Children[0]);
            ColoringPicture.Stretch = Stretch.Uniform;
            ColoringPicture.Source = Game.GetBitmap();            
        }

        private void SelectColorClick(object sender, RoutedEventArgs e)
        {
            Game.SelectColor(sender as Button);
        }

        private void SelectToolClick(object sender, RoutedEventArgs e)
        {
            Game.SelectTool(sender as Button);
        }

        private void RestoreButtonClick(object sender, RoutedEventArgs e)
        {
            Game.Restore();
            ColoringPicture.Source = Game.GetBitmap();
        }
        private void ColoringPictureClick(object sender, MouseEventArgs e)
        {
            double x = e.GetPosition((IInputElement)sender).X / ColoringPicture.ActualWidth;
            double y = e.GetPosition((IInputElement)sender).Y / ColoringPicture.ActualHeight;

            Game.UseTool(x, y);
            ColoringPicture.Source = Game.GetBitmap();

            return;
        }
    }
}
