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

namespace Cardin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void listViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = listViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                case 0:
                    gridPrincipal.Children.Clear();
                    gridPrincipal.Children.Add(new UserControlHome());
                    break;
                case 1:
                    gridPrincipal.Children.Clear();
                    gridPrincipal.Children.Add(new UserControlOthers());
                    break;
                default:
                    break;
            }
        }

        private void MoveCursorMenu(int index)
        {
            try{
                transitioningContentSlider.OnApplyTemplate();
                index += 1;
                gridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
            }
            catch(Exception)
            {

            }
            
        }
    }
}
