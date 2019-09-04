using FactWeb.BusinessLayer;
using FactWeb.Model;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class TemplateFacade
    {
        private readonly Container container;

        public TemplateFacade(Container container)
        {
            this.container = container;
        }

        public async Task<List<Template>> GetAllAsync()
        {
            var manager = this.container.GetInstance<TemplateManager>();

            return await manager.GetAllAsync();
        }

        public List<Template> GetAll()
        {
            var manager = this.container.GetInstance<TemplateManager>();

            return manager.GetAll();
        }

        public Template Add(string name, string text, string createdBy)
        {
            var manager = this.container.GetInstance<TemplateManager>();

            return manager.Add(name, text, createdBy);
        }

        public async Task<Template> AddAsync(string name, string text, string createdBy)
        {
            var manager = this.container.GetInstance<TemplateManager>();

            return await manager.AddAsync(name, text, createdBy);
        }

        public void Update(Guid id, string name, string text, string updatedBy)
        {
            var manager = this.container.GetInstance<TemplateManager>();

            manager.Update(id, name, text, updatedBy);
        }

        public async Task UpdateAsync(Guid id, string name, string text, string updatedBy)
        {
            var manager = this.container.GetInstance<TemplateManager>();

            await manager.UpdateAsync(id, name, text, updatedBy);
        }

        public void Remove(Guid id)
        {
            var manager = this.container.GetInstance<TemplateManager>();

            manager.Remove(id);
        }

        public async Task RemoveAsync(Guid id)
        {
            var manager = this.container.GetInstance<TemplateManager>();

            await manager.RemoveAsync(id);
        }
    }
}
