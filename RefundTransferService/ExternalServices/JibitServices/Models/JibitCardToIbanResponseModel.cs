using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundTransferService.ExternalServices.JibitServices.Models
{
    public class JibitCardToIbanResponseModel
    {
        public string Code { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public IbanInfo IbanInfo { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
    }

    public class IbanInfo
    {
        public IbanInfo()
        {
            Owners = new List<OwnerInfo>();
        }
        public string Bank { get; set; }
        public string Iban { get; set; }
        public string Status { get; set; }
        public string DepositNumber { get; set; }
        public List<OwnerInfo> Owners { get; set; } = null;
    }

    public class OwnerInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
