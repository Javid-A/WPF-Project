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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Project.Models;

namespace WPF_Project
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly WPFProjectEntities _db;
		private readonly Window _login;
		public MainWindow(Window login)
		{
			InitializeComponent();
			_db = new WPFProjectEntities();
			_login = login;
		}

		private async void btnRSignUp_Click(object sender, RoutedEventArgs e)
		{
			string fullname = txtFullName.Text.Trim();
			string email = txtEmail.Text.Trim();
			string pw = txtPw.Password.Trim();
			string conPw = txtConPw.Password.Trim();

			if (!IsValid(email, fullname, pw, conPw))
			{
				return;
			}

			bool EmailInDb = _db.Users.Any(u => u.Email == email);

			if (EmailInDb)
			{
				MessageBox.Show("This email has already been taken", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			User user = new User
			{
				Email = email,
				Fullname = fullname,
				Password = pw.HashPassword()
			};

			MessageBox.Show("Successfully logged", "Register", MessageBoxButton.OK, MessageBoxImage.Information);

			txtFullName.Clear();
			txtEmail.Clear();
			txtPw.Clear();
			txtConPw.Clear();

			_db.Users.Add(user);

			await _db.SaveChangesAsync();
		}

		private bool IsValid(string email, string fullname, string pw, string conPw)
		{
			if (email == "" || fullname == "" || pw == "" || conPw == "")
			{
				MessageBox.Show("Please fill in all fields", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}
			if (!email.IsMail())
			{
				MessageBox.Show("Please enter valid email", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}
			if (pw != conPw)
			{
				MessageBox.Show("Password and confirm password are not same", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}
			if (pw.Length < 8)
			{
				MessageBox.Show("Password has to consist of minimum 8 symbols", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return false;
			}
			return true;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			string srcRegister = @"C:\Users\Nicat\source\repos\WPF_Project\Image\register.png";
			imgRegister.Source = new ImageSourceConverter().ConvertFromString(srcRegister) as ImageSource;
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			this.Close();
			_login.Show();
		}
	}
}
