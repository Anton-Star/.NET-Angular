using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class ApplicationSectionManager : BaseManager<ApplicationSectionManager, IApplicationSectionRepository, ApplicationSection>
    {
        private readonly ApplicationManager applicationManager;

        private readonly List<string> ImportScopes = new List<string>
                            {
                                "AdultAllogeneic",
                                "AdultAutologous",
                                "PediatricAllogeneic",
                                "PediatricAutologous",
                                "OffSiteStorage",
                                "UnrelatedFixed",
                                "RelatedFixed",
                                "UnrelatedNonfixed",
                                "RelatedNonfixed",
                                "PedsAlloAddon",
                                "PedsAutoAddon",
                                "AdultAutoAddon",
                                "AdultAlloAddon",
                                "MoreThanMinimalAddon",
                                "MinimalAddon",
                                "CollectionAddon",
                                "ProcessingAddon",
                                "UnrelatedAddon",
                                "RelatedAddon",
                                "NonFixedAddon",
                                "FixedAddon"
                            };

        public ApplicationSectionManager(IApplicationSectionRepository repository, ApplicationManager apMgr) : base(repository)
        {
            applicationManager = apMgr;
        }


        public override ApplicationSection GetById(int id)
        {
            throw new NotImplementedException("Use GetById Guid");
        }

        public override void Remove(int id)
        {
            throw new NotImplementedException("Use Guid");
        }

        private QuestionTypes? GetQuestionType(string importQuestionTypeName)
        {
            switch (importQuestionTypeName.ToLower())
            {
                case "fileupload":
                    return QuestionTypes.DocumentUpload;
                case "textarea":
                    return QuestionTypes.TextArea;
                case "radiobuttons":
                    return QuestionTypes.RadioButtons;
                case "textbox":
                    return QuestionTypes.Textbox;
                case "checkboxes":
                    return QuestionTypes.Checkboxes;
                case "date":
                    return QuestionTypes.Date;
                case "peoplepicker":
                case "user":
                    return QuestionTypes.PeoplePicker;
                case "daterange":
                    return QuestionTypes.DateRange;
            }

            return null;
        }
        public ApplicationSection GetRequirementById(Guid reqGuid, int appId)
        {
            LogMessage("GetRequirementById (ApplicationSectionManager)");

            return base.Repository.GetById(reqGuid, appId);
        }

        private QuestionTypeFlags GetImportQuestionTypeFlags(string importQuestionType)
        {
            var flags = new QuestionTypeFlags();

            var questionTypes = importQuestionType.Split('|');

            foreach (var questionType in questionTypes)
            {
                switch (questionType)
                {
                    case "FileUpload":
                        flags.DocumentUpload = true;
                        break;
                    case "TextArea":
                        flags.TextArea = true;
                        break;
                    case "RadioButtons":
                        flags.RadioButtons = true;
                        break;
                    case "TextBox":
                        flags.TextBox = true;
                        break;
                    case "CheckBoxes":
                        flags.Checkboxes = true;
                        break;
                    case "Date":
                        flags.Date = true;
                        break;
                    case "User":
                        flags.PeoplePicker = true;
                        break;
                    case "DateRange":
                        flags.DateRange = true;
                        break;
                }

            }

            return flags;
        }

        private ApplicationSectionQuestionScopeType BuildImportQuestionScopeType(List<ScopeType> scopeTypes,
            RequirementImportItem item, string fieldName, string importedBy)
        {
            var field = item.GetType().GetProperty(fieldName).GetValue(item, null);

            if (field != null && !string.IsNullOrWhiteSpace(field.ToString()))
            {
                var scope = scopeTypes.SingleOrDefault(x => x.ImportName == fieldName);

                if (scope != null)
                {
                    return new ApplicationSectionQuestionScopeType
                    {
                        Id = Guid.NewGuid(),
                        ScopeTypeId = scope.Id,
                        CreatedDate = DateTime.Now,
                        CreatedBy = importedBy
                    };
                }
            }

            return null;
        }

        private List<ApplicationSectionQuestionScopeType> BuildImportQuestionScopeTypes(List<ScopeType> scopeTypes, RequirementImportItem item, string importedBy)
        {
            return this.ImportScopes
                .Select(scope => this.BuildImportQuestionScopeType(scopeTypes, item, scope, importedBy))
                .Where(scopeType => scopeType != null)
                .ToList();
        }

        private ApplicationSectionScopeType BuildImportScopeType(List<ScopeType> scopeTypes, RequirementImportItem item,
            string fieldName, string importedBy)
        {
            var field = item.GetType().GetProperty(fieldName).GetValue(item, null);

            if (field != null && !string.IsNullOrWhiteSpace(field.ToString()))
            {
                var scope = scopeTypes.SingleOrDefault(x => x.ImportName == fieldName);

                if (scope != null)
                {
                    return new ApplicationSectionScopeType
                    {
                        Id = Guid.NewGuid(),
                        ScopeTypeId = scope.Id,
                        CreatedDate = DateTime.Now,
                        CreatedBy = importedBy
                    };
                }
            }

            return null;
        }

        private List<ApplicationSectionScopeType> BuildImportScopeTypes(List<ScopeType> scopeTypes, RequirementImportItem item, string importedBy)
        {
            return this.ImportScopes
                .Select(scope => this.BuildImportScopeType(scopeTypes, item, scope, importedBy))
                .Where(scopeType => scopeType != null)
                .ToList();
        }

        public ApplicationVersion Import(List<ScopeType> scopeTypes, ApplicationVersion version, ApplicationType applicationType, List<string> headers, List<string> rows, string importedBy)
        {
            var items = this.BuildImportSections(headers, rows);
            var questionNumber = string.Empty;
            var sections = new Dictionary<string, ApplicationSection>();
            var order = 1;
            Guid? parentId = null;
            var hideSections = new List<ImportHides>();

            foreach (var item in items)
            {
                if (item.FullStandardName.Contains("."))
                {
                    var parentName = item.FullStandardName.Substring(0,
                        item.FullStandardName.LastIndexOf(".", StringComparison.Ordinal));
                    if (sections.ContainsKey($"{parentName}"))
                    {
                        parentId = sections[$"{parentName}"].Id;
                    }
                    else
                    {
                        parentId = null;
                    }
                }
                else
                {
                    var i = 0;

                    for (i = item.FullStandardName.Length; i > 0; i--)
                    {
                        var character = item.FullStandardName.Substring(i - 1);

                        var test = 0;

                        if (!int.TryParse(character, out test))
                        {
                            break;
                        }
                    }

                    var parentName = item.FullStandardName.Substring(0, i);

                    if (sections.ContainsKey($"{parentName}"))
                    {
                        parentId = sections[$"{parentName}"].Id;
                    }
                    else
                    {
                        parentId = null;
                    }
                }

                if (string.IsNullOrWhiteSpace(item.QuestionNumber))
                {
                    var section = new ApplicationSection
                    {
                        Id = Guid.NewGuid(),
                        ApplicationTypeId = applicationType.Id,
                        Name = item.StandardText,
                        IsActive = true,
                        ParentApplicationSectionId = parentId,
                        CreatedDate = DateTime.Now,
                        CreatedBy = importedBy,
                        HelpText = string.IsNullOrWhiteSpace(item.Guidance) ? null : item.Guidance,
                        Order = order.ToString(),
                        UniqueIdentifier = item.FullStandardName,
                        Questions = new List<ApplicationSectionQuestion>(),
                        ApplicationSectionScopeTypes = this.BuildImportScopeTypes(scopeTypes, item, importedBy)
                    };

                    order++;
                    sections.Add($"{item.FullStandardName}", section);
                    questionNumber = string.Empty;
                }
                else if (questionNumber == item.QuestionNumber)
                {
                    var section = sections[$"{item.FullStandardName}"];
                    var question = section.Questions.LastOrDefault(x => x.Text == item.QuestionText);

                    if (question == null) continue;

                    var answer = new ApplicationSectionQuestionAnswer
                    {
                        Id = Guid.NewGuid(),
                        Text = item.OptionText,
                        IsActive = true,
                        Order = question.Answers.Count + 1,
                        CreatedDate = DateTime.Now,
                        Displays = new List<ApplicationSectionQuestionAnswerDisplay>(),
                        IsExpectedAnswer = item.ExpectedAnswer != null && item.ExpectedAnswer.ToLower() == "x",
                        CreatedBy = importedBy
                    };

                    if (!string.IsNullOrWhiteSpace(item.OptionSkipsTheseQuestions))
                    {
                        var importHides = new ImportHides
                        {
                            StandardName = item.FullStandardName,
                            QuestionId = question.Id,
                            AnswerId = answer.Id,
                            ImportHidesSections = new List<ImportHides.ImportHidesSection>()
                        };

                        var hides = item.OptionSkipsTheseQuestions.Split('|');

                        foreach (var hide in hides)
                        {
                            var parts = hide.Split('-');

                            if (parts.Length == 0 || string.IsNullOrWhiteSpace(parts[0].Trim())) continue;

                            importHides.ImportHidesSections.Add(new ImportHides.ImportHidesSection
                            {
                                QuestionNumber = Convert.ToInt32(parts[1].Trim()),
                                SectionNumber = parts[0].Trim()
                            });
                        }

                        hideSections.Add(importHides);
                    }

                    question.Answers.Add(answer);
                }
                else
                {
                    QuestionTypes? questionType = null;

                    questionType = item.QuestionType.Contains("|")
                        ? QuestionTypes.Multiple
                        : this.GetQuestionType(item.QuestionType);


                    if (questionType == null) continue;

                    var section = sections[$"{item.FullStandardName}"];

                    var question = new ApplicationSectionQuestion
                    {
                        Id = Guid.NewGuid(),
                        Text = item.QuestionText,
                        Description = item.QuestionNote,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        Order = Convert.ToInt32(item.QuestionNumber),
                        ComplianceNumber = Convert.ToInt32(item.QuestionNumber),
                        CreatedBy = importedBy,
                        QuestionTypeId = (int)questionType,
                        Answers = new List<ApplicationSectionQuestionAnswer>(),
                        ScopeTypes = this.BuildImportQuestionScopeTypes(scopeTypes, item, importedBy)
                    };

                    if (questionType == QuestionTypes.Multiple)
                    {
                        var flags = this.GetImportQuestionTypeFlags(item.QuestionType);

                        question.QuestionTypesFlag = flags.AllTypes;
                    }

                    questionNumber = item.QuestionNumber;

                    section.Questions.Add(question);
                }
            }

            foreach (var hides in hideSections)
            {
                if (!sections.ContainsKey($"{hides.StandardName}")) continue;

                var section = sections[$"{hides.StandardName}"];

                var question = section.Questions.SingleOrDefault(x => x.Id == hides.QuestionId);

                var answer = question?.Answers.SingleOrDefault(x => x.Id == hides.AnswerId);

                if (answer == null) continue;

                foreach (var hideSec in hides.ImportHidesSections)
                {
                    if (!sections.ContainsKey(hideSec.SectionNumber)) continue;

                    var hideSection = sections[hideSec.SectionNumber];

                    var hideQuestion =
                        hideSection.Questions.LastOrDefault(x => x.Order == hideSec.QuestionNumber);

                    if (hideQuestion == null) continue;

                    answer.Displays.Add(new ApplicationSectionQuestionAnswerDisplay
                    {
                        Id = Guid.NewGuid(),
                        HidesQuestionId = hideQuestion.Id,
                        CreatedDate = DateTime.Now,
                        CreatedBy = importedBy
                    });
                }

            }

            version.ApplicationSections = sections.Values.ToList();

            return version;
        }

        public List<RequirementImportItem> BuildImportSections(List<string> headers, List<string> rows)
        {
            var result = new List<RequirementImportItem>();

            foreach (var r in rows)
            {
                var row = r;

                while (row.Contains('"'))
                {
                    var temp = row.Substring(row.IndexOf('"') + 1);
                    temp = temp.Substring(0, temp.IndexOf('"'));
                    var temp2 = temp.Replace(",", "<comma>");

                    row = row.Replace(string.Format("\"{0}\"", temp), temp2);
                }

                var values = row.Split(',');

                var item = new RequirementImportItem();

                for (var i = 0; i < headers.Count; i++)
                {
                    var header = headers[i].Trim();
                    var value = values[i].Trim();
                    value = value.Replace("<comma>", ",");
                    value = value.Replace("&quot;", "\"");

                    header = header.Replace("\"", "");

                    switch (header)
                    {
                        case "FullStandardName":
                            item.FullStandardName = value;
                            break;
                        case "StandardText":
                            item.StandardText = value;
                            break;
                        case "Guidance":
                            item.Guidance = value;
                            break;
                        case "QuestionNumber":
                            item.QuestionNumber = value;
                            break;
                        case "QuestionText":
                            item.QuestionText = value;
                            break;
                        case "QuestionType":
                            item.QuestionType = value;
                            break;
                        case "QuestionNote":
                            item.QuestionNote = value;
                            break;
                        case "OptionText":
                            item.OptionText = value;
                            break;
                        case "OptionSkipsTheseQuestions":
                            item.OptionSkipsTheseQuestions = value;
                            break;
                        case "AdultAllogeneic":
                            item.AdultAllogeneic = value;
                            break;
                        case "AdultAutologous":
                            item.AdultAutologous = value;
                            break;
                        case "PediatricAllogeneic":
                            item.PediatricAllogeneic = value;
                            break;
                        case "PediatricAutologous":
                            item.PediatricAutologous = value;
                            break;
                        case "OffSiteStorage":
                            item.OffSiteStorage = value;
                            break;
                        case "UnrelatedFixed":
                            item.UnrelatedFixed = value;
                            break;
                        case "RelatedFixed":
                            item.RelatedFixed = value;
                            break;
                        case "UnrelatedNonfixed":
                            item.UnrelatedNonfixed = value;
                            break;
                        case "RelatedNonfixed":
                            item.RelatedNonfixed = value;
                            break;
                        case "PedsAlloAddon":
                            item.PedsAlloAddon = value;
                            break;
                        case "PedsAutoAddon":
                            item.PedsAutoAddon = value;
                            break;
                        case "AdultAutoAddon":
                            item.AdultAutoAddon = value;
                            break;
                        case "AdultAlloAddon":
                            item.AdultAlloAddon = value;
                            break;
                        case "MoreThanMinimalAddon":
                            item.AdultAlloAddon = value;
                            break;
                        case "MinimalAddon":
                            item.MinimalAddon = value;
                            break;
                        case "CollectionAddon":
                            item.CollectionAddon = value;
                            break;
                        case "ProcessingAddon":
                            item.ProcessingAddon = value;
                            break;
                        case "UnrelatedAddon":
                            item.UnrelatedAddon = value;
                            break;
                        case "RelatedAddon":
                            item.RelatedAddon = value;
                            break;
                        case "NonFixedAddon":
                            item.NonFixedAddon = value;
                            break;
                        case "FixedAddon":
                            item.FixedAddon = value;
                            break;
                        case "ExpectedAnswer":
                            item.ExpectedAnswer = value;
                            break;
                    }
                }

                result.Add(item);
            }

            return result;
        }

        public void Remove(Guid id, string updatedBy)
        {
            LogMessage("Remove (ApplicationSectionManager)");

            var applicationSection = this.GetById(id);

            if (applicationSection == null)
            {
                throw new KeyNotFoundException("Cannot find Item");
            }

            applicationSection.IsActive = false;
            applicationSection.UpdatedBy = updatedBy;
            applicationSection.UpdatedDate = DateTime.Now;

            base.Repository.Save(applicationSection);
        }

        public async Task RemoveAsync(Guid id, string updatedBy)
        {
            LogMessage("RemoveAsync (ApplicationSectionManager)");

            var applicationSection = this.GetById(id);

            if (applicationSection == null)
            {
                throw new KeyNotFoundException("Cannot find Section");
            }

            applicationSection.IsActive = false;
            applicationSection.UpdatedBy = updatedBy;
            applicationSection.UpdatedDate = DateTime.Now;

            string serialNumber = applicationSection.Order;
            base.Repository.Save(applicationSection);

            if (applicationSection.ParentApplicationSectionId != null)
            {
                var applicationSectionParent = this.GetById(applicationSection.ParentApplicationSectionId.GetValueOrDefault());
                var childSection = applicationSectionParent.Children.Where(y => y.IsActive == true).OrderBy(x => x.Order);

                for (int i = 0; i < childSection.Count(); i++)
                {
                    int lastIndex = serialNumber.LastIndexOf('.');
                    string result = string.Concat(serialNumber.Substring(0, lastIndex + 1), i + 1);

                    childSection.ElementAt(i).Order = result;
                    await base.Repository.SaveAsync(childSection.ElementAt(i));
                }
            }


        }

        /// <summary>
        /// Gets an Application Section by id
        /// </summary>
        /// <param name="id">Id of the application section</param>
        /// <returns>ApplicationSection object</returns>
        public ApplicationSection GetById(Guid id)
        {
            LogMessage("GetById (ApplicationSectionManager)");

            return base.Repository.GetById(id);
        }

        /// <summary>
        /// Gets all sections by application type
        /// </summary>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of ApplicationSections</returns>
        public List<ApplicationSection> GetAllForApplicationType(int applicationTypeId)
        {
            LogMessage("GetAllForApplicationType (ApplicationSectionManager)");

            return base.Repository.GetAllForApplicationType(applicationTypeId).OrderBy(x => x.PartNumber).ToList();
        }

        /// <summary>
        /// Gets all sections by application type asynchronously
        /// </summary>
        /// <param name="applicationTypeId">Id of the application type</param>
        /// <returns>Collection of ApplicationSections</returns>
        public async Task<List<ApplicationSection>> GetAllForApplicationTypeAsync(int applicationTypeId)
        {
            LogMessage("GetAllForApplicationTypeAsync (ApplicationSectionManager)");

            var sections = await base.Repository.GetAllForApplicationTypeAsync(applicationTypeId);

            return sections.OrderBy(x => x.PartNumber).ToList();
        }

        /// <summary>
        /// Gets all sections by application type
        /// </summary>
        /// <param name="applicationTypeName">Name of the application type</param>
        /// <returns>Collection of ApplicationSections</returns>
        public List<ApplicationSection> GetAllForApplicationType(string applicationTypeName)
        {
            LogMessage("GetAllForApplicationType (ApplicationSectionManager)");

            return base.Repository.GetAllForApplicationType(applicationTypeName).OrderBy(x => x.Order).ToList();
            //return base.Repository.GetAllForApplicationType(applicationTypeName);
        }

        public List<ApplicationSectionItem> GetAllForApplicationTypeItems(string applicationTypeName)
        {
            LogMessage("GetAllForApplicationTypeItems (ApplicationSectionManager)");

            var items = base.Repository.GetForApplicationType(applicationTypeName).OrderBy(x => x.Order).ToList();



            return items.Select(x => ModelConversions.Convert(x, null)).ToList();
        }

        /// <summary>
        /// Gets all sections by application type
        /// </summary>
        /// <param name="applicationTypeName">Name of the application type</param>
        /// <returns>Collection of ApplicationSections</returns>
        public async Task<List<ApplicationSection>> GetAllForApplicationTypeAsync(string applicationTypeName)
        {
            LogMessage("GetAllForApplicationTypeAsync (ApplicationSectionManager)");

            var sections = await base.Repository.GetAllForApplicationTypeAsync(applicationTypeName);

            return sections.OrderBy(x => x.PartNumber).ToList();
        }

        public async Task<List<SectionDocument>> GetDocumentsAsync(int organizationId)
        {
            LogMessage("GetDocuments (ApplicationSectionManager)");

            var sections = await base.Repository.GetAllWithDocumentAsync(organizationId);

            var sectionDocuments = new List<SectionDocument>();

            foreach (var section in sections)
            {
                sectionDocuments.AddRange(ModelConversions.ConvertToSectionDocuments(section));
            }

            return sectionDocuments;
        }

        public List<SectionDocument> GetDocuments(Guid appId, bool isCompliance, int userRoleId)
        {
            LogMessage("GetDocuments (ApplicationSectionManager)");

            var sections = isCompliance ? base.Repository.GetAllWithDocumentForComp(appId) : base.Repository.GetAllWithDocument(appId); //returns all the sections which contain documents 

            var sectionDocuments = new List<SectionDocument>();
            var dictionary = new Dictionary<int, Guid>();
            var sites = new List<Site>();
            var orderedDocs = new List<SectionDocument>();

            if (isCompliance)
            {
                foreach (var sec in sections)
                {
                    foreach (var q in sec.Questions)
                    {
                        foreach (var r in q.ApplicationResponses)
                        {
                            if (r.Application.Site != null && sites.All(x => x.Id != r.Application.Site.Id))
                            {
                                sites.Add(r.Application.Site);
                            }
                        }
                    }
                }

                sites = sites.OrderBy(x => Comparer.OrderSite(x)).ToList();
            }
            
            
            //foreach (var sec in sections) {
            //    var secDocs = new List<SectionDocument>();
            //    secDocs = ModelConversions.TestConvertToSectionDocuments(sec);
            //    if (secDocs.Count > 0)
            //    {
            //        sectionDocuments.AddRange(secDocs);
            //    }
            //}

            foreach (var items in sections.OrderBy(x => Convert.ToInt32(x.Order))
                    .Select(ModelConversions.ConvertToSectionDocumentsSectionWise)
                    .Where(items => items.Count > 0))
            {
                sectionDocuments.AddRange(items);
            }

            for (var i = 0; i < sectionDocuments.Count - 1; i++)
            {
                if (sectionDocuments[i].RequirementId == sectionDocuments[i + 1].RequirementId && 
                    sectionDocuments[i].AppId == sectionDocuments[i + 1].AppId && 
                    sectionDocuments[i].Document.Name == sectionDocuments[i + 1].Document.Name)
                {
                    sectionDocuments.Remove(sectionDocuments[i]);
                }
            }

            foreach (var item in sectionDocuments)
            {
                var id = Convert.ToInt32(item.AppId);

                if (dictionary.ContainsKey(id))
                {
                    item.Document.AppUniqueId = dictionary[id];
                }
                else
                {
                    var guid = applicationManager.getApplicationById(id);

                    dictionary.Add(id, guid.UniqueId);
                    item.Document.AppUniqueId = guid.UniqueId;
                }
                
            }

            if (userRoleId != (int)Constants.Role.FACTAdministrator)
            {
                sectionDocuments = sectionDocuments.Where(x => !x.Document.StaffOnly).ToList();
            }

            foreach (var site in sites)
            {
                orderedDocs.AddRange(sectionDocuments.Where(x => x.SiteName == site.Name));
            }

            return isCompliance ? orderedDocs : sectionDocuments;
        }

        public void Update(ApplicationSectionItem item, string updatedBy)
        {
            LogMessage("Update (ApplicationSectionManager)");

            var applicationSections = item.ParentId.HasValue
                ? base.Repository.GetByParent(item.ParentId.GetValueOrDefault())
                : base.Repository.GetRootLevel();

            var row = applicationSections.SingleOrDefault(x => x.Id == item.Id.GetValueOrDefault());

            if (row == null)
            {
                throw new KeyNotFoundException("Cannot find Section");
            }

            this.UpdateOrder(!item.ParentId.HasValue, applicationSections, item.Id, Convert.ToInt32(row.Order), Convert.ToInt32(item.Order), updatedBy);

            row.Name = item.Name;
            row.PartNumber = item.PartNumber == null ? 0 : Convert.ToInt32(item.PartNumber);
            row.HelpText = item.HelpText;
            row.IsVariance = item.IsVariance;
            row.Version = item.Version;
            row.Order = item.Order;
            row.UniqueIdentifier = item.UniqueIdentifier;
            row.UpdatedBy = updatedBy;
            row.UpdatedDate = DateTime.Now;

            base.Repository.BatchSave(row);

            base.Repository.SaveChanges();
        }

        public async Task<ApplicationSection> UpdateAsync(ApplicationSectionItem item, string updatedBy)
        {
            LogMessage("UpdateAsync (ApplicationSectionManager)");

            var applicationSections = item.ParentId.HasValue
                ? await base.Repository.GetByParentAsync(item.ParentId.GetValueOrDefault())
                : await base.Repository.GetRootLevelAsync();

            var row = applicationSections.SingleOrDefault(x => x.Id == item.Id.GetValueOrDefault());

            if (row == null)
            {
                throw new KeyNotFoundException("Cannot find Section");
            }

            this.UpdateOrder(!item.ParentId.HasValue, applicationSections, item.Id, Convert.ToInt32(row.Order), Convert.ToInt32(item.Order), updatedBy);

            row.Name = item.Name;
            row.PartNumber = item.PartNumber == null ? 0 : Convert.ToInt32(item.PartNumber);
            row.HelpText = item.HelpText;
            row.IsVariance = item.IsVariance;
            row.Version = item.Version;
            row.Order = item.Order;
            row.UniqueIdentifier = item.UniqueIdentifier;
            row.UpdatedBy = updatedBy;
            row.UpdatedDate = DateTime.Now;

            base.Repository.BatchSave(row);
            await base.Repository.SaveChangesAsync();

            return row;
        }

        public ApplicationSection Add(ApplicationType applicationType, ApplicationSectionItem item, string createdBy)
        {
            LogMessage("Add (ApplicationSectionManager)");

            var applicationSections = item.ParentId.HasValue
                ? base.Repository.GetByParent(item.ParentId.GetValueOrDefault())
                : base.Repository.GetRootLevel();

            this.UpdateOrder(!item.ParentId.HasValue, applicationSections, null, -1, Convert.ToInt32(item.Order), createdBy);

            var applicationSection = new ApplicationSection
            {
                Id = Guid.NewGuid(),
                ApplicationTypeId = applicationType.Id,
                Name = item.Name,
                HelpText = item.HelpText,
                PartNumber = item.PartNumber == null ? 0 : Convert.ToInt32(item.PartNumber),
                IsVariance = item.IsVariance,
                IsActive = true,
                Version = item.Version,
                Order = item.Order,
                ParentApplicationSectionId = item.ParentId,
                UniqueIdentifier = item.UniqueIdentifier,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
            };

            base.Repository.BatchAdd(applicationSection);

            base.Repository.SaveChanges();

            var scopeTypes = item.ScopeTypes.Where(x => x.IsSelected == true).ToList();

            if (scopeTypes.Count > 0)
                applicationSection.ApplicationSectionScopeTypes = new List<ApplicationSectionScopeType>();

            foreach (var objectScopeType in scopeTypes)
            {
                ApplicationSectionScopeType applicationSectionScopeType = new ApplicationSectionScopeType();
                applicationSectionScopeType.Id = Guid.NewGuid();
                applicationSectionScopeType.ScopeTypeId = objectScopeType.ScopeTypeId;
                applicationSectionScopeType.ApplicationSectionId = applicationSection.Id;
                applicationSectionScopeType.IsActual = false;
                applicationSectionScopeType.IsDefault = false;
                applicationSectionScopeType.CreatedBy = createdBy;
                applicationSectionScopeType.CreatedDate = DateTime.Now;
                applicationSection.ApplicationSectionScopeTypes.Add(applicationSectionScopeType);
            }
            base.Repository.SaveChanges();

            return applicationSection;
        }

        public async Task<ApplicationSection> AddAsync(ApplicationType applicationType, ApplicationSectionItem item, string createdBy)
        {
            LogMessage("AddAsync (ApplicationSectionManager)");

            var applicationSections = item.ParentId.HasValue
                ? await base.Repository.GetByParentAsync(item.ParentId.GetValueOrDefault())
                : await base.Repository.GetRootLevelAsync();

            this.UpdateOrder(!item.ParentId.HasValue, applicationSections, null, -1, Convert.ToInt32(item.Order), createdBy);

            var applicationSection = new ApplicationSection
            {
                Id = Guid.NewGuid(),
                ApplicationTypeId = applicationType.Id,
                Name = item.Name,
                HelpText = item.HelpText,
                PartNumber = item.PartNumber == null ? 0 : Convert.ToInt32(item.PartNumber),
                IsVariance = item.IsVariance,
                IsActive = true,
                Version = item.Version,
                Order = item.Order,
                ParentApplicationSectionId = item.ParentId,
                UniqueIdentifier = item.UniqueIdentifier,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
                ApplicationVersionId = item.VersionId.GetValueOrDefault()
            };

            base.Repository.BatchAdd(applicationSection);
            await base.Repository.SaveChangesAsync();

            return applicationSection;
        }

        public void AddOrUpdate(ApplicationType applicationType, ApplicationSectionItem item, string updatedBy)
        {
            LogMessage("AddOrUpdate (ApplicationSectionManager)");

            if (item.Id.HasValue)
            {
                this.Update(item, updatedBy);
            }
            else
            {
                this.Add(applicationType, item, updatedBy);
            }
        }

        public async Task<ApplicationSection> AddOrUpdateAsync(ApplicationType applicationType, ApplicationSectionItem item, string updatedBy)
        {
            LogMessage("AddOrUpdateAsync (ApplicationSectionManager)");

            if (item.Id.HasValue)
            {
                return await this.UpdateAsync(item, updatedBy);
            }
            else
            {
                var applicationSection = await this.AddAsync(applicationType, item, updatedBy);
                return applicationSection;
            }
        }

        private string GetUniqueIdentifier(string fullUniqueIdentifier)
        {
            if (fullUniqueIdentifier.Contains("."))
            {
                var index = fullUniqueIdentifier.LastIndexOf(".", StringComparison.Ordinal);
                return fullUniqueIdentifier.Substring(0, index + 1);
            }
            else
            {
                return fullUniqueIdentifier.Substring(0, fullUniqueIdentifier.Length - 1);
            }
        }

        public void UpdateOrder(bool isRoot, List<ApplicationSection> sections, Guid? changingRecordId, int currentOrder, int newOrder, string updatedBy)
        {
            LogMessage("UpdateOrder (ApplicationSectionManager)");

            if (currentOrder == newOrder) return;

            if (!changingRecordId.HasValue)
            {
                var changes =
                    sections.Where(x => Convert.ToInt32(x.Order) >= newOrder)
                        .ToList();

                foreach (var item in changes)
                {
                    item.Order = (Convert.ToInt32(item.Order) + 1).ToString();

                    if (!isRoot)
                    {
                        var id = this.GetUniqueIdentifier(item.UniqueIdentifier);
                        item.UniqueIdentifier = string.Format("{0}{1}", id, item.Order);
                    }

                    item.UpdatedBy = updatedBy;
                    item.UpdatedDate = DateTime.Now;

                    base.Repository.BatchSave(item);
                }
            }
            else if (currentOrder > newOrder)
            {
                var changes =
                    sections.Where(x => x.Id != changingRecordId &&
                        currentOrder >= Convert.ToInt32(x.Order) && Convert.ToInt32(x.Order) >= newOrder)
                        .ToList();

                foreach (var item in changes)
                {
                    item.Order = (Convert.ToInt32(item.Order) + 1).ToString();

                    if (!isRoot)
                    {
                        var id = this.GetUniqueIdentifier(item.UniqueIdentifier);
                        item.UniqueIdentifier = string.Format("{0}{1}", id, item.Order);
                    }

                    item.UpdatedBy = updatedBy;
                    item.UpdatedDate = DateTime.Now;

                    base.Repository.BatchSave(item);
                }
            }
            else
            {
                var changes =
                    sections.Where(x => x.Id != changingRecordId &&
                        Convert.ToInt32(x.Order) >= currentOrder && newOrder >= Convert.ToInt32(x.Order))
                        .ToList();

                foreach (var item in changes)
                {
                    item.Order = (Convert.ToInt32(item.Order) - 1).ToString();

                    if (!isRoot)
                    {
                        var id = this.GetUniqueIdentifier(item.UniqueIdentifier);
                        item.UniqueIdentifier = string.Format("{0}{1}", id, item.Order);
                    }

                    item.UpdatedBy = updatedBy;
                    item.UpdatedDate = DateTime.Now;

                    base.Repository.BatchSave(item);
                }
            }
        }

        public List<ApplicationSection> GetRootItemsByType(int applicationTypeId)
        {
            return base.Repository.GetRootItems(applicationTypeId);
        }

        public Task<List<ApplicationSection>> GetRootItemsByTypeAsync(int applicationTypeId)
        {
            return base.Repository.GetRootItemsAsync(applicationTypeId);
        }

        public List<ApplicationSection> GetFlatForApplicationType(int applicationTypeId, Guid? applicationVersionId)
        {
            return base.Repository.GetFlatForApplicationType(applicationTypeId, applicationVersionId).OrderBy(x => x.Order).ToList();
        }

        private bool RemoveQuestion(ICollection<ApplicationSection> sections, Guid notApplicableQuestionId)
        {
            foreach (var item in sections)
            {
                var foundQuestion =
                    item.Questions?.SingleOrDefault(x => x.Id == notApplicableQuestionId);

                if (foundQuestion != null)
                {
                    item.Questions.Remove(foundQuestion);
                    return true;
                }

                if (item.Children != null)
                {
                    var found = this.RemoveQuestion(item.Children, notApplicableQuestionId);

                    if (found) return true;
                }
            }

            return false;
        }

        private List<ApplicationSection> ProcessSectionBlanks(List<ApplicationSection> sections)
        {
            for (var i = 0; i < sections.Count; i++)
            {
                if (sections[i].Children != null)
                {
                    sections[i].Children = this.ProcessSectionBlanks(sections[i].Children.ToList());
                }

                if ((sections[i].Questions == null || sections[i].Questions.Count == 0) && (sections[i].Children == null || sections[i].Children.Count == 0))
                {
                    sections.RemoveAt(i);
                    i--;
                }

            }

            return sections;
        }

        public List<ApplicationSection> GetApplicationSections(int applicationTypeId, Guid applicationVersionId,  List<ApplicationSectionQuestionAnswerDisplay> displays, ICollection<ApplicationQuestionNotApplicable> notApplicables = null, List<ApplicationSection> allApplicationSections = null)
        {
            var flat = allApplicationSections ?? base.Repository.GetFlatForApplicationType(applicationTypeId, applicationVersionId).OrderBy(x => x.Order).ToList();

            if (notApplicables != null)
            {
                foreach (var notApplicable in notApplicables)
                {
                    var item =
                        flat.FirstOrDefault(
                            x => x.Questions.Any(y => y.Id == notApplicable.ApplicationSectionQuestionId));

                    if (item != null)
                    {
                        flat.Remove(item);
                    }
                }
            }

            var root = flat.Where(x => x.ParentApplicationSectionId == null).ToList();

            foreach (var rootItem in root)
            {
                var childrens = new List<ApplicationSection>();

                if (rootItem.Questions != null && rootItem.Questions.Count > 0 && rootItem.Children != null && rootItem.Children.Count > 0)
                {
                    foreach (var question in rootItem.Questions)
                    {
                        question.HiddenBy = displays.Where(x => x.HidesQuestionId == question.Id).ToList();

                        foreach (var answer in question.Answers)
                        {
                            answer.Displays = displays.Where(x => x.ApplicationSectionQuestionAnswerId == answer.Id).ToList();
                        }
                    }

                    childrens.Add(new ApplicationSection
                    {
                        Id = rootItem.Id,
                        Name = rootItem.Name,
                        IsActive = rootItem.IsActive,
                        HelpText = rootItem.HelpText,
                        IsVariance = rootItem.IsVariance,
                        Version = rootItem.Version,
                        Order = rootItem.Order,
                        UniqueIdentifier = rootItem.UniqueIdentifier,
                        ApplicationSectionScopeTypes = rootItem.ApplicationSectionScopeTypes,
                        Questions = rootItem.Questions,
                        Children = new List<ApplicationSection>()
                    });

                }

                childrens.AddRange(this.BuildChildren(flat, displays, rootItem.Id));

                rootItem.Children = childrens;
            }

            return root;
        }

        private List<ApplicationSection> BuildChildren(List<ApplicationSection> allSections, List<ApplicationSectionQuestionAnswerDisplay> displays, Guid parentId)
        {
            var children = allSections.Where(x => x.ParentApplicationSectionId == parentId && x.IsActive).ToList();

            foreach (var child in children)
            {
                var childrens = new List<ApplicationSection>();

                foreach (var question in child.Questions)
                {
                    question.HiddenBy = displays.Where(x => x.HidesQuestionId == question.Id).ToList();

                    foreach (var answer in question.Answers)
                    {
                        answer.Displays = displays.Where(x => x.ApplicationSectionQuestionAnswerId == answer.Id).ToList();
                    }
                }

                if (child.Questions != null && child.Questions.Count > 0 && child.Children != null && child.Children.Count > 0)
                {
                    foreach (var question in child.Questions)
                    {
                        question.HiddenBy = displays.Where(x => x.HidesQuestionId == question.Id).ToList();

                        foreach (var answer in question.Answers)
                        {
                            answer.Displays = displays.Where(x => x.ApplicationSectionQuestionAnswerId == answer.Id).ToList();
                        }
                    }

                    childrens.Add(new ApplicationSection
                    {
                        Id = child.Id,
                        Name = child.Name,
                        IsActive = child.IsActive,
                        HelpText = child.HelpText,
                        IsVariance = child.IsVariance,
                        Version = child.Version,
                        Order = child.Order,
                        UniqueIdentifier = child.UniqueIdentifier,
                        ApplicationSectionScopeTypes = child.ApplicationSectionScopeTypes,
                        Questions = child.Questions,
                        Children = new List<ApplicationSection>()
                    });

                }

                childrens.AddRange(this.BuildChildren(allSections, displays, child.Id));

                child.Children = childrens;
            }

            return children;
        }

        public List<ApplicationSection> GetAllActiveForApplicationType(int applicationTypeId)
        {
            return base.Repository.GetAllActiveForApplicationType(applicationTypeId);
        }

        public List<ApplicationSection> GetAllForVersion(Guid versionId)
        {
            return base.Repository.GetAllForVersion(versionId);
        }

        public Task<List<ApplicationSection>> GetAllActiveForApplicationTypeAsync(int applicationTypeId)
        {
            return base.Repository.GetAllActiveForApplicationTypeAsync(applicationTypeId);
        }

        public List<ApplicationSection> GetAllForActiveVersionsNoLazyLoad()
        {
            return base.Repository.GetAllForActiveVersionsNoLazyLoad();
        }

        public List<ApplicationSection> GetAllForVersionNoLazyLoad(Guid versionId)
        {
            return base.Repository.GetAllForVersionNoLazyLoad(versionId);
        }

        public List<ApplicationSection> GetAllForApplicationTypeNoLazyLoad(string applicationTypeName)
        {
            return base.Repository.GetAllForApplicationTypeNoLazyLoad(applicationTypeName);
        }

        public List<ApplicationSectionRootItem> GetRootItemStatuses(Application application, string roleName, List<SiteScopeType> scopeTypeList)
        {
            var flatSections = this.GetFlatForApplicationType(application.ApplicationTypeId, application.ApplicationVersionId.GetValueOrDefault());

            var sections = flatSections.Where(x => x.ParentApplicationSectionId == null);

            var result = new List<ApplicationSectionRootItem>();

            foreach (var section in sections.OrderBy(x=>Convert.ToInt32(x.Order)))
            {

                if (!section.IsActive) continue;

                var root = new ApplicationSectionRootItem
                {
                    ApplicationSectionId = section.Id,
                    UniqueIdentifier = section.UniqueIdentifier,
                    Name = section.Name,
                    Items = new List<ApplicationSectionItemStatus>()
                };

                var statusItem = new ApplicationSectionItemStatus();

                if (section.Questions != null && section.Questions.Count > 0)
                {
                    #region "if root section has questions"
                    var item = this.ProcessQuestions(application, section, scopeTypeList, roleName);

                    if (item != null)
                    {
                        var record = new ApplicationSectionItemStatus
                        {
                            ApplicationSectionId = section.Id,
                            UniqueIdentifier = section.UniqueIdentifier,
                            StatusName = item.StatusName
                            /*StatusName =
                                roleName == Constants.Roles.User || roleName == Constants.Roles.OrganizationalDirector
                                    ? GetStatusForApplicant(item.StatusName, application.ApplicationStatus)
                                    : GetStatusForNonApplicant(item.StatusName, application.ApplicationStatus)*/
                        };

                        if (item.HasCitationNotes)
                        {
                            record.HasCitationNotes = true;
                        }

                        if (item.HasAttachments)
                        {
                            record.HasAttachments = true;
                        }

                        if (item.HasSuggestions)
                        {
                            record.HasSuggestions = true;
                        }

                        if (item.HasRFIComments)
                        {
                            record.HasRFIComments = true;
                        }

                        if (item.HasFACTOnlyComments)
                        {
                            record.HasFACTOnlyComments = true;
                        }

                        if (item.IsFlag)
                        {
                            record.IsFlag = true;
                        }

                        if (item.StatusName != null)
                        {
                            root.Items.Add(record);
                            //result.Add(root);
                            //continue;
                        }
                    }
                    #endregion
                }

                if ((section.Children != null) && (section.Children.Count > 0))
                {
                    foreach (var child in section.Children.OrderBy(x => Convert.ToInt32(x.Order)))
                    {
                        #region "if root section has children"
                        if (!child.IsActive) continue;

                        if (child.Questions != null && child.Questions.Count > 0)
                        {
                            statusItem = new ApplicationSectionItemStatus
                            {
                                ApplicationSectionId = child.Id,
                                UniqueIdentifier = child.UniqueIdentifier
                            };

                            var items = this.BuildItemStatuses(application, flatSections, child, scopeTypeList, roleName);

                            if (items != null)
                            {

                                if (items.HasCitationNotes)
                                {
                                    statusItem.HasCitationNotes = true;
                                }

                                if (items.HasAttachments)
                                {
                                    statusItem.HasAttachments = true;
                                }

                                if (items.HasSuggestions)
                                {
                                    statusItem.HasSuggestions = true;
                                }

                                if (items.HasRFIComments)
                                {
                                    statusItem.HasRFIComments = true;
                                }

                                if (items.HasFACTOnlyComments)
                                {
                                    statusItem.HasFACTOnlyComments = true;
                                }

                                if (items.IsFlag)
                                {
                                    statusItem.IsFlag = true;
                                }

                                statusItem.StatusName = items.StatusName;// ?? Constants.ApplicationSectionStatusView.Complete;
                                //the following line caused issues in status view of User.
                                //statusItem.StatusName = roleName == Constants.Roles.User || roleName == Constants.Roles.OrganizationalDirector ? GetStatusForApplicant(items.StatusName, application.ApplicationStatus) : GetStatusForNonApplicant(items.StatusName, application.ApplicationStatus);

                                root.Items.Add(statusItem);
                            }
                        }

                        if (child.Children == null || child.Children.Count == 0) continue;

                        foreach (var nextChild in child.Children.OrderBy(x=>Convert.ToInt32(x.Order)))
                        {
                            if (!nextChild.IsActive)
                            {
                                continue;
                            }

                            statusItem = new ApplicationSectionItemStatus
                            {
                                ApplicationSectionId = nextChild.Id,
                                UniqueIdentifier = nextChild.UniqueIdentifier
                            };

                            var nextItems = this.BuildItemStatuses(application, flatSections, nextChild, scopeTypeList, roleName);

                            if (nextItems != null)
                            {
                                if (nextItems.HasCitationNotes)
                                {
                                    statusItem.HasCitationNotes = true;
                                }

                                if (nextItems.HasAttachments)
                                {
                                    statusItem.HasAttachments = true;
                                }

                                if (nextItems.HasSuggestions)
                                {
                                    statusItem.HasSuggestions = true;
                                }

                                if (nextItems.HasRFIComments)
                                {
                                    statusItem.HasRFIComments = true;
                                }

                                if (nextItems.HasFACTOnlyComments)
                                {
                                    statusItem.HasFACTOnlyComments = true;
                                }

                                if (nextItems.IsFlag)
                                {
                                    statusItem.IsFlag = true;
                                }

                                statusItem.StatusName = nextItems.StatusName;// ?? Constants.ApplicationSectionStatusView.Complete;
                                //statusItem.StatusName = roleName == Constants.Roles.User || roleName == Constants.Roles.OrganizationalDirector ? GetStatusForApplicant(nextItems.StatusName, application.ApplicationStatus) : GetStatusForNonApplicant(nextItems.StatusName, application.ApplicationStatus);

                                root.Items.Add(statusItem);
                            }
                        }
                        #endregion
                    }
                }

                if (root.Items.Count > 0)
                    result.Add(root);
            }

            return result;
        }

        public List<ApplicationSection> GetSectionsWithRFIs(int applicationId)
        {
            return this.Repository.GetSectionsWithRFIs(applicationId);
        }

        public List<ApplicationSiteResponse> GetApplicationSectionsForApplication(Guid? complianceApplicationId, Guid? applicationUniqueId, Guid currentUserId, List<SiteScopeType> scopeTypes = null)
        {
            var siteResponses = new List<ApplicationSiteResponse>();

            var records = this.Repository.GetApplicationSectionsForApplication(complianceApplicationId, applicationUniqueId, currentUserId);

            //if (scopeTypes != null)
            //{
            //    for (var i = 0; i < records.Count; i++)
            //    {
            //        if (string.IsNullOrWhiteSpace(records[i].ScopeTypes)) continue;

            //        var scopes = scopeTypes.Where(x => x.SiteId == records[i].SiteId).ToList();

            //        if (scopes.All(x => !records[i].ScopeTypes.Contains(x.ScopeTypeId.ToString() + ",")))
            //        {
            //            records.RemoveAt(i);
            //            i--;
            //        }
            //    }
            //}

            foreach (var record in records)
            {
                var nextSection = this.BuildNextSection(records, record.ApplicationId, record.ApplicationSectionId);

                if (nextSection == null) continue;

                record.NextSection = new ApplicationSectionResponse
                {
                    ApplicationSectionId = nextSection.ApplicationSectionId,
                    ApplicationSectionName = nextSection.ApplicationSectionName,
                    ApplicationSectionHelpText = nextSection.ApplicationSectionHelpText,
                    ApplicationSectionUniqueIdentifier = nextSection.ApplicationSectionUniqueIdentifier,
                    HasQuestions = nextSection.HasQuestions,
                    IsVisible = nextSection.IsVisible,
                    HasFlag = nextSection.HasFlag,
                    SectionStatusName = nextSection.SectionStatusName,
                    HasRfiComment = nextSection.HasRfiComment,
                    HasCitationComment = nextSection.HasCitationComment,
                    HasFactOnlyComment = nextSection.HasFactOnlyComment,
                    HasSuggestions = nextSection.HasSuggestions
                };
            }

            var sites = records.GroupBy(x => new { x.SiteName, x.SiteId}).ToList();

            foreach (var site in sites)
            {
                var siteRows = records.Where(x => x.SiteName == site.Key.SiteName).ToList();

                var siteResponse = new ApplicationSiteResponse
                {
                    SiteName = site.Key.SiteName,
                    SiteId = site.Key.SiteId,
                    AppResponses = new List<AppResponse>(),
                    SubmittedDate = records.First()?.SubmittedDate,
                    DueDate = records.First()?.DueDate,
                    InspectionDate = records.First()?.InspectionDate,
                    HasFlags = records.Any(x=>x.HasFlag),
                    HasRfis = records.Any(x=>x.SectionStatusName == Constants.ApplicationResponseStatus.RFI),
                    IsReviewer = records.Any(x=>x.IsReviewer),
                    IsInspectionResponsesComplete = records.First()?.IsInspectionResponsesComplete ?? false
                };

                var apps = siteRows.GroupBy(x => x.ApplicationId).ToList();

                foreach (var app in apps)
                {

                    var rows = records.Where(x => x.ApplicationId == app.Key).ToList();

                    var results = new List<ApplicationSectionResponse>();

                    results.AddRange(rows.Where(x => x.ParentApplicationSectionId == null));

                    var first = rows.FirstOrDefault();

                    foreach (var result in results)
                    {
                        result.Children = this.BuildChildrenForApplication(result, rows);
                    }

                    siteResponse.AppResponses.Add(new AppResponse
                    {
                        ApplicationId = app.Key,
                        ApplicationUniqueId = first?.ApplicationUniqueId ?? new Guid(),
                        ApplicationTypeName = first?.ApplicationTypeName,
                        ApplicationSectionResponses = results
                    });
                }

                siteResponses.Add(siteResponse);
            }

            

            return siteResponses;
        }
        

        private ApplicationSectionResponse BuildNextSection(List<ApplicationSectionResponse> allRecords, int applicationId, Guid sectionId)
        {
            var found = false;

            for (var i = 0; i < allRecords.Count; i++)
            {
                if (found)
                {
                    if (allRecords[i].IsVisible && allRecords[i].HasQuestions && allRecords[i].ApplicationId == applicationId)
                    {
                        return allRecords[i];
                    }
                }
                else if (allRecords[i].ApplicationSectionId == sectionId && allRecords[i].ApplicationId == applicationId)
                {
                    found = true;
                }
            }

            return null;
        }

        private List<ApplicationSectionResponse> BuildChildrenForApplication(ApplicationSectionResponse row,
            List<ApplicationSectionResponse> fullList)
        {
            var results = new List<ApplicationSectionResponse>();

            results.AddRange(fullList.Where(x => x.ParentApplicationSectionId == row.ApplicationSectionId));

            if (row.HasQuestions && results.Count > 0)
            {
                var record = new ApplicationSectionResponse
                {
                    ApplicationId = row.ApplicationId,
                    ApplicationUniqueId = row.ApplicationUniqueId,
                    ApplicationTypeName = row.ApplicationTypeName,
                    SiteId = row.SiteId,
                    SiteName = row.SiteName,
                    SubmittedDate = row.SubmittedDate,
                    DueDate = row.DueDate,
                    InspectionDate = row.InspectionDate,
                    ApplicationSectionId = row.ApplicationSectionId,
                    ParentApplicationSectionId = row.ParentApplicationSectionId,
                    ApplicationSectionName = row.ApplicationSectionName,
                    ApplicationSectionHelpText = row.ApplicationSectionHelpText,
                    ApplicationSectionUniqueIdentifier = row.ApplicationSectionUniqueIdentifier,
                    HasQuestions = row.HasQuestions,
                    IsVisible = row.IsVisible,
                    HasFlag = row.HasFlag,
                    SectionStatusName = row.SectionStatusName,
                    HasRfiComment = row.HasRfiComment,
                    HasCitationComment = row.HasCitationComment,
                    HasFactOnlyComment = row.HasFactOnlyComment,
                    HasSuggestions =  row.HasSuggestions,
                    OrganizationName = row.OrganizationName,
                    HasQmRestrictions = row.HasQmRestrictions,
                    IsReviewer = row.IsReviewer,
                    IgnoreChildren = true,
                    NextSection = row.NextSection
                };
                results.Insert(0, record);
            }

            foreach (var result in results.Where(x=>x.IgnoreChildren == false))
            {
                result.Children = this.BuildChildrenForApplication(result, fullList);
            }

            return results;
        }


        private ApplicationSectionItemStatus BuildItemStatuses(Application application, List<ApplicationSection> sections, ApplicationSection section, List<SiteScopeType> scopeTypeList, string roleName = "")
        {
            var statusItem = new ApplicationSectionItemStatus();

            var sect = sections.SingleOrDefault(x => x.Id == section.Id);

            if (sect == null)
            {
                statusItem.StatusName = Constants.ApplicationSectionStatusView.NotStarted;
                return statusItem;
            }
            //statusItem.ApplicationSectionId = sect.Id;
            if (sect.Questions != null && sect.Questions.Count > 0)
            {
                var item = this.ProcessQuestions(application, sect, scopeTypeList, roleName);

                if (item == null)
                    return null;

                if (item.HasCitationNotes)
                {
                    statusItem.HasCitationNotes = true;
                }

                if (item.HasAttachments)
                {
                    statusItem.HasAttachments = true;
                }

                if (item.HasSuggestions)
                {
                    statusItem.HasSuggestions = true;
                }

                if (item.HasRFIComments)
                {
                    statusItem.HasRFIComments = true;
                }

                if (item.HasFACTOnlyComments)
                {
                    statusItem.HasFACTOnlyComments = true;
                }

                if (item.StatusName != null)
                {
                    statusItem.StatusName = item.StatusName;

                    return statusItem;
                }

            }

            if (sect.Children == null || sect.Children.Count == 0)
            {
                return null;
                //return statusItem;
            }

            foreach (var items in sect.Children.Select(child => this.BuildItemStatuses(application, sections, child, scopeTypeList, roleName)))
            {
                if (items == null)
                    continue;

                if (items.HasCitationNotes)
                {
                    statusItem.HasCitationNotes = true;
                }

                if (items.HasAttachments)
                {
                    statusItem.HasAttachments = true;
                }

                if (items.HasRFIComments)
                {
                    statusItem.HasRFIComments = true;
                }

                if (items.HasFACTOnlyComments)
                {
                    statusItem.HasFACTOnlyComments = true;
                }

                if (items.HasSuggestions)
                {
                    statusItem.HasSuggestions = true;
                }

                if (items.StatusName == null) continue;

                statusItem.StatusName = items.StatusName;
                break;
            }

            return statusItem;
        }

        private ApplicationSectionItemStatus ProcessQuestions(Application application, ApplicationSection section, List<SiteScopeType> scopeTypeList, string roleName = "")
        {
            var statusItem = new ApplicationSectionItemStatus(){ StatusName= "" };

            var allResponsesExist = true;
            var isUser = (roleName == Constants.Roles.User || roleName == Constants.Roles.OrganizationalDirector) ? true : false;

            if (application.ApplicationType.Name != Constants.ApplicationTypes.Annual &&
                application.ApplicationType.Name != Constants.ApplicationTypes.Eligibility &&
                application.ApplicationType.Name != Constants.ApplicationTypes.Renewal)
            {
                var sectionList = new List<ApplicationSection> {section};

                this.HideQuestionsOutofScope(sectionList, scopeTypeList);
            }

            if (section.Questions.Count == 0)
                return null;

            if (
                section.Questions.Any(
                    x =>
                        x.ApplicationResponses.Any(
                            y => y.ApplicationId == application.Id && y.DocumentId != null)))
            {
                statusItem.HasAttachments = true;
            }

            if (
                section.Questions.Any(
                    x =>
                        x.ApplicationResponseComments.Any(y => y.ApplicationId == application.Id && y.CommentTypeId == 3)))
            {
                statusItem.HasSuggestions = true;
            }

            if (
                section.Questions.Any(
                    x =>
                        x.ApplicationResponseComments.Any(
                            y => y.ApplicationId == application.Id && y.CommentTypeId == 1)))
            {
                statusItem.HasRFIComments = true;
            }

            if (
                section.Questions.Any(
                    x =>
                        x.ApplicationResponseComments.Any(
                            y => y.ApplicationId == application.Id && y.CommentTypeId == 6)))
            {
                statusItem.HasFACTOnlyComments = true;
            }

            if (
                section.Questions.Any(
                    x =>
                        x.ApplicationResponseComments.Any(
                            y => y.ApplicationId == application.Id && y.CommentTypeId == 2)))
            {
                statusItem.HasCitationNotes = true;
            }

            if (
                section.Questions.Any(
                    x =>
                        x.ApplicationResponses.Any(
                            y => y.ApplicationId == application.Id && y.Flag == true)))
            {
                statusItem.IsFlag = true;
            }
            
            //Filter only those questions which have a response recorded.
            var questionResponsesForApplication =
                section.Questions.Where(x => x.ApplicationResponses.Any(y => y.ApplicationId == application.Id))
                    .ToList();
            
            var responses = new List<ApplicationResponse>();
            foreach (var question in questionResponsesForApplication) 
            {
                responses.AddRange(question.ApplicationResponses.Where(x => x.ApplicationId == application.Id));            
            }
            //Check for No Responses at all.
            if (responses.Count == 0)
            {
                //If none of the questions have any responses then set status to NotStarted
                if(isUser) statusItem.StatusName = Constants.ApplicationSectionStatusView.NotStarted;
                else statusItem.StatusName = Constants.ApplicationSectionStatusView.NotStarted;
            }
            
            //TODO: How to check if the user has answered all non-hidden questions ? 
            
            //See [1459] for the order of precedence of statuses.
            if (statusItem.StatusName == "") SetStatusItem(application, section, statusItem, Constants.ApplicationSectionStatusView.RFI, isUser);
            if (statusItem.StatusName == "") SetStatusItem(application, section, statusItem, Constants.ApplicationSectionStatusView.ForReview, isUser);
            if (statusItem.StatusName == "") SetStatusItem(application, section, statusItem, Constants.ApplicationSectionStatusView.New, isUser);
            if (statusItem.StatusName == "") SetStatusItem(application, section, statusItem, Constants.ApplicationSectionStatusView.NotCompliant, isUser);
            if (statusItem.StatusName == "") SetStatusItem(application, section, statusItem, Constants.ApplicationSectionStatusView.RFIFollowUp, isUser);
            if (statusItem.StatusName == "") SetStatusItem(application, section, statusItem, Constants.ApplicationSectionStatusView.RFICompleted, isUser);
            if (statusItem.StatusName == "") SetStatusItem(application, section, statusItem, Constants.ApplicationSectionStatusView.Reviewed, isUser);
            if (statusItem.StatusName == "") SetStatusItem(application, section, statusItem, Constants.ApplicationSectionStatusView.Compliant, isUser);
            //this.setCircle2(row, this.config.applicationSectionStatuses.complete); //complete is only relevant for applicant
            //this.setCircle2(row, this.config.applicationSectionStatuses.notStarted); //complete is only relevant for applicant
            if (statusItem.StatusName == "") SetStatusItem(application, section, statusItem, Constants.ApplicationSectionStatusView.NotApplicable, isUser);
            if (statusItem.StatusName == "") SetStatusItem(application, section, statusItem, Constants.ApplicationSectionStatusView.NoResponseRequested, isUser);

            if (statusItem.StatusName == "") {
                if (isUser) {
                    statusItem.StatusName = Constants.ApplicationStatus.Complete;
                    if (allResponsesExist) { 
                        statusItem.StatusName = Constants.ApplicationStatus.Complete; //if the user answered all questions
                    } else statusItem.StatusName = Constants.ApplicationStatus.InProgress; ///the user has answered some questions
                }
                else {
                    if (allResponsesExist) {
                        statusItem.StatusName = Constants.ApplicationStatus.ForReview; //if the user answered all questions
                    }
                    else statusItem.StatusName = Constants.ApplicationStatus.InProgress; ///the user has answered some questions. Ideally the application shud never get to FACT if all questions are not answered.
                }
            }
            
            return statusItem;
        }

        private void SetStatusItem(Application application, ApplicationSection section, ApplicationSectionItemStatus statusItem, string sectionStatusView, bool isUser = false)
        {
            if (isUser)
            {
                //for user, check visible application status for all active questions
                if (section.Questions.Any(
                    q => q.IsActive &&
                    q.ApplicationResponses.Any(
                        r => r.ApplicationId == application.Id &&
                        r.VisibleApplicationResponseStatus != null &&
                        r.VisibleApplicationResponseStatus.Name == sectionStatusView)))
                {
                    statusItem.StatusName = sectionStatusView;
                }
            }
            else
            {
                //for fact, check application response status
                if (section.Questions.Any(
                    q => q.IsActive &&
                    q.ApplicationResponses.Any(
                        r => r.ApplicationId == application.Id &&
                        r.ApplicationResponseStatus != null &&
                        r.ApplicationResponseStatus.Name == sectionStatusView)))
                {
                    statusItem.StatusName = sectionStatusView;
                }                
            }

        }

        private void HideQuestionsOutofScope(List<ApplicationSection> sections, List<SiteScopeType> scopeTypeList)
        {
            foreach (var section in sections)
            {
                section.Questions = section.Questions.Where(
                        question =>
                            question != null && question.ScopeTypes != null && question.IsActive == true &&
                            scopeTypeList.Any(x => question.ScopeTypes.Any(y => y != null && y.ScopeTypeId == x.ScopeTypeId)))
                        .ToList();

                if (section.Children != null && section.Children.Count > 0)
                {
                    HideQuestionsOutofScope(section.Children.ToList(), scopeTypeList);
                }
            }
        }
    }
}



