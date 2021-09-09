using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace neigh.Models
{
    public class HorseInClassEditModel
    {
        [Display(Name = "Horse")]
        public int SelectedHorseId { get; set; }
        public List<SelectListItem> AllHorses;

        [Display(Name = "Show")]
        public int SelectedShowId { get; set; }
        public List<SelectListItem> AllShows;

        [Display(Name = "Classes")]
        public List<SelectListItem> AllClasses { get; set; }

        public IList<int> SelectedClasses { get; set; }

        public HorseInClassEditModel()
        {
            AllHorses = new List<SelectListItem>();
            AllShows = new List<SelectListItem>();
            AllClasses = new List<SelectListItem>();
        }
    }
}