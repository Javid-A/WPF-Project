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
	/// Interaction logic for AdminPanel.xaml
	/// </summary>
	public partial class AdminPanel : Window
	{
		private readonly WPFProjectEntities _db;
		private readonly Window _login;
		private readonly int _userId;
		public AdminPanel(Window login, int Id)

		{
			InitializeComponent();
			_db = new WPFProjectEntities();
			_login = login;
			_userId = Id;
		}

		private void RefreshDgv( string email=null)
		{
			
			List<User> users;
			if (email == null)
			{
				users = _db.Users.ToList();
			}
			else
			{
				users = _db.Users.Where(m => m.Email.Contains(email)).ToList();
			}


			dgvUsers.ItemsSource = users;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			string srcEdit = @"C:\Users\Nicat\source\repos\WPF_Project\Image\edit.png";
			edit.Source = new ImageSourceConverter().ConvertFromString(srcEdit) as ImageSource;
			string srcCmd = @"C:\Users\Nicat\source\repos\WPF_Project\Image\cmd.png";
			cmd.Source = new ImageSourceConverter().ConvertFromString(srcCmd) as ImageSource;
			string srcAdmin = @"C:\Users\Nicat\source\repos\WPF_Project\Image\admin.png";
			admin.Source = new ImageSourceConverter().ConvertFromString(srcAdmin) as ImageSource;
			string srcLogOut = @"C:\Users\Nicat\source\repos\WPF_Project\Image\logout.png";
			imgLogOut.Source = new ImageSourceConverter().ConvertFromString(srcLogOut) as ImageSource;
			User user = _db.Users.Find(_userId);
			txtAdmin.Text = user.Fullname;
			RefreshDgv();
		}

		private void ckNonA_Click(object sender, RoutedEventArgs e)
		{
			if (ckNonA.IsChecked == true)
			{
				dgvUsers.ItemsSource = _db.Users.Where(u=>u.IsActivated==false).ToList();
			}
			else
			{
				RefreshDgv();
			}
		}

		private void dgvUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			User user = (User)dgvUsers.SelectedItem;
			Update update = new Update(user.Email,dgvUsers);
			update.ShowDialog();
		}

		private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
		{
			string email = txtSearch.Text;
			if (email.Length > 0 && ckNonA.IsChecked==false)

			{
				RefreshDgv(email);
			}else if (email.Length > 0 && ckNonA.IsChecked == true)
			{
				dgvUsers.ItemsSource = _db.Users.Where(u=>u.IsActivated==false).Where(m => m.Email.Contains(email)).ToList();

			}else if(email.Length == 0 && ckNonA.IsChecked == true)
			{
				dgvUsers.ItemsSource = _db.Users.Where(u => u.IsActivated == false).ToList();
			}else
			{
				RefreshDgv();
			}
		}


		private void btnReport_Click(object sender, RoutedEventArgs e)
		{
			Reports report = new Reports();
			report.ShowDialog();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			_login.Close();
		}

		private void btnLogOut_Click(object sender, RoutedEventArgs e)
		{
			Login login = new Login();
			login.Show();
			this.Close();
		}
	}
}
