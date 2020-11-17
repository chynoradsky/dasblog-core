using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using DasBlog.Core.Security;
using DasBlog.Managers;
using DasBlog.Managers.Interfaces;
using DasBlog.Services.FileManagement.Interfaces;
using DasBlog.Web.Identity;
using Xunit;

namespace DasBlog.Tests.UnitTests.Identity
{
	public class PasswordHasherTest
	{
		[Theory]
		[InlineData("nieco")]
		[InlineData("asfdfdfsfsfsdfzsd")]
		[InlineData("vgesfgdgf5678789")]
		[Trait("Category", "UnitTest")]
		public void HashPassword_HashIsSuccessful(string password)
		{

			using (var mock = AutoMock.GetLoose())
			{
				var user = new DasBlogUser();

				var siteSecurityManager = new SiteSecurityManager(null);
				var passwordHasher = new DasBlogPasswordHasher(siteSecurityManager);

				var hash = passwordHasher.HashPassword(user, password);
				Assert.NotEqual(password, hash);
				Assert.True(hash.Length > 0);
			}
		}
	}
}
