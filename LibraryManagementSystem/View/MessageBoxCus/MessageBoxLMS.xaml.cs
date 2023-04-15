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
using System.Windows.Shapes;

namespace LibraryManagementSystem.View.MessageBoxCus
{
    /// <summary>
    /// Interaction logic for MessageBoxLMS.xaml
    /// </summary>
    public partial class MessageBoxLMS : Window
    {
        public MessageBoxLMS(string Title, string Message, MessageType Type, MessageButtons Buttons)
        {
            InitializeComponent();
            txtMessage.Text = Message;
            txtTitle.Text = Title;

            switch (Type)
            {

                case MessageType.Accept:
                    System.Media.SystemSounds.Beep.Play();
                    BackgroundBorder.ImageSource = new BitmapImage(new Uri("pack://application:,,,/LibraryManagementSystem;component/Resource/AcceptBackground.png"));
                    break;
                case MessageType.Waitting:
                    System.Media.SystemSounds.Beep.Play();
                    BackgroundBorder.ImageSource = new BitmapImage(new Uri("pack://application:,,,/LibraryManagementSystem;component/Resource/WaittingBackground.png"));
                    break;
                case MessageType.Error:
                    System.Media.SystemSounds.Beep.Play();
                    BackgroundBorder.ImageSource = new BitmapImage(new Uri("pack://application:,,,/LibraryManagementSystem;component/Resource/ErrorBackground.png"));
                    break;
            }
            switch (Buttons)
            {
                case MessageButtons.YesNo:
                    btnOk.Visibility = Visibility.Collapsed; btnCancel.Visibility = Visibility.Collapsed;
                    break;
                case MessageButtons.OK:
                    btnOk.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        public void ChangeBackGround(Color newcolor)
        {
            BackGroundTittle.Background = new SolidColorBrush(newcolor);
            btnOk.Background = new SolidColorBrush(newcolor);
            btnYes.Background = new SolidColorBrush(newcolor);
            btnClose.Foreground = new SolidColorBrush(newcolor);
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnClose_MouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;

            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFA5B9D6");
            btn.Background = new SolidColorBrush(Colors.OrangeRed);
        }

        private void btnClose_MouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Background = new SolidColorBrush(Colors.Transparent);
        }
    }
    public enum MessageButtons
    {
        YesNo,
        OK
    }
    public enum MessageType
    {
        Accept,
        Waitting,
        Error
    }

}
