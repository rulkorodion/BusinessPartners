//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessPartners
{
    using System;
    using System.Collections.Generic;
    
    public partial class PartnerProducts
    {
        public int PartnerProductID { get; set; }
        public int PartnerID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public System.DateTime SaleDate { get; set; }
    
        public virtual Partners Partners { get; set; }
        public virtual Products Products { get; set; }
    }
}