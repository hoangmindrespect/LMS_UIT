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
    
    public partial class DETAIL_BBFORM
    {
        public int MAPHIEUMUON { get; set; }
        public int MASACH { get; set; }
        public string TENSACH { get; set; }
        public Nullable<int> SOLUONG { get; set; }
    
        public virtual BBFORM BBFORM { get; set; }
        public virtual BOOK BOOK { get; set; }
    }
}
