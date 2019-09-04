using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ITemplateRepository : IRepository<Template>
    {
        Template GetByName(string name);
        Task<Template> GetByNameAsync(string name);
    }
}
