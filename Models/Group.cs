using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace aspnet_edu_center.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Group_type { get; set; }
        public int Supervisor_id { get; set; }
        public DateTime date_form { get; set; }
        public DateTime date_to { get; set; }
    }
}

