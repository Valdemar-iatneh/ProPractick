using ProPractick.DB;
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

namespace ProPractick.Pages
{
    /// <summary>
    /// Interaction logic for ProductListPage.xaml
    /// </summary>
    public partial class ProductListPage : Page
    {
        private int actualPage;
        private User mainUser { get; set; }
        public ProductListPage(User _user)
        {
            InitializeComponent();
            var FilterProduct = (IEnumerable<Product>)DBConnection.connection.Product.ToList();
            FilterProduct.Where(x => x.IsDelited == true).ToList();
            ProductList.ItemsSource = FilterProduct;
            var LvUnit = DBConnection.connection.Unit.ToList();
            LvUnit.Insert(0, new Unit() { Id = -1, Name = "All" });
            UnitCb.ItemsSource = LvUnit;
            UnitCb.DisplayMemberPath = "Name";

            mainUser = _user;

            if (_user.RoleId == 1)
            {
                AddBtn.Visibility = Visibility.Visible;
                EditBtn.Visibility = Visibility.Visible;
                DeleteBtn.Visibility = Visibility.Visible;
            }
            else if (_user.RoleId == 2)
            {
                AddBtn.Visibility = Visibility.Hidden;
                EditBtn.Visibility = Visibility.Visible;
                DeleteBtn.Visibility = Visibility.Hidden;
            }
            else if (_user.RoleId == 3)
            {
                AddBtn.Visibility = Visibility.Hidden;
                EditBtn.Visibility = Visibility.Hidden;
                DeleteBtn.Visibility = Visibility.Hidden;
            }
        }

        private void Refresh()
        {
            var FilterProduct = (IEnumerable<Product>)DBConnection.connection.Product.ToList();
            if (!string.IsNullOrWhiteSpace(SearchNameDescTb.Text))
                FilterProduct = FilterProduct.Where(x => x.Name.StartsWith(SearchNameDescTb.Text)
                || x.Description.StartsWith(SearchNameDescTb.Text));

            if (UnitCb.SelectedIndex > 0)
            {
                FilterProduct = FilterProduct.Where(x => x.UnitId == (UnitCb.SelectedItem as Unit).Id || x.UnitId == -1);
                
            }

            if (DateCb.SelectedIndex == 1)
                FilterProduct = FilterProduct.OrderBy(x => x.AddDate);
            else if (DateCb.SelectedIndex == 2)
                FilterProduct = FilterProduct.OrderByDescending(x => x.AddDate);

            if (AlfCb.SelectedIndex == 1)
                FilterProduct = FilterProduct.OrderBy(x => x.Name);
            else if (AlfCb.SelectedIndex == 2)
                FilterProduct = FilterProduct.OrderByDescending(x => x.Name);

            if (SortCount.SelectedIndex > 0 && FilterProduct.Count() > 0) 
            {
                var cbb = SortCount.SelectedItem as ComboBoxItem;
                int SelCount = Convert.ToInt32(cbb.Content);
                FilterProduct = FilterProduct.Skip(SelCount * actualPage).Take(SelCount);
                if (FilterProduct.Count() == 0)
                {
                    actualPage--;
                    return;
                }
                ProductList.ItemsSource = FilterProduct;
            }

            ProductList.ItemsSource = FilterProduct;
            act_count.Text = $"{DBConnection.connection.Product.Count()+1}";
            tb_count.Text = ProductList.Items.Count.ToString();
        }

        private void BackListBtn_Click(object sender, RoutedEventArgs e)
        {
            actualPage--;
            if (actualPage < 0)
                actualPage = 0;
            Refresh();
        }

        private void ForwardListBtn_Click(object sender, RoutedEventArgs e)
        {
            actualPage++;
            Refresh();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPage(new Product(), mainUser));
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var isSelProduct = ProductList.SelectedItem as Product;
            if (isSelProduct != null)
                NavigationService.Navigate(new AddEditPage(isSelProduct, mainUser));
            else
                MessageBox.Show("Nothing is selected");
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var isSelProduct = ProductList.SelectedItem as Product;
            if (isSelProduct != null)
            {
                var result = MessageBox.Show($"Delete {isSelProduct.Name}?", "Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.OK)
                {
                    isSelProduct.IsDelited = true;
                    DBConnection.connection.Product.SingleOrDefault(p => p.Id == isSelProduct.Id);
                    DBConnection.connection.SaveChanges();
                }
                Refresh();
            }
            else
                MessageBox.Show("Nothing is selected");
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            actualPage = 0;
            Refresh();
        }

        private void SearchNameDescTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            actualPage = 0;
            Refresh();
        }

        private void UnitCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            actualPage = 0;
            Refresh();
        }

        private void DateCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            actualPage = 0;
            Refresh();
        }

        private void AlfCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            actualPage = 0;
            Refresh();
        }

        private void DateMounthBtn_Click(object sender, RoutedEventArgs e)
        {
            actualPage = 0;
            Refresh();
        }

        private void SortCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            actualPage = 0;
            Refresh();
        }
    }
}
