using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System.Threading.Tasks;

namespace FactWeb.Repository
{
    public class QuestionTypeRepository : BaseRepository<QuestionType>, IQuestionTypeRepository
    {
        public QuestionTypeRepository(FactWebContext context) : base(context)
        {
        }

        public QuestionType GetByName(string name)
        {
            return base.Fetch(x => x.Name == name);
        }

        public Task<QuestionType> GetByNameAsync(string name)
        {
            return base.FetchAsync(x => x.Name == name);
        }
    }
}
