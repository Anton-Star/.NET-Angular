using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class CBCollectionTypeManager : BaseManager<CBCollectionTypeManager, ICBCollectionTypeRepository, CBCollectionType>
    {
        public CBCollectionTypeManager(ICBCollectionTypeRepository repository) : base(repository)
        {
        }
    }
}
