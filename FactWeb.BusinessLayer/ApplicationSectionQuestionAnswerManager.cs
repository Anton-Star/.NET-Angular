using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ApplicationSectionQuestionAnswerManager : BaseManager<ApplicationSectionQuestionAnswerManager, IApplicationSectionQuestionAnswerRepository, ApplicationSectionQuestionAnswer>
    {
        public ApplicationSectionQuestionAnswerManager(IApplicationSectionQuestionAnswerRepository repository) : base(repository)
        {
        }

        public override ApplicationSectionQuestionAnswer GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ApplicationSectionQuestionAnswer GetById(Guid id)
        {
            LogMessage("GetById (ApplicationSectionQuestionAnswerManager)");

            return base.Repository.GetById(id);
        }

        public List<ApplicationSectionQuestionAnswer> GetAllByQuestion(Guid questionId)
        {
            LogMessage("GetAllByQuestion (ApplicationSectionQuestionAnswerManager)");

            return base.Repository.GetByQuestion(questionId);
        }

        public Task<List<ApplicationSectionQuestionAnswer>> GetAllByQuestionAsync(Guid questionId)
        {
            LogMessage("GetAllByQuestionAsync (ApplicationSectionQuestionAnswerManager)");

            return base.Repository.GetByQuestionAsync(questionId);
        }

        public List<QuestionAnswer> GetSectionAnswers(Guid applicationSectionId)
        {
            return base.Repository.GetSectionAnswers(applicationSectionId);
        }

        public List<ApplicationSectionQuestionAnswer> GetAllForVersion(Guid applicationVersionId)
        {
            return base.Repository.GetAllForVersion(applicationVersionId);
        }

        public void Remove(Guid id, string updatedBy)
        {
            LogMessage("Remove (ApplicationSectionQuestionAnswerManager)");

            var item = this.GetById(id);

            if (item == null)
            {
                throw new KeyNotFoundException("Cannot find Section");
            }

            var answers = this.GetAllByQuestion(item.ApplicationSectionQuestionId);
            item = null;

            var row = answers.SingleOrDefault(x => x.Id == id);

            if (row == null)
            {
                throw new KeyNotFoundException("Cannot find Section");
            }

            this.UpdateOrder(answers, row.Id, row.Order, -1, updatedBy);

            row.IsActive = false;
            row.UpdatedBy = updatedBy;
            row.UpdatedDate = DateTime.Now;

            base.Repository.BatchSave(row);
            base.Repository.SaveChanges();
        }

        public async Task RemoveAsync(Guid id, string updatedBy)
        {
            LogMessage("RemoveAsync (ApplicationSectionQuestionAnswerManager)");

            var item = this.GetById(id);

            if (item == null)
            {
                throw new KeyNotFoundException("Cannot find Section");
            }

            var answers = this.GetAllByQuestion(item.ApplicationSectionQuestionId);
            item = null;

            var row = answers.SingleOrDefault(x => x.Id == id);

            if (row == null)
            {
                throw new KeyNotFoundException("Cannot find Section");
            }

            this.UpdateOrder(answers, row.Id, row.Order, -1, updatedBy);

            row.IsActive = false;
            row.UpdatedBy = updatedBy;
            row.UpdatedDate = DateTime.Now;

            base.Repository.BatchSave(row);
            await base.Repository.SaveChangesAsync();
        }

        public void Update(Answer item, string updatedBy)
        {
            LogMessage("Update (ApplicationSectionQuestionAnswerManager)");

            var answers = this.GetAllByQuestion(item.QuestionId.GetValueOrDefault());

            var row = answers.SingleOrDefault(x => x.Id == item.Id.GetValueOrDefault());

            if (row == null)
            {
                throw new KeyNotFoundException("Cannot find Section");
            }

            this.UpdateOrder(answers, row.Id, row.Order, item.Order, updatedBy);

            row.Text = item.Text;
            row.Order = item.Order;
            row.IsExpectedAnswer = item.IsExpectedAnswer;
            row.UpdatedBy = updatedBy;
            row.UpdatedDate = DateTime.Now;

            base.Repository.BatchSave(row);

            base.Repository.SaveChanges();
        }

        public async Task UpdateAsync(Answer item, string updatedBy)
        {
            LogMessage("UpdateAsync (ApplicationSectionQuestionAnswerManager)");

            var answers = await this.GetAllByQuestionAsync(item.QuestionId.GetValueOrDefault());

            var row = answers.SingleOrDefault(x => x.Id == item.Id.GetValueOrDefault());

            if (row == null)
            {
                throw new KeyNotFoundException("Cannot find Section");
            }

            this.UpdateOrder(answers, row.Id, row.Order, item.Order, updatedBy);

            row.Text = item.Text;
            row.Order = item.Order;
            row.IsExpectedAnswer = item.IsExpectedAnswer;
            row.UpdatedBy = updatedBy;
            row.UpdatedDate = DateTime.Now;

            base.Repository.BatchSave(row);

            await base.Repository.SaveChangesAsync();
        }

        public ApplicationSectionQuestionAnswer Add(Answer item, string createdBy)
        {
            LogMessage("Add (ApplicationSectionQuestionAnswerManager)");

            var answers = this.GetAllByQuestion(item.QuestionId.GetValueOrDefault());

            this.UpdateOrder(answers, null, -1, item.Order, createdBy);

            var row = new ApplicationSectionQuestionAnswer
            {
                Id = Guid.NewGuid(),
                ApplicationSectionQuestionId = item.QuestionId.GetValueOrDefault(),
                Text = item.Text,
                IsActive = true,
                IsExpectedAnswer = item.IsExpectedAnswer,
                Order = item.Order,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
            };

            base.Repository.BatchAdd(row);

            base.Repository.SaveChanges();

            return row;
        }

        public async Task<ApplicationSectionQuestionAnswer> AddAsync(Answer item, string createdBy)
        {
            LogMessage("AddAsync (ApplicationSectionQuestionAnswerManager)");

            var answers = await this.GetAllByQuestionAsync(item.QuestionId.GetValueOrDefault());

            this.UpdateOrder(answers, null, -1, item.Order, createdBy);

            var row = new ApplicationSectionQuestionAnswer
            {
                Id = Guid.NewGuid(),
                ApplicationSectionQuestionId = item.QuestionId.GetValueOrDefault(),
                Text = item.Text,
                IsActive = true,
                IsExpectedAnswer = item.IsExpectedAnswer,
                Order = item.Order,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
            };

            base.Repository.BatchAdd(row);

            await base.Repository.SaveChangesAsync();

            return row;
        }

        public void AddOrUpdate(Answer item, string updatedBy)
        {
            LogMessage("AddOrUpdate (ApplicationSectionQuestionAnswerManager)");

            if (!item.QuestionId.HasValue)
            {
                throw new NullReferenceException("Question Id is required");
            }

            if (item.Id.HasValue)
            {
                this.Update(item, updatedBy);
            }
            else
            {
                this.Add(item, updatedBy);
            }
        }

        public async Task<Guid> AddOrUpdateAsync(Answer item, string updatedBy)
        {
            LogMessage("AddOrUpdateAsync (ApplicationSectionQuestionAnswerManager)");

            if (!item.QuestionId.HasValue)
            {
                throw new NullReferenceException("Question Id is required");
            }

            if (item.Id.HasValue)
            {
                await this.UpdateAsync(item, updatedBy);

                return item.Id.Value;
            }
            else
            {
               var row = await this.AddAsync(item, updatedBy);
                return row.Id;
            }
        }

        public void UpdateOrder(List<ApplicationSectionQuestionAnswer> answers, Guid? changingRecordId, int currentOrder, int newOrder, string updatedBy)
        {
            LogMessage("AddOrUpdateAsync (ApplicationSectionQuestionAnswerManager)");

            if (currentOrder == newOrder) return;

            if (currentOrder == -1)
            {
                var changes =
                    answers.Where(x => x.Id != changingRecordId && x.Order >= newOrder)
                        .ToList();

                foreach (var answer in changes)
                {
                    answer.Order = answer.Order + 1;
                    answer.UpdatedBy = updatedBy;
                    answer.UpdatedDate = DateTime.Now;

                    base.Repository.BatchSave(answer);
                }
            }
            else if (currentOrder > newOrder)
            {
                var changes =
                    answers.Where(x => x.Id != changingRecordId && x.Order <= currentOrder && x.Order >= newOrder)
                        .ToList();

                foreach (var answer in changes)
                {
                    answer.Order = answer.Order + 1;
                    answer.UpdatedBy = updatedBy;
                    answer.UpdatedDate = DateTime.Now;

                    base.Repository.BatchSave(answer);
                }
            }
            else
            {
                var changes =
                    answers.Where(x => x.Id != changingRecordId && x.Order >= currentOrder && x.Order <= newOrder)
                        .ToList();

                foreach (var answer in changes)
                {
                    answer.Order = answer.Order - 1;
                    answer.UpdatedBy = updatedBy;
                    answer.UpdatedDate = DateTime.Now;

                    base.Repository.BatchSave(answer);
                }
            }

            
        }
    }
}
