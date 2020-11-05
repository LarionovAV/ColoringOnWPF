using System.Windows.Controls;
using System.Windows.Media;

namespace ColoringWithWPF
{
    //  Класс Settings предназначен для хранения настроек приложения
    static class Settings
    {
        static public Brush MainBackground { get; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#251537")); //   Основной фон
        static public Brush SelectedToolBackground { get; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF00")); //   Фон выбранного инструмента

        static public Color BorderColor { get; } = Color.FromArgb(255, 0, 0, 0);
        static public Color ImageBaseColor { get; } = Color.FromArgb(255, 255, 255, 255);
        static public byte BrightTreshhold { get; } = 200;
        //Дефолтные размеры окна
        static public double DefaultWindowHeight { get; } = 720;
        static public double DefaultWindowWidth { get; } = 1280;
    }
}
