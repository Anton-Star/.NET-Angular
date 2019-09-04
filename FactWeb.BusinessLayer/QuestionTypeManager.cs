using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class QuestionTypeManager : BaseManager<QuestionTypeManager, IQuestionTypeRepository, QuestionType>
    {
        public QuestionTypeManager(IQuestionTypeRepository repository) : base(repository)
        {
        }

        public QuestionType GetByName(string name)
        {
            LogMessage("GetByName (QuestionTypeManager)");

            return base.Repository.GetByName(name);
        }

        public Task<QuestionType> GetByNameAsync(string name)
        {
            LogMessage("GetByNameAsync (QuestionTypeManager)");

            return base.Repository.GetByNameAsync(name);
        }

    }
}
