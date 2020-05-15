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
	/// Interaction logic for Update.xaml
	/// </summary>
	public partial class Update : Window
	{
		private readonly WPFProjectEntities _db;
		private readonly string _email;
		private readonly DataGrid _dgvUsers;
		public Update(string email,DataGrid dgvUsers)
		{
			InitializeComponent();
			_db = new WPFProjectEntities();
			_email = email;
			_dgvUsers = dgvUsers;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			User user = _db.Users.First(u => u.Email == _email);



			var srcUser = @"C:\Users\Nicat\source\repos\WPF_Project\Image\user.png";
			Img1.Source = new ImageSourceConverter().ConvertFromString(srcUser) as ImageSource;

			txtName.Text = user.Fullname;
			txtEmail.Text = user.Email;

			if (user.IsAdmin)
			{
				ckAdmin.IsChecked = true;
			}
			else
			{
				ckAdmin.IsChecked = false;
			}
			if (user.IsActivated)
			{
				ckActivate.IsChecked = true;
			}
			else
			{
				ckActivate.IsChecked = false;
			}
			if (user.IsDeleted)
			{
				ckDelete.IsChecked = true;
			}
			else
			{
				ckDelete.IsChecked = false;
			}
		}

		private async void btnUpdate_Click(object sender, RoutedEventArgs e)
		{
			User user = _db.Users.First(u => u.Email == _email);

			user.IsAdmin = (bool)ckAdmin.IsChecked;
			user.IsActivated=(bool)ckActivate.IsChecked;
			user.IsDeleted = (bool)ckDelete.IsChecked;

			await _db.SaveChangesAsync();
			MessageBox.Show("Successfully updated","Info",MessageBoxButton.OK,MessageBoxImage.Information);
			_dgvUsers.ItemsSource= _db.Users.ToList();

			this.Close();
		}
	}
}
