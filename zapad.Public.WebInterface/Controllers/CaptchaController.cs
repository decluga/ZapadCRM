using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using zapad.Public.WebInterface.Models.Authorization;

namespace zapad.Public.WebInterface.Controllers
{
    public class CaptchaController : Controller
    {
        public ActionResult Image(int id, int g)
        {
            this.Response.Clear();
            this.Response.ContentType = "image/gif";
            using (Bitmap image = id.GetCaptchaImage())
                image.Save(this.Response.OutputStream, ImageFormat.Gif);
            return null;
        }
        public ActionResult NewImage(int id, int CaptchaLength, string CaptchaChars)
        {
            id.ReNewCaptcha(CaptchaLength, CaptchaChars);    // новая капча
            this.Response.Clear();
            return null;
        }
    }
}