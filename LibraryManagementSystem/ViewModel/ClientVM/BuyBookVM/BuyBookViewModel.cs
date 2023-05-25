using DocumentFormat.OpenXml.Office2010.CustomUI;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.MainClientWindow.BuyBookPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LibraryManagementSystem.ViewModel.ClientVM.BuyBookVM
{
    public class BuyBookViewModel:BaseViewModel
    {
        #region Property
        private BookDTO _SelectedItem;
        public BookDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }
        private int _quantity;
        public int Quantity
        {
            get { return this._quantity; }
            set { this._quantity = value; OnPropertyChanged(); }
        }
        #endregion

        public ObservableCollection<BookDTO> Books = new ObservableCollection<BookDTO>();

        public ICommand LoadBuyBookPage { get; set; }
        public ICommand LoadBook { get; set; }
        public ICommand LoadNews { get; set; }
        public ICommand PreImage { get; set; }
        public ICommand NextImage { get; set; }
        public ICommand LoadShoppingCart { get; set; }
        public ICommand BackToShopping { get; set; }
        public ICommand LoadDetails { get; set; }
        public ICommand PlusCommand { get; set; }
        public ICommand MinusCommand { get; set; }
        #region tempVar
        private List<string> imagePaths;
        private int currentImageIndex = 0;
        private DispatcherTimer timer;
        private Image a;
        #endregion
        public BuyBookViewModel()
        {
            Quantity = 1;
            LoadBuyBookPage = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                p.Content = new BuyBookPage();
            });

            LoadBook = new RelayCommand<ItemsControl>((p) => { return p != null; }, (p) =>
            {
                #region Load book to card
                using (var context = new LMSEntities1())
                {
                    
                    foreach (var item in context.BOOKs)
                    {
                        BookDTO book = new BookDTO();
                        book.TenSach = item.TENSACH;
                        book.TacGia = item.TACGIA;
                        book.NXB = item.NHAXUATBAN;
                        book.SoLuong = (int)item.SOLUONG;
                        book.Gia = (decimal)item.GIA;
                        book.ImageSource = item.IMAGESOURCE;
                        book.NamXB = (int)item.NAMXUATBAN;
                        Books.Add(book);
                    }
                }

                p.ItemsSource = Books;
                

            #endregion
            
            });

            LoadNews = new RelayCommand<Image>((p) => { return p != null; }, (p) =>
            {
                #region Load news
                a = p;
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
                p.Source = new BitmapImage(new Uri(imagePaths[imageIndex], UriKind.RelativeOrAbsolute));

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
                    Storyboard.SetTarget(fadeIn, p);
                    Storyboard.SetTargetProperty(fadeIn, new PropertyPath(UIElement.OpacityProperty));

                    DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
                    Storyboard.SetTarget(fadeOut, p);
                    Storyboard.SetTargetProperty(fadeOut, new PropertyPath(UIElement.OpacityProperty));

                    Storyboard storyboard = new Storyboard();
                    storyboard.Children.Add(fadeOut);
                    storyboard.Children.Add(fadeIn);
                    storyboard.Begin();

                    // Set the source of the Image control to the next image
                    p.Source = new BitmapImage(new Uri(imagePaths[imageIndex], UriKind.RelativeOrAbsolute));

                    timer.Start();
                };
                    #endregion
            });

            PreImage = new RelayCommand<Image>((p) => { return p != null; }, (p) =>
            {
                if (currentImageIndex == 0)
                {
                    ShowImage(2, p);
                    currentImageIndex = 2;
                }
                else if (currentImageIndex > 0)
                {
                    ShowImage(currentImageIndex - 1, p);
                    currentImageIndex--;
                }
            });

            NextImage = new RelayCommand<Image>((p) => { return p != null; }, (p) => 
            {
                if (currentImageIndex < 2)
                {
                    ShowImage(currentImageIndex + 1, p);
                    currentImageIndex++;
                }
                else if (currentImageIndex == 2)
                {
                    ShowImage(0, p);
                    currentImageIndex = 0;
                }
            });

            LoadShoppingCart = new RelayCommand<object>((p) => { return p != null; }, (p) =>
            {
                MainClientViewModel.main_frame_client.Content = new ShoppingCartPage();
            });

            BackToShopping = new RelayCommand<object>((p) => { return p != null; }, (p) =>
            {
                MainClientViewModel.main_frame_client.Content = new BuyBookPage();
            });

            LoadDetails = new RelayCommand<ListBox>((p) => { return p != null; }, (p) =>
            {
                DetailsBook w = new DetailsBook(SelectedItem);
                w.Show();
            });

            PlusCommand = new RelayCommand<object>((p) => { return p != null; }, (p) =>
            {
                MessageBox.Show("CC");
                Quantity++;
            });

            MinusCommand = new RelayCommand<object>((p) => { return p != null; }, (p) =>
            {
                Quantity--;
            });
        }
        private void ShowImage(int i, Image imageControl)
        {
            // Load image from file and show in imageControl
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(imagePaths[i], UriKind.RelativeOrAbsolute);
            image.EndInit();
            imageControl.Source = image;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Show next image
            currentImageIndex++;
            if (currentImageIndex >= imagePaths.Count)
            {
                currentImageIndex = 0;
            }
            ShowImage(currentImageIndex, a);
        }
    }
}
