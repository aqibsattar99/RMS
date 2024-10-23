using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class BranchUsers
    {
        [Key]
        public int id { get; set; }
   
        public string? name { get; set; }
        public string? designation { get; set; }
        public bool? active { get; set; }
        public DateTime? updatedon { get; set; }


    }



}
