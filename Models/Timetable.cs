using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace aspnet_edu_center.Models
{
    public class Timetable
    {
        public int Id { get; set; }
        public int Group_id { get; set; }
        public int Week_day { get; set; }
        public string Time { get; set; }
    }
}

