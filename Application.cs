using Microsoft.Extensions.Options;

public class Application
{
    private readonly IExcelReader _excelReader;
    private readonly IHtmlConverter _htmlConverter;
    private readonly IOptions<AppSettings> _settings;

    public Application(
        IExcelReader excelReader,
        IHtmlConverter htmlConverter,
        IOptions<AppSettings> settings
    )
    {
        _excelReader = excelReader;
        _htmlConverter = htmlConverter;
        _settings = settings;
    }

    public async Task RunAsync()
    {
        var dataInExcel = await _excelReader.ReadExcel(_settings.Value.InputExcelPath);

        if (!Directory.Exists(_settings.Value.OutputHtmlPath))
        {
            Directory.CreateDirectory(_settings.Value.OutputHtmlPath);
        }

        var processingOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 4
        };

        await Parallel.ForEachAsync(
            dataInExcel,
            processingOptions,
            async (data, cancellationToken) =>
            {
                var html = await _htmlConverter.CreateHtmlLayout(data);
                await File.WriteAllTextAsync(_settings.Value.OutputHtmlPath + $"{data.SheetName}.html", html);
            }
        );
        
        // foreach (var data in dataInExcel)
        // {
        //     var html = _htmlConverter.CreateHtmlLayout(data);
        //     File.WriteAllText(_settings.Value.OutputHtmlPath + data.SheetName + ".html", html);
        // }
    }
}