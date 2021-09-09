using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace neigh.Datamodel
{
    [MetadataType(typeof(ResultsMetaData))]
    public partial class Result
    {
    }

    public class ResultsMetaData
    {
        [Display(Name = "Horse")]
        public object HorseId { get; set; }
        [Display(Name = "Class")]
        public object ClassId { get; set; }
        [Display(Name = "# In Class")]
        public object HorsesInClass { get; set; }
        [Display(Name = "Overall Placing")]
        public object OverallPlacing { get; set; }
        [Display(Name = "Points")]
        public object Points { get; set; }
        [Display(Name = "Class #")]
        public object ClassNumber { get; set; }
    }
}