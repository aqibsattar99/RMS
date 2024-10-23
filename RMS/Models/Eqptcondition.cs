using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Models
{
   

        public class Eqptcondition
    {
            [Key]
            public int Id { get; set; }
            public string? Condition { get; set; }
          
        }

    

}
