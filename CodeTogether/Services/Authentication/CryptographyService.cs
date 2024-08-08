using System.Security.Cryptography;
using System.Text;

namespace CodeTogether.Services.Authentication;

public interface ICryptographyService
{
	string HashString(string inputValue, out string salt);
	bool VerifyHash(string salt, string hash, string inputValue);
}

public class CryptographyService : ICryptographyService
{
	const int keySize = 64;
	const int hashIterations = 1000;

	public bool VerifyHash(string salt, string hash, string inputValue)
	{
		var saltBytes = Convert.FromHexString(salt);
		var comparisonHash = GenerateHash(inputValue, saltBytes);
		return comparisonHash.SequenceEqual(Convert.FromHexString(hash));
	}

	public string HashString(string inputValue, out string salt)
	{
		var saltBytes = RandomNumberGenerator.GetBytes(keySize);
		salt = Convert.ToHexString(saltBytes);

		var hash = GenerateHash(inputValue, saltBytes);
		return Convert.ToHexString(hash);
	}

	static byte[] GenerateHash(string input, byte[] salt)
	{
		return Rfc2898DeriveBytes.Pbkdf2(
			Encoding.UTF8.GetBytes(input),
			salt,
			hashIterations,
			HashAlgorithmName.SHA512, 
			keySize);
	}
}