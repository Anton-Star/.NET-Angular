using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class ApplicationSettingRepository : BaseRepository<ApplicationSetting>, IApplicationSettingRepository
    {
        public ApplicationSettingRepository(FactWebContext context) : base(context)
        {
        }

        public ApplicationSetting GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<ApplicationSetting> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
