//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAPI
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblMessage
    {
        public int idMessage { get; set; }
        public System.DateTime dteDate { get; set; }
        public int intUser { get; set; }
        public string strMessage { get; set; }
        public int intThread { get; set; }
    
        public virtual tblThread tblThread { get; set; }
        public virtual tblUser tblUser { get; set; }
    }
}
