using FactWeb.Model;
using FactWeb.RepositoryContracts;

namespace FactWeb.BusinessLayer
{
    public class CountryManager : BaseManager<CountryManager, ICountryRepository, Country>
    {
        public CountryManager(ICountryRepository repository) : base(repository)
        {
        }
    }
}
