using System.Web;
using System.Web.Optimization;

namespace FlavorFi.UI.Admin
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/core_scripts").Include(
            //            "~/Content/plugins/jquery/jquery.js",
            //            "~/Content/plugins/jquery-validate/jquery.validate.min.js",
            //            "~/Content/plugins/bootstrap/js/bootstrap.min.js",
            //            "~/Content/plugins/notify/notify.min.js",
            //            "~/Content/plugins/pacejs/pace.min.js",
            //            "~/Content/plugins/knockout/knockout.3.4.2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/view_models").Include("~/ViewModels/*.js"));

            //bundles.Add(new ScriptBundle("~/bundles/others").Include(
            //            "~/Content/plugins/sidebar-nav/dist/sidebar-nav.min.js",
            //            "~/Content/js/jquery.slimscroll.js",
            //            "~/Content/js/waves.js",
            //            "~/Content/plugins/waypoints/lib/jquery.waypoints.js",
            //            "~/Content/plugins/counterup/jquery.counterup.min.js",
            //            "~/Content/plugins/raphael/raphael-min.js",
            //            "~/Content/js/custom.min.js",
            //            "~/Content/plugins/toast-master/js/jquery.toast.js",
            //            "~/Content/plugins/bootstrap-datepicker/bootstrap-datepicker.min.js"));

            //bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
            //            "~/Content/plugins/bootstrap/bootstrap-3.3.7/css/bootstrap.min.css"));

            //bundles.Add(new StyleBundle("~/Content/core_styles").Include(
            //            "~/Content/css/style.css",
            //            "~/Content/less/icons/font-awesome/css/font-awesome.css",
            //            "~/Content/plugins/sidebar-nav/dist/sidebar-nav.min.css",
            //            "~/Content/plugins/toast-master/css/jquery.toast.css",
            //            "~/Content/css/animate.css",
            //            "~/Content/css/colors/default-dark.css",
            //            "~/Content/css/fm-style.min.css"));

            //BundleTable.EnableOptimizations = false;
        }
    }
}