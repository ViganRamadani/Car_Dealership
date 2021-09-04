using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Dealership.Models
{
    public class Favorite
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Auto_Id")]
        public int Auto_Id { get; set; }
        /*public object Auto_id { get; internal set; }*/
        //public Auto Auto { get; set; }
    }
}

