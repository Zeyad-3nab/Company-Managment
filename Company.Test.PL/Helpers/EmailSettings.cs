using Company.Test.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Company.Test.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email) 
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("zeyadenab220@gmail.com", "hfnfvhrpjojkknqu");
			client.Send("zeyadenab220@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}