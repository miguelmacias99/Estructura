namespace Cititrans.Auth.Infraestructura.Services.Razor
{
    public interface IHtmlRenderService
    {
        Task<string> RenderizarVistaModelAsync<TModel>(string vista, TModel modelo);

        Task<string> RenderizarVistaAsync(string vista);
    }
}