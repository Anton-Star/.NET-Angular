using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;

namespace FactWeb.BusinessFacade
{
    public class CpiTypeFacade
    {
        private readonly Container container;

        public CpiTypeFacade(Container container)
        {
            this.container = container;
        }

        public List<CpiType> GetAll()
        {
            var manager = this.container.GetInstance<CpiTypeManager>();

            return manager.GetAll();
        }
    }
}
