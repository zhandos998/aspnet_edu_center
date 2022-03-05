using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspnet_edu_center.Models
{
    public class Group_User
    {
        [Key]
        [ForeignKey("Group")]
        public int Group_Id { get; set; }
        [Key]
        [ForeignKey("User")]
        public int User_Id { get; set; }
    }
}

