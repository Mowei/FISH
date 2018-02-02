using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace FISH.DataAccess.Entities
{
    public class EntityBase : IEntityBase
    {
        [Column(TypeName = "nvarchar(50)")]
        public string Creator { get; set; } = "None";

        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Column(TypeName = "nvarchar(50)")]
        public string LastModifiedBy { get; set; } = "None";

        public DateTime LastModifyDate { get; set; } = DateTime.Now;

        public EntityBase()
        {
            Creator = GetNowUser();
            LastModifiedBy = GetNowUser();
        }

        public static IHttpContextAccessor _contextAccessor;

        public static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;

        }

        public string GetNowUser()
        {
            return _contextAccessor.HttpContext.User.Identity.Name ?? "None";
        }
    }
}
