using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ApplicationSectionQuestionManager : BaseManager<ApplicationSectionQuestionManager, IApplicationSectionQuestionRepository, ApplicationSectionQuestion>
    {
        public ApplicationSectionQuestionManager(IApplicationSectionQuestionRepository repository) : base(repository)
        {
        }

        public override ApplicationSectionQuestion GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ApplicationSectionQuestion GetById(Guid id)
        {
            LogMessage("GetById (ApplicationSectionQuestionManager)");

            return base.Repository.GetById(id);
        }

        public void Remove(Guid id, string updatedBy)
        {
            LogMessage("Remove (ApplicationSectionQuestionManager)");

            var row = this.GetById(id);

            if (row == null)
            {
                throw new KeyNotFoundException("Cannot find Section");
            }

            row.IsActive = false;
            row.UpdatedBy = updatedBy;
            row.UpdatedDate = DateTime.Now;

            base.Repository.Save(row);
        }

        public async Task RemoveAsync(Guid id, string updatedBy)
        {
            LogMessage("RemoveAsync (ApplicationSectionQuestionManager)");

            var row = this.GetById(id);

            if (row == null)
            {
                throw new KeyNotFoundException("Cannot find Section");
            }

            row.IsActive = false;
            row.UpdatedBy = updatedBy;
            row.UpdatedDate = DateTime.Now;

            await base.Repository.SaveAsync(row);
        }

        public void Update(QuestionType questionType, Question item, string updatedBy)
        {
            LogMessage("Update (ApplicationSectionQuestionManager)");

            var row = this.GetById(item.Id.GetValueOrDefault());
                
            if (row == null)
            {
                throw new KeyNotFoundException("Cannot find Section");
            }

            row.Text = item.Text;
            row.ComplianceNumber = item.ComplianceNumber;
            row.Description = item.Description;
            row.QuestionTypeId = questionType.Id;
            row.Order = item.Order;
            row.UpdatedBy = updatedBy;
            row.UpdatedDate = DateTime.Now;

            if (item.QuestionTypeFlags != null)
            {
                row.QuestionTypesFlag = item.QuestionTypeFlags.AllTypes;
            }

            row.ScopeTypes = item.ScopeTypes.Select(x => new ApplicationSectionQuestionScopeType
            {
                Id = Guid.NewGuid(),
                ScopeTypeId = x.ScopeTypeId,
                CreatedDate = DateTime.Now,
                CreatedBy = updatedBy
            })
                .ToList();

            base.Repository.Save(row);
        }

        public async Task<ApplicationSectionQuestion> UpdateAsync(QuestionType questionType, Question item, string updatedBy)
        {
            LogMessage("UpdateAsync (ApplicationSectionQuestionManager)");

            var row = this.GetById(item.Id.GetValueOrDefault());

            if (row == null)
            {
                throw new KeyNotFoundException("Cannot find Section");
            }

            row.Text = item.Text;
            row.ComplianceNumber = item.ComplianceNumber;
            row.Description = item.Description;
            row.QuestionTypeId = questionType.Id;
            row.Order = item.Order;
            row.UpdatedBy = updatedBy;
            row.UpdatedDate = DateTime.Now;
            row.ScopeTypes = item.ScopeTypes.Select(x => new ApplicationSectionQuestionScopeType
            {
                Id = Guid.NewGuid(),
                ScopeTypeId = x.ScopeTypeId,
                CreatedDate = DateTime.Now,
                CreatedBy = updatedBy
            })
                .ToList();

            if (item.QuestionTypeFlags != null)
            {
                row.QuestionTypesFlag = item.QuestionTypeFlags.AllTypes;
            }

            await base.Repository.SaveAsync(row);
            return row;
        }

        public ApplicationSectionQuestion Add(QuestionType questionType, Question item, string createdBy)
        {
            LogMessage("Add (ApplicationSectionQuestionManager)");

            var row = new ApplicationSectionQuestion
            {
                Id = Guid.NewGuid(),
                ApplicationSectionId = item.SectionId.GetValueOrDefault(),
                Text = item.Text,
                IsActive = true,
                Order = item.Order,
                ComplianceNumber = item.ComplianceNumber,
                Description = item.Description,
                QuestionTypeId = questionType.Id,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
                ScopeTypes = item.ScopeTypes.Select(x => new ApplicationSectionQuestionScopeType
                {
                    Id = Guid.NewGuid(),
                    ScopeTypeId = x.ScopeTypeId,
                    CreatedDate = DateTime.Now,
                    CreatedBy = createdBy
                })
                    .ToList()
            };

            if (item.QuestionTypeFlags != null)
            {
                row.QuestionTypesFlag = item.QuestionTypeFlags.AllTypes;
            }

            base.Repository.Add(row);

            return row;
        }

        public async Task<ApplicationSectionQuestion> AddAsync(ApplicationVersion version, QuestionType questionType, Question item, string createdBy)
        {
            LogMessage("AddAsync (ApplicationSectionQuestionManager)");

            var row = new ApplicationSectionQuestion
            {
                Id = Guid.NewGuid(),
                ApplicationSectionId = item.SectionId.GetValueOrDefault(),
                Text = item.Text,
                IsActive = true,
                Order = item.Order,
                ComplianceNumber = item.ComplianceNumber,
                Description = item.Description,
                QuestionTypeId = questionType.Id,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
                ScopeTypes = item.ScopeTypes.Select(x => new ApplicationSectionQuestionScopeType
                    {
                        Id = Guid.NewGuid(),
                        ScopeTypeId = x.ScopeTypeId,
                        CreatedDate = DateTime.Now,
                        CreatedBy = createdBy
                    })
                    .ToList()
            };

            if (item.QuestionTypeFlags != null)
            {
                row.QuestionTypesFlag = item.QuestionTypeFlags.AllTypes;
            }

            await base.Repository.AddAsync(row);

            return row;
        }

        public ApplicationSectionQuestion AddOrUpdate(QuestionType questionType, Question item, string updatedBy)
        {
            LogMessage("AddOrUpdate (ApplicationSectionQuestionManager)");

            if (item.Id.HasValue)
            {
                this.Update(questionType, item, updatedBy);
                return null;
            }
            else
            {
                return this.Add(questionType, item, updatedBy);
            }
        }

        public async Task<ApplicationSectionQuestion> AddOrUpdateAsync(ApplicationVersion version, QuestionType questionType, Question item, string updatedBy)
        {
            LogMessage("AddOrUpdateAsync (ApplicationSectionQuestionManager)");

            if (item.Id.HasValue)
            {
                return await this.UpdateAsync(questionType, item, updatedBy);
            }
            else
            {
                return await this.AddAsync(version, questionType, item, updatedBy);
            }
        }

        public List<ApplicationSectionQuestion> GetAllByApplicationType(int applicationTypeId)
        {
            return base.Repository.GetAllByApplicationType(applicationTypeId);
        }

        public Task<List<ApplicationSectionQuestion>> GetAllByApplicationTypeAsync(int applicationTypeId)
        {
            return base.Repository.GetAllByApplicationTypeAsync(applicationTypeId);
        }

        public List<ApplicationSectionQuestion> GetAllForActiveVersionsNoLazyLoad()
        {
            return base.Repository.GetAllForActiveVersionsNoLazyLoad();
        }

        public List<ApplicationSectionQuestion> GetAllForVersionNoLazyLoad(Guid versionId)
        {
            return base.Repository.GetAllForVersionNoLazyLoad(versionId);
        }

        public List<ApplicationSectionQuestion> GetAllForApplicationTypeNoLazyLoad(string applicationTypeName)
        {
            return base.Repository.GetAllForApplicationTypeNoLazyLoad(applicationTypeName);
        }

        public List<SectionQuestion> GetSectionQuestions(Guid applicationUniqueId, Guid applicationSectionId, Guid userId)
        {
            return this.Repository.GetSectionQuestions(applicationUniqueId, applicationSectionId, userId);
        }

        public List<ApplicationSectionQuestion> GetQuestionsWithRFIs(int applicationId)
        {
            return this.Repository.GetQuestionsWithRFIs(applicationId);
        }
    }
}
