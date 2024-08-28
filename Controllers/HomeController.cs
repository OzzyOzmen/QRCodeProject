using Microsoft.AspNetCore.Mvc;
using QRCodeProject.Models;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static QRCoder.PayloadGenerator;

namespace QRCodeProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            QRCodeModel model = new QRCodeModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(QRCodeModel model)
        {

            Payload payload = null;

            switch (model.QRCodeType)
            {
                case 1: // website url
                    payload = new Url(model.WebsiteURL);
                    break;
                case 2: // bookmark url
                    payload = new Bookmark(model.BookmarkURL, model.BookmarkURL);
                    break;
                case 3: // compose sms
                    payload = new SMS(model.SMSPhoneNumber, model.SMSBody);
                    break;
                case 4: // compose whatsapp message
                    payload = new WhatsAppMessage(model.WhatsAppNumber, model.WhatsAppMessage);
                    break;
                case 5://compose email
                    payload = new Mail(model.ReceiverEmailAddress, model.EmailSubject, model.EmailMessage);
                    break;
                case 6: // wifi qr code
                    payload = new WiFi(model.WIFIName, model.WIFIPassword, WiFi.Authentication.WPA);
                    break;

            }



            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload);
            BitmapByteQRCode qrCode = new (qrCodeData);
            string base64String = Convert.ToBase64String(qrCode.GetGraphic(20));
            model.QRImageURL = "data:image/png;base64," + base64String;
            return View("Index", model);

        }

       
    }
}
