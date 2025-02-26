using Logging;
using Newtonsoft.Json;
using RefundTransferService.Enums;
using RefundTransferService.ExternalServices.FaraBoom;
using RefundTransferService.ExternalServices.FaraBoom.Models;
using RefundTransferService.ExternalServices.JibitServices;
using RefundTransferService.ExternalServices.JibitServices.Models;
using RefundTransferService.Helper_Code;
using RefundTransferService.Helper_Code.Enums;
using RefundTransferService.Model;
using RefundTransferService.Model.RefundModel;
using RefundTransferService.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace RefundTransferService
{
    public partial class RefundTransfer : ServiceBase
    {
        //lock 
        private static readonly object LockObject = new object();
        private static bool _inProgress;

        //logger
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(RefundTransfer));

        //Timer 
        private Timer _mainTimer;
        private bool _timerTaskSuccess;

        //کلیدهای رمزنگاری سرور
        private const string PublicKey = "<RSAKeyValue><Modulus>wOmcBUMLQU9EVavB8t/EWefZrWG+thJOI8ccyTrvcUcU/knO5OXMq7/we0turVjjM6gswsWwjPqqax9JHQdTR0B1VMVOOEKn/3HMOK6S/mZLF+XdEmd2g+6GRkS7sJiM9IKrfu8cYTawS7wgBhtW6/j5h1mj6ja4BSGBvoOozxzHWoFEfpulJ5JzEfhL6UqvNBnRe8eLP/rJKDYiYX2zX/r/cBPLCFyBs05nDrzuZ/1GNEisBYJ8COBV8UrxyE5Utw+wgSLwp+Jf2QWGatT4EANNVjUO+Nzz13cOCXJQ+XQIFDFTDVf4pNQPJeIRJc8QGZWndWkEEiX8AidgCmGnjQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string PrivateKey = "<RSAKeyValue><Modulus>wOmcBUMLQU9EVavB8t/EWefZrWG+thJOI8ccyTrvcUcU/knO5OXMq7/we0turVjjM6gswsWwjPqqax9JHQdTR0B1VMVOOEKn/3HMOK6S/mZLF+XdEmd2g+6GRkS7sJiM9IKrfu8cYTawS7wgBhtW6/j5h1mj6ja4BSGBvoOozxzHWoFEfpulJ5JzEfhL6UqvNBnRe8eLP/rJKDYiYX2zX/r/cBPLCFyBs05nDrzuZ/1GNEisBYJ8COBV8UrxyE5Utw+wgSLwp+Jf2QWGatT4EANNVjUO+Nzz13cOCXJQ+XQIFDFTDVf4pNQPJeIRJc8QGZWndWkEEiX8AidgCmGnjQ==</Modulus><Exponent>AQAB</Exponent><P>4ZFdS9A6j/HDKxK3Emq8qVXS1LS5AwXGIbrbOG8lLHLLwsneMhiBdBw5QdcXQHJZ9Oc/JjQ+28Y/ETsc/FIxlZJBntSJiJUoNvKJaFXerxYhh6dNO/BqxhccSOdswz/gQqB0qkg5/jrSB6UasmbINVIF7mk6XdaOzJ0QgBG/X3s=</P><Q>2vBmecwFtWkt6Tp8b9lV5Bsm8Zqr8izZyQh+KjWwg2FwZdt1rH1u6IZtHh6LmAg6eAWH1MWfJ6gjrPNIzyvyOa/46L7KDmdvSwdbEfjDOuRJ2yWGGIM0I9XbAQPN3TKf30qScvPP5Lp13mxkUjJlrKPQ4Lg2zWNOjF0OfdXWIpc=</Q><DP>SVzg7h5sXZKw+lpc5oWGlMCQEJQytDP1i9TdJc6oVXuEn/bN6Jcly2C+kpZlPpWygj+Pv1ows4QX0P/b3ojRDaeC5iiUDrMMYEqjvCZphaJ6B0e3i+4WnBS6I0/5hMtKogDT0Ooqym/RDaF6PFnHdegWe8MHs6tryEqxKiYbiu8=</DP><DQ>v/yBEBrtgpgZ72QfDIG7xMxeiQzF7RaRX6033VG5WGwQgPFCLiDMKdD/TKMibA4DH45R/y3Qk5jot9eaqDj0Lsv17DqpupnPSS7JGGhY4oKflTFBdqtPBIGaizhHxMmI0eh1paHRUtSDWakZC88vw4TfPL+tJswHbCSJ+aSTIz8=</DQ><InverseQ>vVj+x7ssSIxrEPIoJVaQRB2UzC9drk1TljpADYFtiPkBZyxSo/n2KJzqO5KatCBkzvAZwKpANCsJbH2CQ81AcATa3ONnfgWeypV9uKcyZPD63F0VXwGbJM7AUVuU6zXCkStSEmRJ1io6N5ugtl30x0KmNmQNtNdJ3qanNtzJBtQ=</InverseQ><D>APot/CjWycHpCrYQCXbwu7Pc+m/gU3PMSYocrzhJNj2x8YfWMHqpisUyJq2/JcmpfP2BHIt71Xr/mgNSj38WAOpmrcNCHi7YQwcEjdT0ka1a/AgCErHLe+edboWynbZoIGT5EW+MqUFpqziMwPsqeY+NVA40Ml+MlxoQWjK4jDQK3gY3ulXUKmk23bBRhTC/mwlyhQsKz46CSeiEYCRJwE97LxPB0uKRXYA8aPgk3EsHrsENcI2ovr+cITSf+Qg2HN1nk6zVO3TpotO8B/iKPbnDSmbgVwR1cGatS6nkjwp1RAgetay7l2QSYTQPQDcg0LWKywIaax4TFVtVg++c0Q==</D></RSAKeyValue>";

        //Yaghut TimeOut
        private static readonly string YaghutTimeOut = ConfigurationManager.AppSettings["YaghutTimeOut"];

        //NormalTransfer RetryCount
        private static readonly string NormalTransferRetryCount = ConfigurationManager.AppSettings["NormalTransferRetryCount"];

        private static readonly string CommissionPeriod10000000 = ConfigurationManager.AppSettings["CommissionPeriod10000000"];
        private static readonly string CommissionPeriod100000000 = ConfigurationManager.AppSettings["CommissionPeriod100000000"];
        public RefundTransfer()
        {
            InitializeComponent();
            RefundTransferLog = new EventLog();
            //if (!EventLog.SourceExists("RefundTransferServiceLogSource"))
            //    EventLog.CreateEventSource(
            //        "RefundTransferServiceLogSource", "RefundTransferServiceLog");
            RefundTransferLog.Source = "RefundTransferServiceLogSource";
            RefundTransferLog.Log = "RefundTransferServiceLog";
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Logger.Info("-------------------------------------------------------------------------------------------------------------------------------------------------------");
                Logger.Info("Refund Transfer Service Started");
                var schedulerInterval = ConfigurationManager.AppSettings["SchedulerInterval"];
                _mainTimer = new Timer { Interval = Convert.ToDouble(schedulerInterval) };
                _mainTimer.Elapsed += Timer_Elapsed;
                _mainTimer.AutoReset = false;
                _mainTimer.Start();
                _timerTaskSuccess = false;
            }
            catch (Exception ex)
            {
                RefundTransferLog.WriteEntry("Start Refund Transfer Service - " + ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                Logger.Info("Refund Transfer Service Stopped");
                Logger.Info("-------------------------------------------------------------------------------------------------------------------------------------------------------");
                RefundTransferLog.WriteEntry("Refund Transfer Service Stopped");
                _mainTimer.Stop();
                _mainTimer.Dispose();
                _mainTimer = null;
            }
            catch (Exception ex)
            {
                RefundTransferLog.WriteEntry($"Stop Refund Transfer Service - {ex.Message}");
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_inProgress) return;
            lock (LockObject)
            {
                try
                {
                    _inProgress = true;
                    Logger.Info("-------------------------------------------------------------------------------------------------------------------------------------------------------");
                    Logger.Info("Refund Transfer Service Scheduler Started");
                    RefundTransferService();
                    Thread.Sleep(1000);
                    ProviderRefundTransferService();
                    Thread.Sleep(1000);
                    Logger.Info("Refund Transfer Service Scheduler Ended");
                    Logger.Info("-------------------------------------------------------------------------------------------------------------------------------------------------------");
                    _timerTaskSuccess = true;
                }
                catch (Exception ex)
                {
                    RefundTransferLog.WriteEntry($"Scheduler - {ex.Message}");
                    _timerTaskSuccess = false;
                }
                finally
                {
                    if (_timerTaskSuccess)
                        _mainTimer.Start();
                    _inProgress = false;
                }
            }
        }

        public void RefundTransferService()
        {
            try
            {
                using (var db = new PNA_RefundServiceEntities())
                {

                    var notSettledRefundRequestList = db.RefundRequest.Where(x => x.RefundStatus == "01" &&
                                                         x.CardTransferInserted == false &&
                                                        (x.IsProvider == null || x.IsProvider == false)).OrderBy(s => s.InsertDateTime).ToList();

                    if (notSettledRefundRequestList.Any())
                    {
                        Logger.Info($"notSettledRefundRequestList Count :{notSettledRefundRequestList.Count}");
                        foreach (var item in notSettledRefundRequestList)
                        {
                            Thread.Sleep(5000);
                            Logger.Info($"item :{JsonConvert.SerializeObject(item)}");

                            if (item.TransactionAmount != null)
                            {
                                var userInfoNotNull = false;
                                var yaghutPassword = string.Empty;

                                #region بدست آوردن اطلاعات کاربر

                                var userInfo = db.WebServiceUser.FirstOrDefault(x => x.UserName == item.UserName);
                                if (userInfo != null)
                                {
                                    if (typeof(RefundProviderEnum).GetItems().Any(x => x.Id == userInfo.ProviderId))
                                    {
                                        item.IsProvider = true;
                                        db.SaveChanges();
                                        continue;
                                    }

                                    yaghutPassword = CryptorEngine.Decryption(userInfo.YaghutPassword, PrivateKey);
                                    userInfoNotNull = true;
                                    Logger.Info($"userInfo:{userInfo.UserName}");

                                }

                                #endregion

                                if (userInfoNotNull)
                                {
                                    Logger.Info("Create Yaghut WebServiceRefund Instance");
                                    var digiPayService = new WebServiceRefund.Refund();
                                    Logger.Info("Instance Create ");

                                    #region برگرداندن اطلاعات رمز شده کارت های مقصد برای عملیات کارت به کارت

                                    string cardInfoDecrpted = null;
                                    try
                                    {
                                        cardInfoDecrpted = CryptorEngine.Decryption(item.EncryptedPan, PrivateKey);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error($"Error in decryption bank card info,description: {ex.Message}");
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

                                    #endregion


                                    if (cardInfoDecrpted != null)
                                    {
                                        if (item.RetryCount < int.Parse(NormalTransferRetryCount))
                                        {
                                            #region retryCountIncrement

                                            var retryCountIncrement = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                                            retryCountIncrement.RetryCount++;
                                            db.SaveChanges();

                                            #endregion

                                            var checkStatement = false;
                                            var normalTransferValidate = false;
                                            if ((string.IsNullOrWhiteSpace(item.TransferRrn) || item.TransferRrn == "-"))
                                            {
                                                #region حساب کردن کمیسیون

                                                Logger.Info("حساب کردن کمیسیون ");

                                                long commission = 0;
                                                //var eghtesadNovinBank = cardInfoDecrpted.Substring(0, 6);
                                                //if (eghtesadNovinBank != "627412")
                                                //{
                                                commission = Helpers.GetCommission((long)item.TransactionAmount);
                                                var updateComission = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                                                if (updateComission != null)
                                                {
                                                    updateComission.CommitionAmount = commission;
                                                    db.SaveChanges();
                                                }
                                                //}

                                                var normalTransferAmount = item.TransactionAmount + commission;

                                                #endregion

                                                #region انتقال حساب به حساب
                                                Logger.Info("انتقال حساب به حساب ");

                                                string[] normalTransferSplited;
                                                try
                                                {
                                                    Logger.Info("Normal Transfer");
                                                    var normalTransfer = digiPayService.NormalTransfer
                                                    (
                                                        userInfo.CustomerAccount,
                                                        userInfo.BankAccount,
                                                        (decimal)normalTransferAmount,
                                                        $"|{item.Description}|Amount: {item.TransactionAmount}|Commission: {commission}|Refrence Number: {item.RefrenceNumber}",
                                                        $"|{item.Description}|Refrence Number: {item.RefrenceNumber}",
                                                        int.Parse(YaghutTimeOut),
                                                        userInfo.YaghutUser,
                                                        yaghutPassword
                                                    );
                                                    Logger.Info($"Normal Transfer Reponse: {normalTransfer}");
                                                    normalTransferSplited = normalTransfer.Split('|');
                                                    if (!normalTransferSplited[0].Contains("Res:OK"))
                                                    {
                                                        Logger.Fatal($"Error in refund request with refrence number: {item.RefrenceNumber} and amount: {item.TransactionAmount} and commission: {commission} and description: {normalTransferSplited[1]} for user: {userInfo.UserName}");
                                                    }
                                                    else
                                                    {
                                                        normalTransferValidate = true;
                                                        Logger.Info($"Refund request with refrence number: {item.RefrenceNumber} and amount: {item.TransactionAmount} and commission: {commission} for user: {userInfo.UserName}");
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    var exception = ex.InnerException != null ? $"Exception: {ex.Message} --- {ex.InnerException.Message}" : $"Exception: {ex.Message}";
                                                    normalTransferValidate = false;
                                                    Logger.Fatal($"Error in refund request with refrence number: {item.RefrenceNumber} and amount: {item.TransactionAmount} and commission: {commission} and description: {exception} for user: {userInfo.UserName},NormalTransfer");
                                                    normalTransferSplited = new[] { "Res:KO", $"Desc:{exception}" };
                                                }

                                                #endregion

                                                #region Get Statement
                                                Logger.Info("Get Statement . . . ");
                                                try
                                                {
                                                    if (!normalTransferValidate)
                                                    {
                                                        System.Threading.Thread.Sleep(5000);
                                                        Logger.Info("Get Last Transaction");
                                                        var result = digiPayService.GetLastTransaction(userInfo.BankAccount, userInfo.YaghutUser, yaghutPassword);
                                                        if (result?.statementBeans != null)
                                                        {
                                                            var resultJson = JsonConvert.SerializeObject(result.statementBeans);
                                                            if (resultJson == null || resultJson == "[]")
                                                                Logger.Warn($"اطلاعات صورتحساب برای درخواست {item.RequestId} یافت نشد");
                                                            var statementResult = JsonConvert.DeserializeObject<List<StatementResultModel>>(resultJson);
                                                            var checkStatementModel = statementResult.FirstOrDefault(x => x.Description.Contains(item.Description) && x.Description.Contains($"Amount: {item.TransactionAmount}") && x.Description.Contains($"Refrence Number: {item.RefrenceNumber}"));
                                                            if (checkStatementModel != null)
                                                            {
                                                                checkStatement = true;
                                                                var updateRefundRequest = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                                                                if (updateRefundRequest != null)
                                                                {
                                                                    updateRefundRequest.TransferRrn = checkStatementModel.ReferenceNumber;
                                                                    updateRefundRequest.TransferDateTime = DateTime.Now;
                                                                    db.SaveChanges();
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Logger.Error($"اطلاعات صورتحساب برای درخواست {item.RequestId} یافت نشد");
                                                            checkStatement = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var updateRefundRequest = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                                                        if (updateRefundRequest != null)
                                                        {
                                                            updateRefundRequest.TransferRrn = normalTransferSplited[1].Replace("RRN:", "");
                                                            updateRefundRequest.TransferDateTime = DateTime.Now;
                                                            db.SaveChanges();
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    checkStatement = false;
                                                    normalTransferValidate = false;
                                                    var exception = ex.InnerException != null ? $"Exception: {ex.Message} --- {ex.InnerException.Message}" : $"Exception: {ex.Message}";
                                                    Logger.Fatal($"Error in refund request with refrence number: {item.RefrenceNumber} and amount: {item.TransactionAmount} and commission: {commission} and description: {exception} for user: {userInfo.UserName},GetLastTransaction");
                                                }

                                                #endregion
                                            }
                                            else
                                            {
                                                checkStatement = true;
                                                normalTransferValidate = true;
                                            }
                                            if (normalTransferValidate || checkStatement && item.RetryCount <= int.Parse(NormalTransferRetryCount))
                                            {
                                                #region اضافه کردن کارت به کارت ها به پایگاه داده و برگرداندن شماره شناسایی آن ها 

                                                var cardTransfersIds = Helpers.GetCardForTransfersIds((long)item.TransactionAmount, item.RefrenceNumber, "-", cardInfoDecrpted, userInfo.UserName);

                                                #endregion

                                                if (cardTransfersIds.Any())
                                                {
                                                    #region Update Refund Request Card Transfer Inserted

                                                    var updateRefundRequestCardTransferInserted = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                                                    if (updateRefundRequestCardTransferInserted != null)
                                                        updateRefundRequestCardTransferInserted.CardTransferInserted = true;
                                                    db.SaveChanges();

                                                    #endregion
                                                }
                                                else
                                                {
                                                    var updateRefundRequest = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                                                    if (updateRefundRequest != null)
                                                    {
                                                        updateRefundRequest.RefundDescription = "خطا در تولید کارت به کارت ها";
                                                        updateRefundRequest.RefundStatus = "01";
                                                        db.SaveChanges();
                                                    }
                                                }
                                            }
                                            else if (item.RetryCount == int.Parse(NormalTransferRetryCount))
                                            {
                                                var updateRefundRequest = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                                                if (updateRefundRequest != null)
                                                {
                                                    updateRefundRequest.RefundDescription = "خطا در انتقال وجه از حساب";
                                                    updateRefundRequest.RefundStatus = "03";
                                                    db.SaveChanges();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var updateRefundRequest = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                                            if (updateRefundRequest != null)
                                            {
                                                updateRefundRequest.RefundDescription = "خطا در انتقال وجه از حساب";
                                                updateRefundRequest.RefundStatus = "03";
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var updateRefundRequest = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                                        if (updateRefundRequest != null)
                                        {
                                            updateRefundRequest.RefundDescription = "شماره کارت مقصد معتبر نمی باشد";
                                            updateRefundRequest.RefundStatus = "-45";
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var updateRefundRequest = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                                if (updateRefundRequest != null)
                                {
                                    updateRefundRequest.RefundDescription = "اطلاعات بازپرداخت اشتباه است";
                                    updateRefundRequest.RefundStatus = "-46";
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var description = ex.InnerException != null ? $"{ex.Message} -- {ex.InnerException.Message}" : ex.Message;
                Logger.Fatal($"Error: {description}");
            }
        }

        public void ProviderRefundTransferService()
        {
            try
            {
                using (var db = new PNA_RefundServiceEntities())
                {
                    var notInsertedProviderRefundRequest = db.RefundRequest.Where(x => x.IsProvider == true &&
                                                           (x.IBANTransferInserted == false || x.IBANTransferInserted == null) &&
                                                           x.RefundStatus == "01").ToList();

                    if (notInsertedProviderRefundRequest.Any())
                    {
                        Logger.Info($"notInsertedProviderRefundRequest Count :{notInsertedProviderRefundRequest.Count}");
                        foreach (var item in notInsertedProviderRefundRequest)
                        {
                            Thread.Sleep(2000);
                            Logger.Info($"item :{JsonConvert.SerializeObject(item)}");

                            var updateRefundRequest = db.RefundRequest.FirstOrDefault(x => x.RefrenceNumber == item.RefrenceNumber);
                            if (updateRefundRequest == null)
                                continue;

                            if (item.TransactionAmount == null || item.TransactionAmount <= 0)
                            {
                                updateRefundRequest.RefundDescription = "اطلاعات بازپرداخت اشتباه است";
                                updateRefundRequest.RefundStatus = "-46";
                                db.SaveChanges();
                                continue;
                            }

                            #region بدست آوردن اطلاعات کاربر

                            var userInfoNotNull = false;
                            var userInfo = db.WebServiceUser.FirstOrDefault(x => x.UserName == item.UserName);
                            if (userInfo != null)
                            {
                                userInfoNotNull = true;
                                Logger.Info($"userInfo:{userInfo.UserName}");
                            }

                            #endregion

                            if (userInfoNotNull)
                            {
                                #region برگرداندن اطلاعات رمز شده کارت های مقصد برای عملیات استعلام شبا

                                string cardInfoDecrypted = null;
                                try
                                {
                                    cardInfoDecrypted = CryptorEngine.Decryption(item.EncryptedPan, PrivateKey);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Error($"Error in decryption bank card info,description: {ex.Message}");
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

                                #endregion

                                if (cardInfoDecrypted == null)
                                {
                                    updateRefundRequest.RefundDescription = "شماره کارت مقصد معتبر نمی باشد";
                                    updateRefundRequest.RefundStatus = "-45";
                                    db.SaveChanges();
                                    continue;
                                }

                                #region تبدیل کارت به شبا و ثبت در دیتابیس

                                //var request = new FaraboomCardToIbanRequestModel { pan = cardInfoDecrypted };
                                //var faraboomService = new FaraBoomService();
                                //var result = faraboomService.CardToIbanService(request).Result;

                                var request = new JibitCardToIbanRequestModel { CardNumber = cardInfoDecrypted };
                                var jibitService = new JibitService();

                                var providerDetailes = db.ProviderTransferDetailes.Where(x => x.RefrenceNumber == item.RefrenceNumber).FirstOrDefault();
                                if (providerDetailes == null)
                                {
                                    providerDetailes = new ProviderTransferDetailes()
                                    {
                                        RefrenceNumber = item.RefrenceNumber,
                                        Status = typeof(JibitServiceStatusEnum).GetItems().FirstOrDefault(x => x.Id == (int)JibitServiceStatusEnum.IbanInquiry).Title,
                                    };
                                    db.ProviderTransferDetailes.Add(providerDetailes);
                                    db.SaveChanges();
                                }

                                var result = jibitService.CardToIbanService(request).Result;

                                providerDetailes = db.ProviderTransferDetailes.Where(x => x.RefrenceNumber == item.RefrenceNumber).FirstOrDefault();

                                providerDetailes.ResponseStatus = result.Status;

                                if (result.Status == false)
                                {
                                    providerDetailes.Message = "Card To IBAN : " + result.Message;
                                    providerDetailes.ErrorMessage = result.Message + "-" + result.Code;
                                    updateRefundRequest.RefundDescription = "شماره کارت مقصد معتبر نمی باشد";
                                    updateRefundRequest.RefundStatus = "-45";

                                    db.SaveChanges();

                                    continue;
                                }
                                else
                                {
                                    var ownerNames = new List<string>();
                                    foreach (var owner in result.IbanInfo.Owners)
                                    {
                                        ownerNames.Add(owner.FirstName + " " + owner.LastName);
                                    }
                                    var owners = string.Join("|", ownerNames);

                                    providerDetailes.Message = "Card To IBAN : " + result.Message;
                                    providerDetailes.CardStatus = result.IbanInfo.Status;
                                    providerDetailes.CardOwner = owners;

                                    db.SaveChanges();
                                }

                                var encryptedIban = CryptorEngine.Encryption(result.IbanInfo.Iban, PrivateKey);
                                var ibanTransfer = new IBANTransfer
                                {
                                    EncryptedIBAN = encryptedIban,
                                    EncryptedPan = item.EncryptedPan,
                                    Amount = item.TransactionAmount,
                                    RefundRefrenceNumber = item.RefrenceNumber,
                                    InsertDateTime = DateTime.Now,
                                    Status = "-1",
                                    InquiryCommision = Helpers.GetCardToIbanCommission()
                                };
                                db.IBANTransfer.Add(ibanTransfer);


                                providerDetailes = db.ProviderTransferDetailes.Where(x => x.RefrenceNumber == item.RefrenceNumber).FirstOrDefault();
                                providerDetailes.Status = typeof(JibitServiceStatusEnum).GetItems().FirstOrDefault(x => x.Id == (int)JibitServiceStatusEnum.TransferInserted).Title;

                                updateRefundRequest.IBANTransferInserted = true;

                                db.SaveChanges();

                                #endregion

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var description = ex.InnerException != null ? $"{ex.Message} -- {ex.InnerException.Message}" : ex.Message;
                Logger.Fatal($"Error: {description}");
            }
        }

        private void RefundTransferLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}