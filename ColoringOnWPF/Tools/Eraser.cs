using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColoringWithWPF.Tools
{
    class Eraser : ITool
    {
        private static Eraser instance = null;

        public static Eraser GetInstance()
        {
            if (instance == null)
                instance = new Eraser();

            return instance;
        }
        public void UseTool(double normalX, double normalY)
        {
            BitmapImageColorMatrix.GetInstance().Repaint(normalX, normalY, Settings.ImageBaseColor);
        }
    }
}
