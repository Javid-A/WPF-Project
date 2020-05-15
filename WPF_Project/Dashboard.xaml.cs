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
	/// Interaction logic for Dashboard.xaml
	/// </summary>
	public partial class Dashboard : Window
	{
		private readonly WPFProjectEntities _db;
		private ItemCollection listBooks;
		private Customer _customer;
		private readonly User _user;
		private readonly Window _login;
		public Dashboard(Window login,User user)
		{
			InitializeComponent();
			_db = new WPFProjectEntities();
			_login = login;
			_user = user;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			string srcUserP = @"C:\Users\Nicat\source\repos\WPF_Project\Image\userpanel.png";
			userPanel.Source = new ImageSourceConverter().ConvertFromString(srcUserP) as ImageSource;
			string srcUser = @"C:\Users\Nicat\source\repos\WPF_Project\Image\user.png";
			user.Source = new ImageSourceConverter().ConvertFromString(srcUser) as ImageSource;
			string srcSearch = @"C:\Users\Nicat\source\repos\WPF_Project\Image\search.png";
			imgSearch.Source = new ImageSourceConverter().ConvertFromString(srcSearch) as ImageSource;
			string srcBook = @"C:\Users\Nicat\source\repos\WPF_Project\Image\book.png";
			sales.Source = new ImageSourceConverter().ConvertFromString(srcBook) as ImageSource;
			string srcLogOut = @"C:\Users\Nicat\source\repos\WPF_Project\Image\logout.png";
			imgLogOut.Source = new ImageSourceConverter().ConvertFromString(srcLogOut) as ImageSource;
			string srcCustomer = @"C:\Users\Nicat\source\repos\WPF_Project\Image\customer.png";
			imgCustomer.Source = new ImageSourceConverter().ConvertFromString(srcCustomer) as ImageSource;
			string srcCart = @"C:\Users\Nicat\source\repos\WPF_Project\Image\cart.png";
			imgLisBox.Source = new ImageSourceConverter().ConvertFromString(srcCart) as ImageSource;
			string srcSale = @"C:\Users\Nicat\source\repos\WPF_Project\Image\sale.png";
			imgSale.Source = new ImageSourceConverter().ConvertFromString(srcSale) as ImageSource;
			string srcRemove = @"C:\Users\Nicat\source\repos\WPF_Project\Image\remove.png";
			imgRemove.Source = new ImageSourceConverter().ConvertFromString(srcRemove) as ImageSource;
			txtUserName.Text = _user.Fullname;
			RefreshDgv();
		}

		private void RefreshDgv(string name = null)
		{

			List<Book> books;
			if (name == null)
			{
				books = _db.Books.ToList();
			}
			else
			{
				books = _db.Books.Where(b => b.Name.Contains(name)||b.Author.Contains(name)).ToList();
			}


			dgvBooks.ItemsSource = books;
		}

		private void txtBookSearch_TextChanged(object sender, TextChangedEventArgs e)
		{
			string name = txtBookSearch.Text.Trim();
			if (name.Length > 1)
			{
				RefreshDgv(name);
			}
			else
			{
				RefreshDgv();
			}
		}

		private void dgvBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			Book book = (Book)dgvBooks.SelectedItem;
			bool lsBook = bookSaleList.Items.Contains(book);
			int quantity = book.Quantity;
			if (book.Name != null &&  lsBook!=true  && quantity>0 )
			{
				bookSaleList.Items.Add(book);
				return;
			}
			if (quantity<1)
			{
				MessageBox.Show($"{book.Name} doesn't exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
		}

		private void btnRemoveList_Click(object sender, RoutedEventArgs e)
		{
			Book book = (Book)bookSaleList.SelectedItem;
			bookSaleList.Items.Remove(book);
		}

		private void addCustomer_Click(object sender, RoutedEventArgs e)
		{
			AddCustomer addCustomer = new AddCustomer();
			addCustomer.ShowDialog();
		}

		private void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			string phone = txtPhoneCustomer.Text.Trim();

			_customer = _db.Customers.FirstOrDefault(c => c.PhoneNumber == phone);

			if (_customer == null)
			{
				MessageBox.Show($"This phone number doesn't exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			customerName.Text = _customer.Fullname;
			txtPhoneCustomer.Clear();
		}

		private void btnSaleList_Click(object sender, RoutedEventArgs e)
		{
			if (!bookSaleList.HasItems)
			{
				MessageBox.Show("List is empty", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			if (customerName.Text == "")
			{
				MessageBox.Show("Please choose customer", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			listBooks = bookSaleList.Items;
			customerName.Text="";
			SaleList saleList = new SaleList(dgvBooks,listBooks,_customer,_user);
			saleList.ShowDialog();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_login.Close();
		}

		private void btnNonComp_Click(object sender, RoutedEventArgs e)
		{
			CompleteSale complete = new CompleteSale(dgvBooks);
			complete.ShowDialog();
		}

		private void btnLogOut_Click(object sender, RoutedEventArgs e)
		{
			Login login = new Login();
			login.Show();
			this.Close();
		}

		private void addBook_Click(object sender, RoutedEventArgs e)
		{
			AddBook addBook = new AddBook(dgvBooks);
			addBook.ShowDialog();
		}
	}
}
