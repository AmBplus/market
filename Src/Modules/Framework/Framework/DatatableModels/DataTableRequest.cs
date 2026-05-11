using System.Text.Json.Serialization;

namespace Framework.DatatableModels
{
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public class DataTableRequest
    {
        public int length { get; set; }
        public int start { get; set; }
        public int sortColumn { get; set; }
        public string sortColumnDirection { get; set; }
        public string sortColumnIndex { get; set; }
        public int draw { get; set; }
        public string searchValue { get; set; }
        public int pageSize { get; set; }
        public int skip { get; set; }

    }
    public class DataTableRequest<T>
    {
        public int length { get; set; }
        public int start { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDirection { get; set; }

        public string sortColumnIndex { get; set; }

        public int draw { get; set; }
        public string searchValue { get; set; }
        public int pageSize { get; set; }
        public int skip { get; set; }
        public T data { get; set; }

    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning restore IDE1006 // Naming Styles
}




