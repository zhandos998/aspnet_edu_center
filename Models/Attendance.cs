using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspnet_edu_center.Models
{
    public class Attendance
    {
        [ForeignKey("User")]
        public int User_id { get; set; }
        public DateTime Date { get; set; }
        public bool Camed { get; set; }
    }
}

