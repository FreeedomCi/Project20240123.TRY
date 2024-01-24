using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Models.Infra
{
	public class HashUtility
	{
		public static byte[] ToSHA256(string plainText, byte[] salt)
		{
			using (var mySHA256 = SHA256.Create())
			{
				var passwordBytes = Encoding.UTF8.GetBytes(salt + plainText);
				var hash = mySHA256.ComputeHash(passwordBytes);

				return hash;
			}
		}
	}
}
