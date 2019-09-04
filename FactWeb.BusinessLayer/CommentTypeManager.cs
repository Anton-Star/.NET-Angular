using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class CommentTypeManager : BaseManager<CommentTypeManager, ICommentTypeRepository, CommentType>
    {
        public CommentTypeManager(ICommentTypeRepository repository) : base(repository)
        {
        }

        public CommentType GetByName(string name)
        {
            return base.Repository.GetByName(name);
        }

        public Task<CommentType> GetByNameAsync(string name)
        {
            return base.Repository.GetByNameAsync(name);
        }
    }
}

