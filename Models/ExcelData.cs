public class ExcelData 
{
    public string SheetName { get; set; }
    public List<string> Headers { get; set; }
    public List<List<object>> Data { get; set; }

    public ExcelData(string sheetName, List<string> headers, List<List<object>> data)       
    {
        SheetName = sheetName;
        Headers = headers;
        Data = data;
    }
}