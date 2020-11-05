using System;
using System.Windows.Media.Imaging;

namespace ColoringWithWPF.Tools
{
    class Brush : ITool
    {
        private static Brush instance = null;

        public static Brush GetInstance()
        {
            if (instance == null)
                instance = new Brush();

            return instance;
        }

        public void UseTool(double normalX, double normalY)
        {
            BitmapImageColorMatrix.GetInstance().Repaint(normalX, normalY, Game.GetSelectedColor());
        }
    }
}
