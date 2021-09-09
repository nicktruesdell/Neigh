using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace neigh.Datamodel
{
    [MetadataType(typeof(HorseMetaData))]
    public partial class Horse
    {
    }

    public class HorseMetaData
    {
        [Display(Name = "Full Name"), Required(ErrorMessage = "Full name is required.")]
        public object FullName { get; set; }
        [Required(ErrorMessage = "Nickname is required.")]
        public object Nickname { get; set; }
        [Display(Name = "AMHA Registration Number")]
        public object ARegNumber { get; set; }
        [Display(Name = "AMHR Registration Number")]
        public object RRegNumber { get; set; }
    }
}