public interface IHtmlConverter
{
    Task<string> CreateHtmlLayout(ExcelData data);
}