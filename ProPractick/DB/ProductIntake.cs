//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProPractick.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductIntake
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductIntake()
        {
            this.ProductIntakeProduct = new HashSet<ProductIntakeProduct>();
        }
    
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public decimal TotalAmount { get; set; }
        public System.DateTime Data { get; set; }
    
        public virtual Supplier Supplier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductIntakeProduct> ProductIntakeProduct { get; set; }
    }
}
