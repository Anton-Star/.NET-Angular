using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Infrastructure.Exceptions;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class UserFacade
    {
        private readonly Container container;

        public UserFacade(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// Get all the user
        /// </summary>
        /// <returns></returns>
        public List<User> GetAll()
        {
            var userManager = this.container.GetInstance<UserManager>();

            return userManager.GetAllUsers();
        }

        /// <summary>
        /// Get all the user asynchronously
        /// </summary>
        /// <returns></returns>
        public Task<List<User>> GetAllAsync()
        {
            var userManager = this.container.GetInstance<UserManager>();
            return userManager.GetAllAsync();
        }

        public async Task<List<User>> GetAllUsersForImpersonation()
        {
            var validRoles = new List<string>
            {
                Constants.Roles.User,
                Constants.Roles.Inspector,
                Constants.Roles.OrganizationalDirector,
                Constants.Roles.NonSystemUser
            };

            var userManager = this.container.GetInstance<UserManager>();
            var users = await userManager.GetAllAsync();

            return users.Where(x => x.IsActive && validRoles.Any(y => y == x.Role.Name)).Distinct().ToList();
        }

        public List<User> GetUsersToManage(Guid userId, bool isFactStaff)
        {
            var userManager = this.container.GetInstance<UserManager>();

            if (isFactStaff)
            {
                return this.GetAll();
            }

            var user = this.GetById(userId);

            var orgs = user.Organizations.Select(x => x.OrganizationId).ToList();

            return userManager.GetUsersByOrgs(orgs);
        }

        /// <summary>
        /// Get all the user asynchronously by organization ID
        /// </summary>
        /// <returns></returns>
        public Task<List<User>> GetByOrganizationAsync(int organizationId)
        {
            var userManager = this.container.GetInstance<UserManager>();

            return userManager.GetByOrganizationAsync(organizationId);
        }

        public User GetByEmail(string emailAddress)
        {
            var userManager = this.container.GetInstance<UserManager>();

            return userManager.GetByEmailAddress(emailAddress);
        }

        /// <summary>
        /// Get all users within the radius of 500 miles of site
        /// </summary>
        /// <param name="selectedFacilityId"></param>
        /// <returns></returns>
        public List<UserItem> GetUsersNearSite(string selectedSite)
        {
            var userManager = this.container.GetInstance<UserManager>();
            var selectedSiteId = Convert.ToInt32(selectedSite);

            return userManager.GetInspectorsWithinRadius(selectedSiteId);
        }

        /// <summary>
        /// Gets a user by Id
        /// </summary>
        /// <param name="id">Id of the User</param>
        /// <returns></returns>
        public User GetById(Guid id)
        {
            var userManager = this.container.GetInstance<UserManager>();

            var user = userManager.GetById(id);

            if (user != null && string.IsNullOrWhiteSpace(user.DocumentLibraryUserId))
            {
                user.DocumentLibraryUserId = this.CreateDocumentLibraryUser(user);
                userManager.Save(user);
            }

            return user;
        }

        /// <summary>
        /// Gets a user to validate an email address and password
        /// </summary>
        /// <param name="emailAddress">Email address of the person logging in</param>
        /// <param name="password">Password of the person logging in</param>
        /// <returns>User object</returns>
        public Tuple<User, bool> Login(string emailAddress, string password)
        {
            var userManager = this.container.GetInstance<UserManager>();
            var appSettingsManager = this.container.GetInstance<ApplicationSettingManager>();

            var appSetting = appSettingsManager.GetByName(Constants.ApplicationSettings.PasswordResetTimeout);

            if (string.IsNullOrWhiteSpace(emailAddress) || string.IsNullOrWhiteSpace(password))
            {
                return new Tuple<User, bool>(null, true);
            }

            var user = userManager.Login(emailAddress, password);

            if (user == null) return new Tuple<User, bool>(null, true);

            if (user.IsLocked &&
                DateTime.Now < user.UpdatedDate.GetValueOrDefault().AddHours(Convert.ToInt32(appSetting.Value)))
            {
                return new Tuple<User, bool>(null, false);
            }

            if (user.PasswordChangeDate.HasValue && user.PasswordChangeDate.Value.AddMonths(6) < DateTime.Now)
            {
                return new Tuple<User, bool>(user, true);
            }

            if (string.IsNullOrWhiteSpace(user.DocumentLibraryUserId))
            {
                user.DocumentLibraryUserId = this.CreateDocumentLibraryUser(user);

                userManager.Save(user);
            }

            return new Tuple<User, bool>(user, false);
        }

        /// <summary>
        /// Creates a new user in the system asynchronously
        /// </summary>
        /// <param name="user">User Item object</param>
        /// <param name="password">Password of the User</param>
        /// <param name="passwordConfirm">Password Confirmation of the User</param>
        /// <param name="createdBy">Who is creating the user</param>
        /// <returns></returns>
        public async Task RegisterUserAsync(UserItem userItem, string ipAddress, bool addToExistingUser, string createdBy)
        {
            var roleManager = this.container.GetInstance<RoleManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var userOrganizationManager = this.container.GetInstance<UserOrganizationManager>();
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();
            var auditLogFacade = this.container.GetInstance<AuditLogFacade>();
            var orgManager = this.container.GetInstance<OrganizationManager>();

            var isReassign = false;

            User user = null;

            var groups = trueVaultManager.GetAllGroups();

            if (groups.Result != TrueVaultManager.Success)
            {
                throw new Exception("Cannot get True Vault Groups");
            }

            userItem.Organizations = orgManager.FillTrueVaultGroupsForUsers(userItem.Organizations);

            var password = System.Web.Security.Membership.GeneratePassword(10, 3);

            if (!userItem.UserId.HasValue)
            {
                if (userItem.Role.RoleId == (int)Constants.Role.NonSystemUser)
                {
                    var c = 0;
                    var count = "";
                    var foundUser = userManager.GetByEmailAddress($"Non-System-User{count}");
                    while (foundUser != null)
                    {
                        c++;
                        count = c.ToString();
                        foundUser = userManager.GetByEmailAddress($"Non-System-User{count}");
                    }
                    userItem.EmailAddress = $"Non-System-User{count}";
                    userItem.IsActive = true;

                    user = await userManager.RegisterAsync(userItem, null, createdBy);
                }
                else
                {
                    var found = userManager.GetByEmailAddress(userItem.EmailAddress);

                    if (found != null)
                    {
                        if (!addToExistingUser)
                        {
                            throw new UserAlreadyExistsException(
                                string.Format("User with Email Address {0} already exists.", userItem.EmailAddress));
                        }

                        isReassign = true;

                        foreach (var org in userItem.Organizations)
                        {
                            found.Organizations.Add(new UserOrganization
                            {
                                Id = Guid.NewGuid(),
                                OrganizationId = org.Organization.OrganizationId,
                                JobFunctionId = org.JobFunction.Id,
                                ShowOnAccReport = org.ShowOnAccReport ?? true,
                                //CreatedDate = DateTime.Now,
                                CreatedBy = createdBy
                            });
                        }

                        userManager.Save(found);

                        trueVaultManager.AddUserToGroups(userItem.Organizations, found.DocumentLibraryUserId, groups);
                    }
                    else
                    {
                        userItem.DocumentLibraryUserId = trueVaultManager.CreateUser(userItem.Organizations, userItem.EmailAddress, userItem.DocumentLibraryUserId, groups);

                        user = await
                                userManager.RegisterAsync(userItem, password, createdBy);

                        if (userItem.Role.RoleId == (int)Constants.Role.FACTAdministrator ||
                            userItem.Role.RoleId == (int)Constants.Role.FACTCoordinator ||
                            userItem.Role.RoleId == (int)Constants.Role.QualityManager)
                        {
                            var orgs = orgManager.GetAll();

                            var userOrgs = orgs.Select(x => new UserOrganizationItem
                            {
                                Organization = new OrganizationItem
                                {
                                    OrganizationName = x.Name,
                                    DocumentLibraryGroupId = x.DocumentLibraryGroupId
                                }
                            })
                                .ToList();

                            trueVaultManager.AddUserToGroups(userOrgs, user.DocumentLibraryUserId, groups);
                        }
                    }

                    
                }

                auditLogFacade.AddAuditLog(createdBy, ipAddress, $"New User Created By {createdBy}: {userItem.EmailAddress}");
            }
            else
            {
                if (!userItem.IsActive)
                {
                    var director = userManager.CheckUserIsDirector(userItem.UserId.Value);

                    if (director != null)
                    {
                        throw new Exception("Director and Primary Contact personnel changes must be updated by FACT.  Please contact your coordinator.");
                    }
                }

                userOrganizationManager.RemoveUserOrganizations(userItem.UserId.Value);
                userManager.UpdateUser(userItem, createdBy);

                var userJobFunctionManager = this.container.GetInstance<UserJobFunctionManager>();
                userJobFunctionManager.UpdateJobFunction(userItem, createdBy);

                trueVaultManager.AddUserToGroups(userItem.Organizations, userItem.DocumentLibraryUserId, groups);

                if (userItem.IsActive)
                {
                    auditLogFacade.AddAuditLog(createdBy, ipAddress, $"{userItem.EmailAddress} updated By {createdBy}");
                }
                else
                {
                    auditLogFacade.AddAuditLog(createdBy, ipAddress, $"{userItem.EmailAddress} Deactivated updated By {createdBy}");
                }


            }

            if (userItem.Role.RoleId == (int) Constants.Role.FACTAdministrator || userItem.Role.RoleId == (int) Constants.Role.QualityManager)
            {
                var orgItem = new List<UserOrganizationItem>
                {
                    new UserOrganizationItem
                    {
                        Organization = new OrganizationItem
                        {
                            OrganizationName = "FULL_ADMIN"
                        }
                    }
                };

                trueVaultManager.AddUserToGroups(orgItem, userItem.DocumentLibraryUserId, groups);
            }

            if (!isReassign && userItem.Credentials != null)
            {
                var userCredentialManager = this.container.GetInstance<UserCredentialManager>();
                var credentials = userItem.Credentials.Where(x => x.IsSelected == true).ToList();
                await userCredentialManager.UpdateUserCredentialAsync(userItem.UserId ?? user.Id, credentials, createdBy);
            }

            if (!isReassign && userItem.Languages != null)
            {
                var userLanguageManager = this.container.GetInstance<UserLanguageManager>();
                var languages = userItem.Languages.Where(x => x.IsSelected == true).ToList();
                await userLanguageManager.UpdateUserLanguageAsync(userItem.UserId ?? user.Id, languages, createdBy);
            }

            if (!isReassign && userItem.Memberships != null)
            {
                var userMembershipManager = this.container.GetInstance<UserMembershipManager>();
                var userMemberships = userItem.Memberships.Where(x => x.IsSelected == true).ToList();
                await userMembershipManager.UpdateUserMembershipAsync(userItem.UserId ?? user.Id, userMemberships, createdBy);
            }
        }

        ///// <summary>
        ///// Updates a user basic info in the system
        ///// </summary>
        ///// <param name="userId">User Id</param>
        ///// <param name="firstName">First Name of the User</param>
        ///// <param name="lastName">Last Name of the User</param>
        ///// <param name="orgId">organization is</param>
        ///// <param name="roleId">role id</param>
        ///// <param name="isLocked">Is user locked out</param>
        ///// <param name="updatedBy">who is updating the user's basic info</param>
        ///// <returns></returns>
        //public async Task UpdateBasicUserInfoAsync(UserItem userItem, string updatedBy)
        //{
        //    var manager = this.container.GetInstance<UserManager>();

        //    await manager.UpdateBasicUserInfoAsync(userItem, updatedBy);

        //    var userCredentialManager = this.container.GetInstance<UserCredentialManager>();

        //    await userCredentialManager.UpdateUserCredentialAsync(userItem, updatedBy);

        //    var userJobFunctionManager = this.container.GetInstance<UserJobFunctionManager>();

        //    await userJobFunctionManager.UpdateJobFunctionAsync(userItem, updatedBy);
        //}

        ///// <summary>
        ///// Updates a user basic info in the system
        ///// </summary>
        ///// <param name="userId">User Id</param>
        ///// <param name="firstName">First Name of the User</param>
        ///// <param name="lastName">Last Name of the User</param>
        ///// <param name="orgId">organization is</param>
        ///// <param name="roleId">role id</param>
        ///// <param name="isLocked">Is user locked out</param>
        ///// <param name="updatedBy">who is updating the user's basic info</param>
        ///// <returns></returns>
        //public void UpdateBasicUserInfo(UserItem userItem, string updatedBy)
        //{
        //    var manager = this.container.GetInstance<UserManager>();

        //    manager.UpdateBasicUserInfo(userItem, updatedBy);

        //    var userCredentialManager = this.container.GetInstance<UserCredentialManager>();

        //    userCredentialManager.UpdateUserCredential(userItem, updatedBy);

        //    var userJobFunctionManager = this.container.GetInstance<UserJobFunctionManager>();

        //    userJobFunctionManager.UpdateJobFunction(userItem, updatedBy);
        //}

        public void UpdateUser(Guid userId, UserItem user, string updatedBy)
        {
            var manager = this.container.GetInstance<UserManager>();

            if (userId != user.UserId)
            {
                throw new NotAuthorizedException("Users Dont match");
            }

            manager.UpdateUser(user, updatedBy);
        }

        public async Task UpdateUserAsync(Guid userId, UserItem user, string ipAddress, string updatedBy)
        {
            var manager = this.container.GetInstance<UserManager>();
            var auditLogFacade = this.container.GetInstance<AuditLogFacade>();

            if (userId != user.UserId)
            {
                throw new NotAuthorizedException("Users Dont match");
            }

            await manager.UpdateUserAsync(user, updatedBy);

            auditLogFacade.AddAuditLog(updatedBy, ipAddress, $"New User Created By {updatedBy}: {user.EmailAddress}");
        }

        /// <summary>
        /// Updates a users password asynchronously
        /// </summary>
        /// <param name="token">User Token for doing the update</param>
        /// <param name="password">Password to change to</param>
        /// <param name="passwordConfirm">Confirmation that the password is correct</param>
        /// <param name="updatedBy">Who is doing the update</param>
        /// <returns></returns>
        public async Task<User> UpdatePasswordAsync(string token, string password, string passwordConfirm, string updatedBy)
        {
            if (password != passwordConfirm)
            {
                throw new UserPasswordIncorrectException("Password dont match");
            }

            var userManager = this.container.GetInstance<UserManager>();

            return await userManager.UpdatePasswordAsync(token, password, updatedBy);
        }

        /// <summary>
        /// Updates a users password
        /// </summary>
        /// <param name="token">User Token for doing the update</param>
        /// <param name="password">Password to change to</param>
        /// <param name="passwordConfirm">Confirmation that the password is correct</param>
        /// <param name="updatedBy">Who is doing the update</param>
        /// <returns></returns>
        public void UpdatePassword(string token, string password, string passwordConfirm, string updatedBy)
        {
            if (password != passwordConfirm)
            {
                throw new UserPasswordIncorrectException("Password dont match");
            }

            var userManager = this.container.GetInstance<UserManager>();

            userManager.UpdatePassword(token, password, updatedBy);
        }

        /// <summary>
        /// Sends a password reminder to a user if their information is found asynchronously
        /// </summary>
        /// <param name="firstName">First Name of the User</param>
        /// <param name="lastName">Last Name of the User</param>
        /// <param name="emailAddress">Email Address of the User</param>
        /// <param name="url">Url of the current server</param>
        public async Task SendReminderAsync(string firstName, string lastName, string emailAddress, string url)
        {
            var userManager = this.container.GetInstance<UserManager>();
            var applicationSettingsManager = this.container.GetInstance<ApplicationSettingManager>();

            var applicationSetting =
                await applicationSettingsManager.GetByNameAsync(Constants.ApplicationSettings.PasswordResetTimeout);

            if (applicationSetting == null)
            {
                throw new ObjectNotFoundException("Cannot find Setting.");
            }

            await userManager.SendReminderAsync(firstName, lastName, emailAddress, Convert.ToInt32(applicationSetting.Value), url);
        }

        /// <summary>
        /// Gets a user by their reset token asynchronously
        /// </summary>
        /// <param name="token">Token for the user</param>
        /// <returns>User entity object</returns>
        public Task<User> GetUserByTokenAsync(string token)
        {
            var userManager = this.container.GetInstance<UserManager>();

            return userManager.GetUserByTokenAsync(token);
        }

        /// <summary>
        /// Gets a user by their reset token
        /// </summary>
        /// <param name="token">Token for the user</param>
        /// <returns>User entity object</returns>
        public User GetUserByToken(string token)
        {
            var userManager = this.container.GetInstance<UserManager>();

            return userManager.GetUserByToken(token);
        }

        /// <summary>
        /// Get all the accreditation roles
        /// </summary>
        /// <returns></returns>
        public List<AccreditationRole> GetAllAccreditationRoles()
        {
            var accreditationRoleManager = this.container.GetInstance<AccreditationRoleManager>();

            return accreditationRoleManager.GetAll();
        }

        /// <summary>
        /// Get all the accreditation roles asynchronously
        /// </summary>
        /// <returns></returns>
        public Task<List<AccreditationRole>> GetAllAccreditationRolesAsync()
        {
            var accreditationRoleManager = this.container.GetInstance<AccreditationRoleManager>();
            return accreditationRoleManager.GetAllAsync();
        }

        /// <summary>
        /// Gets all the users of an organization asynchronously
        /// </summary>
        /// <param name="name">Name of the organization</param>
        /// <returns>Collection of User objects</returns>
        public async Task<List<User>> GetAllForOrganizationAsync(string name)
        {
            var userManager = this.container.GetInstance<UserManager>();
            var orgManager = this.container.GetInstance<OrganizationManager>();

            var org = await orgManager.GetByNameAsync(name);

            if (org == null)
            {
                throw new Exception("Cannot find organization");
            }

            return await userManager.GetByOrganizationAsync(org.Id);
        }

        /// <summary>
        /// Gets all the users of an organization
        /// </summary>
        /// <param name="name">Name of the organization</param>
        /// <returns>Collection of User objects</returns>
        public List<User> GetAllForOrganization(string name)
        {
            var userManager = this.container.GetInstance<UserManager>();
            var orgManager = this.container.GetInstance<OrganizationManager>();

            var org = orgManager.GetByName(name);

            if (org == null)
            {
                throw new Exception("Cannot find organization");
            }

            return userManager.GetByOrganization(org.Id);
        }

        /// <summary>
        /// Gets all the users of an organization asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of User objects</returns>
        public Task<List<User>> GetAllForOrganizationAsync(int organizationId)
        {
            var userManager = this.container.GetInstance<UserManager>();

            return userManager.GetByOrganizationAsync(organizationId);
        }

        /// <summary>
        /// Gets all the users of an organization
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of User objects</returns>
        public List<User> GetAllForOrganization(int organizationId)
        {
            var userManager = this.container.GetInstance<UserManager>();

            return userManager.GetByOrganization(organizationId);
        }

        /// <summary>
        /// Gets all Fact staff members
        /// </summary>
        /// <param name="roleId">Id of the current users role</param>
        /// <returns>Collection of fact staff members</returns>
        public List<User> GetFactStaff(int roleId)
        {
            if (roleId != (int)Constants.Role.FACTAdministrator)
            {
                return null;
            }

            var userManager = this.container.GetInstance<UserManager>();

            return userManager.GetFactStaff();
        }

        /// <summary>
        /// Gets all Fact staff members asynchronously
        /// </summary>
        /// <param name="roleId">Id of the current users role</param>
        /// <returns>Collection of fact staff members</returns>
        public Task<List<User>> GetFactStaffAsync(int roleId)
        {
            var userManager = this.container.GetInstance<UserManager>();

            return userManager.GetFactStaffAsync();
        }

        public User GetFailedAttempts(string emailAddress)
        {
            var userManager = this.container.GetInstance<UserManager>();

            var user = userManager.GetFailedAttempts(emailAddress);

            return user;
        }

        public void SetFailedAttempts(string emailAddress, int failedAttempts)
        {
            var userManager = this.container.GetInstance<UserManager>();

            userManager.SetFailedAttempts(emailAddress, failedAttempts);
        }

        public void SetAuditorObserver(Guid userId, bool isAuditor, bool isObserver, string savedBy)
        {
            var userManager = this.container.GetInstance<UserManager>();

            userManager.SetAuditorObserver(userId, isAuditor, isObserver, savedBy);
        }

        public async Task SetAuditorObserverAsync(Guid userId, bool isAuditor, bool isObserver, string savedBy)
        {
            var userManager = this.container.GetInstance<UserManager>();

            await userManager.SetAuditorObserverAsync(userId, isAuditor, isObserver, savedBy);
        }

        public void RequestAccess(string url, AccessRequestItem record, string createdBy)
        {
            MasterServiceType serviceType = null;

            if (record.ServiceType != "Other")
            {
                var masterServiceTypeManager = this.container.GetInstance<MasterServiceTypeManager>();
                serviceType = masterServiceTypeManager.GetByName(record.ServiceType);
            }

            if (serviceType == null && string.IsNullOrWhiteSpace(record.MasterServiceTypeOtherComment))
            {
                throw new Exception("If Service Type is 'Other', Service Type Other Comment is required.");
            }

            var appSettingManager = this.container.GetInstance<ApplicationSettingManager>();
            var supervisor = appSettingManager.GetByName(Constants.ApplicationSettings.CoordinatorSupervisorEmail);
            var factAdminEmail = appSettingManager.GetByName(Constants.ApplicationSettings.AutoCcEmailAddress);

            var manager = this.container.GetInstance<AccessRequestManager>();

            manager.Add(factAdminEmail.Value, supervisor.Value, url, record, serviceType, createdBy);
        }

        public List<User> GetByRole(string roleName)
        {
            var manager = this.container.GetInstance<UserManager>();

            return manager.GetByRole(roleName);
        }

        public Task<List<User>> GetByRoleAsync(string roleName)
        {
            var manager = this.container.GetInstance<UserManager>();

            return manager.GetByRoleAsync(roleName);
        }

        public List<User> GetAuditorObservers()
        {
            var manager = this.container.GetInstance<UserManager>();

            return manager.GetAuditorObservers();
        }

        public Task<List<User>> GetAuditorObserversAsync()
        {
            var manager = this.container.GetInstance<UserManager>();

            return manager.GetAuditorObserversAsync();
        }

        public void SetupDocumentLibrary()
        {
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var users = userManager.GetAllUsers();
            var groups = trueVaultManager.GetAllGroups();

            users = users.Where(x => x.DocumentLibraryUserId == null && x.EmailAddress != null).ToList();

            foreach (var user in users)
            {
                try
                {
                    var orgs = user.Organizations.Select(x => new UserOrganizationItem()
                    {
                        Organization = ModelConversions.Convert(x.Organization, false, false)
                    }).ToList();

                    user.DocumentLibraryUserId = trueVaultManager.CreateUser(orgs, user.EmailAddress, string.Empty,
                        groups);

                    userManager.Save(user);
                }
                catch (Exception e)
                {
                    //skip record
                }
            }
        }

        public void SetupDocumentLibrary(Guid userId)
        {
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();
            var userManager = this.container.GetInstance<UserManager>();

            var users = userManager.GetAllUsers();
            var groups = trueVaultManager.GetAllGroups();

            var user = users.SingleOrDefault(x => x.Id == userId);

            if (user == null) return;

            var orgs = user.Organizations.Select(x => new UserOrganizationItem()
            {
                Organization = ModelConversions.Convert(x.Organization, false, false)
            }).ToList();

            user.DocumentLibraryUserId = trueVaultManager.CreateUser(orgs, user.EmailAddress, string.Empty, groups);

            userManager.BatchSave(user);

            userManager.SaveChanges();
        }

        public string CreateDocumentLibraryUser(User user)
        {
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();

            var orgs = user.Organizations.Select(x => new UserOrganizationItem
            {
                Organization = ModelConversions.Convert(x.Organization, false)
            }).ToList();

            user.DocumentLibraryUserId = trueVaultManager.CreateUser(orgs, user.EmailAddress, string.Empty);

            return user.DocumentLibraryUserId;
        }

        public void AddFactStaffToTrueVaultGroups()
        {
            var orgManager = this.container.GetInstance<OrganizationManager>();
            var userManager = this.container.GetInstance<UserManager>();
            var trueVaultManager = this.container.GetInstance<TrueVaultManager>();

            var factStaff = userManager.GetFactStaff();
            var groups = trueVaultManager.GetAllGroups();

            var userOrgs = new List<UserOrganizationItem>();

            userOrgs.Add(new UserOrganizationItem
            {
                Organization = new OrganizationItem
                {
                    OrganizationName = "FULL_ADMIN"
                }
            });

            foreach (var user in factStaff)
            {
                trueVaultManager.AddUserToGroups(userOrgs, user.DocumentLibraryUserId, groups);
            }


        }

        public string GetTwoFactorCode(Guid userId, string url)
        {
            var userManager = this.container.GetInstance<UserManager>();

            var user = userManager.GetById(userId);

            if (user == null) return null;

            user.TwoFactorCode = this.GenerateTwoFactorCode();

            userManager.Save(user);

            url = url + "app/email.templates/otp.html";

            var html = WebHelper.GetHtml(url);
            html = html.Replace("{Code}", user.TwoFactorCode);

            EmailHelper.Send(user.EmailAddress, "FACT Accreditation Portal Account Verification Code", html);

            return user.TwoFactorCode;
        }

        public bool IsTwoFactorCodeCorrect(Guid userId, string code)
        {
            var user = this.GetById(userId);

            return user != null && user.TwoFactorCode == code;
        }

        private string GenerateTwoFactorCode()
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var result = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                result.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))));
            }

            return result.ToString();
        }
    }
}

