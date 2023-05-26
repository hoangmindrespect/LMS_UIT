//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryManagementSystem.Models.DataProvider
{
    using System;
    using System.Collections.Generic;
    
    public partial class BOOK
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BOOK()
        {
            this.CARTs = new HashSet<CART>();
            this.CTHDs = new HashSet<CTHD>();
            this.DETAIL_BBFORM = new HashSet<DETAIL_BBFORM>();
        }
    
        public int ID { get; set; }
        public string TENSACH { get; set; }
        public string TACGIA { get; set; }
        public string NHAXUATBAN { get; set; }
        public Nullable<int> NAMXUATBAN { get; set; }
        public Nullable<int> SOLUONG { get; set; }
        public string THELOAI { get; set; }
        public Nullable<decimal> GIA { get; set; }
        public string MOTA { get; set; }
        public string IMAGESOURCE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CART> CARTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTHD> CTHDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETAIL_BBFORM> DETAIL_BBFORM { get; set; }
    }
}
