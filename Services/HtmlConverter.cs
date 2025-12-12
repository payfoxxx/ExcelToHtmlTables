using System.Text;

public class HtmlConverter : IHtmlConverter
{
    public Task<string> CreateHtmlLayout(ExcelData data)
    {
        return Task.Run(() => 
        {
            string headersHtml = GetTableHeaders(data.Headers);
            string bodyHtml = GetTableBody(data.Data);
            string htmlLayout = $"""
                <h3>
                    Перечень {data.SheetName}
                </h3>
                <div>
                    <table class="light" style="width:100%;" align="left" border="1" cellpadding="1" cellspacing="1">
                        <thead>
                            <tr>
                                {headersHtml}
                            </tr>
                        </thead>
                        <tbody>
                            {bodyHtml}
                        </tbody>
                    </table>
                </div>
            """;
            return htmlLayout;
        });
    }

    private string GetTableHeaders(List<string> headers)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("""<th scope="col"><p>№</p></th>""");
        foreach (string header in headers)
        {
            sb.Append("""<th scope="col">""");
            sb.Append("<p>");
            sb.Append(header);
            sb.Append("</p>");
            sb.Append("</th>");
        }
        return sb.ToString();
    }

    private string GetTableBody(List<List<object>> data)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < data.Count; i++)
        {
            sb.Append("<tr>");
            sb.Append($"<td><p>{i + 1}</p></td>");
            for (int j = 0; j < data[i].Count; j++)
            {
                sb.Append($"<td><p>{data[i][j]}</p></td>");
            }
            sb.Append("</tr>");
        }
        return sb.ToString();
    }
}