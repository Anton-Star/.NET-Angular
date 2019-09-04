using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class TemplateRepository : BaseRepository<Template>, ITemplateRepository
    {
        public TemplateRepository(FactWebContext context) : base(context)
        {
        }

        public Template GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<Template> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
