using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FISH.DataAccess.Entities
{
    public interface IEntityBase
    {
        string Creator { get; set; }
        DateTime CreateDate { get; set; }
        string LastModifiedBy { get; set; }
        DateTime LastModifyDate { get; set; }

        string GetNowUser();
    }
}
