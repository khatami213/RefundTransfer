//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RefundTransferService.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServerInfo
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
