using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class UserCredentialManager : BaseManager<UserCredentialManager, IUserCredentialRepository, UserCredential>
    {
        public UserCredentialManager(IUserCredentialRepository repository) : base(repository)
        {
        }

        public void AddUserCredential(Guid userId, List<CredentialItem> credentials, string createdBy)
        {
            LogMessage("AddUserCredential (UserCredentialManager)");

            foreach (var objCredential in credentials)
            {
                UserCredential userCredential = new UserCredential();
                userCredential.Id = Guid.NewGuid();
                userCredential.CredentialId = objCredential.Id;
                userCredential.UserId = userId;
                userCredential.CreatedBy = createdBy;
                userCredential.CreatedDate = DateTime.Now;

                base.BatchAdd(userCredential);
            }

            base.Repository.SaveChanges();
        }

        public async Task AddUserCredentialAsync(Guid userId, List<CredentialItem> credentials, string createdBy)
        {
            LogMessage("AddUserCredentialAsync (UserCredentialManager)");

            foreach (var objCredential in credentials)
            {
                UserCredential userCredential = new UserCredential();
                userCredential.Id = Guid.NewGuid();
                userCredential.CredentialId = objCredential.Id;
                userCredential.UserId = userId;
                userCredential.CreatedBy = createdBy;
                userCredential.CreatedDate = DateTime.Now;

                base.BatchAdd(userCredential);
            }

            await base.Repository.SaveChangesAsync();
        }

        public void UpdateUserCredential(Guid userId, List<CredentialItem> credentials, string updatedBy)
        {
            LogMessage("UpdateUserCredential (UserCredentialManager)");

            var existingCredentials = base.Repository.GetAllByUserId(userId);

            foreach (var item in existingCredentials)
            {
                base.BatchRemove(item);
            }

            foreach (var objCredential in credentials)
            {
                UserCredential userCredential = new UserCredential();
                userCredential.Id = Guid.NewGuid();
                userCredential.CredentialId = objCredential.Id;
                userCredential.UserId = userId;
                userCredential.CreatedBy = updatedBy;
                userCredential.CreatedDate = DateTime.Now;
                userCredential.UpdatedBy = updatedBy;
                userCredential.UpdatedDate = DateTime.Now;

                base.BatchAdd(userCredential);
            }

            base.Repository.SaveChanges();
        }

        public async Task UpdateUserCredentialAsync(Guid userId, List<CredentialItem> credentials, string updatedBy)
        {
            LogMessage("UpdateUserCredentialAsync (UserCredentialManager)");

            var existingCredentials = base.Repository.GetAllByUserId(userId);

            foreach (var item in existingCredentials)
            {
                base.BatchRemove(item);
            }

            foreach (var objCredential in credentials)
            {
                UserCredential userCredential = new UserCredential();
                userCredential.Id = Guid.NewGuid();
                userCredential.CredentialId = objCredential.Id;
                userCredential.UserId = userId;
                userCredential.CreatedBy = updatedBy;
                userCredential.CreatedDate = DateTime.Now;
                userCredential.UpdatedBy = updatedBy;
                userCredential.UpdatedDate = DateTime.Now;

                base.BatchAdd(userCredential);
            }

            await base.Repository.SaveChangesAsync();
        }
    }
}
