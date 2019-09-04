using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Gets a user by id
        /// </summary>
        /// <param name="id">Id of the User</param>
        /// <returns></returns>
        User GetById(Guid id);

        List<User> GetAllUsers();
        List<User> GetUsersByOrgs(List<int> organizations);

        /// <summary>
        /// Removes a user by Id
        /// </summary>
        /// <param name="id">Id of the user</param>
        void Remove(Guid id);

        /// <summary>
        /// Gets a user based on email address
        /// </summary>
        /// <param name="emailAddress">Email Address of the user</param>
        /// <returns>User entity object</returns>
        User GetByEmailAddress(string emailAddress);

        /// <summary>
        /// Gets a user by their reset token
        /// </summary>
        /// <param name="token">Token for the user</param>
        /// <returns>User entity object</returns>
        User GetByToken(string token);

        /// <summary>
        /// Gets a user by their reset token asynchronously
        /// </summary>
        /// <param name="token">Token for the user</param>
        /// <returns>User entity object</returns>
        Task<User> GetByTokenAsync(string token);

        /// <summary>
        /// Gets a user based on email address 
        /// </summary>
        /// <param name="firstName">First Name of the user</param>
        /// <param name="lastName">Last Name of the user</param>
        /// <param name="emailAddress">Email Address of the user</param>
        /// <returns>User entity object</returns>
        User GetByFirstNameLastNameOrEmailAddress(string firstName, string lastName, string emailAddress);

        /// <summary>
        /// Gets a user based on email address asynchronously
        /// </summary>
        /// <param name="firstName">First Name of the user</param>
        /// <param name="lastName">Last Name of the user</param>
        /// <param name="emailAddress">Email Address of the user</param>
        /// <returns>User entity object</returns>
        Task<User> GetByFirstNameLastNameOrEmailAddressAsync(string firstName, string lastName, string emailAddress);

        /// <summary>
        /// Gets a list of users by organization asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Asynchronous task with a collection of User entity objects</returns>
        Task<List<User>> GetByOrganizationAsync(int organizationId);

        /// <summary>
        /// Gets a list of users by organization
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <returns>Collection of User entity objects</returns>
        List<User> GetByOrganization(int organizationId);

        /// <summary>
        /// Gets a user based on organization and email address
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="emailAddress">Email Address of the user</param>
        /// <returns>User entity object</returns>
        User GetByOrganizationAndEmailAddress(int organizationId, string emailAddress);

        /// <summary>
        /// Gets a user based on organization and email address asynchronously
        /// </summary>
        /// <param name="organizationId">Id of the organization</param>
        /// <param name="emailAddress">Email Address of the user</param>
        /// <returns>User entity object</returns>
        Task<User> GetByOrganizationAndEmailAddressAsync(int organizationId, string emailAddress);
        /// <summary>
        /// Gets a collection of all fact staff members
        /// </summary>
        /// <returns>Collection of fact staff members</returns>
        List<User> GetFactStaff();
        /// <summary>
        /// Gets a collection of all fact staff members asynchronously
        /// </summary>
        /// <returns>Collection of fact staff members</returns>
        Task<List<User>> GetFactStaffAsync();

        /// <summary>
        /// Get all fact coordinators 
        /// </summary>
        /// <returns></returns>
        List<User> GetFactCoordinators();

        /// <summary>
        /// Get all fact coordinators asynchronously
        /// </summary>
        /// <returns></returns>
        Task<List<User>> GetFactCoordinatorsAsnc();

        List<User> GetAuditorsAndObservers();
        Task<List<User>> GetAuditorsAndObserversAsync();

        List<User> GetAdmins();
        Task<List<User>> GetAdminsAsnc();

        List<User> GetByRole(string roleName);
        Task<List<User>> GetByRoleAsync(string roleName);

        List<User> GetAuditorObservers();
        Task<List<User>> GetAuditorObserversAsync();

        List<Personnel> GetPersonnel(int organizationId);

        User GetAccreditationServicesSupervisor();
        List<UserItem> GetUsersByRole(string roleName);
        List<UserItem> GetInspectorsWithinRadius(int siteId);

        void UpdatePersonnelByCoordinator(int orgId, Guid userId, bool showOnAccReport, string overrideJobFunction, string updatedBy);

        UserItem CheckUserIsDirector(Guid userId);
    }
}
