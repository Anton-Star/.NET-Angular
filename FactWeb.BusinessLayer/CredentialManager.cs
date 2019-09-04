using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class CredentialManager : BaseManager<CredentialManager, ICredentialRepository, Credential>
    {
        public CredentialManager(ICredentialRepository repository) : base(repository)
        {
        }
    }
}
