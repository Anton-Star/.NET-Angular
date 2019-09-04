using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer.Tests
{
    [TestClass]
    public class UserManagerTests
    {
        private Mock<IUserRepository> userRepository;
        private UserManager userManager;

        private const string ValidEmail = "test@test.com";
        private const string InvalidEmail = "test3@test.com";
        private const string Token = "ABC123";

        private static Guid FirstId = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            this.userRepository = new Mock<IUserRepository>();
            this.userManager = new UserManager(this.userRepository.Object);

            var users = new List<User>
                        {
                            new User
                            {
                                Id = FirstId,
                                //OrganizationId = 1,
                                EmailAddress = ValidEmail,
                                PasswordResetToken = Token,
                                Password = "$2a$10$Wx3PgNF19s0lXwAXCHCHHuP4QGM6m0lXk.mjoKegSHWMw4tjryUle",
                                IsActive = true
                            },
                            new User
                            {
                                Id = Guid.NewGuid(),
                                //OrganizationId = 1,
                                EmailAddress = InvalidEmail,
                                Password = "$2a$10$Wx3PgNF19s0lXwAXCHCHHuP4QGM6m0lXk.mjoKegSHWMw4tjryUle",
                                IsActive = false
                            },
                        };

            this.userRepository.Setup(u => u.GetByEmailAddress(It.IsAny<string>()))
                .Returns((string e) => users.SingleOrDefault(x => x.EmailAddress.Equals(e) && x.IsActive));

            this.userRepository.Setup(u => u.GetByOrganization(It.IsAny<int>()))
                .Returns((int e) => users.Where(x => x.Organizations.Any(y=>y.OrganizationId == e)).ToList());

            this.userRepository.Setup(u => u.GetByOrganizationAsync(It.IsAny<int>()))
                .Returns((int e) => Task.FromResult(users.Where(x => x.Organizations.Any(y => y.OrganizationId == e)).ToList()));

            this.userRepository.Setup(u => u.GetByOrganizationAndEmailAddress(It.IsAny<int>(), It.IsAny<string>()))
                .Returns((int o, string e) => users.SingleOrDefault(x => x.Organizations.Any(y => y.OrganizationId == 0) && x.EmailAddress == e));

            this.userRepository.Setup(u => u.GetByOrganizationAndEmailAddressAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns((int o, string e) => Task.FromResult(users.SingleOrDefault(x => x.Organizations.Any(y => y.OrganizationId == 0) && x.EmailAddress == e)));

            this.userRepository.Setup(u => u.GetByToken(It.IsAny<string>()))
                .Returns((string e) => users.SingleOrDefault(x=> x.PasswordResetToken == e));

            this.userRepository.Setup(u => u.GetByTokenAsync(It.IsAny<string>()))
                .Returns((string e) => Task.FromResult(users.SingleOrDefault(x => x.PasswordResetToken == e)));

            this.userRepository.Setup(u => u.GetById(It.IsAny<Guid>()))
                .Returns((Guid u) => users.SingleOrDefault(x => x.Id == u));

            this.userRepository.Setup(u => u.GetAll())
                .Returns(users);

            this.userRepository.Setup(u => u.GetAllAsync())
                .Returns(Task.FromResult(users));
        }

        [TestMethod]
        public void LoginTest()
        {
            Assert.IsNull(this.userManager.Login(InvalidEmail, "password"));

            this.userRepository.Verify(rep => rep.GetByEmailAddress(InvalidEmail), Times.Once());

            Assert.IsNotNull(this.userManager.Login(ValidEmail, "password"));

            this.userRepository.Verify(rep => rep.GetByEmailAddress(ValidEmail), Times.Once);
        }

        [TestMethod]
        public void CheckIfEmailAddressExistsTest()
        {
            Assert.IsFalse(this.userManager.DoesEmailExist("blah@test.com"));
            Assert.IsTrue(this.userManager.DoesEmailExist(ValidEmail));
        }

        [TestMethod]
        public void CheckIfEmailAddressExistsAsyncTest()
        {
            Assert.IsFalse(this.userManager.DoesEmailExistAsync("blah@test.com").Result);
            Assert.IsTrue(this.userManager.DoesEmailExistAsync(ValidEmail).Result);
        }

        [TestMethod]
        public void GetUsersByOrgTest()
        {
            var items = this.userManager.GetByOrganization(1);
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetUsersByOrgAsyncTest()
        {
            var items = this.userManager.GetByOrganizationAsync(1).Result;
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetUsersByOrgAndEmailTest()
        {
            var item = this.userManager.GetByOrganizationAndEmailAddress(1, ValidEmail);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetUsersByOrgAndEmailAsyncTest()
        {
            var item = this.userManager.GetByOrganizationAndEmailAddressAsync(1, ValidEmail).Result;
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetUserByIdTest()
        {
            Assert.IsNotNull(this.userManager.GetById(FirstId));
        }

        [TestMethod, ExpectedException(typeof(NotImplementedException))]
        public void GetUserByIdIntTest()
        {
            Assert.IsNotNull(this.userManager.GetById(1));
        }

        [TestMethod, ExpectedException(typeof (AggregateException))]
        public void CheckForSameUserRegistrationAsyncTest()
        {
            var user =
                this.userManager.RegisterAsync(new UserItem
                {
                    Role = new RoleItem(),
                    Organizations = new List<UserOrganizationItem>(),
                    EmailAddress = ValidEmail,
                }, "", "").Result;
        }

        [TestMethod]
        public void AddUserAsyncTest()
        {
            var user = this.userManager.RegisterAsync(new UserItem
            {
                Role = new RoleItem(),
                Organizations = new List<UserOrganizationItem>(),
                EmailAddress = ValidEmail,
            }, "test", "").Result;

            this.userRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        public void UpdateUserPasswordUserNotFoundTest()
        {
            this.userManager.UpdatePassword(string.Empty, string.Empty, string.Empty);

            this.userRepository.Verify(x => x.GetByToken(It.IsAny<string>()), Times.Once);
            this.userRepository.Verify(x => x.Save(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void UpdateUserPasswordTest()
        {
            this.userManager.UpdatePassword(Token, string.Empty, string.Empty);

            this.userRepository.Verify(x=>x.GetByToken(It.IsAny<string>()), Times.Once);
            this.userRepository.Verify(x => x.Save(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        public void UpdateUserPasswordUserNotFoundAsyncTest()
        {
            Task.Run(async () =>
            {
                await this.userManager.UpdatePasswordAsync(string.Empty, string.Empty, string.Empty);

            }).GetAwaiter().GetResult();

            this.userRepository.Verify(x => x.GetByTokenAsync(It.IsAny<string>()), Times.Once);
            this.userRepository.Verify(x => x.SaveAsync(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void UpdateUserPasswordAsyncTest()
        {
            Task.Run(async () =>
            {
                await this.userManager.UpdatePasswordAsync(Token, string.Empty, string.Empty);

            }).GetAwaiter().GetResult();

            this.userRepository.Verify(x => x.GetByTokenAsync(It.IsAny<string>()), Times.Once);
            this.userRepository.Verify(x => x.SaveAsync(It.IsAny<User>()), Times.Once);
        }
    }
}
