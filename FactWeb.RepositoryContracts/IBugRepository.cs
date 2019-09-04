using System;

namespace FactWeb.RepositoryContracts
{
    public interface IBugRepository
    {
        void AddBug(string text, string version, DateTime? releaseDate, string url, string createdBy);
    }
}
