using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;

namespace FactWeb.BusinessLayer
{
    public class AccessRequestManager : BaseManager<AccessRequestManager, IAccessRequestRepository, AccessRequest>
    {
        public AccessRequestManager(IAccessRequestRepository repository) : base(repository)
        {
        }

        public void Add(string factPortalEmailAddress, string supervisorEmailAddress, string url, AccessRequestItem item, MasterServiceType masterServiceType, string createdBy)
        {
            if (masterServiceType == null && string.IsNullOrWhiteSpace(item.MasterServiceTypeOtherComment))
            {
                throw new Exception("Both Service Type and Service Type Comment cannot be null");
            }

            var accessRequest = new AccessRequest
            {
                Guid = Guid.NewGuid().ToString(),
                OrganizationName = item.OrganizationName,
                DirectorName = item.DirectorName,
                DirectorEmailAddress = item.DirectorEmailAddress,
                PrimaryContactName = item.PrimaryContactName,
                PrimaryContactEmailAddress = item.PrimaryContactEmailAddress,
                PrimaryContactPhoneNumber = item.PrimaryContactPhoneNumber,
                OrganizationAddress = item.OrganizationAddress,
                MasterServiceTypeId = masterServiceType?.Id,
                MasterServiceTypeOtherComment = item.MasterServiceTypeOtherComment,
                OtherComments = item.OtherComments,
                CreatedBy = createdBy ?? "New",
                CreatedDate = DateTime.Now
            };

            base.Repository.Add(accessRequest);

            var getUrl = url + "app/email.templates/requestAccess.html";

            var html = WebHelper.GetHtml(getUrl);
            html = string.Format(html, item.OrganizationName, item.DirectorName, item.DirectorEmailAddress,
                item.PrimaryContactName, item.PrimaryContactEmailAddress, item.PrimaryContactPhoneNumber,
                item.OrganizationAddress, item.ServiceType, item.MasterServiceTypeOtherComment, item.OtherComments);

            EmailHelper.Send(new List<string> { factPortalEmailAddress }, new List<string> { supervisorEmailAddress }, "Request For New Organization Access", html);
        }
    }
}
