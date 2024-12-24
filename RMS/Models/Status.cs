﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
   
        public string? Name { get; set; }

    }

}