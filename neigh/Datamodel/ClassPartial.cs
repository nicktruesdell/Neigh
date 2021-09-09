using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace neigh.Datamodel
{
    [MetadataType(typeof(ClassMetaData))]
    public partial class Class
    {
    }

    public class ClassMetaData
    {
        [Display(Name = "Class Type")]
        [Required(ErrorMessage = "Class type is required.")]
        public object Type { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(500, ErrorMessage = "Name cannot exceed 500 characters.")]
        public object Name { get; set; }
        [Display(Name = "Qualifying Points")]
        [Required(ErrorMessage = "Qualifying points is required.")]
        [RegularExpression("\\d+", ErrorMessage = "Qualifying points must be numeric.")]
        public object QualifyingPoints { get; set; }
    }
}