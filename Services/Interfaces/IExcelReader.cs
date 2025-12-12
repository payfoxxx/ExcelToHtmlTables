public interface IExcelReader 
{
    Task<List<ExcelData>> ReadExcel(string filePath);
}