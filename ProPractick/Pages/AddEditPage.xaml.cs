using Microsoft.Win32;
using ProPractick.DB;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

namespace ProPractick.Pages
{
    /// <summary>
    /// Interaction logic for AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        Product contProduct;
        public AddEditPage(Product postProduct)
        {
            InitializeComponent();

            CountryCb.ItemsSource = DBConnection.connection.Country.ToList();
            CountryCb.DisplayMemberPath = "Name";

            UnitTb.ItemsSource = DBConnection.connection.Unit.ToList();

            contProduct = postProduct;
            this.DataContext = contProduct;
            if (contProduct.Id != 0)
            {
                AddCountryBtn.Visibility = Visibility.Visible;
                DelCountryBtn.Visibility = Visibility.Visible;
                AddCountryBtn.Visibility = Visibility.Visible;
                CountryLabel.Visibility= Visibility.Visible;
                CountryCb.Visibility = Visibility.Visible;  
                CountryLv.Visibility = Visibility.Visible; 
            }
        }

        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog()
                { Filter = "*.png|*.png|*.jpg|*.jpg", };

            if (openFile.ShowDialog().GetValueOrDefault())
            {
                contProduct.Photo = File.ReadAllBytes(openFile.FileName);
                PhotoProductImg.Source = new BitmapImage(new Uri(openFile.FileName));
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            contProduct.UnitId = (UnitTb.SelectedItem as Unit).Id;
            contProduct.AddDate = DateTime.Now;
            if (contProduct.Id == 0)
                DBConnection.connection.Product.Add(contProduct);

            DBConnection.connection.SaveChanges();

            AddCountryBtn.Visibility = Visibility.Visible;
            DelCountryBtn.Visibility = Visibility.Visible;
            CountryLabel.Visibility = Visibility.Visible;   
            CountryCb.Visibility = Visibility.Visible;
            CountryLv.Visibility = Visibility.Visible;
        }

        private void AddCountryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CountryCb.SelectedIndex >= 0)
            {
                var ProdCountry = new ProductCountry();
                var sel = CountryCb.SelectedItem as Country;
                ProdCountry.ProductId = contProduct.Id;
                ProdCountry.CountryId = sel.Id;
                var isCountry = DBConnection.connection.ProductCountry
                    .Where(x => x.CountryId == sel.Id && x.ProductId == contProduct.Id).Count();
                if (isCountry == 0)
                {
                    DBConnection.connection.ProductCountry.Add(ProdCountry);
                    DBConnection.connection.SaveChanges();
                    UpdateCountryList();
                }
            }
        }

        private void UpdateCountryList()
        {
            CountryLv.ItemsSource = DBConnection.connection.ProductCountry
                .Where(x => x.ProductId == contProduct.Id).ToList();
        }

        private void DelCountryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CountryLv.SelectedIndex != 0)
            {
                var selProductCountry = DBConnection.connection.ProductCountry.ToList()
                    .Find(x => x.ProductId == contProduct.Id 
                    && x.CountryId == (CountryLv.SelectedItem as ProductCountry).CountryId);
                DBConnection.connection.ProductCountry.Remove(selProductCountry);
                DBConnection.connection.SaveChanges();
            }
        }
    }
}
