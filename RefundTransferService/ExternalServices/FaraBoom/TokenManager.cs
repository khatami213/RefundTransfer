using Newtonsoft.Json;
using RefundTransferService.ExternalServices.FaraBoom.Models;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RefundTransferService.ExternalServices.FaraBoom
{
    public static class TokenManager
    {
        private static string _accessToken;
        private static DateTime _tokenExpiration;

        public static string Token
        {
            get
            {
                if (_accessToken == null || _tokenExpiration <= DateTime.Now)
                {
                    _accessToken = GetToken().Result;
                    _tokenExpiration = GetTokenExpiration(_accessToken);
                }

                return _accessToken;
            }
            set
            {
                _accessToken = value;
            }
        }

        // FaraBoom configuration
        private static readonly string FaraboomBaseAddress = ConfigurationManager.AppSettings["FaraboomBaseAddress"]; //"http://192.168.18.39:1010/api/Faraboom";
        private static readonly string Username = ConfigurationManager.AppSettings["FaraboomIdentityUsername"]; //"admin";
        private static readonly string Password = ConfigurationManager.AppSettings["FaraboomIdentityPassword"]; //"admin";

        private static async Task<string> GetToken()
        {
            var token = string.Empty;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            using (var client = new HttpClient())
            {
                var request = new
                {
                    userName = Username,
                    password = Password,
                };
                var jsonRequest = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{FaraboomBaseAddress}token", content);
                var stringResposne = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<FaraboomTokenModel>(stringResposne);
                    if (result != null && result.AccessToken != null)
                    {
                        token = result.AccessToken;
                    }
                }
                else
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(stringResponse))
                    {
                        var result = JsonConvert.DeserializeObject<FaraboomTokenModel>(stringResponse);
                    }
                }
            }
            return token;
        }

        private static DateTime GetTokenExpiration(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(accessToken);
            return jwtToken.ValidTo;
        }
    }
}
