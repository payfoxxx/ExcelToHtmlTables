using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddCommandLine(args)
            .Build();
        
        var serviceProvider = new ServiceCollection()
            .Configure<AppSettings>(configuration.GetSection("AppSettings"))
            .AddSingleton<IExcelReader, ExcelReader>()
            .AddSingleton<IHtmlConverter, HtmlConverter>()
            .AddSingleton<Application>()
            .BuildServiceProvider();
        
        try 
        {
            var app = serviceProvider.GetService<Application>();
            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}