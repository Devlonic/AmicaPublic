using System.ComponentModel.DataAnnotations.Schema;

namespace Amica.API.Data.Models {
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.ComponentModel;

    public class Account : IdentityUser {
        public virtual ICollection<Profile>? Profiles { get; set; }
        [Obsolete("Transferred to Profile. Do not use this field")]
        public new string? UserName { get; set; }
    }
}
