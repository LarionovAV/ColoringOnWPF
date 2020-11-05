using ColoringWithWPF.Tools;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColoringWithWPF
{
    class Game
    {
        private static Button selectedTool;
        private static Button selectedColor;

        //  Инициация игры
        public static void GameInit(string picturePath, Button selTool, Button selColor)
        {

            CommonSource.Init();

            //  Инициация матрицы цветов
            BitmapImageColorMatrix.Init(new BitmapImage(new Uri(picturePath)));
            //  Устанавливаем выбранный цвет и инструммент
            SelectColor(selColor);
            SelectTool(selTool);
        }
        
        /// <summary>
        /// Устанавливает новый выбранный инструмент
        /// </summary>
        /// <param name="newSelectedTool"> Кнопка, к которой привязан инструмент. Инструмент должен храниться в поле Tag. </param>
        public static void SelectTool(Button newSelectedTool)
        {
            if (selectedTool != null)
                selectedTool.Background = newSelectedTool.Background;
            selectedTool = newSelectedTool;
            selectedTool.Background = Settings.SelectedToolBackground;
        }

        /// <summary>
        /// Устанавливает новый выбранный цвет
        /// </summary>
        /// <param name="newSelectedColor"> Кнопка, к которой привязан цвет. </param>
        public static void SelectColor(Button newSelectedColor)
        {
            if (selectedColor != null)
                selectedColor.Content = null;
            selectedColor = newSelectedColor;
            selectedColor.Content = CommonSource.SelectedImage;
        }

        public static BitmapSource GetBitmap()
        {
            return BitmapImageColorMatrix.GetInstance().ConvertColorsArrayToBitmap();
        }

        public static void UseTool(double normalX, double normalY)
        {
            (selectedTool.Tag as ITool).UseTool(normalX, normalY);
        }

        /// <summary>
        /// Возвращает текущий выбранный цвет.
        /// </summary>
        /// <returns></returns>
        public static Color GetSelectedColor()
        {
            return (selectedColor.Background is SolidColorBrush) ?
                (selectedColor.Background as SolidColorBrush).Color :
                Settings.ImageBaseColor;
        }

        /// <summary>
        /// Восстанавливает исходное изображение
        /// </summary>
        public static void Restore()
        {
            BitmapImageColorMatrix.GetInstance().ClearAllPicture();
        }
    }
}
