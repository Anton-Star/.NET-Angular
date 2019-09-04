using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(FactWebContext context) : base(context)
        {
        }

        public override User GetById(Guid id)
        {
            return base.Context.Users
                .Include(x => x.Role)
                .Include(x => x.UserJobFunctions)
                .Include(x => x.UserLanguages)
                .Include(x => x.UserCredentials)
                .Include(x=>x.UserCredentials.Select(y=>y.Credential))
                .Include(x => x.UserMemberships)
                .Include(x => x.Organizations)
                .Include(x => x.Organizations.Select(y => y.JobFunction))
                .Include(x => x.Organizations.Select(y => y.Organization))
                .Include(x=>x.OrganizationConsutants)
                .SingleOrDefault(x=>x.Id== id);
        }

        public void Remove(Guid id)
        {
            this.Remove(this.GetById(id));
        }

        public List<User> GetAllUsers()
        {
            return base.Context.Users
                .Include(x => x.Role)
                .Include(x => x.UserJobFunctions.Select(y=>y.JobFunction))
                .Include(x => x.UserLanguages)
                .Include(x => x.UserCredentials.Select(y => y.Credential))
                .Include(x => x.UserMemberships.Select(y=>y.Membership))
                .Include(x => x.UserLanguages.Select(y => y.Language))
                .Include(x => x.Organizations)
                .Include(x => x.Organizations.Select(y => y.JobFunction))
                .Include(x => x.Organizations.Select(y => y.Organization))
                .ToList();
        }

        public List<User> GetUsersByOrgs(List<int> organizations)
        {
            return base.Context.Users
                .Include(x => x.Role)
                .Include(x => x.UserJobFunctions)
                .Include(x => x.UserLanguages)
                .Include(x => x.UserCredentials)
                .Include(x => x.UserMemberships)
                .Include(x => x.Organizations)
                .Include(x => x.Organizations.Select(y => y.JobFunction))
                .Include(x => x.Organizations.Select(y => y.Organization))
                .Include(x => x.Organizations.Select(y => y.Organization.OrganizationType))
                .Where(x => x.Organizations.Any(y => organizations.Any(z => z == y.OrganizationId)))
                .ToList();
        }

        public User GetByEmailAddress(string emailAddress)
        {
            return base.Context.Users
                .Include(x => x.Role)
                .Include(x => x.UserJobFunctions)
                .Include(x => x.UserLanguages)
                .Include(x => x.UserCredentials)
                .Include(x => x.UserMemberships)
                .Include(x => x.Organizations)
                .Include(x => x.Organizations.Select(y => y.JobFunction))
                .Include(x => x.Organizations.Select(y => y.Organization))
                .Include(x => x.Organizations.Select(y => y.Organization.OrganizationType))
                .SingleOrDefault(x => x.EmailAddress == emailAddress);
        }

        public User GetByToken(string token)
        {
            return base.Fetch(x => x.PasswordResetToken == token);
        }

        public Task<User> GetByTokenAsync(string token)
        {
            return base.FetchAsync(x => x.PasswordResetToken == token);
        }

        public User GetByFirstNameLastNameOrEmailAddress(string firstName, string lastName, string emailAddress)
        {
            return base.Fetch(x => (x.FirstName == firstName && x.LastName == lastName) || x.EmailAddress == emailAddress);
        }

        public Task<User> GetByFirstNameLastNameOrEmailAddressAsync(string firstName, string lastName, string emailAddress)
        {
            return base.FetchAsync(x => (x.FirstName == firstName && x.LastName == lastName) || x.EmailAddress == emailAddress);
        }

        public Task<List<User>> GetByOrganizationAsync(int organizationId)
        {
            return base.FetchManyAsync(x => x.Organizations.Any(y => y.OrganizationId == organizationId));
        }

        public List<User> GetByOrganization(int organizationId)
        {
            return base.FetchMany(x => x.Organizations.Any(y => y.OrganizationId == organizationId));
        }

        public User GetByOrganizationAndEmailAddress(int organizationId, string emailAddress)
        {
            return base.Fetch(x => x.Organizations.Any(y => y.OrganizationId == organizationId) && x.EmailAddress == emailAddress);
        }

        public Task<User> GetByOrganizationAndEmailAddressAsync(int organizationId, string emailAddress)
        {
            return base.FetchAsync(x => x.Organizations.Any(y => y.OrganizationId == organizationId) && x.EmailAddress == emailAddress);
        }

        public List<User> GetFactStaff()
        {
            return
                base.FetchMany(
                    x =>
                        x.Role.Name == Constants.Roles.FACTAdministrator ||
                        x.Role.Name == Constants.Roles.QualityManager ||
                        x.Role.Name == Constants.Roles.FACTCoordinator);
        }

        public Task<List<User>> GetFactStaffAsync()
        {
            return
                base.FetchManyAsync(
                    x =>
                        x.Role.Name == Constants.Roles.FACTAdministrator ||
                        x.Role.Name == Constants.Roles.QualityManager ||
                        x.Role.Name == Constants.Roles.FACTCoordinator);
        }

        public List<User> GetAuditorsAndObservers()
        {
            return base.FetchMany(x => x.IsAuditor == true || x.IsObserver == true && x.IsActive == true);

        }
        public Task<List<User>> GetAuditorsAndObserversAsync()
        {
            return base.FetchManyAsync(x => x.IsAuditor == true || x.IsObserver == true && x.IsActive == true);
        }

        public List<User> GetAdmins()
        {
            return base.FetchMany(x => x.Role.Name == Constants.Roles.FACTAdministrator && x.IsActive);
        }

        public Task<List<User>> GetAdminsAsnc()
        {
            return base.FetchManyAsync(x => x.Role.Name == Constants.Roles.FACTAdministrator);
        }

        public List<User> GetFactCoordinators()
        {
            return base.FetchMany(x => x.Role.Name == Constants.Roles.FACTCoordinator);
        }

        public Task<List<User>> GetFactCoordinatorsAsnc()
        {
            return base.FetchManyAsync(x => x.Role.Name == Constants.Roles.FACTCoordinator);
        }

        public List<User> GetByRole(string roleName)
        {
            return base.FetchMany(x => x.Role.Name == roleName);
        }

        public Task<List<User>> GetByRoleAsync(string roleName)
        {
            return base.FetchManyAsync(x => x.Role.Name == roleName);
        }

        public List<User> GetAuditorObservers()
        {
            return base.FetchMany(x => x.IsAuditor.GetValueOrDefault() || x.IsObserver.GetValueOrDefault());
        }

        public Task<List<User>> GetAuditorObserversAsync()
        {
            return base.FetchManyAsync(x => x.IsAuditor.GetValueOrDefault() || x.IsObserver.GetValueOrDefault());
        }

        public List<Personnel> GetPersonnel(int organizationId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = organizationId;

            var data = objectContext.ExecuteStoreQuery<Personnel>(
                "EXEC usp_inspectionSummaryPersonnelByOrg @organizationId={0}", paramList).ToList();

            return data;
        }

        public User GetAccreditationServicesSupervisor()
        {
            return base.Fetch(x => x.Title == "Accreditation Services Supervisor");
        }

        public List<UserItem> GetUsersByRole(string roleName)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = roleName;

            var data = objectContext.ExecuteStoreQuery<UserItem>(
                "EXEC usp_getUsersByRole @roleName={0}", paramList).ToList();

            return data;
        }

        public List<UserItem> GetInspectorsWithinRadius(int siteId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = siteId;

            var data = objectContext.ExecuteStoreQuery<UserItem>(
                "EXEC usp_getInspectorsWithinRadius @siteId={0}", paramList).ToList();

            return data;
        }

        public void UpdatePersonnelByCoordinator(int orgId, Guid userId, bool showOnAccReport, string overrideJobFunction, string updatedBy)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[5];

            paramList[0] = orgId;
            paramList[1] = userId;
            paramList[2] = showOnAccReport;
            paramList[3] = overrideJobFunction;
            paramList[4] = updatedBy;


            objectContext.ExecuteStoreCommand(
                "EXEC usp_updatePersonnelByCoordinator @organizationId={0}, @userId={1}, @saveOnAccReport={2}, @OverrideJobFunction={3}, @updatedBy={4}",
                paramList);
            
        }

        public UserItem CheckUserIsDirector(Guid userId)
        {
            //

            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = userId;

            var data = objectContext.ExecuteStoreQuery<UserItem>(
                "EXEC usp_CheckUserIsDirector @UserId={0}", paramList).FirstOrDefault();

            return data;
        }
    }
}
