using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColoringWithWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        //  Поля-константы
        #region

        private const string CHOICE_PIC_LABEL_TEXT = "Выберите изображение";    //Заголовок, выводимый над списком изображений
        private const string WINDOW_TITLE = "Раскраска";                        //Заголовок окна
        private readonly string PIC_LOCATION = Environment.CurrentDirectory + "\\Pictures";  //Путь к папке с изображениями

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            //  Настраиваем окно
            Title = WINDOW_TITLE;
            ChoicePicLabel.Content = CHOICE_PIC_LABEL_TEXT;
            Background = Settings.MainBackground;
            Width = Settings.DefaultWindowWidth;
            Height = Settings.DefaultWindowHeight;

            //  Ищем в заданной папке с изображениями все изображения 
            //  (для удобства ограничимся форматом png)
            //  теперь для добавления картинок для раскрашивания достаточно закинуть нужное изображение в папку
            //  (рекомендуется предварительно убедиться что изображение ч/б и не имеет разрывов в контурах)
            foreach (string pic in Directory.GetFiles(PIC_LOCATION, "*.png"))
            {
                //  Для кажжого найденного изображения создаем кнопку
                Button newButton = new Button();

                //  В качестве заполнителя кнопки будет выступать само изображение
                Image img = new Image();
                //  Установим высоту изображения на кнопке
                img.Height = 250;
                //  Добавим тэг, хранящий путь к изображению.
                //  Будем его использовать для открытия окна-раскраски
                img.Tag = pic;
                //  Установим способ растяжени изображения с сохранением пропорций
                img.Stretch = Stretch.Uniform;
                //  Загрузим изображение и установим его в качестве контента кнопки
                img.Source = new BitmapImage(new Uri(pic));
                newButton.Content = img;
                //  Добавим обработчик события на нажатие кнопки
                newButton.PreviewMouseLeftButtonDown += SelectPicture;
                //  Добавим кнопку в Wrap Panel, где видны доступные для раскрашивания изображения
                ChoicePicPanel.Children.Add(newButton);
            }
        }

        private void SelectPicture(object sender, EventArgs args)
        {
            if (sender is Button)
            {
                if ((sender as Button).Content is FrameworkElement)
                {
                    if (((sender as Button).Content as FrameworkElement).Tag != null)
                    {
                        new ColoringWindow(((sender as Button).Content as FrameworkElement).Tag.ToString()).Show();
                    }
                    else
                        throw new NullReferenceException("Свойство \"Tag\" не задано");
                }
                else
                    throw new Exception("Обработчик события не поддерживает объекты с данным типом контента");
            }
            else
                throw new Exception("Обработчик события не поддерживает данный тип объектов");

        }
    }
}
