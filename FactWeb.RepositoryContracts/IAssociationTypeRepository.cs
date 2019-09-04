using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IAssociationTypeRepository : IRepository<AssociationType>
    {
        AssociationType GetByName(string name);
        Task<AssociationType> GetByNameAsync(string name);
    }
}
