using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class UserLanguageManager : BaseManager<UserLanguageManager, IUserLanguageRepository, UserLanguage>
    {
        public UserLanguageManager(IUserLanguageRepository repository) : base(repository)
        {
        }

        public void UpdateUserLanguage(Guid userId, List<LanguageItem> Languages, string updatedBy)
        {
            LogMessage("UpdateUserLanguage (UserLanguageManager)");

            var existingLanguages = base.Repository.GetAllByUserId(userId);

            foreach (var item in existingLanguages)
            {
                base.BatchRemove(item);
            }

            foreach (var objLanguage in Languages.Where(x=>x.IsSelected.GetValueOrDefault()))
            {
                UserLanguage userLanguage = new UserLanguage();
                userLanguage.LanguageId = objLanguage.Id;
                userLanguage.UserId = userId;
                userLanguage.CreatedBy = updatedBy;
                userLanguage.CreatedDate = DateTime.Now;
                userLanguage.UpdatedBy = updatedBy;
                userLanguage.UpdatedDate = DateTime.Now;

                base.BatchAdd(userLanguage);
            }

            base.Repository.SaveChanges();
        }

        public async Task UpdateUserLanguageAsync(Guid userId, List<LanguageItem> Languages, string updatedBy)
        {
            LogMessage("UpdateUserLanguageAsync (UserLanguageManager)");

            var existingLanguages = base.Repository.GetAllByUserId(userId);

            foreach (var item in existingLanguages)
            {
                base.BatchRemove(item);
            }

            foreach (var objLanguage in Languages)
            {
                UserLanguage userLanguage = new UserLanguage();
                userLanguage.LanguageId = objLanguage.Id;
                userLanguage.UserId = userId;
                userLanguage.CreatedBy = updatedBy;
                userLanguage.CreatedDate = DateTime.Now;
                userLanguage.UpdatedBy = updatedBy;
                userLanguage.UpdatedDate = DateTime.Now;

                base.BatchAdd(userLanguage);
            }

            await base.Repository.SaveChangesAsync();
        }
    }
}
