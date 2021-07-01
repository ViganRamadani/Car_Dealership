using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Dealership.Models
{
    public class Favorite
    {
        public Guid Id { get; set; }
        public ApplicationUser User{get; set;}

        public Guid UserId { get ; set;}
        public int Auto_Id { get; set; }
        public Auto Auto { get; set; }
    }
}

    