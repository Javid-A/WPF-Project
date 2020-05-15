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
using ClosedXML.Excel;

namespace WPF_Project
{
	/// <summary>
	/// Interaction logic for Reports.xaml
	/// </summary>
	public partial class Reports : Window
	{
		private readonly WPFProjectEntities _db;
		List<SaleDetail> details;
		public Reports()
		{
			InitializeComponent();
			_db = new WPFProjectEntities();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			details = _db.SaleDetails.Where(d => d.Completed_Date != null).ToList();
			RefreshDgv();
			
		}

		private void RefreshDgv(string name = null)
		{

			List<SaleDetail> users;
			if (name == null)
			{
				users = _db.SaleDetails.Where(d=>d.Completed_Date!=null).ToList();
			}
			else
			{
				users = _db.SaleDetails.Where(d => d.Completed_Date != null).Where(m => m.Seller.Contains(name) || m.Customer.Contains(name)).ToList();
			}
			dgReports.ItemsSource = users;
		}

		private void dgReports_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			dgBooks.Items.Clear();
			int saleId = ((SaleDetail)dgReports.SelectedItem).Sale_ID;

			List<BookSale> saledBooks = _db.BookSales.Where(b => b.SaleId == saleId).ToList();
			foreach (BookSale saledBook in saledBooks)
			{
				Book book = _db.Books.Find(saledBook.BookId);
				dgBooks.Items.Add(book);
			}
		}

		private void txtSaleSearch_TextChanged(object sender, TextChangedEventArgs e)
		{
			string name = txtSaleSearch.Text;
			if (name.Length > 0)
			{
				RefreshDgv(name);
			}
			else
			{
				RefreshDgv();
			}
		}

		private void btnExcel_Click(object sender, RoutedEventArgs e)
		{
			var workbook = new XLWorkbook();
			var worksheet = workbook.Worksheets.Add("Report List");

			worksheet.Row(1).Height = 30;
			worksheet.Column(1).Width = 15;
			worksheet.Column(2).Width = 15;
			worksheet.Column(3).Width = 15;
			worksheet.Column(4).Width = 15;
			worksheet.Column(5).Width = 15;
			worksheet.Column(6).Width = 15;
			worksheet.Column(7).Width = 15;
			worksheet.Column(8).Width = 15;

			worksheet.Cell("A1").Value = "Sale Id";
			worksheet.Cell("B1").Value = "Seller";
			worksheet.Cell("C1").Value = "Customer";
			worksheet.Cell("D1").Value = "Customer Phone";
			worksheet.Cell("E1").Value = "Sale Date";
			worksheet.Cell("F1").Value = "Deadline";
			worksheet.Cell("G1").Value = "Completed Date";
			worksheet.Cell("H1").Value = "Total";

			int rowN = 2;

			foreach (SaleDetail item in details)
			{
				worksheet.Cell(rowN, 1).Value = item.Sale_ID;
				worksheet.Cell(rowN, 2).Value = item.Seller;
				worksheet.Cell(rowN, 3).Value = item.Customer;
				worksheet.Cell(rowN, 4).Value = item.Customer_Phone;
				worksheet.Cell(rowN, 5).Value = item.Sale_Date;
				worksheet.Cell(rowN, 6).Value = item.Deadline;
				worksheet.Cell(rowN, 7).Value = item.Completed_Date;
				worksheet.Cell(rowN, 8).Value = item.Total;
				rowN++;
			}
			workbook.SaveAs(@"C:\Users\Nicat\Desktop\ExportExcel.xlsx");

			MessageBox.Show("Succesfully exported", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

			
		}

		private void btnGetReport_Click(object sender, RoutedEventArgs e)
		{
			if(dpStart.Text == "" && dpEnd.Text == "")
			{
				return;
			}

			if(dpStart.Text != "" && dpEnd.Text=="")
			{
				DateTime start = DateTime.Parse(dpStart.Text);
				dgReports.ItemsSource = _db.SaleDetails.Where(d => d.Completed_Date != null).Where(d => d.Sale_Date >= start).ToList();
				return;
			}
			if (dpStart.Text=="" && dpEnd.Text != "")
			{
				DateTime end = DateTime.Parse(dpEnd.Text);

				dgReports.ItemsSource = _db.SaleDetails.Where(d => d.Completed_Date != null).Where(d =>d.Sale_Date <= end).ToList();
				return;
			}
			if (dpStart.Text != "" && dpEnd.Text != "")
			{
				DateTime start = DateTime.Parse(dpStart.Text);
				DateTime end = DateTime.Parse(dpEnd.Text);

				dgReports.ItemsSource = _db.SaleDetails.Where(d => d.Completed_Date != null).Where(d => d.Sale_Date >= start && d.Sale_Date <= end).ToList();
				return;
			}
			
			
		}
	}
}
