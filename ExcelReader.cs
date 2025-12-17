using ClosedXML.Excel;

public class ExcelReader
{
    public List<ExcelData> ReadExcel(string filePath)
    {
        var result = new List<ExcelData>();
        
        using (var workbook = new XLWorkbook(filePath)) 
        {
            var worksheets = workbook.Worksheets.ToList();

            foreach (var worksheet in worksheets)
            {
                var sheetData = ReedSheet(worksheet);
                result.Add(sheetData);
            }
        }

        return result;
    }

    private ExcelData ReedSheet(IXLWorksheet worksheet)
    {

        var range = worksheet.RangeUsed();
        if (range == null) 
        {
            return new ExcelData (
                worksheet.Name,
                new List<string>(),
                new List<List<object>>()
            );
        }

        var headers = range.FirstRow().Cells()
            .Where(c => !c.IsEmpty())
            .Select(c => c.GetValue<string>())
            .ToList();
        
        var dataRows = range.Rows().Skip(1);
        var data = new List<List<object>>();

        foreach(var row in dataRows) 
        {
            if (row.IsEmpty()) continue;

            var rowData = new List<object>();

            for (int i = 0; i < headers.Count() && i < row.CellCount(); i++)
            {
                var cell = row.Cell(i + 1);
                rowData.Add(GetCellValue(cell));
            }

            data.Add(rowData);
        }
        return new ExcelData(worksheet.Name, headers, data);
    }

    private object? GetCellValue(IXLCell cell) 
    {
        if (cell.IsEmpty())
            return null;

        try 
        {
            return cell.DataType switch 
            {
                XLDataType.Text => cell.GetString(),
                XLDataType.Number => cell.GetValue<decimal>(),
                _ => cell.Value
            };
        }
        catch 
        {
            return cell.Value.ToString();
        }
    }

}