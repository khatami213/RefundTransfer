using Logging;
using Newtonsoft.Json;
using RefundTransferService.ExternalServices.FaraBoom.Models;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace RefundTransferService.ExternalServices.FaraBoom
{
    public class FaraBoomService
    {
        private readonly string _baseUrl; //FaraBoom BaseAddress
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(FaraBoomService));

        public FaraBoomService()
        {
            _baseUrl = ConfigurationManager.AppSettings["FaraboomBaseAddress"]; //"http://192.168.18.39:1010/api/Faraboom";
        }

        public async Task<FaraboomCardToIbanResponseModel> CardToIbanService(FaraboomCardToIbanRequestModel request)
        {
            var externalServices = new CallExternalServices(_baseUrl);
            try
            {
                var token = TokenManager.Token;
                var result = new FaraboomCardToIbanResponseModel();
                var jsonBody = JsonConvert.SerializeObject(request);
                result = await externalServices.CallPostApi<FaraboomCardToIbanResponseModel>("CardToIban", jsonBody, token);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Fatal($"CardToIbanService Failed : {ex.Message}");
                throw;
            }
        }
    }
}
