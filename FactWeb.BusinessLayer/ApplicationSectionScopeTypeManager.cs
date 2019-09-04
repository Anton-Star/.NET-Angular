using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ApplicationSectionScopeTypeManager : BaseManager<ApplicationSectionScopeTypeManager, IApplicationSectionScopeTypeRepository, ApplicationSectionScopeType>
    {
        public ApplicationSectionScopeTypeManager(IApplicationSectionScopeTypeRepository repository) : base(repository)
        {
        }

        public void UpdateApplicationSectionScopeType(ApplicationSectionItem applicationSectionItem, string updatedBy, Guid? applicationSectionId)
        {
            LogMessage("RemoveItems (ApplicationSectionScopeTypeManager)");

            if (applicationSectionItem.Id.HasValue)
            {
                var existingScopetypes = base.Repository.GetAllByApplicationSectionId(applicationSectionItem.Id);

                foreach (var item in existingScopetypes)
                {
                    base.BatchRemove(item);
                }

                foreach (var objectScopeType in applicationSectionItem.ScopeTypes)
                {
                    ApplicationSectionScopeType applicationSectionScopeType = new ApplicationSectionScopeType();
                    applicationSectionScopeType.Id = Guid.NewGuid();
                    applicationSectionScopeType.ScopeTypeId = objectScopeType.ScopeTypeId;
                    applicationSectionScopeType.ApplicationSectionId = applicationSectionItem.Id.GetValueOrDefault();
                    applicationSectionScopeType.IsActual = false;
                    applicationSectionScopeType.IsDefault = false;
                    applicationSectionScopeType.CreatedBy = updatedBy;
                    applicationSectionScopeType.CreatedDate = DateTime.Now;
                    applicationSectionScopeType.UpdatedBy = updatedBy;
                    applicationSectionScopeType.UpdatedDate = DateTime.Now;

                    base.BatchAdd(applicationSectionScopeType);
                }
                base.Repository.SaveChanges();
            }
            else
            {
                var scopeTypes = applicationSectionItem.ScopeTypes.Where(x => x.IsSelected == true).ToList();

                foreach (var objectScopeType in scopeTypes)
                {
                    ApplicationSectionScopeType applicationSectionScopeType = new ApplicationSectionScopeType();
                    applicationSectionScopeType.Id = Guid.NewGuid();
                    applicationSectionScopeType.ScopeTypeId = objectScopeType.ScopeTypeId;
                    applicationSectionScopeType.ApplicationSectionId = applicationSectionItem.Id.GetValueOrDefault();
                    applicationSectionScopeType.IsActual = false;
                    applicationSectionScopeType.IsDefault = false;
                    applicationSectionScopeType.CreatedBy = updatedBy;
                    applicationSectionScopeType.CreatedDate = DateTime.Now;
                    base.BatchAdd(applicationSectionScopeType);
                }
                base.Repository.SaveChanges();
            }
        }

        public async Task AddOrUpdateApplicationSectionScopeTypeAsync(Question question, string updatedBy)
        {
            LogMessage("AddOrUpdateAsync (AddOrUpdateApplicationSectionScopeTypeAsync)");

            await this.AddDefaultScopeTypeAsync(question, updatedBy);

            //if (question.Id.HasValue)
            //{
            //    //await this.UpdateDefaultScopeTypeAsync(question, updatedBy);
            //}
            //else
            //{
            //    await this.AddDefaultScopeTypeAsync(question, updatedBy);
            //}
        }

        public async Task AddDefaultScopeTypeAsync(Question question, string updatedBy)
        {

            var applicationSectionScopeTypeList = await base.Repository.GetAllByApplicationSectionIdAsync(question.SectionId);

            foreach (var scopeType in question.ScopeTypes)
            {
                var found = applicationSectionScopeTypeList.Where(x => x.ScopeTypeId == scopeType.ScopeTypeId).FirstOrDefault();

                if (found == null) // add new record in default scope
                {
                    var applicationSectionScopeType = new ApplicationSectionScopeType();
                    applicationSectionScopeType.Id = Guid.NewGuid();
                    applicationSectionScopeType.ApplicationSectionId = question.SectionId.GetValueOrDefault();
                    applicationSectionScopeType.ScopeTypeId = scopeType.ScopeTypeId;
                    applicationSectionScopeType.IsDefault = true;
                    applicationSectionScopeType.IsActual = true;
                    applicationSectionScopeType.CreatedDate = DateTime.Now;
                    applicationSectionScopeType.CreatedBy = updatedBy;

                    base.Add(applicationSectionScopeType);
                }
                else // update record to actual
                {
                    found.IsActual = true;
                    await base.SaveChangesAsync();
                }
            }
            
        }

        public async Task UpdateApplicationSectionScopeTypeAsync(ApplicationSectionItem applicationSectionItem, string updatedBy, Guid? applicationSectionId)
        {
            LogMessage("RemoveItems (ApplicationSectionScopeTypeManager)");

            if (applicationSectionItem.Id.HasValue)
            {
                var existingScopetypes = base.Repository.GetAllByApplicationSectionIdAsync(applicationSectionItem.Id).Result;

                foreach (var item in existingScopetypes)
                {
                    base.BatchRemove(item);
                }

                var scopeTypes = applicationSectionItem.ScopeTypes.Where(x => x.IsSelected == true).ToList();

                foreach (var objectScopeType in scopeTypes)
                {
                    ApplicationSectionScopeType applicationSectionScopeType = new ApplicationSectionScopeType();
                    applicationSectionScopeType.Id = Guid.NewGuid();
                    applicationSectionScopeType.ScopeTypeId = objectScopeType.ScopeTypeId;
                    applicationSectionScopeType.ApplicationSectionId = applicationSectionItem.Id.GetValueOrDefault();
                    applicationSectionScopeType.IsActual = false;
                    applicationSectionScopeType.IsDefault = true;
                    applicationSectionScopeType.CreatedBy = updatedBy;
                    applicationSectionScopeType.CreatedDate = DateTime.Now;
                    applicationSectionScopeType.UpdatedBy = updatedBy;
                    applicationSectionScopeType.UpdatedDate = DateTime.Now;

                    base.BatchAdd(applicationSectionScopeType);
                }
                await base.Repository.SaveChangesAsync();
            }
            else
            {
                var scopeTypes = applicationSectionItem.ScopeTypes.Where(x => x.IsSelected == true).ToList();

                foreach (var objectScopeType in scopeTypes)
                {
                    ApplicationSectionScopeType applicationSectionScopeType = new ApplicationSectionScopeType();
                    applicationSectionScopeType.Id = Guid.NewGuid();
                    applicationSectionScopeType.ScopeTypeId = objectScopeType.ScopeTypeId;
                    applicationSectionScopeType.ApplicationSectionId = applicationSectionId.GetValueOrDefault();
                    applicationSectionScopeType.IsActual = false;
                    applicationSectionScopeType.IsDefault = true;
                    applicationSectionScopeType.CreatedBy = updatedBy;
                    applicationSectionScopeType.CreatedDate = DateTime.Now;
                    base.BatchAdd(applicationSectionScopeType);
                }
                await base.Repository.SaveChangesAsync();
            }
        }
    }
}
