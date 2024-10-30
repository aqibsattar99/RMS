using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class BranchUsers
    {
        [Key]
        public int Id { get; set; }
   
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? Rolesid { get; set; }

        public string? Designation { get; set; }
        public bool? Active { get; set; }
        public DateTime? Updatedon { get; set; }


        public virtual Roles? Roles { get; set; }

    }



}
