using FactWeb.Model;
using FactWeb.RepositoryContracts;
using System;
using System.Data.Entity;

namespace FactWeb.Repository
{
    public class BugRepository : IBugRepository
    {
        private readonly IDbSet<Bug> dbset;
        private readonly FactWebContext context;

        public BugRepository(FactWebContext context)
        {
            this.context = context;
            this.dbset = context.SetEntity<Bug>();
        }

        public void AddBug(string text, string version, DateTime? releaseDate, string url, string createdBy)
        {
            var item = new Bug
            {
                BugText = text,
                ApplicationVersion = version,
                ReleaseDate = releaseDate,
                BugUrl = url,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy
            };

            this.dbset.Add(item);
            this.context.SaveChanges();
        }
    }
}
