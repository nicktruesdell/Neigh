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
    
    public partial class Horse
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Horse()
        {
            this.Results = new HashSet<Result>();
        }
    
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Nickname { get; set; }
        public string ARegNumber { get; set; }
        public string RRegNumber { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Result> Results { get; set; }
    }
}
