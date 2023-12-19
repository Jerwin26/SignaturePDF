using System.Web;
using System.Web.Optimization;

namespace SignaturePDF
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/pdf.js/css").Include(
               "~/pdf.js/viewer.css"
               ));

            bundles.Add(new ScriptBundle("~/pdf.js/js").Include(
                "~/pdf.js/compatibility.js",
                "~/pdf.js/l10n.js",
                "~/pdf.js/core.js",
                "~/pdf.js/util.js",
                "~/pdf.js/api.js",
                "~/pdf.js/metadata.js",
                "~/pdf.js/canvas.js",
                "~/pdf.js/obj.js",
                "~/pdf.js/function.js",
                "~/pdf.js/charsets.js",
                "~/pdf.js/cidmaps.js",
                "~/pdf.js/colorspace.js",
                "~/pdf.js/crypto.js",
                "~/pdf.js/evaluator.js",
                "~/pdf.js/fonts.js",
                "~/pdf.js/glyphlist.js",
                "~/pdf.js/image.js",
                "~/pdf.js/metrics.js",
                "~/pdf.js/parser.js",
                "~/pdf.js/pattern.js",
                "~/pdf.js/stream.js",
                "~/pdf.js/worker.js",
                "~/pdf.js/jpg.js",
                "~/pdf.js/jpx.js",
                "~/pdf.js/jbig2.js",
                "~/pdf.js/bidi.js"
                ));

            bundles.Add(new ScriptBundle("~/pdf.js/vd").Include(
                "~/pdf.js/viewer.js",
                "~/pdf.js/debugger.js"
                ));
        }
    }
}
