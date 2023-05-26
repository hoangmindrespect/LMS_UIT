using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.MessageBoxCus;
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

namespace LibraryManagementSystem.View.MainWindow.BorrowBook
{
    /// <summary>
    /// Interaction logic for Item2.xaml
    /// </summary>
    public partial class Item2 : UserControl
    {
        public Item2()
        {
            InitializeComponent();
        }
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(Item2));


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Item2));


        public string Ref
        {
            get { return (string)GetValue(RefProperty); }
            set { SetValue(RefProperty, value); }
        }

        public static readonly DependencyProperty RefProperty = DependencyProperty.Register("Ref", typeof(string), typeof(Item2));

        public string ID
        {
            get { return (string)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }

        public static readonly DependencyProperty IDProperty = DependencyProperty.Register("ID", typeof(string), typeof(Item2));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int _id = int.Parse(txb.Text);
            BookInBorrowDTO a = new BookInBorrowDTO();
            using(var context = new LMSEntities1())
            {
                a.TenSach = (from s in context.BOOKs where s.ID == _id select s.TENSACH).FirstOrDefault();
                a.MaSach = _id;
                if (!IsInList(_id))
                {
                    a.SoLuong = 1;
                }
            }
            if (!IsInList(_id))
            {
                LibraryManagementSystem.ViewModel.AdminVM.BorrowBookVM.BorrowBookViewModel.ListBookBorrow.Add(a);
            }
            else
            {
                PlusOneUnit(_id);
            }   
        }

        public bool IsInList(int id)
        {
            foreach(var item in LibraryManagementSystem.ViewModel.AdminVM.BorrowBookVM.BorrowBookViewModel.ListBookBorrow)
            {
                if(id == item.MaSach)
                {
                    return true;
                }    
            }
            return false;
        }

        public void PlusOneUnit(int id)
        {
            foreach(var item in LibraryManagementSystem.ViewModel.AdminVM.BorrowBookVM.BorrowBookViewModel.ListBookBorrow)
            {
                if (id == item.MaSach)
                {
                    if(item.SoLuong + 1 > getMaxCount(id))
                    {
                        MessageBoxLMS msb = new MessageBoxLMS("Notice", "Exceed the max count", MessageType.Accept, MessageButtons.OK);
                        msb.ShowDialog();
                    }
                    else
                        item.SoLuong += 1;
                }   
            }    
        }

        int getMaxCount(int id)
        {
            using (var context = new LMSEntities1())
            {
                return (int)(from s in context.BOOKs where s.ID == id select s.SOLUONG).FirstOrDefault();
            }
        }
    }

}
