using Cardin.Helper;
using Cardin.Model;
using Cardin.Service;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace Cardin.View.UserControlView
{
    /// <summary>
    /// Interaction logic for UserControlListOfWorkers.xaml
    /// </summary>
    public partial class UserControlListOfWorkers : UserControl
    {
        HttpClientServices httpClientServices = new HttpClientServices();
        IsolatedLocalStorage isolatedLocalStorage = new IsolatedLocalStorage();
        Person selectedPerson = new Person();
        string passportBase64, signatureBase64, passportFileName, whatItemPicked = "";
        Image img;
        public UserControlListOfWorkers()
        {
            InitializeComponent();
            GetListOfWorkers();
            GetWorkerPosts();
            GetEngagementType();
        }

        private async void GetListOfWorkers()
        {
            grdProgressBar.Visibility = Visibility.Visible;
            string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
            var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);

            string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
            var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);

            string url = EndPoints.persons + "?facilityId=" + deserializedFacility.Id;
            string strPersons = await httpClientServices.GetAsync(url, deserializedAuth.AccessToken);
            if(strPersons!= "")
            {
                var deserializedPerson = JsonConvert.DeserializeObject<PersonStructure>(strPersons);
                dataGrid.ItemsSource = deserializedPerson.Data;
                grdProgressBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                Login login = new Login();
                login.Show();
            }
            
        }

        private void btnNewAttendees_Click(object sender, RoutedEventArgs e)
        {
            gridAttendeeList.Visibility = Visibility.Collapsed;
            gridNewAttendees.Visibility = Visibility.Visible;
        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFirstName.Text) &&
                !string.IsNullOrWhiteSpace(txtlastName.Text) &&
                !string.IsNullOrWhiteSpace(txtPhoneNo.Text) &&
                cbxPost.SelectedItem != null &&
                !string.IsNullOrWhiteSpace(txtExtentionNo.Text))
            {
                btnSaveWorkers.IsEnabled = true;
            }
            else
            {
                btnSaveWorkers.IsEnabled = false;
            }
        }


        private void GetSignatureData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.png) | *.jpg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                signatureBase64 = Convert.ToBase64String(Encode(bitmapImage));
            }
            else
            {
                signatureBase64 = "";
            }
        }

        private void GetImageData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.png) | *.jpg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                passportFileName = openFileDialog.FileName;
                GetImageFromDialog(openFileDialog.FileName, whatItemPicked == "P" ? imgProfile : imgSignature);
            }
            else
            {
                passportBase64 = "";
            }
        }

        private void GetImageFromDialog(string file,Image imgControl)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(file));
            imgControl.Source = bitmapImage;
            DrawingVisual dv = new DrawingVisual();
            VisualBrush vb = new VisualBrush(imgProfile);
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawRectangle(vb, null, new Rect(0, 0, imgProfile.ActualWidth, imgProfile.ActualHeight));
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(270, 270, 96, 96, PixelFormats.Default);
            rtb.Render(imgControl);
            rtb.Freeze();
            img.Source = rtb;
            img.LayoutUpdated += img_LayoutUpdated;
            BitmapEncoder enc = new JpegBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(rtb));
            var dt = DateTime.Now;
            string path = @"c:\cardin";
            Directory.CreateDirectory(path);
            string pathString = System.IO.Path.Combine(path, "compressed_passport" + dt.Minute.ToString() + dt.Day.ToString() + dt.Month.ToString() + dt.Year.ToString() + ".jpg");
            FileStream fs = new FileStream(pathString, FileMode.Create);
            enc.Save(fs);
            fs.Flush();
            fs.Close();
            if(whatItemPicked=="P")
                passportBase64 = Convert.ToBase64String(Encode(rtb));
            else
                signatureBase64 = Convert.ToBase64String(Encode(rtb));
        }


        private void img_LayoutUpdated(object sender, EventArgs e)
        {
            GetImageFromDialog(passportFileName,whatItemPicked=="P"?imgProfile:imgSignature);
            img.LayoutUpdated -= img_LayoutUpdated;
        }


        private byte[] Encode(RenderTargetBitmap bmp)
        {
            byte[] data = null;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            if (encoder != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                using (var ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    data = ms.ToArray();
                }
            }

            return data;
        }

        private static byte[] Encode(BitmapImage bitmapImage)
        {
            byte[] data = null;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            if (encoder != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                using (var ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    data = ms.ToArray();
                }
            }

            return data;
        }

        private void btnUploadPassportImg_Click(object sender, RoutedEventArgs e)
        {
            img = new Image();
            whatItemPicked = "P";
           GetImageData();
        }

        private void btnUploadSignatureImg_Click(object sender, RoutedEventArgs e)
        {
            img = new Image();
            whatItemPicked = "S";
            GetImageData();
        }

        public async void savePerson()
        {
            string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
            var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);

            string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
            var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);
            Person person = new Person();
            person.firstName = txtFirstName.Text;
            person.lastName = txtlastName.Text;
            person.phone = txtPhoneNo.Text;
            person.IsActive = true;
            if (tglBtnActive.IsChecked == false)
                person.IsActive = false;
            person.post = ((ItemClass)cbxPost.SelectedItem).name;
            person.engagementType = ((ItemClass)cbxEngagement.SelectedItem).name;
            person.email = txtEmail.Text;
            person.extentionNo = txtExtentionNo.Text;
            person.facilityId = deserializedFacility.Id;
            person.profileImage = passportBase64;
            person.signature = signatureBase64;
            grdProgressBar.Visibility = Visibility.Visible;
            string strPersons = await httpClientServices.CreateAsync(EndPoints.persons, person,deserializedAuth.AccessToken);
            var deserializedPerson = JsonConvert.DeserializeObject<Person>(strPersons);
            grdProgressBar.Visibility = Visibility.Collapsed;
        }

        private void btnSaveWorkers_Click(object sender, RoutedEventArgs e)
        {
            if (btnSaveWorkers.Content.ToString() != "Update")
            {
                savePerson();
            }
            else
            {
                update();
            }
            selectedPerson = null;
            passportBase64 = ""; signatureBase64 = "";
            GetListOfWorkers();
            gridNewAttendees.Visibility = Visibility.Collapsed;
            gridAttendeeList.Visibility = Visibility.Visible;
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
                    cbxPost.ItemsSource = deserializedWorkerPosts.Data;
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
                    cbxEngagement.ItemsSource = deserializedEngagementType.Data;
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

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPerson = dataGrid.SelectedItem as Person;
            if (selectedPerson != null)
            {
                gridNewAttendees.Visibility = Visibility.Visible;
                gridAttendeeList.Visibility = Visibility.Collapsed;
                txtEmail.Text = selectedPerson.email;
                txtExtentionNo.Text = selectedPerson.extentionNo;
                txtFirstName.Text = selectedPerson.firstName;
                txtlastName.Text = selectedPerson.lastName;
                txtPhoneNo.Text = selectedPerson.phone;
                cbxPost.SelectedItem = selectedPerson.post;
                cbxEngagement.SelectedItem = selectedPerson.engagementType;
                try
                {
                    imgProfile.Source = new BitmapImage(new Uri(selectedPerson.profileImagePath));
                    passportBase64 = selectedPerson.profileImage;
                }
                catch (Exception) { }
                try
                {
                    imgSignature.Source = new BitmapImage(new Uri(selectedPerson.signatureImagePath));
                    signatureBase64 = selectedPerson.signature;
                }
                catch (Exception) { }

                btnSaveWorkers.IsEnabled = true;
                btnSaveWorkers.Content = "Update";
            }            
        }

        private async void update()
        {
            string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
            var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);
            string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
            var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);
            Person person = new Person();
            person.firstName = txtFirstName.Text;
            person.lastName = txtlastName.Text;
            person.phone = txtPhoneNo.Text;
            person.IsActive = true;
            if (tglBtnActive.IsChecked == false)
                person.IsActive = false;
            person.post = ((ItemClass)cbxPost.SelectedItem).name;
            person.engagementType = ((ItemClass)cbxEngagement.SelectedItem).name; ;
            person.email = txtEmail.Text;
            person.extentionNo = txtExtentionNo.Text;
            person.facilityId = deserializedFacility.Id;
            if(passportBase64!="")
                person.profileImage = passportBase64;
            if (signatureBase64!="")
                person.signature = signatureBase64;

            string url = EndPoints.persons + "/" + selectedPerson.Id;
            var response = await httpClientServices.PatchAsync(url, deserializedAuth.AccessToken, person,true);
        }
    }
}
