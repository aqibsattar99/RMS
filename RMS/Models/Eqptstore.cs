using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class Eqptstore
    {
        [Key]
        public int Id { get; set; }

        public DateTime? Date { get; set; }
     
        public int? Eqptid { get; set; }

        public int? Qty { get; set; }
       
        public bool? Active { get; set; }

        public DateTime? Updatedon { get; set; }

        public virtual Eqpttype? Eqpttype { get; set; }

    }

    public class EqptstoreVM
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Eqptname { get; set; }
        public int? Qty { get; set; }
    }
}
