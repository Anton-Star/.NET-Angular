using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class MembershipManager : BaseManager<MembershipManager, IMembershipRepository, Membership>
    {
        public MembershipManager(IMembershipRepository repository) : base(repository)
        {
        }
    }
}
