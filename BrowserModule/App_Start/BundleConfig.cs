using System.Web;
using System.Web.Optimization;

namespace DF_WebModule
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/jquery-confirm.js",
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/dataTables.bootstrap4.js",
                        "~/Scripts/DataTables/dataTables.fixedColumns.js",
                        "~/Scripts/DataTables/dataTables.dateTime.min.js",
                        "~/Scripts/Chart.js",
                        "~/Scripts/jquery.form.js",
                        "~/Scripts/bootstrap-datetimepicker.min.js",
                          "~/Scripts/bootstrap-datepicker.min.js",
                        "~/Scripts/jTimeout.min.js",
                        "~/Scripts/jAlert.min.js",
                        "~/Scripts/jAlert-functions.min.js",
                        "~/Scripts/bootstrap-timepicker.min.js",
                        "~/Scripts/tempusdominus-bootstrap-4.min.js",
                        "~/Scripts/date-euro.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/jquery-confirm.css",
                      "~/Content/Loading.css",
                      "~/Content/DataTables/css/dataTables.bootstrap4.css",
                      "~/Content/DataTables/css/fixedColumns.bootstrap4.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/bootstrap-timepicker.css",
                         "~/Content/bootstrap-datepicker.min.css",
                      "~/Content/jAlert.css",
                      "~/Content/tempusdominus-bootstrap-4.min.css"));
            bundles.Add(new ScriptBundle("~/Scripts/").Include(
                        "~/Scripts/Chart.js"));
        }
    }
}
