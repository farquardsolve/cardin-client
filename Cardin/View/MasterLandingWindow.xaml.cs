using Cardin.Helper;
using Cardin.View.UserControlView;
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

namespace Cardin.View
{
    /// <summary>
    /// Interaction logic for MasterLandingWindow.xaml
    /// </summary>
    public partial class MasterLandingWindow : Window
    {
        public MasterLandingWindow()
        {
            InitializeComponent();
            InitializeContentFlow();
        }

        public void InitializeContentFlow()
        {
            IsolatedLocalStorage isolatedLocalStorage = new IsolatedLocalStorage();
            string status = isolatedLocalStorage.Read(IsolatedFiles.isFacilityDetailsCompleted);
            status = status.Replace(System.Environment.NewLine, "");
            if (status != "true")
            {
                gridContentLayout.Children.Clear();
                gridContentLayout.Children.Add(new UserControlFacility());
                txtWindowHeader.Text = "Organisation Details";
            }
            else
            {
                gridContentLayout.Children.Clear();
                gridContentLayout.Children.Add(new UserControlClockInOut());
                txtWindowHeader.Text = "Clock In/Out";
                listViewMenu.SelectedIndex = 0;
            }
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            if(gridMenuLayout.Visibility == Visibility.Visible)
            {
                gridMenuLayout.Visibility = Visibility.Collapsed;
            }
            else
            {
                gridMenuLayout.Visibility = Visibility.Visible;
            }
        }

        private void listViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = listViewMenu.SelectedIndex;
            switch (index)
            {
                case 0:
                    gridContentLayout.Children.Clear();
                    gridContentLayout.Children.Add(new UserControlClockInOut());
                    break;
                case 1:
                    gridContentLayout.Children.Clear();
                    gridContentLayout.Children.Add(new UserControlListOfWorkers());
                    break;
                case 4:
                    gridContentLayout.Children.Clear();
                    gridContentLayout.Children.Add(new UserControlSetting());
                    break;
                default:
                    break;
            }
        }
    }
}
