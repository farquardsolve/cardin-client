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

namespace Cardin.View.UserControlView
{
    /// <summary>
    /// Interaction logic for UserControlRegistrationStep2.xaml
    /// </summary>
    public partial class UserControlRegistrationStep2 : UserControl
    {

        BrushConverter converter = new System.Windows.Media.BrushConverter();
        Brush brush;

        public UserControlRegistrationStep2()
        {
            InitializeComponent();
            InitiatePrimaryColor();
            txtPhoneNo.Text = App.facilityRegistration.phone;
        }

        void InitiatePrimaryColor()
        {
            brush = (Brush)converter.ConvertFromString("#FF1671B6");
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                App.facilityRegistration.password = pwd1.Password;
            }
            catch (Exception)
            {

                throw;
            }

            if (!string.IsNullOrWhiteSpace(pwd1.Password) &&
                !string.IsNullOrWhiteSpace(pwd2.Password) &&
                pwd1.Password == pwd2.Password)
            {
                App.globalValueIndicator.IIndicator = true;
                pwd1.BorderBrush = brush;
                pwd2.BorderBrush = brush;
            }
            else
            {
                App.globalValueIndicator.IIndicator = false;
                pwd1.BorderBrush = Brushes.Red;
                pwd2.BorderBrush = Brushes.Red;
            }
        }

    }
}
