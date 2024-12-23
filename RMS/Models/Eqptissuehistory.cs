using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Models
{
    public class Eqptissuehistory
    {
        [Key]
        public int Id { get; set; }
        public int issueid { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? updatedon { get; set; }
        public string? Branch{ get; set; }
        public string? Eqpttype { get; set; }
        public string? Eqptname { get; set; }        
        public int? Qty { get; set; }
        public string? Issueto { get; set; }
        public string? Status { get; set; }
        public string? Condition { get; set; }
        public string? Issuevoucher { get; set; }
        public string? Details { get; set; }
      

   
    }



}
