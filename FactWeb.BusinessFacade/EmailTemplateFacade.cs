using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class EmailTemplateFacade
    {
        readonly Container container;

        public EmailTemplateFacade(Container container)
        {
            this.container = container;
        }

        public List<EmailTemplate> GetAll()
        {
            var manager = this.container.GetInstance<EmailTemplateManager>();

            return manager.GetAll();
        }

        public Task<List<EmailTemplate>> GetAllAsync()
        {
            var manager = this.container.GetInstance<EmailTemplateManager>();

            return manager.GetAllAsync();
        }
    }
}
