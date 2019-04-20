using Cardin.Helper;
using Cardin.Model;
using Cardin.Service;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for UserControlFacility.xaml
    /// </summary>
    public partial class UserControlFacility : UserControl
    {
        string facilityBase64Logo = "";
        IsolatedLocalStorage isolatedLocalStorage = new IsolatedLocalStorage();
        HttpClientServices httpClientServices = new HttpClientServices();
        public UserControlFacility()
        {
            InitializeComponent();
            SetControlContent();
        }

        public void SetControlContent()
        {
            try
            {
                string status = isolatedLocalStorage.Read(IsolatedFiles.isFacilityDetailsCompleted);
                status = status.Replace(@"\r\n", "");
                if (status == "true")
                {
                    btnFacilitySaveUpdate.Content = "Update";
                }
                string strFacility = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
                var facility = JsonConvert.DeserializeObject<Facility>(strFacility);
                txtName.Text = facility.name;
                txtEmail.Text = facility.email;
                txtAddress.Text = facility.street;
                txtCity.Text = facility.city;
                txtContact.Text = facility.contact;
                txtState.Text = facility.state;
                txtPoBox.Text = facility.poBox;
            }
            catch(Exception ex)
            {
                string el = ex.Message;
            }
            

        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(txtAddress.Text)&&
                !string.IsNullOrWhiteSpace(txtCity.Text)&&
                !string.IsNullOrWhiteSpace(txtContact.Text)&&
                !string.IsNullOrWhiteSpace(txtCountry.Text)&&
                !string.IsNullOrWhiteSpace(txtEmail.Text)&&
                !string.IsNullOrWhiteSpace(txtName.Text)&&
                !string.IsNullOrWhiteSpace(txtState.Text))
            {
                btnFacilitySaveUpdate.IsEnabled = true;
            }
            else
            {
                btnFacilitySaveUpdate.IsEnabled = false;
            }

        }

        private void btnUploadFacilityLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.png) | *.jpg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                facilityBase64Logo = Convert.ToBase64String(Encode(bitmapImage));
                txtFacilityLogo.Text = openFileDialog.FileName;
            }
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

        private async void btnFacilitySaveUpdate_Click(object sender, RoutedEventArgs e)
        {
            
            string strfacilityDetails = isolatedLocalStorage.Read(IsolatedFiles.facilityDetails);
            var deserializedFacility = JsonConvert.DeserializeObject<Facility>(strfacilityDetails);

            string strAuth = isolatedLocalStorage.Read(IsolatedFiles.authFile);
            var deserializedAuth = JsonConvert.DeserializeObject<Auth>(strAuth);

            var facility = new Facility();
            facility.street = txtAddress.Text;
            facility.name = txtName.Text;
            facility.poBox = txtPoBox.Text;
            facility.state = txtState.Text;
            facility.email = txtEmail.Text;
            facility.city = txtCity.Text;
            facility.contact = txtContact.Text;
            facility.country = txtCountry.Text;
            facility.logo = facilityBase64Logo;
            //deserializedFacility
            string response = await httpClientServices.UpdateAsync(EndPoints.savefacilities,deserializedFacility.Id,facility, deserializedAuth.AccessToken);
            isolatedLocalStorage.Write(IsolatedFiles.facilityDetails, response);
            isolatedLocalStorage.Write(IsolatedFiles.isFacilityDetailsCompleted, "true");

            MessageBox.Show("Organisation Information updated successfully", "CardIn");
            
        }
    }
}
