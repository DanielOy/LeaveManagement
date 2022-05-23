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
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "29302d49-39e3-45c7-8fe0-30a2413b6135",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                },
                new IdentityRole
                {
                    Id = "81dcb577-b543-437a-91f3-6554cf3fcc58",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
                );
        }
    }
}
