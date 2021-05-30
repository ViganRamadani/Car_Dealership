using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Dealership.Models
{
    public class Auto
    {
        public int Id { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Engine { get; set; }
        [Required]
        public string Body_Type { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Start_Production { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime End_Production { get; set; }
        [Required]
        [MaxLength(256)]
        public string Photo { get; set; }
        [Required]
        public int Sets { get; set; }
        [Required]
        public int Doors { get; set; }
        [Required]
        public int Fuel_Consumption { get; set; }
        [Required]
        public int Fuel_Type { get; set; }
        public int Acceleration { get; set; }
        public int Max_Speed { get; set; }
        [Required]
        public int Power { get; set; }
        [Required]
        public int Torque { get; set; }
        
    }
}
