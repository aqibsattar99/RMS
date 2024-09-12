using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class Eqptstore
    {
        [Key]
        public int Id { get; set; }

        public DateTime? date { get; set; }
     
        public int? eqptid { get; set; }
        public int? qty { get; set; }
       
        public bool? active { get; set; }

        public DateTime? updatedon { get; set; }

        public virtual Eqptname? Eqptname { get; set; }

    }

    public class EqptstoreVM
    {
        public int Id { get; set; }

        public DateTime? date { get; set; }

        public string? eqptname { get; set; }
        public int? qty { get; set; }

    

      

    }
}
