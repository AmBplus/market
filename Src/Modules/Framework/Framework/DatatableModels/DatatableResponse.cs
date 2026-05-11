using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.DatatableModels
{
    public class DataTableResponse<T>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<T>? Data { get; set; }
        public string? Error { get; set; }
    }

}

