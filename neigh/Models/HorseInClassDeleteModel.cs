using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace neigh.Models
{
    public class HorseInClassDeleteModel
    {
        [Display(Name = "Horse")]
        public String HorseName { get; set; }
        [Display(Name = "Show")]
        public String ShowName { get; set; }
        [Display(Name = "Classes")]
        public String Classes { get; set; }
    }
}