using Cardin.View.UserControlView;
using Cardin.Helper;
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
using System.ComponentModel;
using Cardin.Service;
using Newtonsoft.Json;
using Cardin.Model;
using Cardin.View;

namespace Cardin
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        int menuIndex = 1;
        IGlobalValueIndicator globalValueIndicator = new IGlobalValueIndicator();
        public Registration()
        {
            InitializeComponent();
            initializeRegMenuItem();
            App.globalValueIndicator.PropertyChanged += PropertyChangedIIndicator;
        }

        private async void buttonNextButtonLabel_Click(object sender, RoutedEventArgs e)
        {
            
            switch (menuIndex)
            {
                case 1:
                    btnNext.IsEnabled = false;
                    App.globalValueIndicator.IIndicator = false;
                    indicatorStep1.Visibility = Visibility.Visible;
                    gridregistrationMainLayout.Children.Clear();
                    gridregistrationMainLayout.Children.Add(new UserControlRegistrationStep1());
                    menuIndex = 2;
                    break;
                case 2:
                    btnNext.IsEnabled = false;
                    App.globalValueIndicator.IIndicator = false;
                    tbkWelcomeLabel.Visibility = Visibility.Hidden;
                    indicatorStep1.Visibility = Visibility.Collapsed;
                    indicatorStep2.Visibility = Visibility.Visible;
                    chkStep1.Visibility = Visibility.Visible;
                    gridregistrationMainLayout.Children.Clear();
                    gridregistrationMainLayout.Children.Add(new UserControlRegistrationStep2());
                    menuIndex = 3;
                    break;
                case 3:
                    indicatorStep2.Visibility = Visibility.Collapsed;
                    chkStep2.Visibility = Visibility.Visible;
                    indicatorStep3.Visibility = Visibility.Visible;
                    gridregistrationMainLayout.Children.Clear();
                    gridregistrationMainLayout.Children.Add(new UserControlRegistrationFinish());
                    menuIndex = 4;
                    tbkNextButtonLabel.Text = "Finish";
                    break;
                case 4:
                    indicatorStep3.Visibility = Visibility.Collapsed;
                    chkStep3.Visibility = Visibility.Visible;
                    btnNext.Visibility = Visibility.Collapsed;
                    tbkNextButtonLabel.Visibility = Visibility.Collapsed;
                    gridregistrationMainLayout.Children.Clear();
                    gridregistrationMainLayout.Children.Add(new UserControlSettingUpCardIn());
                    HttpClientServices httpClientServices = new HttpClientServices();
                    try
                    {
                        string response = await httpClientServices.CreateAsync(EndPoints.registration, App.facilityRegistration,"");
                        var payload = JsonConvert.DeserializeObject<RegistrationItem>(response);
                        if (payload.Status == "success")
                        {
                            this.Hide();
                            string serializedFacility = JsonConvert.SerializeObject(payload.Data.Facility);
                            string serializedAuth = JsonConvert.SerializeObject(payload.Data.Auth);
                            IsolatedLocalStorage isolatedLocalStorage = new IsolatedLocalStorage();
                            isolatedLocalStorage.Write(IsolatedFiles.facilityDetails, serializedFacility);
                            isolatedLocalStorage.Write(IsolatedFiles.isFacilityDetailsCompleted, "true");
                            isolatedLocalStorage.Write(IsolatedFiles.authFile, serializedAuth);
                            MasterLandingWindow mainWindow = new MasterLandingWindow();
                            mainWindow.Show();
                        }
                        
                    }
                    catch (Exception ex){
                        string t = ex.Message;
                    }
                    break;
                default:
                    break;
            }
        }

        private void initializeRegMenuItem()
        {
            
            IsolatedLocalStorage isolatedLocalStorage = new IsolatedLocalStorage();
            string status = isolatedLocalStorage.Read(IsolatedFiles.isFacilityDetailsCompleted);
            status = status.Replace(System.Environment.NewLine, "");
            if (status != "true")
            {
                indicatorStep1.Visibility = Visibility.Visible;
                gridregistrationMainLayout.Children.Clear();
                gridregistrationMainLayout.Children.Add(new UserControlRegistrationStep1());
                menuIndex = 2;
            }
            else
            {
                this.Hide();
                MasterLandingWindow masterLandingWindow = new MasterLandingWindow();
                masterLandingWindow.Show();

            }
        }

        void PropertyChangedIIndicator(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "IIndicator":
                TriggerButtonSate(App.globalValueIndicator.IIndicator);
                break;
            }
        }

        void TriggerButtonSate(bool _IIndicator)
        {
            if(_IIndicator == true)
            {
                btnNext.IsEnabled = true;
            }
            else
            {
                btnNext.IsEnabled = false;
            }
        }
    }
}
