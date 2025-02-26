using Logging;
using RefundTransferService.Helper_Code.Types;
using RefundTransferService.Model;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace RefundTransferService.Helper_Code
{
    public static class Helpers
    {
        //logger
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(Helpers));
        //جدول کارمزدها 
        private static readonly string Under10000000Rilas = ConfigurationManager.AppSettings["0-10,000,000"];
        private static readonly string From10000001To20000000Rilas = ConfigurationManager.AppSettings["10,000,001-20,000,000"];
        private static readonly string From20000001To30000000Rilas = ConfigurationManager.AppSettings["20,000,001-30,000,000"];
        private static readonly string From30000001To40000000Rilas = ConfigurationManager.AppSettings["30,000,001-40,000,000"];
        private static readonly string From40000001To50000000Rilas = ConfigurationManager.AppSettings["40,000,001-50,000,000"];
        private static readonly string From50000001To60000000Rilas = ConfigurationManager.AppSettings["50,000,001-60,000,000"];
        private static readonly string From60000001To70000000Rilas = ConfigurationManager.AppSettings["60,000,001-70,000,000"];
        private static readonly string From70000001To80000000Rilas = ConfigurationManager.AppSettings["70,000,001-80,000,000"];
        private static readonly string From80000001To90000000Rilas = ConfigurationManager.AppSettings["80,000,001-90,000,000"];
        private static readonly string From90000001To100000000Rilas = ConfigurationManager.AppSettings["90,000,001-100,000,000"];



        //عدد آستانه کارمزد و کارت به کارت ها
        private static readonly string AmountThreshold = ConfigurationManager.AppSettings["CheckAmountThreshold"];

        //بازه های کارمزد
        private static readonly string CommissionPeriod0 = ConfigurationManager.AppSettings["CommissionPeriod0"];
        private static readonly string CommissionPeriod10000000 = ConfigurationManager.AppSettings["CommissionPeriod10000000"];
        private static readonly string CommissionPeriod10000001 = ConfigurationManager.AppSettings["CommissionPeriod10000001"];
        private static readonly string CommissionPeriod20000000 = ConfigurationManager.AppSettings["CommissionPeriod20000000"];
        private static readonly string CommissionPeriod20000001 = ConfigurationManager.AppSettings["CommissionPeriod20000001"];
        private static readonly string CommissionPeriod30000000 = ConfigurationManager.AppSettings["CommissionPeriod30000000"];
        private static readonly string CommissionPeriod30000001 = ConfigurationManager.AppSettings["CommissionPeriod30000001"];
        private static readonly string CommissionPeriod40000000 = ConfigurationManager.AppSettings["CommissionPeriod40000000"];
        private static readonly string CommissionPeriod40000001 = ConfigurationManager.AppSettings["CommissionPeriod40000001"];
        private static readonly string CommissionPeriod50000000 = ConfigurationManager.AppSettings["CommissionPeriod50000000"];
        private static readonly string CommissionPeriod50000001 = ConfigurationManager.AppSettings["CommissionPeriod50000001"];
        private static readonly string CommissionPeriod60000000 = ConfigurationManager.AppSettings["CommissionPeriod60000000"];
        private static readonly string CommissionPeriod60000001 = ConfigurationManager.AppSettings["CommissionPeriod60000001"];
        private static readonly string CommissionPeriod70000000 = ConfigurationManager.AppSettings["CommissionPeriod70000000"];
        private static readonly string CommissionPeriod70000001 = ConfigurationManager.AppSettings["CommissionPeriod70000001"];
        private static readonly string CommissionPeriod80000000 = ConfigurationManager.AppSettings["CommissionPeriod80000000"];
        private static readonly string CommissionPeriod80000001 = ConfigurationManager.AppSettings["CommissionPeriod80000001"];
        private static readonly string CommissionPeriod90000000 = ConfigurationManager.AppSettings["CommissionPeriod90000000"];
        private static readonly string CommissionPeriod90000001 = ConfigurationManager.AppSettings["CommissionPeriod90000001"];
        private static readonly string CommissionPeriod100000000 = ConfigurationManager.AppSettings["CommissionPeriod100000000"];

        //کارمزد تبدیل کارت به شبا
        private static readonly string CardToIBANCommission = ConfigurationManager.AppSettings["CardToIBANCommission"];

        public static string GetMaskedPan(string pan)
        {
            var maskedPan = pan.Substring(0, 6) + "******" + pan.Substring(12);
            return maskedPan;
        }

        public static long GetCommission_Old(long amount)
        {
            long commission = 0;

            if (amount > long.Parse(AmountThreshold))
            {
                var quotient = Math.DivRem(long.Parse(amount.ToString()), long.Parse(AmountThreshold), out var remainder);
                commission = quotient * long.Parse(From20000001To30000000Rilas);
                if (remainder > long.Parse(CommissionPeriod0) && remainder <= long.Parse(CommissionPeriod10000000))
                    commission += long.Parse(Under10000000Rilas);
                else if (remainder >= long.Parse(CommissionPeriod10000001) && remainder <= long.Parse(CommissionPeriod20000000))
                    commission += long.Parse(From10000001To20000000Rilas);
                else if (remainder >= long.Parse(CommissionPeriod20000001) && remainder <= long.Parse(CommissionPeriod30000000))
                    commission += long.Parse(From20000001To30000000Rilas);
            }
            else
            {
                if (amount > long.Parse(CommissionPeriod0) && amount <= long.Parse(CommissionPeriod10000000))
                    commission += long.Parse(Under10000000Rilas);
                else if (amount >= long.Parse(CommissionPeriod10000001) && amount <= long.Parse(CommissionPeriod20000000))
                    commission += long.Parse(From10000001To20000000Rilas);
                else if (amount >= long.Parse(CommissionPeriod20000001) && amount <= long.Parse(CommissionPeriod30000000))
                    commission += long.Parse(From20000001To30000000Rilas);
            }

            return commission;
        }

        public static long GetCommission(long amount)
        {

            long commission = 0;

            if (amount > long.Parse(AmountThreshold))
            {
                var quotient = Math.DivRem(long.Parse(amount.ToString()), long.Parse(AmountThreshold), out var remainder);
                commission = quotient * long.Parse(From90000001To100000000Rilas);

                if (remainder > long.Parse(CommissionPeriod0) && remainder <= long.Parse(CommissionPeriod10000000))
                    commission += long.Parse(Under10000000Rilas);
                else if (remainder >= long.Parse(CommissionPeriod10000001) && remainder <= long.Parse(CommissionPeriod20000000))
                    commission += long.Parse(From10000001To20000000Rilas);
                else if (remainder >= long.Parse(CommissionPeriod20000001) && remainder <= long.Parse(CommissionPeriod30000000))
                    commission += long.Parse(From20000001To30000000Rilas);
                else if (remainder >= long.Parse(CommissionPeriod30000001) && remainder <= long.Parse(CommissionPeriod40000000))
                    commission += long.Parse(From30000001To40000000Rilas);
                else if (remainder >= long.Parse(CommissionPeriod40000001) && remainder <= long.Parse(CommissionPeriod50000000))
                    commission += long.Parse(From40000001To50000000Rilas);
                else if (remainder >= long.Parse(CommissionPeriod50000001) && remainder <= long.Parse(CommissionPeriod60000000))
                    commission += long.Parse(From50000001To60000000Rilas);
                else if (remainder >= long.Parse(CommissionPeriod60000001) && remainder <= long.Parse(CommissionPeriod70000000))
                    commission += long.Parse(From60000001To70000000Rilas);
                else if (remainder >= long.Parse(CommissionPeriod70000001) && remainder <= long.Parse(CommissionPeriod80000000))
                    commission += long.Parse(From70000001To80000000Rilas);
                else if (remainder >= long.Parse(CommissionPeriod80000001) && remainder <= long.Parse(CommissionPeriod90000000))
                    commission += long.Parse(From80000001To90000000Rilas);
                else if (remainder >= long.Parse(CommissionPeriod90000001) && remainder <= long.Parse(CommissionPeriod100000000))
                    commission += long.Parse(From90000001To100000000Rilas);
            }
            else
            {
                if (amount > long.Parse(CommissionPeriod0) && amount <= long.Parse(CommissionPeriod10000000))
                    commission += long.Parse(Under10000000Rilas);
                else if (amount >= long.Parse(CommissionPeriod10000001) && amount <= long.Parse(CommissionPeriod20000000))
                    commission += long.Parse(From10000001To20000000Rilas);
                else if (amount >= long.Parse(CommissionPeriod20000001) && amount <= long.Parse(CommissionPeriod30000000))
                    commission += long.Parse(From20000001To30000000Rilas);
                else if (amount >= long.Parse(CommissionPeriod30000001) && amount <= long.Parse(CommissionPeriod40000000))
                    commission += long.Parse(From30000001To40000000Rilas);
                else if (amount >= long.Parse(CommissionPeriod40000001) && amount <= long.Parse(CommissionPeriod50000000))
                    commission += long.Parse(From40000001To50000000Rilas);
                else if (amount >= long.Parse(CommissionPeriod50000001) && amount <= long.Parse(CommissionPeriod60000000))
                    commission += long.Parse(From50000001To60000000Rilas);
                else if (amount >= long.Parse(CommissionPeriod60000001) && amount <= long.Parse(CommissionPeriod70000000))
                    commission += long.Parse(From60000001To70000000Rilas);
                else if (amount >= long.Parse(CommissionPeriod70000001) && amount <= long.Parse(CommissionPeriod80000000))
                    commission += long.Parse(From70000001To80000000Rilas);
                else if (amount >= long.Parse(CommissionPeriod80000001) && amount <= long.Parse(CommissionPeriod90000000))
                    commission += long.Parse(From80000001To90000000Rilas);
                else if (amount >= long.Parse(CommissionPeriod90000001) && amount <= long.Parse(CommissionPeriod100000000))
                    commission += long.Parse(From90000001To100000000Rilas);
            }

            return commission;
        }

        public static List<long> GetCardForTransfersIds(long amount, long refundRefrenceNumber, string rrn, string destinationPan, string username)
        {
            var cardTrsnaferIds = new List<long>();

            try
            {
                if (amount > long.Parse(AmountThreshold))
                {
                    var quotient = Math.DivRem(long.Parse(amount.ToString()), long.Parse(AmountThreshold), out var remainder);
                    for (var i = 0; i < quotient; i++)
                        using (var db = new PNA_RefundServiceEntities())
                        {
                            var cardTransfer = new CardTransfer
                            {
                                Amount = long.Parse(AmountThreshold),
                                EncryptedSourcePan = "",
                                RefundRefrenceNumber = refundRefrenceNumber,
                                Rrn = rrn,
                                Status = "-1",
                                InsertDateTime = DateTime.Now,
                                RetryCount = 0
                            };
                            var result = db.CardTransfer.Add(cardTransfer);
                            if (db.SaveChanges() > 0)
                                cardTrsnaferIds.Add(result.Id);
                            Logging.Log("Added card transfer request to database", refundRefrenceNumber.ToString(), rrn, amount.ToString(), GetMaskedPan(destinationPan), $"card transfer request for refund refrence number : {refundRefrenceNumber}", LogType.Info.ToString(), username);
                            Logger.Info($"Card transfer request with refrence number: {refundRefrenceNumber} and PAN: {GetMaskedPan(destinationPan)} for user: {username} added to database");
                        }

                    if (remainder > 0)
                        using (var db = new PNA_RefundServiceEntities())
                        {
                            var cardTransfer = new CardTransfer
                            {
                                Amount = remainder,
                                EncryptedSourcePan = "",
                                RefundRefrenceNumber = refundRefrenceNumber,
                                Rrn = rrn,
                                Status = "-1",
                                InsertDateTime = DateTime.Now,
                                RetryCount = 0
                            };
                            var result = db.CardTransfer.Add(cardTransfer);
                            if (db.SaveChanges() > 0)
                                cardTrsnaferIds.Add(result.Id);
                            Logging.Log("Added card transfer request to database", refundRefrenceNumber.ToString(), rrn, amount.ToString(), GetMaskedPan(destinationPan), $"card transfer request for refund refrence number : {refundRefrenceNumber}", LogType.Info.ToString(), username);
                            Logger.Info($"Card transfer request with refrence number: {refundRefrenceNumber} and PAN: {GetMaskedPan(destinationPan)} for user: {username} added to database");
                        }
                }
                else
                {
                    using (var db = new PNA_RefundServiceEntities())
                    {
                        var cardTransfer = new CardTransfer
                        {
                            Amount = amount,
                            EncryptedSourcePan = "",
                            RefundRefrenceNumber = refundRefrenceNumber,
                            Rrn = rrn,
                            Status = "-1",
                            InsertDateTime = DateTime.Now,
                            RetryCount = 0
                        };
                        var result = db.CardTransfer.Add(cardTransfer);
                        if (db.SaveChanges() > 0)
                            cardTrsnaferIds.Add(result.Id);

                        Logging.Log("Added card transfer request to database", refundRefrenceNumber.ToString(), rrn, amount.ToString(), GetMaskedPan(destinationPan), $"card transfer request for refund refrence number : {refundRefrenceNumber}", LogType.Info.ToString(), username);
                        Logger.Info($"Card transfer request with refrence number: {refundRefrenceNumber} and PAN: {GetMaskedPan(destinationPan)} for user: {username} added to database");
                    }
                }
            }
            catch (Exception ex)
            {
                cardTrsnaferIds = new List<long>();
                Logger.Info($"Error in add card transfer to database for refrence number: {refundRefrenceNumber} and PAN: {GetMaskedPan(destinationPan)} and user: {username}, description: {ex.Message}");
                Logging.Log("Added card transfer request to database", refundRefrenceNumber.ToString(), rrn, amount.ToString(), GetMaskedPan(destinationPan), $"Error in add card transfer to database for refrence number: {refundRefrenceNumber} and PAN: {GetMaskedPan(destinationPan)} and user: {username}, description: {ex.Message}", LogType.Error.ToString(), username);

                if (ex.InnerException != null)
                {
                    Logger.Fatal($"Inner Exception: {ex.InnerException.Message}");
                    if (ex.InnerException.InnerException != null)
                    {
                        Logger.Fatal($"Inner Exception: {ex.InnerException.InnerException.Message}");
                        if (ex.InnerException.InnerException.InnerException != null)
                            Logger.Fatal($"Inner Exception: {ex.InnerException.InnerException.InnerException.Message}");
                    }
                }
            }

            return cardTrsnaferIds;
        }

        public static long GetCardToIbanCommission()
        {
            var commission = CardToIBANCommission != null ? long.Parse(CardToIBANCommission) : 0;
            return commission;
        }

    }
}