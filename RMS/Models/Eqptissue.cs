using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class Eqptissue
    {
        [Key]
        public int Id { get; set; }

        
        public DateTime? date { get; set; }
        public int? branchid { get; set; }
        public int? EqptnameId { get; set; }
        public int? qty { get; set; }
        public string? remarks { get; set; }
        public bool? active { get; set; }

        public virtual Branch? Branch { get; set; } 
        public virtual Eqptname? Eqptname { get; set; }

    }


    public class EqptissueVM
    {
  

        public int id { get; set; }
        public DateTime? date { get; set; }
        public string? branch { get; set; }
        public string? eqpt { get; set; }
        public int? qty { get; set; }
        public string? remarks { get; set; }
        public bool? active { get; set; }

    }

}
