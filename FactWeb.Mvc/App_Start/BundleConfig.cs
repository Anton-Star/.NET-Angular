using System.IO;
using System.Web;
using System.Web.Optimization;

namespace FactWeb.Mvc
{
    public class BundleConfig
    {
        public static string appDir = "app";

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/angular-ui/ui-bootstrap.js",
                      "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                      "~/Scripts/angular-recaptcha/angular-recaptcha.js",
                      "~/Scripts/toastr.js",
                      "~/Scripts/kendo/kendo.all.min.js",
                      "~/Scripts/kendo/kendo.angular.js",
                      "~/Scripts/angular-local-storage.js",
                      "~/Scripts/lodash.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/ui-select/select.js",
                      "~/Scripts/FileSaver.js",
                      "~/Scripts/jszip/jszip.js",
                      "~/Scripts/telerikReportViewer-10.1.16.615.min.js"));

            var scriptBundle = new ScriptBundle("~/appScripts");
            var adminAppDirFullPath = HttpContext.Current.Server.MapPath(string.Format("~/{0}", appDir));
            if (Directory.Exists(adminAppDirFullPath))
            {
                scriptBundle.Include(
                    string.Format("~/{0}/app.module.js", appDir),
                    string.Format("~/{0}/app.core.module.js", appDir),
                    string.Format("~/{0}/app.routes.js", appDir))

                    .IncludeDirectory(string.Format("~/{0}", appDir), "*.js", false)
                    .IncludeDirectory(string.Format("~/{0}", appDir), "*.module.js", true)
                    .IncludeDirectory(string.Format("~/{0}", appDir), "*.js", true);
            }
            bundles.Add(scriptBundle);

            var siteStyleBundle = new[]
                                  {
                                      "~/Content/bootstrap.css",
                                      //"~/Content/select2.css",
                                      "~/Scripts/ui-select/select.css",
                                      "~/Content/toastr.css",
                                      //"~/Content/customtheme.css",
                                      //"~/Content/styles.css",
                                      "~/Content/breeze.directives.css",
                                      "~/Content/font-awesome/css/font-awesome.min.css",
                                      "~/Content/kendo/kendo.common.min.css",
                                      "~/Content/kendo/kendo.default.min.css",
                                      "~/Content/global-style.css",
                                      "~/Content/textAngular.css",
                                      "~/Content/site.css",
                                      "~/assets/sky-forms/css/sky-forms.css",
                                      "~/Content/spin.css"
                                  };

            bundles.Add(new StyleBundle("~/Content/site").Include(
                        siteStyleBundle));
        }
    }
}
