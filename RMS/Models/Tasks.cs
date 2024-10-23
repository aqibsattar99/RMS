using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Models
{
   

        public class Tasks
        {
            [Key]
            public int Id { get; set; }
            public int? Branchid { get; set; }
            public string? Eqptrepair { get; set; }
            public int? Assignedid { get; set; } // Foreign key to BranchUsers
            public DateTime? Completiondate { get; set; }
            public DateOnly? Assigndate { get; set; }
            public string? Problem { get; set; }
            public string? Ionno { get; set; }
            public string? Remarks { get; set; }
            public bool? Status { get; set; }
            public bool? Active { get; set; }




            [ForeignKey("Branchid")]
            public virtual Branch? Branch { get; set; }

            [ForeignKey("Assignedid")] // Define the foreign key relationship
            public virtual BranchUsers? BranchUsers { get; set; }
        }

    


    public class TasksVM
    {

        public int Id { get; set; }
        public string? Branch { get; set; }
        public string? Eqptrepair { get; set; }
        public string? Ionno { get; set; }
        public string? Assigned { get; set; }
        public string? Remarks { get; set; }
        public DateTime? Completiondate { get; set; }
        public DateOnly? Assigndate { get; set; }
        public string? Problem { get; set; }
        public bool? Status { get; set; }
        public bool? Active { get; set; }

    

    }

}
