using FactWeb.RepositoryContracts;
using System;

namespace FactWeb.BusinessLayer
{
    public class BugManager
    {
        private readonly IBugRepository repository;

        public BugManager(IBugRepository bugRepository)
        {
            this.repository = bugRepository;
        }

        public void AddBug(string text, string version, DateTime? releaseDate, string url, string createdBy)
        {
            this.repository.AddBug(text, version, releaseDate, url, createdBy);
        }
    }
}

