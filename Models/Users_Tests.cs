using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspnet_edu_center.Models
{
    public class Users_Tests
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Test_id { get; set; }
    }
}

