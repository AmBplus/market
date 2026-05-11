using Feature.Client.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Feature.Client.Common.Service;

namespace Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger , ICommonServices commonServices)
        {
            _logger = logger;
            CommonServices = commonServices;
        }

        public ICommonServices CommonServices { get; }

        public async Task OnGet()
        {
           
        }
    }
}
