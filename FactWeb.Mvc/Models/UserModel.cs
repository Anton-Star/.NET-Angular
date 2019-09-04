using FactWeb.Model.InterfaceItems;
using System;

namespace FactWeb.Mvc.Models
{
    public class UserModel
    {
        public UserItem user { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public bool isNewUser { get; set; }
        public bool AddToExistingUser { get; set; }
    }

    public class UserAuditorObserverModel
    {
        public Guid UserId { get; set; }
        public bool IsAuditor { get; set; }
        public bool IsObserver { get; set; }
    }

    public class DistanceModel
    {
        public double Distance { get; set; }
    }
}