using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DatatableModels;

public class DatatableFullRequest
{
    public int draw { get; set; }
    public Column[] columns { get; set; }
    public Order[] order { get; set; }
    public int start { get; set; }
    public int length { get; set; }
    public Search search { get; set; } = new Search();
    public DatatableExportOption ExportOption { get; set; } = new DatatableExportOption();

    public class Column
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public Search search { get; set; }
    }

    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; }
    }

    public class Search
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }
}

public static class DataRequestHelper
{

    public static int PageNumber(this DatatableFullRequest request)
    {
        return request.start / request.length;
    }
    public static int PageSize(this DatatableFullRequest request)
    {
        return request.length <= 0 || request.length > 1000 ? 10 : request.length;
    }

}
public class DatatableExportOption
{
    public int exportType { get; set; } = (int)DatatableExportType.Json;
}
public enum DatatableExportType : int
{
    Json = 1, Excel = 2
}

public static class DatatableFullRequestExtension
{
    public static int GetPageSize(this DatatableFullRequest request)
    {
        if (request.ExportOption.exportType == (int)DatatableExportType.Excel)
        {
            return Int32.MaxValue;
        }
        int pageSize = request.length <= 0 || request.length > 10000 ? 10 : request.length;
        return pageSize;
    }

    public static int GetPageNumber(this DatatableFullRequest request)
    {
        if (request.ExportOption.exportType == (int)DatatableExportType.Excel)
        {
            return 0;
        }
        int pageNumber = request.start / request.length;
        return pageNumber;
    }
}