using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf.Transitions;
using DocumentFormat.OpenXml.Presentation;
using System.Windows.Media.Animation;
using DocumentFormat.OpenXml.Spreadsheet;
using LibraryManagementSystem.DTOs;

namespace LibraryManagementSystem.View.MainClientWindow.BuyBookPage
{
    /// <summary>
    /// Interaction logic for BuyBookPage.xaml
    /// </summary>
    public partial class BuyBookPage : System.Windows.Controls.Page
    {
        private List<string> imagePaths;
        private int currentImageIndex = 0;
    private DispatcherTimer timer;

        public BuyBookPage()
        {

            InitializeComponent();
            imagePaths = new List<string>()
            {
                "/Resource/BuyBook/1.jpg",
                "/Resource/BuyBook/2.jpg",
                "/Resource/BuyBook/3.jpeg",
                "/Resource/BuyBook/4.png",
                "/Resource/BuyBook/5.jpeg"
            };

            int imageIndex = 0;

            // Set the source of the Image control to the first image
            imageControl.Source = new BitmapImage(new Uri(imagePaths[imageIndex], UriKind.RelativeOrAbsolute));

            // Start a timer to change the image every 5 seconds
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (s, e) =>
            {
                // Increment the image index and loop back to the start if at the end
                imageIndex++;
                if (imageIndex >= imagePaths.Count)
                {
                    imageIndex = 0;
                }

                // Apply transition effect
                DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1));
                Storyboard.SetTarget(fadeIn, imageControl);
                Storyboard.SetTargetProperty(fadeIn, new PropertyPath(UIElement.OpacityProperty));

                DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
                Storyboard.SetTarget(fadeOut, imageControl);
                Storyboard.SetTargetProperty(fadeOut, new PropertyPath(UIElement.OpacityProperty));

                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(fadeOut);
                storyboard.Children.Add(fadeIn);
                storyboard.Begin();

                // Set the source of the Image control to the next image
                imageControl.Source = new BitmapImage(new Uri(imagePaths[imageIndex], UriKind.RelativeOrAbsolute));
            };
            timer.Start();
        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            Card a = sender as Card;
            a.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#DFDCD7");
            a.Cursor = Cursors.Hand;
            a.RenderTransform = new TranslateTransform(0, -2);
        }

        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            Card a = sender as Card;
            a.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
            a.Cursor = Cursors.None;
            a.RenderTransform = new TranslateTransform(0, 2);

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Show next image
            currentImageIndex++;
            if (currentImageIndex >= imagePaths.Count)
            {
                currentImageIndex = 0;
            }
            ShowImage(currentImageIndex);
        }

        private void ShowImage(int i)
        {
            // Load image from file and show in imageControl
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(imagePaths[i], UriKind.RelativeOrAbsolute);
            image.EndInit();
            imageControl.Source = image;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            //mediaPlayer.Play();
        }

        private void PreviousImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex == 0)
            {
                ShowImage(2);
                currentImageIndex = 2;
            }
            else if (currentImageIndex > 0)
            {
                ShowImage(currentImageIndex - 1);
                currentImageIndex--;
            }
        }

        private void NextImageButton_Click(object sender, RoutedEventArgs e)
        {
            if(currentImageIndex < 2)
            {
                ShowImage(currentImageIndex + 1);
                currentImageIndex++;
            }
            else if(currentImageIndex == 2)
            {
                ShowImage(0);
                currentImageIndex = 0;
            }    

        }

        #region Search
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(txbFilter.Text))
                return true;
            else
                return ((item as BookDTO).TenSach.IndexOf(txbFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as BookDTO).TacGia.IndexOf(txbFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public void CreateTextBoxFilter()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListViewProducts.ItemsSource);
            view.Filter = Filter;
        }

        private void TextBox_TextChanged_Find(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ListViewProducts.ItemsSource).Refresh();
            CreateTextBoxFilter();
        }
        #endregion

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainClientWindow w = Application.Current.Windows.OfType<MainClientWindow>().FirstOrDefault();

            if(w.WindowState == WindowState.Maximized)
            {
                imageControl.Width *= 1.2;
                imageControl.Height *= 1.2;

                imageControl.Margin = new Thickness(-140, 10 ,10, 10);
                st1.Margin = new Thickness(0, 0, 180, 18);
            }
            else if(w.WindowState == WindowState.Normal) {
                imageControl.Width = 580;
                imageControl.Height = 326;

                st1.Margin = new Thickness(0, 0, 32, 18);
                imageControl.Margin = new Thickness(10, 10, 10, 10);

            }
        }
    }
}
