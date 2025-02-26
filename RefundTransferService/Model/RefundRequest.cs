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
    
    public partial class RefundRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RefundRequest()
        {
            this.CardTransfer = new HashSet<CardTransfer>();
        }
    
        public long RefrenceNumber { get; set; }
        public Nullable<long> RequestId { get; set; }
        public string UserName { get; set; }
        public string EncryptedPan { get; set; }
        public Nullable<long> TransactionAmount { get; set; }
        public Nullable<long> CommitionAmount { get; set; }
        public string RefundStatus { get; set; }
        public string TransferRrn { get; set; }
        public string Description { get; set; }
        public string RefundDescription { get; set; }
        public Nullable<System.DateTime> InsertDateTime { get; set; }
        public Nullable<System.DateTime> TransferDateTime { get; set; }
        public Nullable<bool> CardTransferInserted { get; set; }
        public Nullable<int> RetryCount { get; set; }
        public string SubRefundStatus { get; set; }
        public string RRN { get; set; }
        public Nullable<bool> IsProvider { get; set; }
        public Nullable<bool> IBANTransferInserted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CardTransfer> CardTransfer { get; set; }
    }
}
