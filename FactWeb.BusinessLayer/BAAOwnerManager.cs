using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class BAAOwnerManager : BaseManager<BAAOwnerManager, IBAAOwnerRepository, BAAOwner>
    {
        public BAAOwnerManager(IBAAOwnerRepository repository) : base(repository)
        {
        }
    }
}
