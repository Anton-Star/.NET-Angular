using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class TemplateManager : BaseManager<TemplateManager, ITemplateRepository, Template>
    {
        public TemplateManager(ITemplateRepository repository) : base(repository)
        {
        }

        public Template GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public async Task<Template> GetByNameAsync(string name)
        {
            return await base.Repository.GetByNameAsync(name);
        }

        public Template Add(string name, string text, string createdBy)
        {
            var temp = this.GetByName(name);

            if (temp != null)
            {
                throw new Exception($"{name} already exists.");
            }

            var template = new Template
            {
                Id = Guid.NewGuid(),
                Name = name,
                Text = text,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy
            };

            base.Repository.Add(template);

            return template;
        }

        public async Task<Template> AddAsync(string name, string text, string createdBy)
        {
            var temp = await this.GetByNameAsync(name);

            if (temp != null)
            {
                throw new Exception($"{name} already exists");
            }

            var template = new Template
            {
                Id = Guid.NewGuid(),
                Name = name,
                Text = text,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy
            };

            await base.Repository.AddAsync(template);

            return template;
        }

        public void Update(Guid id, string name, string text, string updatedBy)
        {
            var template = this.GetById(id);

            if (template == null)
            {
                throw new Exception("Unable to find Template");
            }


            var nameCheck = this.GetByName(name);

            if (nameCheck != null && nameCheck.Id != id)
            {
                throw new Exception($"{name} already exists");
            }

            template.Name = name;
            template.Text = text;
            template.UpdatedBy = updatedBy;
            template.UpdatedDate = DateTime.Now;

            base.Repository.Save(template);
        }

        public async Task UpdateAsync(Guid id, string name, string text, string updatedBy)
        {
            var template = this.GetById(id);

            if (template == null)
            {
                throw new Exception("Unable to find Template");
            }


            var nameCheck = await this.GetByNameAsync(name);

            if (nameCheck != null && nameCheck.Id != id)
            {
                throw new Exception($"{name} already exists");
            }

            template.Name = name;
            template.Text = text;
            template.UpdatedBy = updatedBy;
            template.UpdatedDate = DateTime.Now;

            await base.Repository.SaveAsync(template);
        }
    }
}
