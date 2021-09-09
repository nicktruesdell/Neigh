using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neigh.Models
{
    public class ResultReorderModel
    {
        public String ClassName { get; set; }
        public int? ClassId { get; set; }
        public int? HorseId { get; set; }
        public String HorseName { get; set; }
        public String SortOrder { get; set; }
        public String ClassNumber { get; set; }
    }
}