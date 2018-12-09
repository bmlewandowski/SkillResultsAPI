using System;
using System.Collections.Generic;

namespace SkillResultsAPI.Models
{
    // Models returned by AccountController actions.


    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserObjectViewModel
    {
        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public string UserPhoneNumber { get; set; }

        public string UserName { get; set; }

        public DateTime UserCreatedOn { get; set; }

        public int IsAdmin { get; set; }

        public int OrgId { get; set; }

        public string OrgName { get; set; }

        public DateTime OrgCreatedOn { get; set; }

    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
