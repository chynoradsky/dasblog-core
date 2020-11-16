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
		public void UserShouldBeDeleted()
		{
			var userRepo = new UserDataRepo(new OptionsAccessor<ConfigFilePathsDataOption>());
			IConfigFileService<SiteSecurityConfigData> securityFileService = new SiteSecurityConfigFileService(new OptionsAccessor<ConfigFilePathsDataOption>());
			var userDataRepo = new UserDataRepo(new OptionsAccessor<ConfigFilePathsDataOption>());

			var userService = new UserService(userRepo, securityFileService);
			var users = new List<User>() { 
				new User(){DisplayName = "Janko"},
				new User(){DisplayName = "Ferko"},
				new User(){DisplayName = "Branko"}
			};

			var user = new User()
			{
				DisplayName = "Peter"
			};

			userService.AddOrReplaceUser(user, "somemeamil@gmail.com", users);

			Assert.True(userService.HasUsers()); 
		}

		//public bool DeleteUser(string email)
		//{
		//	var users = userRepo.LoadUsers().ToList();
		//	var userToDelete = users.FirstOrDefault(user => user.EmailAddress == email);
		//	if (users.Remove(userToDelete))
		//	{
		//		siteSecurityFileService.SaveConfig(new SiteSecurityConfigData() { Users = users });
		//		return true;
		//	}
		//	else
		//	{
		//		return false;   // no user with that email address
		//	}
		//}

	}

	public class OptionsAccessor<T> : IOptions<T> where T : class, new()
	{
		public T Value { get; set; }
	}
}
