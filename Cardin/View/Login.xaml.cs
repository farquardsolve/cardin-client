using Cardin.Helper;
using Cardin.Model;
using Cardin.Service;
using Cardin.View;
using Newtonsoft.Json;
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

namespace Cardin
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void AuthUser()
        {
            btnLogin.IsEnabled = false;
            pBarLogin.Visibility = Visibility.Visible;
            LoginAuthentication authObject = new LoginAuthentication();
            authObject.phone = txtPhone.Text;
            authObject.password = pbxPassword.Password;
            authObject.strategy = "local";
            HttpClientServices httpClientServices = new HttpClientServices();
            try
            {
                string response = await httpClientServices.CreateAsync(EndPoints.authentication, authObject, "");
                var payload = JsonConvert.DeserializeObject<Auth>(response);
                if (response != "")
                {
                    string serializedAuth = JsonConvert.SerializeObject(payload);
                    IsolatedLocalStorage isolatedLocalStorage = new IsolatedLocalStorage();
                    isolatedLocalStorage.Write(IsolatedFiles.authFile, serializedAuth);
                    pBarLogin.Visibility = Visibility.Collapsed;
                    this.Hide();

                }
                else
                {
                    pBarLogin.Visibility = Visibility.Collapsed;
                    btnLogin.IsEnabled = true;
                }


            }
            catch (Exception ex)
            {
                string t = ex.Message;
                pBarLogin.Visibility = Visibility.Collapsed;
                btnLogin.IsEnabled = false;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            AuthUser();
        }
    }
}
