using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages._Client.Search
{
    public class IndexModel : PageModel
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public int? CategoryId { get; set; }
        public string? MainCategory { get; set; }
        public string SubCategory { get; set; }
        public string SubSubCategory { get; set; }

        public void OnGet(string? mainCategory, string? subCategory, string? subSubCategory , int? id )
        {
            MainCategory = mainCategory;
            SubCategory = subCategory!;
            SubSubCategory = subSubCategory!;
            CategoryId = id;
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    }
}
