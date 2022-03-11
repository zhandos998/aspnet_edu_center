using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspnet_edu_center.Models
{
    public class Tests
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Group")]
        public int Group_id { get; set; }
    }
}

