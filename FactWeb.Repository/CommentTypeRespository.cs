using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class CommentTypeRespository : BaseRepository<CommentType>, ICommentTypeRepository
    {
        public CommentTypeRespository(FactWebContext context) : base(context)
        {
        }

        public CommentType GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<CommentType> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
