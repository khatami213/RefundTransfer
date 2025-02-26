using System.Collections.Generic;

namespace RefundTransferService.ExternalServices.FaraBoom.Models
{
    public class FaraboomTokenModel
    {
        public FaraboomTokenModel()
        {
            Exception = new ExceptionInfo();
        }
        public string AccessToken { get; set; }
        public ExceptionInfo Exception { get; set; } = null;
    }

    public class ExceptionInfo
    {
        public string Message { get; set; }
        public List<ExceptionInfo> InnerException { get; set; }
    }
}
