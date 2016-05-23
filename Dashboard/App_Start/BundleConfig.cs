using System.Web;
using System.Web.Optimization;

namespace Dashboard
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/font-awesome/css/font-awesome.min.css",
                "~/Content/animate.css",
                "~/Content/style.css",
                "~/Content/plugins/sweetalert/sweetalert.css",
                "~/Content/plugins/select2/select2.min.css",
                "~/Content/plugins/dataTables/dataTables.bootstrap.css",
                "~/Content/plugins/dataTables/dataTables.responsive.css",
                "~/Content/plugins/dataTables/dataTables.tableTools.min.css",
                "~/Content/plugins/dropzone/dropzone.css",
                "~/Content/datepicker.css"
                ));



            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(

                "~/Scripts/jquery-2.1.1.js",
                "~/Scripts/plugins/jquery-ui/jquery-ui.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/datepicker.js",
                "~/Scripts/plugins/metisMenu/jquery.metisMenu.js",
                "~/Scripts/plugins/pace/pace.min.js",
                "~/Scripts/plugins/select2/select2.full.min.js",
                "~/Scripts/inspinia.js",
                "~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js",
                "~/Scripts/plugins/sweetalert/sweetalert.min.js",
                "~/Scripts/plugins/gritter/jquery.gritter.min.js",
                "~/Scripts/plugins/sparkline/jquery.sparkline.min.js",
                "~/Scripts/plugins/dataTables/jquery.dataTables.js",
                "~/Scripts/plugins/dataTables/dataTables.bootstrap.js",
                "~/Scripts/plugins/dataTables/dataTables.responsive.js",
                "~/Scripts/plugins/dataTables/dataTables.tableTools.js",
                "~/Scripts/plugins/peity/jquery.peity.min.js",
                "~/Scripts/plugins/dropzone/dropzone.js",
                "~/Scripts/highcharts/highcharts.js",
                "~/Scripts/highcharts/modules/data.js",
                "~/Scripts/highcharts/modules/exporting.js",
                "~/Scripts/highcharts/modules/highcharts-3d.js",
                "~/Scripts/knockout-3.3.0.js",
                "~/Scripts/knockout.mapping-latest.js",
                "~/Scripts/KO-Models/ko-helpers.js",
                "~/Scripts/plugins/idle-timer/idle-timer.min.js"


                ));
        }
    }
}
