using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;
namespace Web.Helper;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;

public class CustomRouteConvention : IPageRouteModelConvention
{
    public void Apply(PageRouteModel model)
    {
        var selectors = model.Selectors.ToList();
        model.Selectors.Clear();

        foreach (var selector in selectors)
        {
            if (selector.AttributeRouteModel?.Template == null) continue;

            var template = selector.AttributeRouteModel.Template;

            // Split into segments and remove ALL segments that start with _
            var newSegments = template.Split('/')
                .Where(segment => !segment.StartsWith("_")) // Remove ALL segments starting with _
                .ToArray();

            var newTemplate = string.Join("/", newSegments);

            model.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel
                {
                    Template = newTemplate.Length > 0 ? newTemplate : ""
                }
            });
        }
    }
}