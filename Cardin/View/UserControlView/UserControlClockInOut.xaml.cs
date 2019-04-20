using Cardin.Helper;
using Cardin.Model;
using Cardin.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Threading;

namespace Cardin.View.UserControlView
{
    /// <summary>
    /// Interaction logic for UserControlClockInOut.xaml
    /// </summary>
    public partial class UserControlClockInOut : UserControl
    {
        HttpClientServices httpClientServices = new HttpClientServices();
        IsolatedLocalStorage isolatedLocalStorage = new IsolatedLocalStorage();
        DateTime dt = DateTime.Now;
        Person selectedPerson;
        public UserControlClockInOut()
        {
            InitializeComponent();
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
           
            txtTime.Text = dt.ToLongTimeString() + dt.ToString("tt", CultureInfo.InvariantCulture);
        }

        public async void FindWorkers()
        {
            try
            {
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);

                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);

                string url = EndPoints.fiiterWorker + "?facilityId=" + deserializedFacility.Id+"&name="+txtSearchWorkers.Text;
                string strFilterWorker = await httpClientServices.GetAsync(url, deserializedAuth.AccessToken);
                if (strFilterWorker != "")
                {
                    var deserializedFilterWorker = JsonConvert.DeserializeObject<PersonStructure>(strFilterWorker);
                    if(deserializedFilterWorker.Data.Count() != 1 && selectedPerson.Id != deserializedFilterWorker.Data[0].Id)
                    {
                        lvFilteredWorkers.Visibility = Visibility.Visible;
                        lvFilteredWorkers.ItemsSource = deserializedFilterWorker.Data;
                    }
                    else if (selectedPerson == null)
                    {
                        lvFilteredWorkers.Visibility = Visibility.Visible;
                        lvFilteredWorkers.ItemsSource = deserializedFilterWorker.Data;
                    }
                }
                else
                {
                    Login login = new Login();
                    login.Show();
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void txtSearchWorkers_TextChanged(object sender, TextChangedEventArgs e)
        {
            FindWorkers();
        }

        private void lvFilteredWorkers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPerson = (Person)lvFilteredWorkers.SelectedItem;
            txtSearchWorkers.Text = selectedPerson.fullName;
            profileImg.Source = new BitmapImage(new Uri(selectedPerson.profileImagePath));
            if (selectedPerson.clocked ==null)
            {
                btnClockInOut.Content = "Clock-In";
                txtCommentLabel.Text = "Clock-In Comment";
            }
            else
            {
                if(selectedPerson.clocked.clockOut == null)
                {
                    btnClockInOut.Content = "Clock-Out";
                    txtCommentLabel.Text = "Clock-Out Comment";
                }
                else
                {
                    MessageBox.Show("You have Clock-Out for the day", "CardIn");
                    txtClockComment.Text = string.Empty;
                    profileImg.Source = new BitmapImage(new Uri("/Cardin;component/Assets/Profile-icon-9.png", UriKind.RelativeOrAbsolute));
                    selectedPerson = null;
                }
                
            }
            lvFilteredWorkers.Visibility = Visibility.Collapsed;
        }

        private async void btnClockInOut_Click(object sender, RoutedEventArgs e)
        {
            string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
            var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);

            string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
            var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);
            if (btnClockInOut.Content.ToString() == "Clock-In")
            {
                CardInOut cardInOut = new CardInOut();
                cardInOut.personId = selectedPerson.Id;
                cardInOut.facilityId = deserializedFacility.Id;
                Clocked clocked = new Clocked();
                clocked.comment = txtClockComment.Text;
                cardInOut.clockIn = clocked;
                string strClockActivities = await httpClientServices.CreateAsync(EndPoints.clockActivities, cardInOut, deserializedAuth.AccessToken);
                if (strClockActivities != "")
                {
                    txtClockComment.Text = string.Empty;
                    profileImg.Source = new BitmapImage(new Uri("/Cardin;component/Assets/Profile-icon-9.png", UriKind.RelativeOrAbsolute));
                    selectedPerson = null;
                    MessageBox.Show("You successfullly clock-in","CardIn");
                }
            }
            else
            {
                CardInOut cardInOut = selectedPerson.clocked;
                Clocked clocked = new Clocked();
                clocked.comment = txtClockComment.Text;
                cardInOut.clockOut = clocked;
                string strClockActivities = await httpClientServices.UpdateAsync(EndPoints.clockActivities, cardInOut.id,cardInOut, deserializedAuth.AccessToken);
                if (strClockActivities != "")
                {
                    txtClockComment.Text = string.Empty;
                    profileImg.Source = new BitmapImage(new Uri("/Cardin;component/Assets/Profile-icon-9.png", UriKind.RelativeOrAbsolute));
                    selectedPerson = null;
                    MessageBox.Show("You successfullly clock-out", "CardIn");
                }
            }
        }
    }
}
