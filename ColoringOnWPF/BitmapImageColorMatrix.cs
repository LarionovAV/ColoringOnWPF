using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColoringWithWPF
{
    class BitmapImageColorMatrix
    {
        private static BitmapImageColorMatrix instance = null;
        private Color[,] Colors { get; set; } = null;

        private int width = 0;
        private int height = 0;

        public static BitmapImageColorMatrix GetInstance()
        {
            return instance;
        }

        private BitmapImageColorMatrix() { }
        private BitmapImageColorMatrix(BitmapImage bitmapImage)
        {
            instance = new BitmapImageColorMatrix();
            InitColors(bitmapImage);
        }
        public static void Init(BitmapImage bitmapImage)
        {
            instance = new BitmapImageColorMatrix(bitmapImage);
        }

        /// <summary>
        /// Инициализирует матрицу цветов на основе BitmapImage
        /// </summary>
        /// <param name="BitmapResource"> Экземпляр BitmapImage, на основе которого будет построена матрица цветов</param>
        public void InitColors(BitmapImage BitmapResource)
        {
            //Если источник равен null, сбрасываем текущую матрицу цветов
            //и обнуляем высоту и ширину
            if (BitmapResource == null)
            {
                Colors = null;
                width = height = 0;
            }
            //иначе...
            else
            {
                //... устанавливаем высоту и ширину и создаем новую матрицу цветов
                width = (int)(BitmapResource.PixelWidth);
                height = (int)(BitmapResource.PixelHeight);
                Colors = new Color[height, width];
                //Создаем байтовый массив, в который скопируем данные пикселей изображения
                //Изображение копируется побайтово в формате BGRA 
                //(то есть для получения цвета одного пикселя требуется 4 байта - Синий, Зеленый, Красный и Альфа)
                byte[] colorBytes = new byte[height * width * 4];
                BitmapResource.CopyPixels(colorBytes, width * 4, 0);

                // Для простоты заведеи переменную-индекс для байтового массива
                // Можно обойтись без нее, использовав формулу 4 * (i * width + j) + k
                // (здесь k - число от 0 до 3, указывающее дополнительное смещение для доступа к байтам BGRA-каналов соответственно)
                int currentColorByteIndex = 0;
                //Пройдемся по матрице цветов, устанавливая байты в нужном порядке
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Colors[i, j].B = colorBytes[currentColorByteIndex++];
                        Colors[i, j].G = colorBytes[currentColorByteIndex++];
                        Colors[i, j].R = colorBytes[currentColorByteIndex++];
                        Colors[i, j].A = colorBytes[currentColorByteIndex++];
                    }
                }

                //Проведем базовую подготовку матрицы цветов
                BasePreparation();
            }
        }

        /// <summary>
        /// Конвертирует матрицу цветов в BitmapSource.
        /// </summary>
        /// <returns> Экземпляр BitmapSource, построенный на основе матрицы цветов. </returns>
        public BitmapSource ConvertColorsArrayToBitmap()
        {
            // Создадим массив байтов для хранения цветов изображения побайтово
            byte[] colorBytes = new byte[height * width * 4];

            // Создадим переменную-индекс для удобства работы с байтовым массивом
            int currentColorByteIndex = 0;
            //Пройдемся по матрице цветов, заполняя байтовый массив в правильном порядке
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    colorBytes[currentColorByteIndex++] = Colors[i, j].B;
                    colorBytes[currentColorByteIndex++] = Colors[i, j].G;
                    colorBytes[currentColorByteIndex++] = Colors[i, j].R;
                    colorBytes[currentColorByteIndex++] = Colors[i, j].A;
                }
            }
            //Создадим экземпляр BitmapSource на основе байтового массива и вернем его
            return BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, BitmapPalettes.WebPaletteTransparent, colorBytes, width * 4);
        }

        /// <summary>
        /// Заменяет текущий цвет области, к которой принадлежит точка с нормированными координатами x и y,
        /// на новый указанный цвет
        /// </summary>
        /// <param name="normalX"> Нормированная координата x. Должна принимать значение от 0 до 1. </param>
        /// <param name="normalY"> Нормированная координата y. Должна принимать значение от 0 до 1. </param>
        /// <param name="newColor"> Новый цает для области, содержащей точку (x, y) </param>
        public void Repaint(double normalX, double normalY, Color newColor)
        {
            //  Получаем реальные координаты начальной точки перерисовки
            int x = (int)(width * normalX);
            int y = (int)(height * normalY);

            //  Для перерисовки используется очередб точек
            //  Для точек был создан отдельный класс Point
            //  Можно было бы использовать аналогичный класс из стандартного пространства имен,
            //  но координаты в нем принимают вещственное хначение, 
            //  что может привести к проблемам сравнения, вызванным ошибками округления
            Queue<Point> pointsToPaint = new Queue<Point>();
            //  Поместим начальную точку в очередь
            pointsToPaint.Enqueue(new Point(x, y));

            //  Перекрашиваем точки из очереди, пока очередб не опустеет
            //  При этом очередь на каждой иттерации может пополняться
            while (pointsToPaint.Count > 0)
            {
                //  Извлекаем первую точку в очереди
                Point curPoint = pointsToPaint.Dequeue();

                //   Если точка не прозрачная и имеет цвет, отличный от цвета контура изображения, перекрашиваем ее
                if (Colors[curPoint.Y, curPoint.X].A != 0 && Colors[curPoint.Y, curPoint.X] != Settings.BorderColor)
                    Colors[curPoint.Y, curPoint.X] = newColor;
                //  В противном случае такая точка могла попасть в очередь только в качестве начальной, 
                //  и имеет смысл прервать цикл
                else
                    break;

                // Получим 4 новых точки: выше, ниже, правее и левее текущей
                // Если новая точка будет выходить за пределы изображения, установим ее в NULL
                Point up = curPoint.Y == 0 ? null : new Point(curPoint.X, curPoint.Y - 1);
                Point down = curPoint.Y == height - 1 ? null : new Point(curPoint.X, curPoint.Y + 1);
                Point left = curPoint.X == 0 ? null : new Point(curPoint.X - 1, curPoint.Y);
                Point right = curPoint.X == width - 1 ? null : new Point(curPoint.X + 1, curPoint.Y);

                //  Для каждой соседней точки:
                //      если:
                //              - точка не принимает значение NULL
                //              - точка не принадлежит множеству точек контура изображения
                //              - точка еще не была перекрашена
                //              - точка еще не содержится в очереди
                //      то добавляем эту точку в очередь
                if (up != null &&
                    Colors[up.Y, up.X] != Settings.BorderColor &&
                    Colors[up.Y, up.X] != newColor &&
                    !pointsToPaint.Contains(up))
                    pointsToPaint.Enqueue(up);
                if (down != null &&
                    Colors[down.Y, down.X] != Settings.BorderColor &&
                    Colors[down.Y, down.X] != newColor &&
                    !pointsToPaint.Contains(down))
                    pointsToPaint.Enqueue(down);
                if (left != null &&
                    Colors[left.Y, left.X] != Settings.BorderColor &&
                    Colors[left.Y, left.X] != newColor &&
                    !pointsToPaint.Contains(left))
                    pointsToPaint.Enqueue(left);
                if (right != null &&
                    Colors[right.Y, right.X] != Settings.BorderColor &&
                    Colors[right.Y, right.X] != newColor &&
                    !pointsToPaint.Contains(right))
                    pointsToPaint.Enqueue(right);
            }
        }

        /// <summary>
        /// Возвращает значение цвета в точке с указанными нормированными координатами.
        /// </summary>
        /// <param name="normalX"> Нормированная координата x. Должна принимать значение от 0 до 1. </param>
        /// <param name="normalY"> Нормированная координата y. Должна принимать значение от 0 до 1. </param>
        /// <returns> Цвет точки с указанными координатами. </returns>
        public Color GetColor(double normalX, double normalY)
        {
            return Colors[(int)(normalY * height), (int)(normalX * width)];
        }

        /// <summary>
        /// Сбрасывет текущую раскраску изображения
        /// </summary>
        public void ClearAllPicture()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (Colors[i, j].A != 0 && Colors[i, j] != Settings.BorderColor)
                    {
                        Colors[i, j] = Settings.ImageBaseColor;
                    }
                }
            }
        }

        /// <summary>
        /// Начальное приготовление изображения. 
        /// После подготовки изображение будет содержать только цвет контура и базовый цвет.
        /// </summary>
        private void BasePreparation()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (Colors[j, i].A > 0)
                    {
                        if (Colors[j, i].R * 0.299 + Colors[j, i].R * 0.299 + Colors[j, i].R * 0.299 > Settings.BrightTreshhold)
                            Colors[j, i] = Settings.ImageBaseColor;
                        else
                            Colors[j, i] = Settings.BorderColor;
                    }

                }
            }
        }
    }
}
