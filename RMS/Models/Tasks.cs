using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }

        
       
        public int? branchid { get; set; }
        public string? eqptrepair { get; set; }
        public string? assigned { get; set; }
        public DateTime? completiondate { get; set; }
        public string? problem { get; set; }
        public bool? status { get; set; }

        public virtual Branch? Branch { get; set; } 
       

    }


    public class TasksVM
    {

         public int Id { get; set; }

        public string? branch { get; set; }
        public string? eqptrepair { get; set; }
        public string? assigned { get; set; }
        public DateTime? completiondate { get; set; }
        public string? problem { get; set; }
        public bool? status { get; set; }

    

    }

}
