using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class UserJobFunctionManager : BaseManager<UserJobFunctionManager, IUserJobFunctionRepository, UserJobFunction>
    {
        public UserJobFunctionManager(IUserJobFunctionRepository repository) : base(repository)
        {
        }

        public void AddJobFunction(UserItem userItem, string createdBy)
        {
            LogMessage("AddJobFunction (JobFunctionManager)");

            var newJobFunctions = userItem.JobFunctions.Where(x => x.selected == true).ToList();

            foreach (var objJobFunction in newJobFunctions)
            {
                UserJobFunction userJobFunction = new UserJobFunction();
                //userJobFunction.Id = Guid.NewGuid();
                userJobFunction.JobFunctionId = objJobFunction.Id;
                userJobFunction.UserId = userItem.UserId.GetValueOrDefault();
                userJobFunction.CreatedBy = createdBy;
                userJobFunction.CreatedDate = DateTime.Now;

                base.BatchAdd(userJobFunction);
            }

            base.Repository.SaveChanges();
        }

        public async Task AddJobFunctionAsync(UserItem userItem, string createdBy)
        {
            LogMessage("AddJobFunctionAsync (JobFunctionManager)");

            var newJobFunctions = userItem.JobFunctions.Where(x => x.selected == true).ToList();

            foreach (var objJobFunction in newJobFunctions)
            {
                UserJobFunction userJobFunction = new UserJobFunction();
                //userJobFunction.Id = Guid.NewGuid();
                userJobFunction.JobFunctionId = objJobFunction.Id;
                userJobFunction.UserId = userItem.UserId.GetValueOrDefault();
                userJobFunction.CreatedBy = createdBy;
                userJobFunction.CreatedDate = DateTime.Now;

                base.BatchAdd(userJobFunction);
            }

            await base.Repository.SaveChangesAsync();
        }

        public void UpdateJobFunction(UserItem userItem, string updatedBy)
        {
            LogMessage("UpdateJobFunction (JobFunctionManager)");

            var existingJobFunctions = base.Repository.GetAllByUserId(userItem.UserId);

            foreach (var item in existingJobFunctions)
            {
                base.BatchRemove(item);
            }

            var newJobFunctions = userItem.JobFunctions.Where(x => x.selected == true).ToList();

            foreach (var objJobFunction in newJobFunctions)
            {
                UserJobFunction userJobFunction = new UserJobFunction();
                //userJobFunction.Id = Guid.NewGuid();
                userJobFunction.JobFunctionId = objJobFunction.Id;
                userJobFunction.UserId = userItem.UserId.GetValueOrDefault();
                userJobFunction.CreatedBy = updatedBy;
                userJobFunction.CreatedDate = DateTime.Now;
                userJobFunction.UpdatedBy = updatedBy;
                userJobFunction.UpdatedDate = DateTime.Now;

                base.BatchAdd(userJobFunction);
            }

            base.Repository.SaveChanges();
        }

        public async Task UpdateJobFunctionAsync(UserItem userItem, string updatedBy)
        {
            LogMessage("UpdateJobFunctionAsync (JobFunctionManager)");

            var existingJobFunctions = base.Repository.GetAllByUserId(userItem.UserId);

            foreach (var item in existingJobFunctions)
            {
                base.BatchRemove(item);
            }

            var newJobFunctions = userItem.JobFunctions.Where(x => x.selected == true).ToList();

            foreach (var objJobFunction in newJobFunctions)
            {
                UserJobFunction userJobFunction = new UserJobFunction();
                //userJobFunction.Id = Guid.NewGuid();
                userJobFunction.JobFunctionId = objJobFunction.Id;
                userJobFunction.UserId = userItem.UserId.GetValueOrDefault();
                userJobFunction.CreatedBy = updatedBy;
                userJobFunction.CreatedDate = DateTime.Now;
                userJobFunction.UpdatedBy = updatedBy;
                userJobFunction.UpdatedDate = DateTime.Now;

                base.BatchAdd(userJobFunction);
            }

            await base.Repository.SaveChangesAsync();
        }

    }
}
