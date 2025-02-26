using Logging;
using Newtonsoft.Json;
using RefundTransferService.ExternalServices.FaraBoom;
using RefundTransferService.ExternalServices.JibitServices.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefundTransferService.ExternalServices.JibitServices
{
    public class JibitService
    {
        private readonly string _baseUrl;
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(JibitService));
        public JibitService()
        {
            _baseUrl = ConfigurationManager.AppSettings["JibitBaseAddress"]; //"http://192.168.18.85:1007/v1/jibit/";
        }

        public async Task<JibitCardToIbanResponseModel> CardToIbanService(JibitCardToIbanRequestModel request)
        {
            //if(request.CardNumber == "Test")
            //    return new JibitCardToIbanResponseModel()
            //    {
            //        Message = "successful operation",
            //        Status = true,
            //        IbanInfo = new IbanInfo()
            //        {
            //            Bank = "Novin",
            //            Iban = "IR670550011186200165000001",
            //            Status = "ACTIVE",
            //            Owners = new List<OwnerInfo>()
            //            {
            //                new OwnerInfo()
            //                {
            //                    FirstName = "Novin"
            //                }
            //            }
            //        }
            //    };
            var externalServices = new CallExternalServices(_baseUrl);
            try
            {
                var token = TokenManager.Token;
                var result = new JibitCardToIbanResponseModel();
                var parameters = new Dictionary<string, string>() 
                {
                    {"CardNumber" , request.CardNumber },
                };
                result = await externalServices.CallGetApi<JibitCardToIbanResponseModel>("get-iban", token, parameters);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Fatal($"cardtoiban Service Failed : {ex.Message}");
                throw;
            }
        }

    }
}
