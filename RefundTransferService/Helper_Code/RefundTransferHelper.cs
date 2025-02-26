using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using RefundTransferService.Model.RefundModel;
using RefundTransferService.Model;
using RefundTransferService.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Logging;
using System.Configuration;
using RefundTransferService.Helper_Code.Constants;

namespace RefundTransferService.Helper_Code
{
    public class RefundTransferHelper
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(RefundTransferHelper));
        private static readonly string YaghutTimeOut = ConfigurationManager.AppSettings["YaghutTimeOut"];
        private static readonly string NormalTransferRetryCount = ConfigurationManager.AppSettings["NormalTransferRetryCount"];

        public static void ProcessRefundRequest(PNA_RefundServiceEntities db, RefundRequest request)
        {
            Thread.Sleep(5000);

            if (request.TransactionAmount == null)
            {
                UpdateRefundStatus(db, request, "Transaction amount is null", "-45");
                return;
            }

            var userInfo = GetUserInfo(db, request.UserName);
            if (userInfo == null)
            {
                UpdateRefundStatus(db, request, "User info not found", "-45");
                return;
            }

            var decryptedCardInfo = DecryptCardInfo(request.EncryptedPan);
            if (decryptedCardInfo == null)
            {
                UpdateRefundStatus(db, request, "Invalid destination card number", "-45");
                return;
            }

            if (request.RetryCount >= int.Parse(NormalTransferRetryCount))
            {
                UpdateRefundStatus(db, request, "Maximum retry limit reached", "03");
                return;
            }

            PerformTransfer(db, request, userInfo, decryptedCardInfo);
        }

        private static WebServiceUser GetUserInfo(PNA_RefundServiceEntities db, string userName)
        {
            try
            {
                var userInfo = db.WebServiceUser.FirstOrDefault(x => x.UserName == userName);
                if (userInfo != null)
                {
                    userInfo.YaghutPassword = CryptorEngine.Decryption(userInfo.YaghutPassword, ConstantHelper.PrivateKey);
                }
                return userInfo;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error fetching user info: {ex.Message}");
                return null;
            }
        }

        private static string DecryptCardInfo(string encryptedPan)
        {
            try
            {
                return CryptorEngine.Decryption(encryptedPan, ConstantHelper.PrivateKey);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error decrypting card info: {ex.Message}");
                return null;
            }
        }

        private static void PerformTransfer(PNA_RefundServiceEntities db, RefundRequest request, WebServiceUser userInfo, string decryptedCardInfo)
        {
            try
            {
                var commission = CalculateCommission(decryptedCardInfo, request.TransactionAmount.Value);
                var transferAmount = request.TransactionAmount.Value + commission;

                var digiPayService = new WebServiceRefund.Refund();
                var transferResponse = PerformNormalTransfer(digiPayService, userInfo, request, transferAmount, commission);

                if (!transferResponse.IsSuccessful)
                {
                    Logger.Warn($"Transfer failed: {transferResponse.Message}");
                    request.RetryCount++;
                    db.SaveChanges();
                }
                else
                {
                    UpdateTransferDetails(db, request, transferResponse);
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal($"Error in transfer process: {ex.Message}");
            }
        }

        private static long CalculateCommission(string cardPrefix, long amount)
        {
            if (cardPrefix.StartsWith("627412")) return 0; // No commission for specific bank cards
            return Helpers.GetCommission(amount);
        }

        private static TransferResponse PerformNormalTransfer(WebServiceRefund.Refund digiPayService, WebServiceUser userInfo, RefundRequest request, decimal amount, long commission)
        {
            try
            {
                Logger.Info("Initiating normal transfer...");
                var response = digiPayService.NormalTransfer(
                    userInfo.CustomerAccount,
                    userInfo.BankAccount,
                    amount,
                    BuildTransferDescription(request, amount, commission),
                    BuildTransferReference(request),
                    int.Parse(YaghutTimeOut),
                    userInfo.YaghutUser,
                    userInfo.YaghutPassword
                );

                return new TransferResponse { IsSuccessful = response.Contains("Res:OK"), Message = response };
            }
            catch (Exception ex)
            {
                Logger.Fatal($"Transfer exception: {ex.Message}");
                return new TransferResponse { IsSuccessful = false, Message = ex.Message };
            }
        }

        private static void UpdateRefundStatus(PNA_RefundServiceEntities db, RefundRequest request, string description, string status)
        {
            request.RefundDescription = description;
            request.RefundStatus = status;
            db.SaveChanges();
        }

        private static void UpdateTransferDetails(PNA_RefundServiceEntities db, RefundRequest request, TransferResponse response)
        {
            request.TransferRrn = ParseRRN(response.Message);
            request.TransferDateTime = DateTime.Now;
            request.CardTransferInserted = true;
            db.SaveChanges();
        }

        private static string BuildTransferDescription(RefundRequest request, decimal amount, long commission) =>
            $"|{request.Description}|Amount: {amount}|Commission: {commission}|Reference Number: {request.RefrenceNumber}";

        private static string BuildTransferReference(RefundRequest request) =>
            $"|{request.Description}|Reference Number: {request.RefrenceNumber}";

        private static string ParseRRN(string response) =>
            response.Split('|').FirstOrDefault(part => part.Contains("RRN:"))?.Replace("RRN:", "");

    }
}
