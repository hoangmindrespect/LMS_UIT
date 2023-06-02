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
using LibraryManagementSystem.ViewModel.ClientVM.BuyBookVM;

namespace LibraryManagementSystem.View.MainClientWindow.BuyBookPage
{
    /// <summary>
    /// Interaction logic for Item.xaml
    /// </summary>
    public partial class Item : UserControl
    {
        public Item()
        {
            InitializeComponent();
        }

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(Item));


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Item));


        public string Ref
        {
            get { return (string)GetValue(RefProperty); }
            set { SetValue(RefProperty, value); }
        }

        public static readonly DependencyProperty RefProperty = DependencyProperty.Register("Ref", typeof(string), typeof(Item));


        public string Color
        {
            get { return (string)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(string), typeof(Item));


        public string Count
        {
            get { return (string)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(string), typeof(Item));


        public string Price
        {
            get { return (string)GetValue(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }

        public static readonly DependencyProperty PriceProperty = DependencyProperty.Register("Price", typeof(string), typeof(Item));

        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(Item), new PropertyMetadata(null));

        public object DeleteCommandParameter
        {
            get { return (object)GetValue(DeleteCommandParameterProperty); }
            set { SetValue(DeleteCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty DeleteCommandParameterProperty = DependencyProperty.Register("DeleteCommandParameter", typeof(object), typeof(Item));

        public ICommand PlusCommand
        {
            get { return (ICommand)GetValue(PlusCommandProperty); }
            set { SetValue(PlusCommandProperty, value); }
        }

        public static readonly DependencyProperty PlusCommandProperty = DependencyProperty.Register("PlusCommand", typeof(ICommand), typeof(Item), new PropertyMetadata(null));
        public object PlusCommandParameter
        {
            get { return (object)GetValue(PlusCommandParameterProperty); }
            set { SetValue(PlusCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty PlusCommandParameterProperty = DependencyProperty.Register("PlusCommandParameter", typeof(object), typeof(Item));

        public ICommand MinusCommand
        {
            get { return (ICommand)GetValue(MinusCommandProperty); }
            set { SetValue(MinusCommandProperty, value); }
        }

        public static readonly DependencyProperty MinusCommandProperty = DependencyProperty.Register("MinusCommand", typeof(ICommand), typeof(Item), new PropertyMetadata(null));
        public object MinusCommandParameter
        {
            get { return (object)GetValue(MinusCommandParameterProperty); }
            set { SetValue(MinusCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty MinusCommandParameterProperty = DependencyProperty.Register("MinusCommandParameter", typeof(object), typeof(Item));

    }
}
