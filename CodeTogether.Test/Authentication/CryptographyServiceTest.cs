using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeTogether.Services.Authentication;

namespace CodeTogether.App.Authentication.Test
{
	internal class CryptographyServiceTest
	{
		const string testHash = "49B8C967648C139EE91A983E399C842151DDFEFC402EEC79C46B84C448742993E1C537D7512F149DECF120443C4831F0D185F34AFDE62AE13AD385D4841FD825";
		const string testSalt = "645C0F298E721CBE78FBA71D41C4C71CA3F429C246284A8E9863C15B4E2AFF0A95DE358CB251A1D8FC7591DF580429D9C4585331CE64B87BD34B9164E6EA9C17";

		[Test]
		public void TestVerifiesHash()
		{
			var service = new CryptographyService();
			Assert.IsTrue(service.VerifyHash(testSalt, testHash, "password"));
		}
	}
}
