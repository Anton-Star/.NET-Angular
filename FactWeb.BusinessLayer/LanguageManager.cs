using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class LanguageManager : BaseManager<LanguageManager, ILanguageRepository, Language>
    {
        public LanguageManager(ILanguageRepository repository) : base(repository)
        {
        }


    }
}
