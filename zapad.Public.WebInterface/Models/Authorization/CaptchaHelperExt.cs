using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace zapad.Public.WebInterface.Models.Authorization
{
    /// <summary>
    /// Класс, позволяющий работать с капчей
    /// </summary>
    public static class CaptchaHelperExt
    {
        public static MvcHtmlString frwdCaptcha(this HtmlHelper html, string textRefreshButton, string inviteText, int captchaLength, string captchaChars)
        {
            string rndCaptcha = GenRandomString(captchaLength, captchaChars); // 1. генерируем строку, которую будем проверять
            int idCaptcha = RegCaptchaString(rndCaptcha);                     // 2. регистрируем ее в хранилище и присваиваем ей идентификатор

            return MvcHtmlString.Create(
                "<div class='captcha' id='" + idCaptcha + "' >" +
                    html.Hidden("captchaId", idCaptcha).ToHtmlString() +
                    "<img id='img" + idCaptcha + "' src='/Captcha/Image?id=" + idCaptcha + "&g=1' name='1' />" +
                    "<br/>" +
                    "<a href=\"\" id=\"newcaptcha" + idCaptcha + "\" >" + textRefreshButton + "</a>" +
                "</div>" +
                "<script>" +
                    "$(\"#newcaptcha" + idCaptcha + "\").click(function (e) { " +
                       "e.preventDefault();" +
                       "$.ajax({ url: \"/Captcha/NewImage?id=" + idCaptcha + "&CaptchaLength=" + captchaLength + "&CaptchaChars=" + captchaChars + "\", " +
                        "success: function () { " +
                            "$(\"#img" + idCaptcha + "\").attr({ name: Number($(\"#img" + idCaptcha + "\").attr(\"name\")) + 1 }); " +
                            "$(\"#img" + idCaptcha + "\").attr({ src: \"/Captcha/Image?id=" + idCaptcha + "&g=\"+$(\"#img" + idCaptcha + "\").attr(\"name\") }); " +
                            "}" +
                        "});" +
                    "})" +
                "</script>" +
                "<div class='captchaInput'>" + inviteText + "<br/>" +
                    html.TextBox("captchaInputText", "", new { maxlength = 5, size = 10, @class = "text-box single-line", }).ToHtmlString() +
                "</div>");
        }

        #region Генератор случайной строки (капчи) :private static string GenRandomString(int lenString, string rndMask)
        
        /// <summary>
        /// Генератор случайной строки (из букв и цифр)
        /// </summary>
        /// <param name="lenString">Длина выходной строки</param>
        /// <param name="rndMask">Маска (какие символы можно использовать)</param>
        /// <returns>Вновь созданная случайная строка</returns>
        private static string GenRandomString(int lenString, string rndMask)
        {
            Random rnd = new Random();
            string result = string.Empty;
            for (int i = 0; i < lenString; i++)
                result += rndMask[rnd.Next(0, rndMask.Length - 1)];
            return result;
        }
        #endregion

        private static object lk_obj = new object();

        #region Регистрация и хранилище сгенерированных капчей
        
        /// <summary>
        /// Хранилище сгенерированных капчей
        /// </summary>
        private static Dictionary<int, string> captchaSet = new Dictionary<int, string>();

        /// <summary>
        /// Регистрация капчи в хранилище
        /// </summary>
        /// <param name="captcha">капча</param>
        /// <returns>идентификатор в хранилище</returns>
        private static int RegCaptchaString(string captcha)
        {
            Random rnd = new Random();
            int id = rnd.Next();
            lock (lk_obj)
            {
                while (captchaSet.ContainsKey(id) == true) id = rnd.Next();
                captchaSet.Add(id, captcha);
            }
            return id;
        }
        #endregion

        /// <summary>
        /// Обновление капчи (создание новой строки под тем же идентификатором)
        /// </summary>
        /// <param name="id">Идентификтор капчи в хранилище</param>
        public static void ReNewCaptcha(this int id, int captchaLength, string captchaChars)
        {
            lock (lk_obj)
            {
                captchaSet[id] = GenRandomString(captchaLength, captchaChars);
            }
        }
        /// <summary>
        /// Удаление капчи из хранилища
        /// </summary>
        /// <param name="id">Идентификтор капчи в хранилище</param>
        public static void RemoveCaptcha(this int id)
        {
            lock (lk_obj)
            {
                try
                {
                    captchaSet.Remove(id);
                }
                catch { }
            }
        }

        #region Генератор картинки

        private const int Width = 120;
        private const int Height = 60;
        private static readonly string[] _fonts = new string[] {
                                        "Times New Roman",
                                        "Georgia",
                                        "Arial",
                                        "Comic Sans MS",
                                      };
        private const double xFreq = 2 * Math.PI / Width;
        private const double yFreq = 2 * Math.PI / Height;
        private const double WarpFactor = 1.6;
        private const double xAmp = WarpFactor * Width / 100;
        private const double yAmp = WarpFactor * Height / 85;

        /// <summary>
        /// Генерируем изображение для соответствующей капчи
        /// </summary>
        /// <param name="id">Идентификатор капчи в хранилище</param>
        /// <returns>Изображение</returns>
        public static Bitmap GetCaptchaImage(this int id)
        {
            Bitmap result = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(result))
            {
                Rectangle rect = new Rectangle(0, 0, Width, Height);
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                using (var solidBrush = new SolidBrush(Color.White))
                {
                    graphics.FillRectangle(solidBrush, rect);
                }
                Random rnd = new Random();
                FontFamily fnt = new FontFamily(_fonts[rnd.Next(_fonts.Length - 1)]);
                using (Font rndFont = new Font(fnt, 25)) // выбираем случайный шрифт с фиксированным размером - 25
                using (StringFormat fontFormat = new StringFormat())
                {
                    fontFormat.Alignment = StringAlignment.Center;
                    fontFormat.LineAlignment = StringAlignment.Center;

                    GraphicsPath path = new GraphicsPath();
                    path.AddString(captchaSet[id], rndFont.FontFamily, (int)rndFont.Style, rndFont.Size, rect, fontFormat);
                    using (SolidBrush solidBrush = new SolidBrush(Color.Red))
                    {   // портим изображение
                        PointF[] deformed = new PointF[path.PathPoints.Length];
                        double xSeed = rnd.NextDouble() * 2 * Math.PI;
                        double ySeed = rnd.NextDouble() * 2 * Math.PI;
                        for (int i = 0; i < path.PathPoints.Length; i++)
                        {
                            PointF original = path.PathPoints[i];
                            double val = xFreq * original.X * yFreq * original.Y;
                            int xOffset = (int)(xAmp * Math.Sin(val + xSeed));
                            int yOffset = (int)(yAmp * Math.Sin(val + ySeed));
                            deformed[i] = new PointF(original.X + xOffset, original.Y + yOffset);
                        }
                        graphics.FillPath(solidBrush, new GraphicsPath(deformed, path.PathTypes));
                    }
                }
            }
            return result;
        }
        #endregion

        /// <summary>
        /// Проверка корректности капчи (если результат сравнения положительный - капча удаляется из хранилища)
        /// </summary>
        /// <param name="id">Идентификатор капчи</param>
        /// <param name="captcha">Капча</param>
        /// <returns>Результат сравнения</returns>
        public static bool VerifyCaptcha(this int id, string captcha)
        {
            try
            {
                bool result;
                lock (lk_obj)
                {
                    result = captchaSet[id].Trim().ToUpper() == captcha.Trim().ToUpper();
                }
                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}