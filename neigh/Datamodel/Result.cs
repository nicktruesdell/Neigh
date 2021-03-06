//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace neigh.Datamodel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Result
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Result()
        {
            this.Results_Show_Judges = new HashSet<Results_Show_Judges>();
        }
    
        public int HorseId { get; set; }
        public int ShowId { get; set; }
        public int ClassId { get; set; }
        public Nullable<int> HorsesInClass { get; set; }
        public Nullable<int> OverallPlacing { get; set; }
        public Nullable<double> Points { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public string ClassNumber { get; set; }
    
        public virtual Class Class { get; set; }
        public virtual Horse Horse { get; set; }
        public virtual Show Show { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Results_Show_Judges> Results_Show_Judges { get; set; }
    }
}
