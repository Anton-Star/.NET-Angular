using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class StateManager : BaseManager<StateManager, IStateRepository, State>
    {
        public StateManager(IStateRepository repository) : base(repository)
        {
        }
    }
}
