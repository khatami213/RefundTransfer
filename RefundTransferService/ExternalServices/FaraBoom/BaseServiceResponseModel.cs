using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundTransferService.ExternalServices.FaraBoom
{
    public class BaseServiceResponseModel<T> where T : class
    {
        public bool Status { get; set; }
        public string Error { get; set; } = null;
        public T Data { get; set; } = null;
    }
}
