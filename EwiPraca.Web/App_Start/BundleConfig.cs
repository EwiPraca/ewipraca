using System.Web.Optimization;

namespace EwiPraca
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-1.7.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.min.js",
                      "~/Scripts/bootstrap-dialog.min.js",
                      "~/Scripts/ewi-praca.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      // "~/Content/bootstrap.min.css",
                      "~/Content/Site.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                      "~/Scripts/DataTables/jquery.dataTables.min.js",
                      "~/Scripts/DataTables/dataTables.responsive.min.js",
                      "~/Scripts/DataTables/dataTables.bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/datatables").Include(
            "~/Content/DataTables/css/dataTables.bootstrap.min.css",
            "~/Content/DataTables/css/responsive.dataTables.min.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
               "~/Scripts/jquery.validate.min.js",
               "~/Scripts/jquery.validate.unobtrusive.min.js",
               "~/Scripts/jquery.unobtrusive-ajax.min.js"));
        }
    }
}