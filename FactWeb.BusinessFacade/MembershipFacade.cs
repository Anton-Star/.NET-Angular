using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class MembershipFacade
    {
        private readonly Container container;

        public MembershipFacade(Container container)
        {
            this.container = container;
        }

        public List<Membership> GetAll()
        {
            var manager = this.container.GetInstance<MembershipManager>();

            return manager.GetAll();
        }

        public Task<List<Membership>> GetAllAsync()
        {
            var manager = this.container.GetInstance<MembershipManager>();

            return manager.GetAllAsync();
        }
    }
}
