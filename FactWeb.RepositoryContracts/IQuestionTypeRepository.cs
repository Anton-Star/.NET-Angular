using FactWeb.Model;
using System.Threading.Tasks;

namespace FactWeb.RepositoryContracts
{
    public interface IQuestionTypeRepository : IRepository<QuestionType>
    {
        QuestionType GetByName(string name);
        Task<QuestionType> GetByNameAsync(string name);
    }
}
