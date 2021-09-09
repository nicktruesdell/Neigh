using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace neigh.Models
{
    public class ShowCopyModel
    {
        public int Id { get; set; }
        [Display(Name = "Show Type")]
        [Required(ErrorMessage = "Show type is required.")]
        public ShowType Type { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; }
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:M/d/yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Start date must be a valid date.")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:M/d/yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date, ErrorMessage = "End date must be a valid date.")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Location")]
        [Required(ErrorMessage = "Location is required.")]
        public String Location { get; set; }
        public List<String> Judges { get; set; }
        [Display(Name = "Level")]
        [Required(ErrorMessage = "Level is required.")]
        public ShowLevel Level { get; set; }
        public List<ResultReorderForCopyModel> Entries { get; set; }
    }
}