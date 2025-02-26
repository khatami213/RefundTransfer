using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundTransferService.ExternalServices.FaraBoom.Models
{
    public class FaraboomCardToIbanResponseModel
    {
        public bool Status { get; set; }
        public string Description { get; set; }
        public string Iban { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DepositNumber { get; set; }
    }
}
