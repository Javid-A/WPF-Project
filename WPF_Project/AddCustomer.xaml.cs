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
	/// Interaction logic for AddCustomer.xaml
	/// </summary>
	public partial class AddCustomer : Window
	{
		private readonly WPFProjectEntities _db;
		public AddCustomer()
		{
			InitializeComponent();
			_db = new WPFProjectEntities();
		}

		private async void btnAddCustomer_Click(object sender, RoutedEventArgs e)
		{
			string serial = txtSerial.Text.Trim();
			string fullname = txtFull.Text.Trim();
			string phone = txtPhone.Text.Trim();

			if (serial == "" || fullname == "" || phone == "")
			{
				MessageBox.Show("Please fill in all fields", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			Customer customer = new Customer
			{
				SerialNumber = serial,
				Fullname = fullname,
				PhoneNumber = phone
			};

			_db.Customers.Add(customer);
			await _db.SaveChangesAsync();

			MessageBox.Show("Successfully added", "Register", MessageBoxButton.OK, MessageBoxImage.Information);

			txtSerial.Clear();
			txtFull.Clear();
			txtPhone.Clear();

		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			string srcCustomer = @"C:\Users\Nicat\source\repos\WPF_Project\Image\customer.png";
			imgCustomer.Source = new ImageSourceConverter().ConvertFromString(srcCustomer) as ImageSource;
		}
	}
}
