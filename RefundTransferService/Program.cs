using System;
using System.ServiceProcess;
using System.Threading;

namespace RefundTransferService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        static void Main()
        {

            if (!Environment.UserInteractive)
                // running as service
                using (var service = new RefundTransfer())
                    ServiceBase.Run(service);
            else
            {
                // running as console app

                var a = new RefundTransfer();
                a.RefundTransferService();
                Thread.Sleep(1000);
                a.ProviderRefundTransferService();
                Thread.Sleep(1000);
            }
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new global::RefundTransferService.RefundTransfer()
            //};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}
