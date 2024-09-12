using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class Eqptname
    {
        [Key]
        public int Id { get; set; }

        
        public string? name { get; set; }
        public bool? active { get; set; }

    }
}
