using System.Collections.Generic;
using DasBlog.Core.Security;
using Xunit;
using DasBlog.Core.Services;
using Microsoft.Extensions.Options;
using System.Linq;
using DasBlog.Services.Users;
using DasBlog.Services.FileManagement;
using DasBlog.Services.ConfigFile;
using DasBlog.Services.FileManagement.Interfaces;
using Moq;
using Autofac.Extras.Moq;
using System;

namespace DasBlog.Tests.UnitTests.Services
{
	public class UserDataRepoTest
	{
		[Fact]
		[Trait("Category", "UnitTest")]
		public void Load_OnStandardConfig_ReturnsContainedUser()
		{
			//IUserDataRepo repo = new UserDataRepo(
			//  new OptionsAccessor<LocalUserDataOptions>{ Value = 
			//  new LocalUserDataOptions{Path = "../../../Config/SiteSecurity.config"}});
			//List<User> users = repo.LoadUsers().ToList();

			//Assert.Single(users, u => u.Name == "myemail@myemail.com");		// email is switched in for name by design
		}

		[Fact]
		[Trait("Category", "UnitTest")]
		public void HasUsers_ReturnsCorrectValue()
		{
			var users = new List<User>()
			{
				new User(){DisplayName = "Janko"},
				new User(){DisplayName = "Ferko"},
				new User(){DisplayName = "Branko"}
			};

			using (var mock = AutoMock.GetLoose())
			{
				mock.Mock<IUserDataRepo>().Setup(u => u.LoadUsers()).Returns(users);

				var userService = mock.Create<UserService>();
				Assert.True(userService.HasUsers());
			}
		}

		[Theory]
		[InlineData("janko@gmail.com")]
		[InlineData("Matko@niecoine.com")]
		[InlineData("Simon@centrum.com")]
		[Trait("Category", "UnitTest")]
		public void FindMatchingUser_ReturnsCorrectValue(string userEmail)
		{
			var users = getUsers();

			using (var mock = AutoMock.GetLoose())
			{
				mock.Mock<IUserDataRepo>().Setup(u => u.LoadUsers()).Returns(users);

				var userService = mock.Create<UserService>();

				(bool userFound, User user) result = userService.FindMatchingUser(userEmail);

				Assert.True(result.userFound);
				Assert.Equal(result.user.EmailAddress, userEmail);
			}
		}

		[Theory]
		[InlineData("janko@gmail.com")]
		[InlineData("Matko@niecoine.com")]
		[InlineData("Simon@centrum.com")]
		[Trait("Category", "UnitTest")]
		public void DeleteUser_UserIsDeleted(string userEmail)
		{
			var users = getUsers();

			using (var mock = AutoMock.GetLoose())
			{
				mock.Mock<IUserDataRepo>().Setup(u => u.LoadUsers()).Returns(users);
				mock.Mock<IConfigFileService<SiteSecurityConfigData>>().
					Setup(x => x.SaveConfig(new SiteSecurityConfigData() { Users = users }));

				var userService = mock.Create<UserService>();

				Assert.True(userService.DeleteUser(userEmail));
			}
		}

		[Theory]
		[InlineData("Misko@gmail.com")]
		[InlineData("Kubko@niecoine.com")]
		[InlineData("Marek@centrum.com")]
		[Trait("Category", "UnitTest")]
		public void DeleteUser_UserNotFound(string userEmail)
		{
			var users = getUsers();

			using (var mock = AutoMock.GetLoose())
			{
				mock.Mock<IUserDataRepo>().Setup(u => u.LoadUsers()).Returns(users);
				mock.Mock<IConfigFileService<SiteSecurityConfigData>>().
					Setup(x => x.SaveConfig(new SiteSecurityConfigData() { Users = users }));

				var userService = mock.Create<UserService>();

				Assert.False(userService.DeleteUser(userEmail));
			}
		}

		public List<User> getUsers()
		{
			var users = new List<User>()
			{
				new User(){DisplayName = "Janko", EmailAddress = "janko@gmail.com"},
				new User(){DisplayName = "Ferko", EmailAddress = "ferko@centrum.com"},
				new User(){DisplayName = "Branko", EmailAddress = "branko@niecoine.com"},
				new User(){DisplayName = "Matko", EmailAddress = "Matko@niecoine.com"},
				new User(){DisplayName = "Simon", EmailAddress = "Simon@centrum.com"}
			};

			return users;
		}

		public class OptionsAccessor<T> : IOptions<T> where T : class, new()
		{
			public T Value { get; set; }
		}
	}
}
