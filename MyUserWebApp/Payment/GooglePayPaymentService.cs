
using Google.Apis.Auth.OAuth2;
using Google.Apis.PaymentsResellerSubscription.v1;
using Google.Apis.PaymentsResellerSubscription.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyUserWebApp.Payment
{
    public class GooglePayPaymentService
    {
        //    private readonly PaymentsResellerSubscriptionService _service;

        //    public GooglePayPaymentService()
        //    {
        //        var credential = GoogleCredential.FromFile("path/to/your/credential.json")
        //            .CreateScoped(PaymentsResellerSubscriptionService.Scope.PaymentsResellerSubscription);
        //        _service = new PaymentsResellerSubscriptionService(new BaseClientService.Initializer
        //        {
        //            HttpClientInitializer = credential,
        //            ApplicationName = "Your Application Name",
        //        });
        //    }

        //    public async Task<string> CreatePaymentAsync()
        //    {
        //        var payment = new SubscriptionPurchase();
        //        payment.SubscriptionId = "Your Subscription Id";
        //        payment.ProductId = "Your Product Id";
        //        payment.Quantity = 1;
        //        payment.StartTime = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz");
        //        payment.TrialPeriodDays = 7;

        //        var request = _service.Subscriptions.Purchases.Create(payment, "partnerId");
        //        request.RequestBody = payment;
        //        var response = await request.ExecuteAsync();
        //        return response.Id;
        //    }
        //}
    }
}

