using Microsoft.AspNetCore.Mvc;
using System;

public class LiqPayController : Controller
{
    //private readonly string _liqpayPublicKey;
    //private readonly string _liqpayPrivateKey;

    //public LiqPayController(IConfiguration configuration)
    //{
    //    _liqpayPublicKey = configuration["LiqPay:PublicKey"];
    //    _liqpayPrivateKey = configuration["LiqPay:PrivateKey"];
    //}

    //public IActionResult Index()
    //{
    //    return View();
    //}

    //[HttpPost]
    //public IActionResult Pay(string amount, string card_number, string exp_month, string exp_year, string cvc)
    //{
    //    // Создайте экземпляр класса LiqPayClient
    //    var client = new LiqPay.SDK.LiqPayClient(_liqpayPublicKey, _liqpayPrivateKey);

    //    // Создайте токен карты
    //    var token = client.RequestAsync /*RequestLiqPayApi<string>*/("https://www.liqpay.ua/api/request",
    //        requestParams: new LiqPay.SDK.Dto.LiqPayRequest
    //        {
    //            Action = LiqPay.SDK.Dto.Enums.LiqPayRequestAction.PayToken,
    //            Version = 3,
    //            PublicKey = _liqpayPublicKey,
    //            Amount = Convert.ToInt32(amount),
    //            Currency = "UAH",
    //            Description = "Payment",
    //            CardToken = card_number + exp_month + exp_year + cvc
    //        }) ;
    //            //card_exp_month = exp_month,
    //            //card_exp_year = exp_year,
    //            //card_cvv = cvc });

    //    // Верните пользователю страницу с формой оплаты
    //    return View("PayForm", new { token });
    //}
}
