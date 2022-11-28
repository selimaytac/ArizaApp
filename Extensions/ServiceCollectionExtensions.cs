using ArizaApp.Services;
using ArizaApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ArizaApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadCustomServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IMailSenderService, MailSenderService>();
            serviceCollection.AddScoped<IFileUploadService, FileUploadService>();
            
            return serviceCollection;
        }
    }
}