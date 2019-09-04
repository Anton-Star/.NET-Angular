using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ICBCategoryRepository : IRepository<CBCategory>
    {
        CBCategory GetByName(string name);
        Task<CBCategory> GetBynameAsync(string name);
    }
}
