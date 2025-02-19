using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Plantilla.Infraestructura.Services.Razor
{
    internal class HtmlRenderService : IHtmlRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public HtmlRenderService(
            IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderizarVistaModelAsync<TModel>(string vista, TModel modelo)
        {
            return await RenderizarVistaPrivada(vista, modelo);
        }

        public async Task<string> RenderizarVistaAsync(string vista)
        {
            return await RenderizarVistaPrivada<object>(vista, default);
        }

        private async Task<string> RenderizarVistaPrivada<TModel>(string vista, TModel? modelo)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());

            using var sw = new StringWriter();

            // Verifica si la vista es una ruta absoluta o relativa
            var viewResult = _razorViewEngine.GetView(null, vista, false);
            if (!viewResult.Success)
            {
                viewResult = _razorViewEngine.FindView(actionContext, vista, false);
            }

            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"No se encontró la vista '{vista}'");
            }

            var viewDictionary = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            if (modelo is not null) viewDictionary.Model = modelo!;

            var tempData = new TempDataDictionary(httpContext, _tempDataProvider);
            var viewContext = new ViewContext(actionContext, viewResult.View, viewDictionary, tempData, sw, new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
    }
}