using Cardin.Helper;
using Cardin.Model;
using Cardin.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for UserControlSetting.xaml
    /// </summary>
    public partial class UserControlSetting : UserControl
    {
        HttpClientServices httpClientServices = new HttpClientServices();
        IsolatedLocalStorage isolatedLocalStorage = new IsolatedLocalStorage();
        BrushConverter converter = new BrushConverter();
        ItemClass selectedPostItem, selectedEngagementTypeItem;
        CarLot selectedCarLot;
        ObservableCollection<CarLot> CbxCarLotItems { get; set; }
        public UserControlSetting()
        {
            InitializeComponent();
        }

        private void settingItems_click(object sender, RoutedEventArgs e)
        {
            string name = ((Button)sender).Name;
            switch (name)
            {
                case "btnPost":
                    if (gridPost.Visibility == Visibility.Collapsed)
                    {
                        gridPost.Visibility = Visibility.Visible;
                        matPost.Foreground = new SolidColorBrush(Colors.Gray);
                        GetWorkerPosts();
                    }
                    else
                    {
                        gridPost.Visibility = Visibility.Collapsed;
                        matPost.Foreground = (Brush)converter.ConvertFromString("#FF1671B6");
                    }
                    break;
                case "btnEngagementType":
                    if ( gridEngagementType.Visibility == Visibility.Collapsed)
                    {
                        gridEngagementType.Visibility = Visibility.Visible;
                        matEngagementType.Foreground = new SolidColorBrush(Colors.Gray);
                        GetEngagementType();
                    }
                    else
                    {
                        gridEngagementType.Visibility = Visibility.Collapsed;
                        matEngagementType.Foreground = (Brush)converter.ConvertFromString("#FF1671B6");
                    }
                    break;
                case "btnCarPark":
                    if (gridCarPark.Visibility == Visibility.Collapsed)
                    {
                        gridCarPark.Visibility = Visibility.Visible;
                        matCarPark.Foreground = new SolidColorBrush(Colors.Gray);
                        GetCarPark();
                    }
                    else
                    {
                        gridCarPark.Visibility = Visibility.Collapsed;
                        matCarPark.Foreground = (Brush)converter.ConvertFromString("#FF1671B6");
                    }
                    break;
                default:
                    break;
            }
        }

        private void GetCarPark()
        {
            GetCarLot();
        }

        private void btnNewPost_Click(object sender, RoutedEventArgs e)
        {
            if (gridNewPost.Visibility == Visibility.Visible)
            {
                gridNewPost.Visibility = Visibility.Collapsed;
                selectedPostItem = null;
                btnSaveNewPost.Content = "Add";
            }
            else
            {
                gridNewPost.Visibility = Visibility.Visible;
            }
        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                btnSaveNewPost.IsEnabled = false;
            }
            else
            {
                btnSaveNewPost.IsEnabled = true;
            }
        }

        public async void savePost()
        {
            try
            {
                btnSaveNewPost.IsEnabled = false;
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);
                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);
                ItemClass workerPostItem = new ItemClass();
                workerPostItem.name = txtName.Text.ToUpper();
                workerPostItem.facilityId = deserializedFacility.Id;
                string strPersons = await httpClientServices.CreateAsync(EndPoints.workerPosts, workerPostItem, deserializedAuth.AccessToken);
                var deserializedworkerPostItem = JsonConvert.DeserializeObject<ItemClass>(strPersons);
                GetWorkerPosts();
            }
            catch(Exception ex)
            {

            }
        }

        public async void updatePost()
        {
            try
            {
                btnSaveNewPost.IsEnabled = false;
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);
                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);
                ItemClass workerPostItem = new ItemClass();
                workerPostItem.name = txtName.Text.ToUpper();
                workerPostItem.facilityId = deserializedFacility.Id;
                string _strPost = await httpClientServices.PatchAsync(EndPoints.workerPosts+"/"+selectedPostItem.id, deserializedAuth.AccessToken, workerPostItem,true);
                var deserializedworkerPostItem = JsonConvert.DeserializeObject<ItemClass>(_strPost);
                selectedPostItem = null;
                btnSaveNewPost.Content = "Add";
                GetWorkerPosts();
            }
            catch (Exception ex)
            {
                selectedPostItem = null;
                btnSaveNewPost.Content = "Add";
                btnSaveNewPost.IsEnabled = true;
            }
        }

        public async void deletePost()
        {
            try
            {
                btnSaveNewPostDelete.IsEnabled = false;
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);
                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);
                ItemClass workerPostItem = new ItemClass();
                workerPostItem.name = txtName.Text.ToUpper();
                workerPostItem.facilityId = deserializedFacility.Id;
                string _strPost = await httpClientServices.DeleteAsync(EndPoints.workerPosts, selectedPostItem.id, deserializedAuth.AccessToken);
                var deserializedworkerPostItem = JsonConvert.DeserializeObject<ItemClass>(_strPost);
                selectedPostItem = null;
                btnSaveNewPost.Content = "Add";
                GetWorkerPosts();
                
            }
            catch (Exception ex)
            {
                selectedPostItem = null;
                btnSaveNewPost.Content = "Add";
                btnSaveNewPostDelete.IsEnabled = true;
            }
        }

        public async void GetWorkerPosts()
        {
            try
            {
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);

                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);

                string url = EndPoints.workerPosts + "?facilityId=" + deserializedFacility.Id;
                string strWorkerPosts = await httpClientServices.GetAsync(url, deserializedAuth.AccessToken);
                if (strWorkerPosts != "")
                {
                    var deserializedWorkerPosts = JsonConvert.DeserializeObject<ItemStructure>(strWorkerPosts);
                    dataGridPost.ItemsSource = deserializedWorkerPosts.Data;
                    txtName.Text = string.Empty;
                }
                else
                {
                    Login login = new Login();
                    login.Show();
                }
                btnSaveNewPost.IsEnabled = true;
                btnSaveNewPostDelete.IsEnabled = true;
            }
            catch(Exception ex)
            {

            }
            
        }


        public async void saveEngagementType()
        {
            try
            {
                btnSaveNewEngagementType.IsEnabled = false;
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);
                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);
                ItemClass engagementTypeItem = new ItemClass();
                engagementTypeItem.name = txtNameEngagementType.Text.ToUpper();
                engagementTypeItem.facilityId = deserializedFacility.Id;
                string strEngagementTypeItem = await httpClientServices.CreateAsync(EndPoints.engagementTypes, engagementTypeItem, deserializedAuth.AccessToken);
                var deserializedEngagementTypeItem = JsonConvert.DeserializeObject<ItemClass>(strEngagementTypeItem);
                GetEngagementType();
            }
            catch (Exception ex)
            {

            }
        }

        public async void updateEngagementType()
        {
            try
            {
                btnSaveNewEngagementType.IsEnabled = false;
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);
                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);
                ItemClass engagementTypeItem = new ItemClass();
                engagementTypeItem.name = txtNameEngagementType.Text.ToUpper();
                engagementTypeItem.facilityId = deserializedFacility.Id;
                string _strEngagementTypeItem = await httpClientServices.PatchAsync(EndPoints.engagementTypes + "/" + selectedEngagementTypeItem.id, deserializedAuth.AccessToken, engagementTypeItem, true);
                var deserializedEngagementTypeItem = JsonConvert.DeserializeObject<ItemClass>(_strEngagementTypeItem);
                selectedEngagementTypeItem = null;
                btnSaveNewEngagementType.Content = "Add";
                GetEngagementType();
            }
            catch (Exception ex)
            {
                selectedEngagementTypeItem = null;
                btnSaveNewEngagementType.Content = "Add";
                btnSaveNewEngagementType.IsEnabled = true;
            }
        }





        private void btnSaveNewPost_Click(object sender, RoutedEventArgs e)
        {
            if (btnSaveNewPost.Content.ToString() == "Add")
            {
                savePost();
            }
            else
            {
                updatePost();
            }
            
        }

        private void dataGridPost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPostItem = (ItemClass)dataGridPost.SelectedItem;
            if (selectedPostItem != null)
            {
                txtName.Text = selectedPostItem.name;
                btnSaveNewPost.Content = "Update";
                gridNewPost.Visibility = Visibility.Visible;
            }
        }

        private void btnSaveNewPostDelete_Click(object sender, RoutedEventArgs e)
        {
            deletePost();
        }

        private void dataGridEngagementType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEngagementTypeItem = (ItemClass)dataGridEngagementType.SelectedItem;
            if (selectedEngagementTypeItem != null)
            {
                txtNameEngagementType.Text = selectedEngagementTypeItem.name;
                btnSaveNewEngagementType.Content = "Update";
                gridNewEngagementType.Visibility = Visibility.Visible;
            }
        }

        public async void GetEngagementType()
        {
            try
            {
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);

                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);

                string url = EndPoints.engagementTypes + "?facilityId=" + deserializedFacility.Id;
                string strEngagementType = await httpClientServices.GetAsync(url, deserializedAuth.AccessToken);
                if (strEngagementType != "")
                {
                    var deserializedEngagementType = JsonConvert.DeserializeObject<ItemStructure>(strEngagementType);
                    dataGridEngagementType.ItemsSource = deserializedEngagementType.Data;
                    txtNameEngagementType.Text = string.Empty;
                }
                else
                {
                    Login login = new Login();
                    login.Show();
                }
                btnSaveNewEngagementType.IsEnabled = true;
                btnSaveNewEngagementTypeDelete.IsEnabled = true;
            }
            catch (Exception ex)
            {

            }

        }

        public async void deleteEngagementType()
        {
            try
            {
                btnSaveNewEngagementTypeDelete.IsEnabled = false;
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);
                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);
                string _strPost = await httpClientServices.DeleteAsync(EndPoints.engagementTypes, selectedEngagementTypeItem.id, deserializedAuth.AccessToken);
                var deserializedengagementTypeItem = JsonConvert.DeserializeObject<ItemClass>(_strPost);
                selectedEngagementTypeItem = null;
                btnSaveNewEngagementType.Content = "Add";
                GetEngagementType();

            }
            catch (Exception ex)
            {
                selectedPostItem = null;
                btnSaveNewPost.Content = "Add";
                btnSaveNewPostDelete.IsEnabled = true;
            }
        }


        private void btnSaveNewEngagementType_Click(object sender, RoutedEventArgs e)
        {
            if (btnSaveNewEngagementType.Content.ToString() == "Add")
            {
                saveEngagementType();
            }
            else
            {
                updateEngagementType();
            }
        }

        private void btnSaveNewEngagementTypeDelete_Click(object sender, RoutedEventArgs e)
        {
            deleteEngagementType();
        }

        private void btnNewCarPark_Click(object sender, RoutedEventArgs e)
        {
            if (gridNewCarPark.Visibility == Visibility.Visible)
            {
                gridNewCarPark.Visibility = Visibility.Collapsed;
                selectedPostItem = null;
                btnSaveNewCarPark.Content = "Add";
            }
            else
            {
                gridNewCarPark.Visibility = Visibility.Visible;
            }
        }

        private void btnSaveNewCarPark_Click(object sender, RoutedEventArgs e)
        {
            if (btnSaveNewCarPark.Content.ToString() == "Add")
            {
                saveCarPark();
            }
            else
            {
                updateCarPark();
            }
        }

        private async void updateCarPark()
        {
            try
            {
                btnSaveNewCarPark.IsEnabled = false;
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);
                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);
                selectedCarLot.name = txtNameCarPark.Text.ToUpper();
                string _strCarLotItem = await httpClientServices.PatchAsync(EndPoints.carLots + "/" + selectedCarLot.id, deserializedAuth.AccessToken, selectedCarLot, true);
                var deserializedCarLotItem = JsonConvert.DeserializeObject<dynamic>(_strCarLotItem);
                selectedCarLot = null;
                btnSaveNewCarPark.Content = "Add";
                GetCarPark();
            }
            catch (Exception ex)
            {
                selectedCarLot = null;
                btnSaveNewCarPark.Content = "Add";
                btnSaveNewCarPark.IsEnabled = true;
            }
            txtNameCarPark.Text = string.Empty;
            txtNameCarParkSpace.Text = string.Empty;

        }

        private async void saveCarPark()
        {
            try
            {
                btnSaveNewCarPark.IsEnabled = false;
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);
                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);
                CarLot carLot = new CarLot();
                carLot.name = txtNameCarPark.Text.ToUpper();
                carLot.facilityId = deserializedFacility.Id;
                string strCarParkItem = await httpClientServices.CreateAsync(EndPoints.carLots, carLot, deserializedAuth.AccessToken);
                var deserializedCarParkItem = JsonConvert.DeserializeObject<CarLot>(strCarParkItem);
                GetCarLot();
            }
            catch (Exception ex)
            {

            }
        }

        private async void GetCarLot()
        {
            try
            {
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);

                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);

                string url = EndPoints.carLots + "?facilityId=" + deserializedFacility.Id;
                string strCarLots = await httpClientServices.GetAsync(url, deserializedAuth.AccessToken);

                if (strCarLots != "")
                {
                    var deserializedCarLot = JsonConvert.DeserializeObject<CarLotStructure>(strCarLots);
                    dataGridCarPark.ItemsSource = deserializedCarLot.Data;
                    CbxCarLotItems = deserializedCarLot.Data;
                    cbxCarLot.ItemsSource = CbxCarLotItems;
                    txtNameCarPark.Text = string.Empty;
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
            cbxCarLot.SelectedIndex = -1;
            btnSaveNewCarPark.IsEnabled = true;
            btnSaveNewCarParkDelete.IsEnabled = true;
        }


        public async void deleteCarLot()
        {
            try
            {
                btnSaveNewCarParkDelete.IsEnabled = false;
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);
                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);
                string _strPost = await httpClientServices.DeleteAsync(EndPoints.carLots, selectedCarLot.id, deserializedAuth.AccessToken);
                var deserializedcarParkItem = JsonConvert.DeserializeObject<CarLot>(_strPost);
                selectedCarLot = null;
                btnSaveNewCarPark.Content = "Add";
                GetCarPark();

            }
            catch (Exception ex)
            {
                selectedCarLot = null;
                btnSaveNewCarPark.Content = "Add";
                btnSaveNewCarParkDelete.IsEnabled = true;
            }
        }

        private void btnSaveNewCarParkDelete_Click(object sender, RoutedEventArgs e)
        {
            deleteCarLot();
        }

        private void cbxCarLot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCarLot = (CarLot)cbxCarLot.SelectedItem;
            txtNameCarPark.Text = selectedCarLot.name;
            txtNameCarParkSpace.Text = selectedCarLot.spaceItems;
            btnSaveNewCarPark.Content = "Update";
        }

        private async void btnSaveNewCarSpacePark_Click(object sender, RoutedEventArgs e)
        {
            
            
            try
            {
                List<CarParkSpace> lstCarParkSpace = new List<CarParkSpace>();
                string[] strSpaces = txtNameCarParkSpace.Text.ToUpper().Split(',');
                for (int i = 0; i < strSpaces.Length; i++)
                {
                    CarParkSpace carParkSpace = new CarParkSpace();
                    carParkSpace.name = strSpaces[i];
                    lstCarParkSpace.Add(carParkSpace);
                }
                selectedCarLot.spaces = lstCarParkSpace;
                btnSaveNewCarSpacePark.IsEnabled = false;
                string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);
                string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
                var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);
                string _strCarLotItem = await httpClientServices.PatchAsync(EndPoints.carLots + "/" + selectedCarLot.id, deserializedAuth.AccessToken, selectedCarLot, true);
                var deserializedCarLotItem = JsonConvert.DeserializeObject<dynamic>(_strCarLotItem);
                selectedCarLot = null;
                GetCarPark();
            }
            catch (Exception ex)
            {
                selectedCarLot = null;
            }
            btnSaveNewCarSpacePark.IsEnabled = true;
            txtNameCarParkSpace.Text = string.Empty;
        }

        private void btnNewEngagementType_Click(object sender, RoutedEventArgs e)
        {
            if (gridNewEngagementType.Visibility == Visibility.Visible)
            {
                gridNewEngagementType.Visibility = Visibility.Collapsed;
                selectedPostItem = null;
                btnSaveNewEngagementType.Content = "Add";
            }
            else
            {
                gridNewEngagementType.Visibility = Visibility.Visible;
            }
        }
    }
}
