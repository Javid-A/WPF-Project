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
	/// Interaction logic for SaleList.xaml
	/// </summary>
	public partial class SaleList : Window
	{
		private readonly WPFProjectEntities _db;
		private ItemCollection _books;
		private readonly DataGrid _dg;
		private readonly User _user;
		private readonly Customer _customer;
		public SaleList(DataGrid dg,ItemCollection books,Customer customer, User user)
		{
			InitializeComponent();
			_db = new WPFProjectEntities();
			_dg = dg;
			_books = books;
			_customer = customer;
			_user = user;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			DateTime date = DateTime.Today;
			decimal total=0;
			foreach (Book book in _books)
			{
				dgSaleList.Items.Add(book);
				total += book.Price;
			};

			lblStart.Content = date.ToString("dd/MM/yyyy");
			lblTotal.Content = total;
			dtPicker.DisplayDateStart = DateTime.Today.AddDays(+1);
		}

		private async void btnSell_Click(object sender, RoutedEventArgs e)
		{
			if (dtPicker.Text == "")
			{
				MessageBox.Show("Please select deadline","Warning",MessageBoxButton.OK,MessageBoxImage.Warning);
				return;
			};
			ItemCollection books = dgSaleList.Items;
			decimal total = decimal.Parse(lblTotal.Content.ToString());
			DateTime startDate = DateTime.Parse(lblStart.Content.ToString());
			DateTime deadline = DateTime.Parse(dtPicker.Text);
			Sale sale = new Sale
			{
				UserId = _user.Id,
				CustomerId = _customer.Id,
				Total = total,
				StartDate = startDate,
				Deadline=deadline,
			};

			foreach (Book book in dgSaleList.Items)
			{
				sale.BookSales.Add(new BookSale
				{
					BookId=book.Id,
					SaleId=sale.Id,
					Quantity=1
				});
				Book soldBook = _db.Books.Find(book.Id);
				soldBook.Quantity -= 1;
			}
			_db.Sales.Add(sale);
			await _db.SaveChangesAsync();
			MessageBox.Show("Succesfully sold", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
			this.Close();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_dg.ItemsSource = _db.Books.ToList();
			_books.Clear();
		}
	}
}
