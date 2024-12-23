using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Models
{
    public class Eqptissue
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? Branchid { get; set; }
        public int? EqptId { get; set; }
        public int? StatusId { get; set; }
        public string? Eqptname { get; set; }        
        public int? Qty { get; set; }
        public string? Issueto { get; set; }
        public int? Conditionid { get; set; }
        public string? Issuevoucher { get; set; }
        public string? Details { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("Branchid")]
        public virtual Branch? Branch { get; set; }

        [ForeignKey("EqptId")]  // Foreign key for Eqpttype
        public virtual Eqpttype? Eqpttype { get; set; }

        [ForeignKey("Conditionid")]  // Foreign key for Eqpttype
        public virtual Eqptcondition? Eqptcondition { get; set; }

        [ForeignKey("StatusId")]  // Foreign key for Eqpttype
        public virtual Status? Status { get; set; }


    }


    public class EqptissueVM
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string? DateFormat { get; set; }
        public string? Branch { get; set; }
        public string? Eqpttypename { get; set; }
        public string? Eqptname { get; set; }
        
        public string? Status { get; set; }
        public int? Qty { get; set; }
        public string? Issueto { get; set; }
        public string? Condition { get; set; }
        public string? Issuevoucher { get; set; }
        public string? Details { get; set; }
        public bool? Active { get; set; }

    }

    public class EquipmentIssueRequest
    {
        public string Date { get; set; }
        public string Issuevoucher { get; set; }
        public int Branchid { get; set; }
        public List<EquipmentItem> Items { get; set; }
    }

    public class EquipmentItem
    {
        
        public string StatusId { get; set; }
        public string Conditionid { get; set; }
        public string EqptId { get; set; }
        public string Eqptname { get; set; }
        public string Qty { get; set; }
        public string Issueto { get; set; }
        public string Details { get; set; }
    }



    public class EquipPrintVM
    {
        public string? Branchid { get; set; }
        public string? Conditionid { get; set; }
        public string? EqptId { get; set; }
       
        public DateTime? Dateto { get; set; }
        public DateTime? Datefrom { get; set; }
    }



}
