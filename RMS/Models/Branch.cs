using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }

        
        public string? name { get; set; }
        public bool? active { get; set; }

    }
}
