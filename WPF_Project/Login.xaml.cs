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
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class Login : Window
	{
		private readonly WPFProjectEntities _db;
		public Login()
		{
			InitializeComponent();
			_db = new WPFProjectEntities();
		}

		private void btnSignIn_Click(object sender, RoutedEventArgs e)
		{
			string email = txtEmail.Text.Trim();
			string pw = txtPw.Password.Trim();

			if (email == "" || pw == "")
			{
				MessageBox.Show("Please fill in all fields", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			User user = _db.Users.FirstOrDefault(u => u.Email == email);

			if (!IsValid(user, pw))
			{
				txtPw.Clear();
				return;
			}
			if (user.IsAdmin==true)
			{
				AdminPanel admin = new AdminPanel(this,user.Id);
				this.Hide();
				admin.Show();
			}
			if (user.IsAdmin==false)
			{
				Dashboard dash = new Dashboard(this,user);
				this.Hide();
				dash.Show();
			}
			txtEmail.Clear();
			txtPw.Clear();


		}

		private bool IsValid(User user, string password)
		{
			if (user == null)
			{
				MessageBox.Show("Wrong email", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}
			if (user.IsDeleted==true)
			{
				MessageBox.Show("Your account has been blocked", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}
			if (user.Password != password.HashPassword())
			{
				MessageBox.Show("Password is wrong", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}
			if (user.IsActivated==false)
			{
				MessageBox.Show("Please wait activation", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}

			return true;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			string srcLogin = @"C:\Users\Nicat\source\repos\WPF_Project\Image\enter.png";
			imgLogin.Source = new ImageSourceConverter().ConvertFromString(srcLogin) as ImageSource;
		}

		private void btnRegister_Click(object sender, RoutedEventArgs e)
		{
			MainWindow register = new MainWindow(this);
			register.Show();
			this.Hide();
		}
	}
}
