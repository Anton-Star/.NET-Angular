using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface ICommentTypeRepository : IRepository<CommentType>
    {
        /// <summary>
        /// Gets an Application Type by name
        /// </summary>
        /// <param name="name">Name of the comment type</param>
        /// <returns>Application Type object</returns>
        CommentType GetByName(string name);
        /// <summary>
        /// Gets an Application Type by name async
        /// </summary>
        /// <param name="name">Name of the comment type</param>
        /// <returns>Application Type object</returns>
        Task<CommentType> GetByNameAsync(string name);
    }
}
