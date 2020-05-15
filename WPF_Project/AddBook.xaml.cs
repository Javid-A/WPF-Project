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
using WPF_Project.Models;

namespace WPF_Project
{
	/// <summary>
	/// Interaction logic for AddBook.xaml
	/// </summary>
	public partial class AddBook : Window
	{
		private readonly WPFProjectEntities _db;
		private readonly DataGrid _dg;

		public AddBook(DataGrid dg)
		{
			InitializeComponent();
			_db = new WPFProjectEntities();
			_dg = dg;
		}

		private async void btnAddBook_Click(object sender, RoutedEventArgs e)
		{
			
			try
			{
				string name = txtBook.Text.Trim();
				string author = txtAuthor.Text.Trim();
				string genre = txtGenre.Text.Trim();
				string strquantity = txtQuantity.Text.Trim();
				string strprice = txtPrice.Text.Trim();

				if (name == "" || author == "" || genre == "" || strquantity == "" || strprice == "")
				{
					MessageBox.Show("Please fill in all fields", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
				int quantity = int.Parse(strquantity);
				decimal price = decimal.Parse(strprice);
				Book book = new Book
				{
					Name = name,
					Author = author,
					Genre = genre,
					Quantity = quantity,
					Price=price
				};
				_db.Books.Add(book);
				await _db.SaveChangesAsync();
				MessageBox.Show("Succesfully added", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
				txtBook.Clear();
				txtAuthor.Clear();
				txtGenre.Clear();
				txtQuantity.Clear();
				txtPrice.Clear();
			}
			catch (Exception)
			{
				MessageBox.Show("Please enter valid quantity or price", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			_dg.ItemsSource = _db.Books.ToList();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			string srcBook = @"C:\Users\Nicat\source\repos\WPF_Project\Image\book.png";
			imgBook.Source = new ImageSourceConverter().ConvertFromString(srcBook) as ImageSource;
		}
	}
}
