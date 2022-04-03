using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspnet_edu_center.Models
{
    public class StudDocument
    {
        public int Id { get; set; }
        public int Document_id { get; set; }
        public int Student_id { get; set; }
        public string Url { get; set; }
        public string Doc_name { get; set; }
        public DateTime created_at { get; set; }
    }
}

