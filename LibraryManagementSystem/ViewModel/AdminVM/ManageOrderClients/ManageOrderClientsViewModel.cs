using DocumentFormat.OpenXml.EMMA;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
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

namespace LibraryManagementSystem.ViewModel.AdminVM.ManageOrderClients
{
    public class ManageOrderClientsViewModel:BaseViewModel
    {
        public ObservableCollection<OrderDTO> Orders = new ObservableCollection<OrderDTO>();
        public ObservableCollection<BookDTO> ListDetails = new ObservableCollection<BookDTO>();
        ListView lv;
        public ICommand Loaded { get; set; }
        public ICommand LoadedDetails { get; set; }
        public ICommand NextStep { get; set; }
        public ICommand PreviousStep { get; set; }
        public ManageOrderClientsViewModel()
        {
           
            Loaded = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                Orders = new ObservableCollection<OrderDTO>();
                using (var context = new LMSEntities1())
                {
                    foreach (var item in context.ORDER_BOOKS)
                    {
                        OrderDTO order = new OrderDTO();
                        order.Name = item.orderName;
                        order.Id = item.orderID;
                        order.Address = item.orderAddress;
                        order.PhoneNumber = item.orderPhone;
                        order.Email = item.orderEmail;
                        order.OrderDate = item.orderDate.ToLongDateString();
                        order.CusId = (int)item.orderCusId;
                        order.OrderStatus =  (int)item.orderStatus;
                        order.OrderStatusDisplay = (from s in context.STATUS_ORDER where order.OrderStatus == s.statusId select s.orderStatus).FirstOrDefault();
                        order.OrderStatusColor = (from s in context.STATUS_ORDER where order.OrderStatus == s.statusId select s.COLOR).FirstOrDefault();
                        order.Details = new ObservableCollection<BookDTO>();
                        // thêm chi tiết sách
                        foreach (var item2 in context.ORDER_DETAIL)
                        {
                            if (item2.orderID == item.orderID)
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
                p.ItemsSource = Orders;
                lv = (ListView)p;
            });

            NextStep = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                OrderDTO order = lv.SelectedItem as OrderDTO;
                using (var context = new LMSEntities1())
                {
                    foreach (var item in context.ORDER_BOOKS)
                    {
                        if (item.orderID == order.Id)
                        {
                            if (item.orderStatus == 4)
                                return;
                            else
                                item.orderStatus += 1;
                            break;
                        }
                    }
                    context.SaveChanges();
                }
                Loaded.Execute(lv);
            });

            PreviousStep = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                OrderDTO order = lv.SelectedItem as OrderDTO;
                using (var context = new LMSEntities1())
                {
                    foreach (var item in context.ORDER_BOOKS)
                    {
                        if (item.orderID == order.Id)
                        {
                            if (item.orderStatus == 1)
                                return;
                            else
                                item.orderStatus -= 1;
                            break;
                        }
                    }
                    context.SaveChanges();
                }
                Loaded.Execute(lv);
            });
        }
    }
}
