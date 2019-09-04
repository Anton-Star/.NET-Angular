using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class CredentialFacade
    {
        readonly Container container;

        public CredentialFacade(Container container)
        {
            this.container = container;
        }

        public List<Credential> GetAll()
        {
            var manager = this.container.GetInstance<CredentialManager>();

            return manager.GetAll();
        }

        public Task<List<Credential>> GetAllAsync()
        {
            var manager = this.container.GetInstance<CredentialManager>();

            return manager.GetAllAsync();
        }
    }
}
