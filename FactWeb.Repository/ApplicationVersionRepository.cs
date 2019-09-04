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
    public class ApplicationVersionRepository : BaseRepository<ApplicationVersion>, IApplicationVersionRepository
    {
        public ApplicationVersionRepository(FactWebContext context) : base(context)
        {
        }

        public List<ApplicationVersion> GetByType(int applicationTypeId)
        {
            return base.FetchMany(x => x.ApplicationTypeId == applicationTypeId);
        }

        public Task<List<ApplicationVersion>> GetByTypeAsync(int applicationTypeId)
        {
            return base.FetchManyAsync(x => x.ApplicationTypeId == applicationTypeId);
        }

        public List<ApplicationVersion> GetByType(string applicationTypeName)
        {
            //base.Context.Configuration.LazyLoadingEnabled = false;

            var versions = base.Context.ApplicationVersions
                .Where(x => x.ApplicationType.Name == applicationTypeName && (x.IsDeleted == null || x.IsDeleted == false))
                .Include(x => x.ApplicationType)
                .Include(x=>x.ApplicationSections)
                .ToList();

            return versions;
        }

        public Task<List<ApplicationVersion>> GetByTypeAsync(string applicationTypeName)
        {
            return base.FetchManyAsync(x => x.ApplicationType.Name == applicationTypeName);
        }

        public List<ApplicationVersion> GetActive()
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            var versions = base.Context.ApplicationVersions
                .AsNoTracking()
                .Where(x => x.IsActive && (x.IsDeleted == null || x.IsDeleted == false))
                .Include(x => x.ApplicationType)
                .ToList();

            return versions;
        }

        public ApplicationVersion GetVersion(Guid versionId)
        {
            base.Context.Configuration.LazyLoadingEnabled = false;

            var versions = base.Context.ApplicationVersions
                .AsNoTracking()
                .Include(x => x.ApplicationType)
                .SingleOrDefault(x => x.Id == versionId);

            return versions;
        }

        public Task<List<ApplicationVersion>> GetActiveAsync()
        {
            return base.FetchManyAsync(x => x.IsActive);
        }

        public List<FlatApplication> GetFlatApplication(Guid versionId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = versionId;

            var data = objectContext.ExecuteStoreQuery<FlatApplication>(
                "EXEC usp_getApplicationQuestions @applicationVersionId={0}", paramList).ToList();

            return data;
        }

        public async Task<List<FlatApplication>> GetFlatApplicationAsync(Guid versionId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = versionId;

            var data = await objectContext.ExecuteStoreQueryAsync<FlatApplication>(
                "EXEC usp_getApplicationQuestions @applicationVersionId={0}", paramList);

            return data.ToList();
        }

        public ApplicationVersion GetByApplicationSection(Guid applicationSectionId)
        {
            return base.Fetch(x => x.ApplicationSections.Any(y => y.Id == applicationSectionId));
        }

        public Task<ApplicationVersion> GetByApplicationSectionAsync(Guid applicationSectionId)
        {
            return base.FetchAsync(x => x.ApplicationSections.Any(y => y.Id == applicationSectionId));
        }

        public List<ExportModel> Export(Guid applicationVersionId)
        {
            var objectContext = ((IObjectContextAdapter)Context).ObjectContext;

            var paramList = new Object[1];

            paramList[0] = applicationVersionId;

            var data = objectContext.ExecuteStoreQuery<ExportModel>(
                "EXEC usp_exportVersion @applicationVersionId={0}", paramList);

            return data.ToList();
        }
    }
}

