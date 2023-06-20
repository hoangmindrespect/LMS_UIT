using CloudinaryDotNet.Actions;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.Login;
using LibraryManagementSystem.ViewModel.LoginVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace LibraryManagementSystem.ViewModel.ClientVM.ManageOrderVM
{
    public class ManageOrdersViewModel : BaseViewModel
    {
        public ObservableCollection<OrderDTO> Orders = new ObservableCollection<OrderDTO>();
        public ObservableCollection<BookDTO> ListDetails = new ObservableCollection<BookDTO>();
        public ICommand Loaded { get; set; }
        public ICommand LoadedDetails { get; set; }
        public ManageOrdersViewModel()
        {
            Loaded = new RelayCommand<ItemsControl>((p) => { return true; }, (p) =>
            {
                Orders = new ObservableCollection<OrderDTO>();
                using (var context = new LMSEntities1())
                {
                    foreach (var item in context.ORDER_BOOKS)
                    {
                        if (item.orderCusId == (from s in context.ACCOUNTs where s.USERNAME == LoginRegisViewModel.username select s.ID).FirstOrDefault())
                        {
                            OrderDTO order = new OrderDTO();
                            order.Name = item.orderName;
                            order.Id = item.orderID;
                            order.Address = item.orderAddress;
                            order.PhoneNumber = item.orderPhone;
                            order.Email = item.orderEmail;
                            order.OrderDate = item.orderDate.ToLongDateString();
                            order.CusId = (int)item.orderCusId;
                            order.OrderStatus = (int)item.orderStatus;
                            order.OrderStatusDisplay = (from s in context.STATUS_ORDER where s.statusId == (int)item.orderStatus select s.orderStatus).FirstOrDefault();
                            order.OrderStatusColor = (from s in context.STATUS_ORDER where s.statusId == (int)item.orderStatus select s.COLOR).FirstOrDefault();
                            order.Details = new ObservableCollection<BookDTO>();
                            // thêm chi tiết sách
                            foreach (var item2 in context.ORDER_DETAIL)
                            {
                                if(item2.orderID == item.orderID)
                                {
                                    BookDTO book = new BookDTO();
                                    book.SoLuong = (int)item2.quantity;
                                    book.TenSach = (from s in context.BOOKs where s.ID == item2.bookID select s.TENSACH).FirstOrDefault();
                                    order.Details.Add(book);
                                }    
                            }    

                            Orders.Add(order);
                        }
                    }
                }
                p.ItemsSource = Orders;
            });
        }
    }
}
