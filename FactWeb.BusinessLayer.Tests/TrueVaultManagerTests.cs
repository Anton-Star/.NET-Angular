using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FactWeb.BusinessLayer.Tests
{
    [TestClass]
    public class TrueVaultManagerTests
    {
        [TestMethod]
        public void LoginTest()
        {
            var manager = new TrueVaultManager();

            var response = manager.Login("Sample", "sample", "9cd6bb87-f26f-40bd-aa1b-aab422909b4c");

            Console.WriteLine(response);
        }

        [TestMethod]
        public void CreateOrgTest()
        {
            var manager = new TrueVaultManager();

            var response = manager.CreateOrganization("My Org", "myOrg");

            Assert.IsNotNull(response);
            Assert.AreNotEqual("", response.GroupId);
            Assert.AreNotEqual("", response.VaultId);
        }

        [TestMethod]
        public void DeleteAllTest()
        {
            var manager = new TrueVaultManager();

            try
            {
                var vaults = manager.GetAllVaults();

                foreach (var vault in vaults.Vaults)
                {
                    manager.DeleteVault(vault.Id);
                }

            }
            catch
            {
                
            }
            

            var groups = manager.GetAllGroups();

            foreach (var group in groups.Groups)
            {
                manager.DeleteGroup(group.Group_id);
            }

            var users = manager.GetAllUsers();

            foreach (var user in users.Users)
            {
                if (!user.Username.Contains("ample"))
                {
                    manager.DeleteUser(user.Id);
                }
            }
        }
    }
    
}
