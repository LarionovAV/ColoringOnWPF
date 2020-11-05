using System.Windows.Media.Imaging;

namespace ColoringWithWPF.Tools
{
    //Интерфейс для инструментов, действующих на определенную зону
    interface ITool
    {
        void UseTool(double normalX, double normalY);
    }
}
