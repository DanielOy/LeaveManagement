using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "81dcb577-b543-437a-91f3-6554cf3fcc58",
                    UserId = "2abae296-a7b2-4e6a-9d74-8d079a63ef38"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "29302d49-39e3-45c7-8fe0-30a2413b6135",
                    UserId = "6e039f2a-fe7a-49b8-9245-3e8bf40173b2"
                });
        }
    }
}
