using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using FactWeb.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class UserMembershipManager : BaseManager<UserMembershipManager, IUserMembershipRepository, UserMembership>
    {
        public UserMembershipManager(IUserMembershipRepository repository) : base(repository)
        {
        }

        public void UpdateUserMembership(Guid userId, List<UserMembershipItem> UserMemberships, string updatedBy)
        {
            LogMessage("UpdateUserMembership (UserMembershipManager)");

            var existingMemberships = base.Repository.GetAllByUserId(userId);

            foreach (var item in existingMemberships)
            {
                base.BatchRemove(item);
            }

            foreach (var objUserMembership in UserMemberships.Where(x=>x.IsSelected.GetValueOrDefault()))
            {
                UserMembership userMembership = new UserMembership();
                userMembership.MembershipId = objUserMembership.Membership.Id;
                userMembership.UserId = userId;
                userMembership.MembershipNumber = objUserMembership.MembershipNumber;
                userMembership.CreatedBy = updatedBy;
                userMembership.CreatedDate = DateTime.Now;
                userMembership.UpdatedBy = updatedBy;
                userMembership.UpdatedDate = DateTime.Now;

                base.BatchAdd(userMembership);
            }

            base.Repository.SaveChanges();
        }

        public async Task UpdateUserMembershipAsync(Guid userId, List<UserMembershipItem> UserMemberships, string updatedBy)
        {
            LogMessage("UpdateUserMembershipAsync (UserMembershipManager)");

            var existingMemberships = base.Repository.GetAllByUserId(userId);

            foreach (var item in existingMemberships)
            {
                base.BatchRemove(item);
            }

            foreach (var objUserMembership in UserMemberships)
            {
                UserMembership userMembership = new UserMembership();
                userMembership.MembershipId = objUserMembership.Membership.Id;
                userMembership.UserId = userId;
                userMembership.MembershipNumber = objUserMembership.MembershipNumber;
                userMembership.CreatedBy = updatedBy;
                userMembership.CreatedDate = DateTime.Now;
                userMembership.UpdatedBy = updatedBy;
                userMembership.UpdatedDate = DateTime.Now;

                base.BatchAdd(userMembership);
            }

            await base.Repository.SaveChangesAsync();
        }
    }
}
