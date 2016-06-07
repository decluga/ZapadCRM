using System.Web;
using System.Web.Optimization;
using zapad.Public.WebInterface.Models.Tools;

namespace zapad.Public.WebInterface
{
    public class BundleConfig
    {
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            string vKendo = "2016.2.518";

            // All css
            bundles.Add(new StylePathBundle("~/Content/AllCss").Include(
                "~/Content/kendo/" + vKendo + "/kendo.common-bootstrap.min.css",
                "~/Content/kendo/" + vKendo + "/kendo.bootstrap.min.css",
               "~/Content/bootstrap/bootstrap.css",
               "~/Content/bxslider/jquery.bxslider.css",
               "~/Content/fancybox/jquery.fancybox.css",
               "~/Content/Site.css"
            ));

            // Kendo JS
            bundles.Add(new ScriptBundle("~/Scripts/KendoJS").Include(
                "~/Scripts/kendo/" + vKendo + "/jquery.min.js",
                "~/Scripts/kendo/" + vKendo + "/kendo.all.min.js",
                //"~/Scripts/kendo/" + vKendo + "/kendo.modernizr.custom.js",
                "~/Scripts/kendo/" + vKendo + "/cultures/kendo.culture.ru-RU.min.js",
                "~/Scripts/kendo/" + vKendo + "/messages/kendo.messages.ru-RU.min.js",

                "~/Scripts/customKendo.js"
            ));

            // Bootstrap JS
            bundles.Add(new ScriptBundle("~/Scripts/BootstrapJS").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"
            ));

            // App JS
            bundles.Add(new ScriptBundle("~/Scripts/AppJS").Include(
                "~/Scripts/app/property.js",
                "~/Scripts/app/helpers.js",
                "~/Scripts/app/message.js"
            ));

            // ThirdParty JS
            bundles.Add(new ScriptBundle("~/Scripts/ThirdPartyJS").Include(
                "~/Scripts/moment/moment-with-locales.js",
                "~/Scripts/bxslider/jquery.bxslider.js",
                "~/Scripts/fancybox/jquery.fancybox.js",
                "~/Scripts/tablehead/tableHeadFixer.js"
            ));

            // Kinetic JS
            bundles.Add(new ScriptBundle("~/Scripts/KineticJS").Include(
                "~/Scripts/kinetic/jquery.kinetic.js",
                "~/Scripts/kinetic/jquery.mousewheel.js"
            ));

            // SignalR JS
            bundles.Add(new ScriptBundle("~/Scripts/SignalRJS").Include(
                "~/Scripts/jquery.signalR-2.2.0.min.js"
            ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
