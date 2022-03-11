using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspnet_edu_center.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public int Test_id { get; set; }
        public string Question_title { get; set; }
    }
}

