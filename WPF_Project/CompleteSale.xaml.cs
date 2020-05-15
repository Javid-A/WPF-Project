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
	/// Interaction logic for CompleteSale.xaml
	/// </summary>
	public partial class CompleteSale : Window
	{
		private readonly WPFProjectEntities _db;
		private readonly DataGrid _dg;
		public CompleteSale(DataGrid dg)
		{
			InitializeComponent();
			_db = new WPFProjectEntities();
			_dg = dg;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			RefreshDg();
		}

		private void RefreshDg(string name = null)
		{

			List<SaleDetail> users;
			if (name == null)
			{
				users = _db.SaleDetails.Where(d => d.Completed_Date == null).ToList();
			}
			else
			{
				users = _db.SaleDetails.Where(d => d.Completed_Date == null).Where(m => m.Seller.Contains(name) || m.Customer.Contains(name)).ToList();
			}
			dgSaleDetails.ItemsSource = users;
		}

		private void dgSaleDetails_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			try
			{
				SaleDetail saleDetail = (SaleDetail)dgSaleDetails.SelectedItem;

				lblSaleId.Content = saleDetail.Sale_ID.ToString();
				lblDeadline.Content = saleDetail.Deadline.ToString("dd/MM/yyyy");
				dtPicker.DisplayDateStart = saleDetail.Sale_Date;
			}
			catch (Exception)
			{
				MessageBox.Show("Please select correct sale","Warning",MessageBoxButton.OK,MessageBoxImage.Warning);
			}
		}

		private async void btnComplete_Click(object sender, RoutedEventArgs e)
		{
			if((string)lblSaleId.Content=="Id" || dtPicker.Text=="")
			{
				MessageBox.Show("Please enter all properties", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			SaleDetail saleDetail = (SaleDetail)dgSaleDetails.SelectedItem;
			DateTime completed = DateTime.Parse(dtPicker.Text);
			Sale sale = _db.Sales.Find(saleDetail.Sale_ID);
			List<BookSale> saledBooks = _db.BookSales.Where(sb => sb.SaleId == saleDetail.Sale_ID).ToList();

			foreach (BookSale saledbook in saledBooks)
			{
				Book book = _db.Books.Find(saledbook.BookId);
				book.Quantity+=1;
			}
			sale.EndDate = completed;
			await _db.SaveChangesAsync();
			if (saleDetail.Deadline < completed)
			{
				TimeSpan date = saleDetail.Deadline - completed;
				int count = (int)date.TotalDays;
				count = 0 - count;
				decimal fine=saleDetail.Total*(decimal)0.1*count;
				
				MessageBox.Show($"{saleDetail.Customer} have to pay {fine.ToString("N2")} manat more for delay", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			else
			{
				MessageBox.Show("Succesfully completed", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			RefreshDg();
			lblSaleId.Content = "Id";
			lblDeadline.Content = "Deadline";
			dtPicker.Text = "";

		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_dg.ItemsSource = _db.Books.ToList();
		}

		private void txtSaleSearch_TextChanged(object sender, TextChangedEventArgs e)
		{
			string name = txtSaleSearch.Text;
			if (name.Length > 0)
			{
				RefreshDg(name);
			}
			else
			{
				RefreshDg();
			}
		}
	}
}
