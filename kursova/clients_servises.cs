//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace kursova
{
    using System;
    using System.Collections.Generic;
    
    public partial class clients_servises
    {
        public int id { get; set; }
        public int id_client { get; set; }
        public int id_service { get; set; }
        public System.DateTime date { get; set; }
        
        public Nullable<int> id_working_staff { get; set; }
        public Nullable<decimal> Price { get; set; }
        public virtual Clients Clients { get; set; }
        public virtual Services Services { get; set; }
        public virtual working_staff working_staff { get; set; }
    }
}
