using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace neigh.Models
{
    public class HorseInClassDisplayModel
    {
        [Display(Name = "Show")]
        public int SelectedShowId { get; set; }
        public List<SelectListItem> AllShows;
        public List<HorseInClassDisplayItemModel> Items;

        public HorseInClassDisplayModel()
        {
            AllShows = new List<SelectListItem>();
            Items = new List<HorseInClassDisplayItemModel>();
        }
    }

    public class HorseInClassDisplayItemModel
    {
        [Display(Name = "Horse")]
        public String HorseName { get; set; }
        [Display(Name = "Show")]
        public String ShowName { get; set; }
        public int HorseId { get; set; }
        public int ShowId { get; set; }
    }
}