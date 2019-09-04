using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class LanguageFacade
    {
        readonly Container container;

        public LanguageFacade(Container container)
        {
            this.container = container;
        }

        public List<Language> GetAll()
        {
            var manager = this.container.GetInstance<LanguageManager>();

            return manager.GetAll();
        }

        public Task<List<Language>> GetAllAsync()
        {
            var manager = this.container.GetInstance<LanguageManager>();

            return manager.GetAllAsync();
        }
    }
}
