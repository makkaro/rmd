using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Remedium.Web.Utilities
{
    public static class Extensions
    {
        public static async Task ForEachAsync<T>(this IEnumerable<T> collection, Func<T, Task> func)
        {
            foreach (var value in collection)
            {
                await func(value);
            }
        }

        public static async Task<String> RenderViewAsync<TModel>(this Controller controller, String view, TModel model,
            Boolean partialView = false)
        {
            if (String.IsNullOrEmpty(view))
            {
                view = controller.ControllerContext.ActionDescriptor.ActionName;
            }
            controller.ViewData.Model = model;

            await using var writer = new StringWriter();
            var viewEngine = controller.HttpContext.RequestServices
                .GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            var viewResult = viewEngine?.FindView(controller.ControllerContext, view, !partialView);
            if (viewResult is null || !viewResult.Success)
            {
                return $"Failed to render: {view}";
            }

            var viewContext = new ViewContext
            (
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                writer,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);

            return writer.GetStringBuilder().ToString();
        }
    }
}