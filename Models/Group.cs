using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspnet_edu_center.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Group_type")]
        public int Group_type { get; set; }
        [ForeignKey("User")]
        public int Supervisor_id { get; set; }
        public DateTime date_form { get; set; }
        public DateTime date_to { get; set; }
    }
}

