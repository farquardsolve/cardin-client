using Cardin.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for UserControlRegistrationStep1.xaml
    /// </summary>
    public partial class UserControlRegistrationStep1 : UserControl
    {
        BrushConverter converter = new System.Windows.Media.BrushConverter();
        Brush brush;
        public UserControlRegistrationStep1()
        {
            InitializeComponent();
            InitiatePrimaryColor();
        }

        void InitiatePrimaryColor()
        {
            brush = (Brush)converter.ConvertFromString("#FF1671B6");
        }


        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                App.facilityRegistration.firstName = txtFirstName.Text;
                App.facilityRegistration.lastName = txtlastName.Text;
                App.facilityRegistration.phone = txtPhoneNo.Text;
                App.facilityRegistration.email = txtEmail.Text;
                App.facilityRegistration.facilityName = txtCompanyName.Text;
            }
            catch (Exception)
            {

                throw;
            }
            

            if (!string.IsNullOrWhiteSpace(txtFirstName.Text) && 
                !string.IsNullOrWhiteSpace(txtlastName.Text) && 
                !string.IsNullOrWhiteSpace(txtCompanyName.Text)&&
                !string.IsNullOrWhiteSpace(txtPhoneNo.Text))
            {
                App.globalValueIndicator.IIndicator = true;
                txtFirstName.BorderBrush = brush;
                txtlastName.BorderBrush = brush;
                txtCompanyName.BorderBrush = brush;
                txtPhoneNo.BorderBrush = brush;
                txtEmail.BorderBrush = brush;
            }
            else
            {
                App.globalValueIndicator.IIndicator = false;
                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    txtFirstName.BorderBrush = Brushes.Red;
                }
                else
                {
                    txtFirstName.BorderBrush = brush;
                }
                if (string.IsNullOrWhiteSpace(txtlastName.Text))
                {
                    txtlastName.BorderBrush = Brushes.Red;
                }
                else
                {
                    txtlastName.BorderBrush = brush;
                }
                if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
                {
                    txtCompanyName.BorderBrush = Brushes.Red;
                }
                else
                {
                    txtCompanyName.BorderBrush = brush;
                }
                if (string.IsNullOrWhiteSpace(txtPhoneNo.Text))
                {
                    txtPhoneNo.BorderBrush = Brushes.Red;
                }
                else
                {
                    txtPhoneNo.BorderBrush = brush;
                }
                if (!string.IsNullOrEmpty(txtEmail.Text) && !IsValidEmail(txtEmail.Text))
                {
                    txtEmail.BorderBrush = Brushes.Red;
                }
                else
                {

                    txtEmail.BorderBrush = brush;
                }
            }
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
