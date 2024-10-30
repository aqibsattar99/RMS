using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class Roles
    {
        [Key]
        public int Id { get; set; }
   
        public string? Role { get; set; }

    }

}
