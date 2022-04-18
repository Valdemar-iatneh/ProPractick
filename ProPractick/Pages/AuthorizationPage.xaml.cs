using ProPractick.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            LoginTB.Text = "Kami";
            PasswordTB.Password = "K99mi";
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
            users = new ObservableCollection<User>(DBConnection.connection.User.ToList());

            if (CorrectPass(PasswordTB.Password))
            {
                User new_user = new User 
                {    
                    Login = LoginTB.Text,
                    Password = PasswordTB.Password,
                    RoleId = 3
                };
                
                DBConnection.connection.User.Add(new_user);
                DBConnection.connection.SaveChanges();
                MessageBox.Show("The client is registered");
                NavigationService.Navigate(new ProductListPage());
            }
            else
            {
                MessageBox.Show("The password doesn't match the pattern");
            }
        }
        private bool CorrectPass(string _password)
        {
            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[^a-zA-Z0-9])\S{6,16}$");
            foreach (var i in users)
            {
                if (i.Login == LoginTB.Text)
                {
                    MessageBox.Show("The login already exists");
                    return false;
                }
            }

            return regex.IsMatch(_password) ? true : false;
        }
    }
}
