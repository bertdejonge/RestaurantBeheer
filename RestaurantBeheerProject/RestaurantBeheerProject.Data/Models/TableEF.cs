﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Datalayer.Models {
    public class TableEF {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableID { get; set; }

        [Required]
        public int TableNumber { get; set; }

        [Required]
        public int Seats {  get; set; }

        [ForeignKey("RestaurantID")]
        public int RestaurantID { get; set; }

        public RestaurantEF restaurant { get; set; }
    }
}
