using DocumentFormat.OpenXml.Office2010.CustomUI;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.MainClientWindow.BuyBookPage;
using LibraryManagementSystem.View.MessageBoxCus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LibraryManagementSystem.ViewModel.ClientVM.BuyBookVM
{
    public class BuyBookViewModel : BaseViewModel
    {
        #region Property

        private string _AccountID;
        public string AccountID
        {
            get { return _AccountID; }
            set { _AccountID = value; OnPropertyChanged(); }
        }

        private BookDTO _SelectedItem;
        public BookDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        private BookDTO _SelectedItemCart;
        public BookDTO SelectedItemCart
        {
            get { return _SelectedItemCart; }
            set { _SelectedItemCart = value; OnPropertyChanged(); }
        }

        private int _SelectedItemCartID;
        public int SelectedItemCartID
        {
            get { return _SelectedItemCartID; }
            set { _SelectedItemCartID = value; OnPropertyChanged(); }
        }

        private int _quantity;
        public int Quantity
        {
            get { return this._quantity; }
            set { this._quantity = value; OnPropertyChanged(); }
        }

        private long _totalValueForOneBookID;
        public long TotalValueForOneBookID
        {
            get { return this._totalValueForOneBookID; }
            set { this._totalValueForOneBookID = value; OnPropertyChanged(); }
        }

        private decimal _totalCartvalue;
        public decimal TotalCartValue
        {
            get { return this._totalCartvalue; }
            set { this._totalCartvalue = value;
                if (TotalCartValue != 0)
                    CanCheckout = true;
                else
                    CanCheckout = false;
                OnPropertyChanged(); }
        }

        private string _totalCartvalueStr;
        public string TotalCartValueStr
        {
            get { return this._totalCartvalueStr; }
            set { this._totalCartvalueStr = value; OnPropertyChanged(); }
        }

        private decimal _totalShow;
        public decimal TotalShow
        {
            get { return this._totalShow; }
            set { this._totalShow = value; OnPropertyChanged(); }
        }

        private int _orderID;
        public int OrderID
        {
            get { return _orderID; }
            set { _orderID = value; OnPropertyChanged(); }
        }

        private string _orderName;
        public string OrderName
        {
            get { return _orderName; }
            set { _orderName = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(); }
        }

        private string _phonenumber;
        public string PhoneNumber
        {
            get { return _phonenumber; }
            set { _phonenumber = value; OnPropertyChanged(); }
        }

        private bool _canCheckout;
        public bool CanCheckout
        {
            get { return _canCheckout; }
            set { _canCheckout = value; OnPropertyChanged(); }
        }

        private string _toDay;
        public string ToDay
        {
            get { return _toDay; }
            set { _toDay = value; OnPropertyChanged(); }
        }

        public bool IsGetNow;
        public bool IsGetInCart;

        #region Constrain
        private bool _IsNullNameOrderForm;
        public bool IsNullNameOrderForm
        {
            get { return _IsNullNameOrderForm; }
            set { _IsNullNameOrderForm = value; OnPropertyChanged(); }
        }

        private bool _IsNullPhoneOrderForm;
        public bool IsNullPhoneOrderForm
        {
            get { return _IsNullPhoneOrderForm; }
            set { _IsNullPhoneOrderForm = value; OnPropertyChanged(); }
        }

        private bool _IsNullEmailOrderForm;
        public bool IsNullEmailOrderForm
        {
            get { return _IsNullEmailOrderForm; }
            set { _IsNullEmailOrderForm = value; OnPropertyChanged(); }
        }

        private bool _IsNullAddressOrderForm;
        public bool IsNullAddressOrderForm
        {
            get { return _IsNullAddressOrderForm; }
            set { _IsNullAddressOrderForm = value; OnPropertyChanged(); }
        }
        #endregion
        #endregion

        public ObservableCollection<BookDTO> Books = new ObservableCollection<BookDTO>();
        public ObservableCollection<BookDTO> BooksInCart = new ObservableCollection<BookDTO>();
        public  ObservableCollection<BookDTO> ListDetails = new ObservableCollection<BookDTO>();

        Window _purchaseWindow = new Window();
        #region Command
        public ICommand LoadBuyBookPage { get; set; }
        public ICommand LoadBook { get; set; }
        public ICommand LoadNews { get; set; }
        public ICommand PreImage { get; set; }
        public ICommand NextImage { get; set; }
        public ICommand BackToShopping { get; set; }
        public ICommand LoadDetails { get; set; }
        public ICommand PlusCommand { get; set; }
        public ICommand MinusCommand { get; set; }
        public ICommand CloseDetailBook { get; set; }
        public ICommand AddToCart { get; set; }
        public ICommand LoadShoppingCart { get; set; }
        public ICommand LoadCart { get; set; }
        public ICommand ChangeSelectedItem { get; set; }
        public ICommand DeleteBookInCart { get; set; }
        public ICommand PlusQuantityBookInCart { get; set; }
        public ICommand MinusQuantityBookInCart { get; set; }
        public ICommand GetBookNow { get; set; }
        public ICommand ClosePurchasePage { get; set; }
        public ICommand BuyBookInCart { get; set; }
        public ICommand CompleteOrder { get; set; }

        public ICommand LoadBrief { get; set; }
        //2 command nay de t test thoi, dung quan tam
        public ICommand ShowOrder_Test { get; set; }
        public ICommand DeleteAllOrder_Test { get; set; }
        #endregion

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
                p.Content = new BuyBookPage(AccountID);
            });

            LoadBook = new RelayCommand<ItemsControl>((p) => { return p != null; }, (p) =>
            {
                #region Load book
                Books = new ObservableCollection<BookDTO>();
                using (var context = new LMSEntities1())
                {
                    foreach (var item in context.BOOKs)
                    {
                        if (string.IsNullOrEmpty(item.IMAGESOURCE))
                            continue;
                        BookDTO book = new BookDTO();
                        book.MaSach = item.ID;
                        book.TenSach = item.TENSACH;
                        book.TacGia = item.TACGIA;
                        book.NXB = item.NHAXUATBAN;
                        book.SoLuong = (int)item.SOLUONG;
                        book.MoTa = item.MOTA;
                        book.TheLoai = item.THELOAI;
                        book.Gia = (decimal)item.GIA;
                        book.ImageSource = item.IMAGESOURCE;
                        book.NamXB = (item.NAMXUATBAN).ToString();
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

            LoadDetails = new RelayCommand<ListBox>((p) => { return p != null; }, (p) =>
            {
                Quantity = 1;
                DetailsBook w = new DetailsBook(SelectedItem);
                //MessageBox.Show(SelectedItem.MoTa);
                w.ShowDialog();
            });

            PlusCommand = new RelayCommand<object>((p) => { if (Quantity < SelectedItem.SoLuong) return true; return false; }, (p) =>
            {
                Quantity++;
            });

            MinusCommand = new RelayCommand<object>((p) => { if (Quantity > 0) return true; return false; }, (p) =>
            {
                Quantity--;
            });

            CloseDetailBook = new RelayCommand<Window>((p) => { return p != null; }, (p) =>
            {
                p.Close();
            });

            AddToCart = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                try
                {
                    using (var context = new LMSEntities1())
                    {
                        CART cart = new CART();
                        cart.MAKH = Int32.Parse(AccountID);
                        cart.MASACH = SelectedItem.MaSach;
                        cart.SOLUONGHT = Quantity;
                        context.CARTs.Add(cart);
                        context.SaveChanges();
                    }
                    MessageBoxLMS msb = new MessageBoxLMS("Notification", "Successful add this book to your cart!", MessageType.Accept, MessageButtons.OK);
                    msb.ShowDialog();
                }
                catch
                {
                    MessageBoxLMS msb = new MessageBoxLMS("Warning", "Error - This book is already in cart!.", MessageType.Error, MessageButtons.OK);
                    msb.ShowDialog();
                }
            });

            LoadShoppingCart = new RelayCommand<object>((p) => { return p != null; }, (p) =>
            {
                MainClientViewModel.main_frame_client.Content = new ShoppingCartPage();
            });

            LoadCart = new RelayCommand<ListBox>((p) => { return p != null; }, (p) =>
            {
                LoadBookInCart(p);
            });

            ChangeSelectedItem = new RelayCommand<ListBox>((p) => { return true; }, (p) =>
            {
                if (p.SelectedIndex != -1)
                    SelectedItemCartID = p.SelectedIndex;
            });

            DeleteBookInCart = new RelayCommand<ListBox>((p) => { return p.SelectedItem != null; }, (p) =>
            {
                string masach = (p.SelectedItem as BookDTO).MaSach.ToString();
                using (var context = new LMSEntities1())
                {
                    SqlConnection connection = new SqlConnection(context.Database.Connection.ConnectionString);
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.Parameters.AddWithValue("@masach", masach);
                    command.Parameters.AddWithValue("@makhachhang", AccountID);
                    MessageBoxLMS msb = new MessageBoxLMS("Warning", "Delete this book?", MessageType.Waitting, MessageButtons.YesNo);
                    if (msb.ShowDialog() == true)
                    {
                        try
                        {
                            command.CommandText = "delete from CART where MAKH = @makhachhang and MASACH = @masach";
                            context.SaveChanges();
                            if (command.ExecuteNonQuery() != 0)
                            {
                                msb = new MessageBoxLMS("Notification", "Deleting is successful", MessageType.Accept, MessageButtons.OK);
                                msb.ShowDialog();
                                LoadBookInCart(p);
                            }
                        }
                        catch
                        {
                            msb = new MessageBoxLMS("Notification", "Cannot delete this user", MessageType.Error, MessageButtons.OK);
                            msb.ShowDialog();
                        }
                    }
                }
                LoadBookInCart(p);

            });

            PlusQuantityBookInCart = new RelayCommand<ListBox>((p) => { return p.SelectedItem != null; }, (p) =>
            {
                int masach = (p.SelectedItem as BookDTO).MaSach;
                int maxQuantity = 0;
                using (var context = new LMSEntities1())
                {
                    foreach (var item in context.BOOKs)
                    {
                        if (item.ID == masach)
                        {
                            maxQuantity = item.SOLUONG ?? 0;
                            break;
                        }
                    }
                    foreach (var item in context.CARTs)
                    {
                        if(item.MAKH == Int32.Parse(AccountID))
                        {
                            if (item.MASACH == masach && item.SOLUONGHT < maxQuantity)
                            {
                                item.SOLUONGHT++;
                                break;
                            }
                            if (item.MASACH == masach && item.SOLUONGHT == maxQuantity)
                            {
                                MessageBoxLMS msb = new MessageBoxLMS("Notification", "The maximum amount has been reached", MessageType.Accept, MessageButtons.OK);
                                msb.ShowDialog();
                                break;
                            }
                        }                                                
                    }
                    context.SaveChanges();
                }
                LoadBookInCart(p);

            });

            MinusQuantityBookInCart = new RelayCommand<ListBox>((p) => { return p.SelectedItem != null; }, (p) =>
            {
                int masach = (p.SelectedItem as BookDTO).MaSach;
                using (var context = new LMSEntities1())
                {
                    foreach (var item in context.CARTs)
                    {
                        if (item.MAKH == Int32.Parse(AccountID))
                        {
                            if (item.MASACH == masach && item.SOLUONGHT > 0)
                            {
                                item.SOLUONGHT--;
                                break;
                            }
                        }
                    }
                    context.SaveChanges();
                }
                LoadBookInCart(p);
            });

            BackToShopping = new RelayCommand<object>((p) => { return p != null; }, (p) =>
            {
                MainClientViewModel.main_frame_client.Content = new BuyBookPage(AccountID);
            });

            GetBookNow = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                TotalValueForOneBookID = (long)(SelectedItem.Gia * Quantity);
                TotalShow = TotalValueForOneBookID;
                PurchasePage w = new PurchasePage("GetBookNow");
                IsGetNow = true;
                IsGetInCart = false;
                ToDay = DateTime.Now.ToShortDateString();
                w.ShowDialog();
                Application.Current.Windows.OfType<DetailsBook>().FirstOrDefault().Close();
            });

            BuyBookInCart = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                TotalShow = (long)TotalCartValue;
                PurchasePage w = new PurchasePage("BuyBookInCart");
                ToDay = DateTime.Now.ToShortDateString();
                IsGetNow = false;
                IsGetInCart = true;
                w.ShowDialog();
            });

            ClosePurchasePage = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });

            LoadBrief = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                ListDetails = new ObservableCollection<BookDTO>();
                if(IsGetNow)
                {
                    ListDetails.Add(new BookDTO { TenSach = SelectedItem.TenSach, SoLuong = Quantity});
                    p.ItemsSource = ListDetails;
                    return;
                }    
                ListDetails = BooksInCart;
                p.ItemsSource = ListDetails;
            });

            CompleteOrder = new RelayCommand<string>((p) => { return p != null; }, (p) =>
            {
                IsNullNameOrderForm = IsNullEmailOrderForm = IsNullPhoneOrderForm = IsNullAddressOrderForm = false;

                if (string.IsNullOrEmpty(OrderName)) IsNullNameOrderForm = true;
                if (string.IsNullOrEmpty(Email)) IsNullEmailOrderForm = true;
                if (string.IsNullOrEmpty(PhoneNumber)) IsNullPhoneOrderForm = true;
                if (string.IsNullOrEmpty(Address)) IsNullAddressOrderForm = true;

                if (IsNullNameOrderForm || IsNullEmailOrderForm || IsNullPhoneOrderForm || IsNullAddressOrderForm) return;

                string match = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                Regex reg = new Regex(match);

                if (reg.IsMatch(Email) == false)
                {
                    MessageBoxLMS ms = new MessageBoxLMS("Thông báo", "Email address is not valid!", MessageType.Error, MessageButtons.OK);
                    ms.ShowDialog();
                    return;
                }
                else
                {
                    match = @"^(\+[0-9]{1,3}[- ]?)?([0-9]{10})$";
                    reg = new Regex(match);
                    if (!reg.IsMatch(PhoneNumber))
                    {
                        MessageBoxLMS ms = new MessageBoxLMS("Thông báo", "Phone number is not valid!", MessageType.Error, MessageButtons.OK);
                        ms.ShowDialog();
                        return;
                    }
                    else
                    {
                        if (p == "GetBookNow")
                        {
                            int id;
                            MessageBoxLMS msb = new MessageBoxLMS("Notification", "Do you really want to complete the order?", MessageType.Accept, MessageButtons.YesNo);
                            if (msb.ShowDialog() == true)
                            {
                                try
                                {
                                    using (var context = new LMSEntities1())
                                    {
                                        ORDER_BOOKS order = new ORDER_BOOKS();
                                        order.orderCusId = int.Parse(AccountID);
                                        order.orderName = OrderName;
                                        order.orderEmail = Email;
                                        order.orderPhone = PhoneNumber;
                                        order.orderAddress = Address;
                                        order.totalValue = TotalValueForOneBookID;
                                        order.orderDate = DateTime.Now;
                                        order.orderStatus = 1;
                                        context.ORDER_BOOKS.Add(order);
                                        context.SaveChanges();
                                        id = order.orderID;
                                    }
                                    using (var context = new LMSEntities1())
                                    {
                                        ORDER_DETAIL detail = new ORDER_DETAIL();
                                        detail.orderID = id;
                                        detail.bookID = Int32.Parse(SelectedItem.MaSach.ToString());
                                        detail.quantity = Quantity;
                                        context.ORDER_DETAIL.Add(detail);

                                        foreach (var item in context.BOOKs)
                                        {
                                            if (item.ID == Int32.Parse(SelectedItem.MaSach.ToString()))
                                            {
                                                item.SOLUONG -= Quantity;
                                                break;
                                            }
                                        }
                                        context.SaveChanges();
                                    }

                                    msb = new MessageBoxLMS("Notification", "Order Successful", MessageType.Accept, MessageButtons.OK);
                                    msb.ShowDialog();
                                    Application.Current.Windows.OfType<PurchasePage>().FirstOrDefault().Close();
                                    SelectedItem = null;
                                }
                                catch
                                {
                                    msb = new MessageBoxLMS("Error", "Checkout Failed!", MessageType.Accept, MessageButtons.OK);
                                    CanCheckout = false;
                                    msb.ShowDialog();
                                }
                            }
                        }
                        else
                        {
                            int id;
                            MessageBoxLMS msb = new MessageBoxLMS("Notification", "Do you really want to complete the order?", MessageType.Accept, MessageButtons.YesNo);
                            if (msb.ShowDialog() == true)
                            {
                                try
                                {
                                    using (var context = new LMSEntities1())
                                    {
                                        ORDER_BOOKS order = new ORDER_BOOKS();
                                        order.orderCusId = int.Parse(AccountID);
                                        order.orderName = OrderName;
                                        order.orderEmail = Email;
                                        order.orderPhone = PhoneNumber;
                                        order.orderAddress = Address;
                                        order.totalValue = TotalCartValue;
                                        order.orderDate = DateTime.Now;
                                        order.orderStatus = 1;
                                        context.ORDER_BOOKS.Add(order);
                                        context.SaveChanges();
                                        id = order.orderID;
                                    }
                                    using (var context = new LMSEntities1())
                                    {
                                        foreach (BookDTO item in BooksInCart)
                                        {
                                            ORDER_DETAIL detail = new ORDER_DETAIL();
                                            detail.orderID = id;
                                            detail.bookID = Int32.Parse(item.MaSach.ToString());
                                            detail.quantity = item.SoLuong;
                                            context.ORDER_DETAIL.Add(detail);

                                            foreach (var book in context.BOOKs)
                                            {
                                                if (book.ID == Int32.Parse(item.MaSach.ToString()))
                                                {
                                                    book.SOLUONG -= item.SoLuong;
                                                    break;
                                                }
                                            }
                                            context.SaveChanges();
                                        }
                                    }

                                    using (var context = new LMSEntities1())
                                    {
                                        foreach (var item in context.CARTs)
                                        {
                                            if (item.MAKH == int.Parse(AccountID))
                                                context.CARTs.Remove(item);
                                        }
                                        context.SaveChanges();

                                    }
                                    BooksInCart.Clear();
                                    TotalCartValueStr = "₫0";
                                    msb = new MessageBoxLMS("Notification", "Order Successful", MessageType.Error, MessageButtons.OK);
                                    msb.ShowDialog();
                                    Application.Current.Windows.OfType<PurchasePage>().FirstOrDefault().Close();
                                    CanCheckout = false;
                                }
                                catch
                                {
                                    msb = new MessageBoxLMS("Error", "Checkout Failed!", MessageType.Error, MessageButtons.OK);
                                    msb.ShowDialog();
                                }
                            }
                        }
                    }
                }
            });

            //Test
            ShowOrder_Test = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                using (var context = new LMSEntities1())
                {
                    foreach (var order in context.ORDER_BOOKS)
                    {
                        MessageBox.Show("Ma Don Hang: " + order.orderID.ToString());
                        MessageBox.Show("Ten: " + order.orderName);
                        MessageBox.Show("Tong gia tri: " + order.totalValue);

                        foreach (var detail in context.ORDER_DETAIL)
                        {
                            if (order.orderID == detail.orderID)
                            {
                                MessageBox.Show("Chi Tiet Ma Don Hang So " + detail.orderID.ToString());
                                MessageBox.Show("Ma Sach: " + detail.bookID.ToString());
                                MessageBox.Show("So luong: " + detail.quantity.ToString());
                            }
                        }
                    }
                }
            });

            DeleteAllOrder_Test = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                using (var context = new LMSEntities1())
                {
                    SqlConnection connection = new SqlConnection(context.Database.Connection.ConnectionString);
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    MessageBoxLMS msb = new MessageBoxLMS("Warning", "Delete this order?", MessageType.Waitting, MessageButtons.YesNo);
                    if (msb.ShowDialog() == true)
                    {
                        try
                        {
                            command.CommandText = "delete from ORDER_DETAIL";
                            context.SaveChanges();
                            if (command.ExecuteNonQuery() != 0)
                            {
                                msb = new MessageBoxLMS("Notification", "Deleting is successful", MessageType.Accept, MessageButtons.OK);
                                msb.ShowDialog();
                            }
                            command.CommandText = "delete from ORDER_BOOKS";
                            context.SaveChanges();
                            if (command.ExecuteNonQuery() != 0)
                            {
                                msb = new MessageBoxLMS("Notification", "Deleting is successful", MessageType.Accept, MessageButtons.OK);
                                msb.ShowDialog();
                            }
                            command.CommandText = "DBCC CHECKIDENT ('ORDER_BOOKS', RESEED, 0)";
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            msb = new MessageBoxLMS("Notification", "Cannot delete this order", MessageType.Error, MessageButtons.OK);
                            msb.ShowDialog();
                        }
                    }
                }
            });
        }

        public void LoadBookInCart(ListBox p)
        {
            BooksInCart.Clear();
            using (var context = new LMSEntities1())
            {
                foreach (var item in context.CARTs)
                {
                    if(item.MAKH == Int32.Parse(AccountID))
                    {
                        foreach (var book in context.BOOKs)
                        {
                            if (item.MASACH == book.ID)
                            {
                                BookDTO bookInCart = new BookDTO();
                                bookInCart.MaSach = book.ID;
                                bookInCart.TenSach = book.TENSACH;
                                bookInCart.TacGia = book.TACGIA;
                                bookInCart.NXB = book.NHAXUATBAN;
                                bookInCart.SoLuong = (int)item.SOLUONGHT;
                                bookInCart.Gia = (decimal)book.GIA * (int)item.SOLUONGHT;
                                bookInCart.ImageSource = book.IMAGESOURCE;
                                bookInCart.NamXB = book.NAMXUATBAN.ToString();
                                BooksInCart.Add(bookInCart);
                            }
                        }
                    }                 
                }
            }
            CountTotalCartValue();
            p.ItemsSource = BooksInCart;
            p.SelectedIndex = SelectedItemCartID;
        }

        public void CountTotalCartValue()
        {
            TotalCartValue = 0;
            foreach (BookDTO item in BooksInCart)
            {
                TotalCartValue += item.Gia;
            }
            TotalCartValueStr = Decimal.Round(TotalCartValue, 0).ToString("C0").Replace("$", "₫");
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
