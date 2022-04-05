using ProPractick.DB;
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

namespace ProPractick.Pages
{
    /// <summary>
    /// Interaction logic for AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public static ObservableCollection<User> users { get; set; }
        public AuthorizationPage()
        {
            InitializeComponent();
            LoginTB.Text = Properties.Settings.Default.Login;
        }

        private void ButtonAuthoriz_Click(object sender, RoutedEventArgs e)
        {
            users = new ObservableCollection<User>(DBConnection.connection.User.ToList());
            try
            {
                var entry = users.Where(a => a.Login == LoginTB.Text && a.Password == PasswordTB.Password).FirstOrDefault();
                if (entry == null)
                {
                    MessageBox.Show($"User isn't found");
                    return;
                }
                if (RememberMeCB.IsChecked.GetValueOrDefault())
                {
                    Properties.Settings.Default.Login = entry.Login;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.Login = null;
                    Properties.Settings.Default.Save();
                }

                switch (entry.RoleId)
                {
                    case 1:
                        NavigationService.Navigate(new ProductListPage());
                        break;
                    case 3:
                        NavigationService.Navigate(new ProductListPage());
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);   
            }
        }

        private void ButtonRegistr_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
