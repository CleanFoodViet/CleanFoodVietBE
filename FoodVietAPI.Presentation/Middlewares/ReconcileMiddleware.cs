using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Presentation.Middlewares
{
    public class ReconcileMiddleware
    {
        private static readonly Regex CreatePostRoute =
            new(@"^/api/v1/gardener/[^/]+/posts/?$", RegexOptions.Compiled);

        private static readonly Regex UpdatePostRoute =
            new(@"^/api/v1/posts/[^/]+$", RegexOptions.Compiled);

        private readonly RequestDelegate _next;

        public ReconcileMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext ctx,
            // ISubscriptionReconciler reconciler,  // no longer needed
            // IGardenerRepository repo,            // no longer needed
            ILogger<ReconcileMiddleware> logger)
        {
            try
            {
                if (ctx.User.Identity?.IsAuthenticated == true)
                {
                    var path = ctx.Request.Path.Value ?? "";
                    var method = ctx.Request.Method;

                    var isCreate = method == HttpMethods.Post
                                && CreatePostRoute.IsMatch(path);
                    var isUpdate = method == HttpMethods.Patch
                                && UpdatePostRoute.IsMatch(path);

                    if (isCreate || isUpdate)
                    {
                        // per-request reconciliation was here.
                        // we've moved to a single stored-proc call in the 15-min job,
                        // so nothing needs doing on each post for now.

                        /*
                        var gidClaim = ctx.User.FindFirst("GardenerId")?.Value;
                        if (!string.IsNullOrEmpty(gidClaim)
                            && Ulid.TryParse(gidClaim, out var gardenerId))
                        {
                            await reconciler.ReconcileAsync(gardenerId);
                        }
                        */
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ReconcileMiddleware failed on {Path}",
                                ctx.Request.Path);
            }

            await _next(ctx);
        }
    }
}