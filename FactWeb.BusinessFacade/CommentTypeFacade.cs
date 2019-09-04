using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactWeb.BusinessFacade
{
    public class CommentTypeFacade
    {
        private readonly Container container;

        public CommentTypeFacade(Container container)
        {
            this.container = container;
        }

        public async Task<List<CommentType>> GetAllAsync()
        {
            var commentTypeManager = this.container.GetInstance<CommentTypeManager>();

            return await commentTypeManager.GetAllAsync();
        }

        public List<CommentType> GetAll()
        {
            var commentTypeManager = this.container.GetInstance<CommentTypeManager>();

            return commentTypeManager.GetAll();
        }

        public CommentType GetByName(string commentTypeName)
        {
            var commentTypeManager = this.container.GetInstance<CommentTypeManager>();

            return commentTypeManager.GetByName(commentTypeName);
        }

        public async Task<CommentType> GetByNameAsync(string commentTypeName)
        {
            var commentTypeManager = this.container.GetInstance<CommentTypeManager>();

            return await commentTypeManager.GetByNameAsync(commentTypeName);
        }



    }
}
