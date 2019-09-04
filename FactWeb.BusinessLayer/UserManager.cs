using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class UserManager : BaseManager<UserManager, IUserRepository, User>
    {
        public UserManager(IUserRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets a user by Id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns></returns>
        public User GetById(Guid id)
        {
            LogMessage("GetById (UserManager)");

            return base.Repository.GetById(id);
        }

        /// <summary>
        /// Id on user is a GUID not Int. Use other Get method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            return base.Repository.GetAllUsers();
        }

        public List<User> GetAuditorsAndObservers()
        {
            return base.Repository.GetAuditorsAndObservers();
        }

        public Task<List<User>> GetAuditorsAndObserversAsync()
        {
            return base.Repository.GetAuditorsAndObserversAsync();
        }


        public List<User> GetUsersByOrgs(List<int> organizations)
        {
            return base.Repository.GetUsersByOrgs(organizations);
        }

        /// <summary>
        /// Gets a coordinator by key. Exception thrown if the user isnt Fact.
        /// </summary>
        /// <param name="coordinatorId">Id of the coordinator</param>
        /// <returns>User entity object</returns>
        public User GetCoordinator(Guid coordinatorId)
        {
            var coordinator = this.GetById(coordinatorId);

            if (coordinator == null || Constants.FactRoles.Names.All(x => x != coordinator.Role.Name))
            {
                throw new Exception("Invalid Coordinator");
            }

            return coordinator;
        }

        /// <summary>
        /// Gets a user based on email address
        /// </summary>
        /// <param name="emailAddress">Email Address of the user</param>
        /// <returns>User entity object</returns>
        public User GetByEmailAddress(string emailAddress)
        {
            LogMessage("GetByEmailAddress (UserManager)");

            return base.Repository.GetByEmailAddress(emailAddress);
        }

        /// <summary>
        /// Gets a list of users by organization asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Asynchronous task with a collection of User entity objects</returns>
        public Task<List<User>> GetByOrganizationAsync(int organizationId)
        {
            LogMessage("GetByOrganizationAsync (UserManager)");

            return base.Repository.GetByOrganizationAsync(organizationId);
        }

        /// <summary>
        /// Gets a list of users by organization
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of User entity objects</returns>
        public List<User> GetByOrganization(int organizationId)
        {
            LogMessage("GetByOrganization (UserManager)");

            return base.Repository.GetByOrganization(organizationId);
        }

        /// <summary>
        /// Gets a user based on organization and email address
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="emailAddress">Email Address of the user</param>
        /// <returns>User entity object</returns>
        public User GetByOrganizationAndEmailAddress(int organizationId, string emailAddress)
        {
            LogMessage("GetByOrganizationAndEmailAddress (UserManager)");

            return base.Repository.GetByOrganizationAndEmailAddress(organizationId, emailAddress);
        }

        /// <summary>
        /// Gets a user based on organization and email address asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="emailAddress">Email Address of the user</param>
        /// <returns>User entity object</returns>
        public Task<User> GetByOrganizationAndEmailAddressAsync(int organizationId, string emailAddress)
        {
            LogMessage("GetByOrganizationAndEmailAddressAsync (UserManager)");

            return base.Repository.GetByOrganizationAndEmailAddressAsync(organizationId, emailAddress);
        }

        /// <summary>
        /// Get the user by their email address and password
        /// </summary>
        /// <param name="emailAddress">Users email address</param>
        /// <param name="password">Users password</param>
        /// <returns>User object if the email address and password are correct</returns>
        public User Login(string emailAddress, string password)
        {
            LogMessage("Login (UserManager)");

            var user = base.Repository.GetByEmailAddress(emailAddress);

            return user == null || !user.IsActive ? null : BCrypt.Net.BCrypt.Verify(password, user.Password) ? user : null;
        }

        /// <summary>
        /// Creates a new user in the system
        /// </summary>
        /// <param name="userItem">User Item Object</param>
        /// <param name="password">Password of the User</param>
        /// <param name="createdBy">Who is creating this user</param>
        /// <returns></returns>
        public async Task<User> RegisterAsync(UserItem userItem, string password, string createdBy)
        {
            LogMessage("RegisterAsync (UserManager)");

            if (string.IsNullOrEmpty(userItem.EmailAddress))
            {
                if (userItem.Role.RoleId != (int)Constants.Role.NonSystemUser)
                {
                    throw new NullReferenceException("EmailAddress cannot be null");
                }
            }
            

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = userItem.FirstName,
                LastName = userItem.LastName,
                EmailAddress = userItem.EmailAddress,
                PreferredPhoneNumber = userItem.PreferredPhoneNumber,
                PhoneExtension = userItem.Extension,
                MedicalLicensePath = userItem.MedicalLicensePath,
                MedicalLicenseExpiry = userItem.MedicalLicenseExpiry,
                Password = string.IsNullOrEmpty(password) ? null : BCrypt.Net.BCrypt.HashPassword(password),
                CreatedBy = createdBy ?? userItem.EmailAddress,
                PasswordChangeDate = DateTime.Now,
               // CreatedDate = DateTime.Now,
               // PasswordChangeDate = DateTime.Now,
                IsActive = userItem.IsActive,
                RoleId = userItem.Role.RoleId,
                IsLocked = false,
                CompletedStep2 = false,
                CanManageUsers = userItem.CanManageUsers,
                DocumentLibraryUserId = userItem.DocumentLibraryUserId
            };

            if (userItem.Organizations != null)
            {
                user.Organizations = userItem.Organizations.Select(x => new UserOrganization
                {
                    Id = Guid.NewGuid(),
                    OrganizationId = x.Organization.OrganizationId,
                    JobFunctionId = x.JobFunction.Id,
                    ShowOnAccReport = x.ShowOnAccReport ?? true,
                    //CreatedDate = DateTime.Now,
                    CreatedBy = createdBy
                })
                    .ToList();
            }

            base.Add(user);

            return user;
        }

        public string EncryptUser(Guid userId)
        {
            return BCrypt.Net.BCrypt.HashPassword(userId.ToString());
        }

        //public async Task<User> UpdateUserAsync(UserItem userItem, string password, string createdBy)
        //{
        //    LogMessage("RegisterAsync (UserManager)");

        //    var check = this.GetByEmailAddress(userItem.EmailAddress);
        //    var user = this.GetById(userItem.UserId.Value);

        //    if (userItem.Role.RoleName != Constants.Roles.NonSystemUser && check != null && check.Id != userItem.UserId.Value)
        //    {
        //        throw new UserAlreadyExistsException(string.Format("User with Email Address {0} already exists.", userItem.EmailAddress));
        //    }

        //    user.FirstName = userItem.FirstName;
        //    user.LastName = userItem.LastName;
        //    user.PreferredPhoneNumber = userItem.PreferredPhoneNumber;
        //    user.PhoneExtension = userItem.Extension;
        //    user.EmailAddress = userItem.Role.RoleName == Constants.Roles.NonSystemUser ? user.EmailAddress : userItem.EmailAddress;
        //    user.MedicalLicensePath = userItem.MedicalLicensePath;
        //    user.MedicalLicenseExpiry = userItem.MedicalLicenseExpiry == null ? (DateTime?)null : userItem.MedicalLicenseExpiry.GetValueOrDefault();
        //    user.UpdatedBy = createdBy;
        //    user.UpdatedDate = DateTime.Now;
        //    user.RoleId = userItem.Role.RoleId;
        //    user.IsActive = userItem.IsActive;
        //    user.IsLocked = userItem.IsLocked;
        //    user.FailedLoginAttempts = 0;
        //    user.CanManageUsers = userItem.CanManageUsers;

        //    if (userItem.Organizations != null)
        //    {
        //        user.Organizations = userItem.Organizations.Select(x => new UserOrganization
        //        {
        //            Id = Guid.NewGuid(),
        //            OrganizationId = x.Organization.OrganizationId,
        //            JobFunctionId = x.JobFunction.Id,
        //            ShowOnAccReport = x.ShowOnAccReport ?? true,
        //            CreatedDate = DateTime.Now,
        //            CreatedBy = createdBy
        //        })
        //            .ToList();
        //    }

        //    base.Save(user);

        //    return user;
        //}


        /// <summary>
        /// Creates a new user in the system
        /// </summary>
        /// <param name="UserItem">User item object</param>
        /// <param name="createdBy">Who is creating this user</param>
        /// <returns></returns>
        public User RegisterNonSystemUser(UserItem userItem, string createdBy)
        {
            LogMessage("Register (UserManager)");

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = userItem.FirstName,
                LastName = userItem.LastName,
                PreferredPhoneNumber = userItem.PreferredPhoneNumber,
                PhoneExtension = userItem.Extension,
                RoleId = userItem.Role.RoleId,
                CreatedBy = createdBy,
                //CreatedDate = DateTime.Now,
                IsActive = userItem.IsActive,
                IsLocked = false,
                CompletedStep2 = false,
                MedicalLicensePath = userItem.MedicalLicensePath,
                MedicalLicenseExpiry = userItem.MedicalLicenseExpiry.GetValueOrDefault(),
                CanManageUsers = false
            };

            base.Add(user);

            return user;
        }

        //public void UpdateBasicUserInfo(UserItem userItem, string updatedBy)
        //{
        //    LogMessage("UpdateBasicUserInfo (UserManager)");

        //    var user = this.Repository.GetById(userItem.UserId.GetValueOrDefault());

        //    if (user == null)
        //    {
        //        throw new KeyNotFoundException("User not found.");
        //    }

        //    if (userItem.Role.RoleId == 19)
        //    {
        //        user.FirstName = userItem.FirstName;
        //        user.LastName = userItem.LastName;
        //        user.IsActive = userItem.IsActive;
        //        user.IsLocked = userItem.IsLocked;
        //        user.MedicalLicensePath = userItem.MedicalLicensePath;
        //        user.MedicalLicenseExpiry = userItem.MedicalLicenseExpiry;
        //    }
        //    else
        //    {
        //        user.FirstName = userItem.FirstName;
        //        user.LastName = userItem.LastName;
        //        user.EmailAddress = userItem.EmailAddress;
        //        user.RoleId = userItem.Role.RoleId;
        //        user.IsLocked = userItem.IsLocked;
        //        user.IsActive = userItem.IsActive;

        //        user.Organizations = userItem.Organizations.Select(x => new UserOrganization
        //        {
        //            OrganizationId = x.Organization.OrganizationId,
        //            CreatedBy = updatedBy,
        //            UpdatedBy = updatedBy
        //        })
        //            .ToList();
        //    }

        //    user.UpdatedBy = updatedBy;
        //    user.UpdatedDate = DateTime.Now;

        //    this.Repository.Save(user);
        //}

        //public async Task UpdateBasicUserInfoAsync(UserItem userItem, string updatedBy)
        //{
        //    LogMessage("UpdateBasicUserInfoAsync (UserManager)");

        //    var user = this.Repository.GetById(userItem.UserId.GetValueOrDefault());

        //    if (user == null)
        //    {
        //        throw new KeyNotFoundException("User not found.");
        //    }

        //    if (userItem.Role.RoleId == 19)
        //    {
        //        user.FirstName = userItem.FirstName;
        //        user.LastName = userItem.LastName;
        //        user.IsActive = userItem.IsActive;
        //        user.IsLocked = userItem.IsLocked;
        //        user.MedicalLicensePath = userItem.MedicalLicensePath;
        //        user.MedicalLicenseExpiry = userItem.MedicalLicenseExpiry;
        //    }
        //    else
        //    {
        //        user.FirstName = userItem.FirstName;
        //        user.LastName = userItem.LastName;
        //        user.EmailAddress = userItem.EmailAddress;
        //        user.RoleId = userItem.Role.RoleId;
        //        user.IsLocked = userItem.IsLocked;
        //        user.IsActive = userItem.IsActive;

        //        user.Organizations = userItem.Organizations.Select(x => new UserOrganization
        //        {
        //            OrganizationId = x.Organization.OrganizationId,
        //            CreatedBy = updatedBy,
        //            UpdatedBy = updatedBy
        //        })
        //            .ToList();
        //    }

        //    user.UpdatedBy = updatedBy;
        //    user.UpdatedDate = DateTime.Now;

        //    await this.Repository.SaveAsync(user);
        //}

        public void UpdateUser(UserItem userItem, string updatedBy)
        {
            LogMessage("UpdateUser (UserManager)");

            var user = this.Repository.GetById(userItem.UserId.GetValueOrDefault());

            if (user == null)
            {
                throw new KeyNotFoundException("Cannot find user");
            }

            var userMatch = this.Repository.GetByEmailAddress(userItem.EmailAddress);

            if (userMatch != null && user.Id != userMatch.Id)
            {
                throw new KeyNotFoundException("Email Address already exists");
            }

            user.WebPhotoPath = userItem.WebPhotoPath;
            user.Organizations = userItem.Organizations.Select(x => new UserOrganization
            {
                OrganizationId = x.Organization.OrganizationId,
                CreatedBy = updatedBy,
                CreatedDate = DateTime.Now,
                Id = Guid.NewGuid(),
                JobFunctionId = x.JobFunction.Id,
                ShowOnAccReport = x.ShowOnAccReport ?? true
            }).ToList();
            //user.UserJobFunctions = userItem.JobFunctions.Select(x => new UserJobFunction
            //{
            //    UserId = user.Id,
            //    JobFunctionId = x.Id,
            //    CreatedDate = DateTime.Now,
            //    CreatedBy = updatedBy
            //}).ToList();
            user.EmailOptOut = userItem.EmailOptOut;
            user.MailOptOut = userItem.MailOptOut;
            //user.UserLanguages = userItem.Languages.Select(x => new UserLanguage
            //{
            //    UserId = user.Id,
            //    LanguageId = x.Id,
            //    CreatedDate = DateTime.Now,
            //    CreatedBy = updatedBy
            //}).ToList();
            //user.UserMemberships = userItem.Memberships.Select(x => new UserMembership
            //{
            //    UserId = user.Id,
            //    MembershipId = x.Membership.Id,
            //    MembershipNumber = x.MembershipNumber,
            //    CreatedDate = DateTime.Now,
            //    CreatedBy = updatedBy
            //}).ToList();
            user.ResumePath = userItem.ResumePath;
            user.StatementOfCompliancePath = userItem.StatementOfCompliancePath;
            user.AgreedToPolicyDate = userItem.AgreedToPolicyDate;
            user.AnnualProfessionHistoryFormPath = userItem.AnnualProfessionHistoryFormPath;
            user.MedicalLicensePath = userItem.MedicalLicensePath;
            user.MedicalLicenseExpiry = userItem.MedicalLicenseExpiry == null ? (DateTime?)null : userItem.MedicalLicenseExpiry.GetValueOrDefault();
            user.CompletedStep2 = true;
            user.UpdatedBy = updatedBy;
            user.CanManageUsers = userItem.CanManageUsers;
            user.UpdatedDate = DateTime.Now;
            user.EmailAddress = userItem.EmailAddress;
            user.FirstName = userItem.FirstName;
            user.LastName = userItem.LastName;
            user.IsActive = userItem.IsActive;
            user.IsLocked = userItem.IsLocked;
            user.PreferredPhoneNumber = userItem.PreferredPhoneNumber;
            user.PhoneExtension = userItem.Extension;


            this.Repository.Save(user);
        }

        public async Task UpdateUserAsync(UserItem userItem, string updatedBy)
        {
            LogMessage("UpdateUserAsync (UserManager)");

            var user = this.Repository.GetById(userItem.UserId.GetValueOrDefault());

            if (user == null)
            {
                throw new KeyNotFoundException("Cannot find user");
            }

            user.WebPhotoPath = userItem.WebPhotoPath;
            user.UserJobFunctions = userItem.JobFunctions.Select(x => new UserJobFunction
            {
                JobFunctionId = x.Id,
                CreatedDate = DateTime.Now,
                CreatedBy = updatedBy
            }).ToList();
            user.EmailOptOut = userItem.EmailOptOut;
            user.MailOptOut = userItem.MailOptOut;
            user.UserLanguages = userItem.Languages.Select(x => new UserLanguage
            {
                LanguageId = x.Id,
                CreatedDate = DateTime.Now,
                CreatedBy = updatedBy
            }).ToList();
            user.UserMemberships = userItem.Memberships.Select(x => new UserMembership
            {
                MembershipId = x.Membership.Id,
                MembershipNumber = x.MembershipNumber,
                CreatedDate = DateTime.Now,
                CreatedBy = updatedBy
            }).ToList();
            user.ResumePath = userItem.ResumePath;
            user.StatementOfCompliancePath = userItem.StatementOfCompliancePath;
            user.AgreedToPolicyDate = userItem.AgreedToPolicyDate;
            user.AnnualProfessionHistoryFormPath = userItem.AnnualProfessionHistoryFormPath;
            user.MedicalLicensePath = userItem.MedicalLicensePath;
            user.MedicalLicenseExpiry = userItem.MedicalLicenseExpiry;
            user.CompletedStep2 = true;
            user.CanManageUsers = userItem.CanManageUsers;
            user.UpdatedBy = updatedBy;
            user.UpdatedDate = DateTime.Now;

            await this.Repository.SaveAsync(user);
        }

        /// <summary>
        /// Check to see if the email address is already in the system
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public bool DoesEmailExist(string emailAddress)
        {
            LogMessage("DoesEmailExist (UserManager)");

            return this.Repository.GetByEmailAddress(emailAddress) != null;
        }

        /// <summary>
        /// Check to see if the email address is already in the system asynchronously
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public async Task<bool> DoesEmailExistAsync(string emailAddress)
        {
            LogMessage("DoesEmailExistAsync (UserManager)");

            var user = this.Repository.GetByEmailAddress(emailAddress);

            return user != null;
        }

        /// <summary>
        /// Updates a users password asynchronously
        /// </summary>
        /// <param name="token">User Token for doing the update</param>
        /// <param name="password">Password to change to</param>
        /// <param name="updatedBy">Who is doing the update</param>
        /// <returns></returns>
        public async Task<User> UpdatePasswordAsync(string token, string password, string updatedBy)
        {
            LogMessage("UpdatePasswordAsync (UserManager)");

            var user = await this.GetUserByTokenAsync(token);

            if (user == null) return null;

            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            user.UpdatedDate = DateTime.Now;
            user.PasswordChangeDate = DateTime.Now;
            user.IsLocked = false;
            user.FailedLoginAttempts = 0;
            user.UpdatedBy = updatedBy;

            await this.Repository.SaveAsync(user);

            return user;
        }

        /// <summary>
        /// Updates a users password
        /// </summary>
        /// <param name="token">User Token for doing the update</param>
        /// <param name="password">Password to change to</param>
        /// <param name="updatedBy">Who is doing the update</param>
        /// <returns></returns>
        public void UpdatePassword(string token, string password, string updatedBy)
        {
            LogMessage("UpdatePassword (UserManager)");

            var user = this.GetUserByToken(token);

            if (user == null) return;

            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            user.IsLocked = false;
            user.FailedLoginAttempts = 0;
            user.UpdatedDate = DateTime.Now;
            user.UpdatedBy = updatedBy;

            this.Repository.Save(user);
        }

        /// <summary>
        /// Sends a password reminder to a user if their information is found asynchronously
        /// </summary>
        /// <param name="firstName">First Name of the User</param>
        /// <param name="lastName">Last Name of the User</param>
        /// <param name="emailAddress">Email Address of the User</param>
        /// <param name="passwordResetInHours">How long the password reset token is valid for</param>
        /// <param name="url">Url of the current server</param>
        public async Task SendReminderAsync(string firstName, string lastName, string emailAddress, int passwordResetInHours, string url)
        {
            LogMessage("SendReminderAsync (UserManager)");

            var user = this.Repository.GetByEmailAddress(emailAddress);

            if (user == null) return;
            
            var reminderText = this.SaveAndGetReminderText(user, passwordResetInHours);

            var reminderHtml = url + "app/email.templates/reminder.html";
            var reminderUrl = string.Format("{0}{1}{2}", url, "#/Account/PasswordReset?token=", reminderText);

            var html = WebHelper.GetHtml(reminderHtml);
            html = string.Format(html, reminderUrl, reminderUrl);

            EmailHelper.Send(user.EmailAddress, "FACT Accreditation Portal Password Reset", html);
            
        }

        /// <summary>
        /// Saves a reminder Text to the user and returns it
        /// </summary>
        /// <param name="user">User being updated</param>
        /// <param name="passwordResetInHours">How long the password reset token is valid for</param>
        /// <returns>Random Reminder Text String</returns>
        private string SaveAndGetReminderText(User user, int passwordResetInHours)
        {
            LogMessage("SaveAndGetReminderText (UserManager)");

            var reminderText = Guid.NewGuid().ToString();
            user.PasswordResetToken = reminderText;
            user.PasswordResetExpirationDate = DateTime.Now.AddHours(passwordResetInHours);
            user.UpdatedBy = "system";
            user.UpdatedDate = DateTime.Now;

            this.Repository.Save(user);

            return reminderText;
        }
        
        /// <summary>
        /// Gets a user by their reset token asynchronously
        /// </summary>
        /// <param name="token">Token for the user</param>
        /// <returns>User entity object</returns>
        public async Task<User> GetUserByTokenAsync(string token)
        {
            LogMessage("GetUserByTokenAsync (UserManager)");

            var user = await base.Repository.GetByTokenAsync(token);

            if (user == null || user.PasswordResetExpirationDate < DateTime.Now) return null;

            return user;
        }

        /// <summary>
        /// Gets a user by their reset token
        /// </summary>
        /// <param name="token">Token for the user</param>
        /// <returns>User entity object</returns>
        public User GetUserByToken(string token)
        {
            LogMessage("GetUserByToken (UserManager)");

            var user = base.Repository.GetByToken(token);

            if (user == null || user.PasswordResetExpirationDate < DateTime.Now) return null;

            return user;
        }

        public User GetFailedAttempts(string emailAddress)
        {
            LogMessage("GetFailedAttempts (UserManager)");

            var user = this.GetByEmailAddress(emailAddress);

            return user;
        }

        public void SetFailedAttempts(string emailAddress, int failedAttempts)
        {
            LogMessage("SetFailedAttempts (UserManager)");

            var user = this.GetByEmailAddress(emailAddress);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            else
            {
                user.FailedLoginAttempts = failedAttempts;

                user.IsLocked = failedAttempts >= 5;
                user.UpdatedDate = DateTime.Now;
                user.UpdatedBy = "Login";

                this.Repository.Save(user);
            }
        }
        /// <summary>
        /// Gets all Fact staff members
        /// </summary>
        /// <returns>Collection of fact staff members</returns>
        public List<User> GetFactStaff()
        {
            LogMessage("GetFactStaff (UserManager)");

            return base.Repository.GetFactStaff();
        }

        /// <summary>
        /// Gets all Fact staff members asynchronously
        /// </summary>
        /// <returns>Collection of fact staff members</returns>
        public Task<List<User>> GetFactStaffAsync()
        {
            LogMessage("GetFactStaffAsync (UserManager)");

            return base.Repository.GetFactStaffAsync();
        }

        public void SetAuditorObserver(Guid userId, bool isAuditor, bool isObserver, string savedBy)
        {
            LogMessage("SetAuditorObserver (UserManager)");

            var user = this.GetById(userId);

            if (user == null)
            {
                throw new Exception("Cannot find user");
            }

            user.IsAuditor = isAuditor;
            user.IsObserver = isObserver;
            user.UpdatedBy = savedBy;
            user.UpdatedDate = DateTime.Now;

            base.Repository.Save(user);
        }

        public async Task SetAuditorObserverAsync(Guid userId, bool isAuditor, bool isObserver, string savedBy)
        {
            LogMessage("SetAuditorObserverAsync (UserManager)");

            var user = this.GetById(userId);

            if (user == null)
            {
                throw new Exception("Cannot find user");
            }

            user.IsAuditor = isAuditor;
            user.IsObserver = isObserver;
            user.UpdatedBy = savedBy;
            user.UpdatedDate = DateTime.Now;

            await base.Repository.SaveAsync(user);
        }

        public List<User> GetAdmins()
        {
            return base.Repository.GetAdmins();
        }

        public Task<List<User>> GetAdminsAsync()
        {
            return base.Repository.GetAdminsAsnc();
        }

        public List<User> GetFactCoordinators()
        {
            return base.Repository.GetFactCoordinators();
        }

        public Task<List<User>> GetFactCoordinatorsAsnc()
        {
            return base.Repository.GetFactCoordinatorsAsnc();
        }

        public List<User> GetByRole(string roleName)
        {
            return base.Repository.GetByRole(roleName);
        }

        public Task<List<User>> GetByRoleAsync(string roleName)
        {
            return base.Repository.GetByRoleAsync(roleName);
        }

        public List<User> GetAuditorObservers()
        {
            return base.Repository.GetAuditorObservers();
        }

        public Task<List<User>> GetAuditorObserversAsync()
        {
            return base.Repository.GetAuditorObserversAsync();
        }

        public List<Personnel> GetPersonnel(int organizationId)
        {
            return base.Repository.GetPersonnel(organizationId);
        }

        public User GetAccreditationServicesSupervisor()
        {
            return base.Repository.GetAccreditationServicesSupervisor();
        }

        public List<UserItem> GetUsersByRole(string roleName)
        {
            return base.Repository.GetUsersByRole(roleName);
        }

        public List<UserItem> GetInspectorsWithinRadius(int siteId)
        {
            return base.Repository.GetInspectorsWithinRadius(siteId);
        }

        public void UpdatePersonnelByCoordinator(int orgId, Guid userId, bool showOnAccReport, string overrideJobFunction, string updatedBy)
        {
            this.Repository.UpdatePersonnelByCoordinator(orgId, userId, showOnAccReport, overrideJobFunction, updatedBy);
        }

        public UserItem CheckUserIsDirector(Guid userId)
        {
            return this.Repository.CheckUserIsDirector(userId);
        }
    }
}
