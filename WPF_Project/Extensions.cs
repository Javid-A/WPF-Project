using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project
{
	public static class  Extensions
	{
		public static bool IsMail(this string email)
		{
			try
			{
				MailAddress mail = new MailAddress(email);
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public static string HashPassword(this string password)
		{
			byte[] bytePw = Encoding.ASCII.GetBytes(password);
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] HashedPw = md5.ComputeHash(bytePw);

			return Encoding.ASCII.GetString(HashedPw);
		}
	}
}
