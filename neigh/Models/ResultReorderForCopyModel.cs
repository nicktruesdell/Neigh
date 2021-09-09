using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neigh.Models
{
    public class ResultReorderForCopyModel
    {
        public String ClassId { get; set; }
        public int HorseId { get; set; }
        public String SortOrder { get; set; }
        public String ClassNumber { get; set; }
        public int OldClassId { get; set; }
        public int OldHorseId { get; set; }
    }
}