using Feature.Client.Common.Models;
using Framework.Services.Orm;
using System.Threading.Tasks;

namespace Web.Feature.Client.Common.Service; 

public interface ICommonServices
{
    Task<List<CategoryMenuDto>> GetCategoryMenu();
    Task<List<ProductRuleDto>> GetProductRule();
}


public class CommonServices : ICommonServices
{
    public CommonServices(IDapperHelper dapperHelper)
    {
        Context = dapperHelper;
    }

    public IDapperHelper Context { get; }

    public async Task<List<CategoryMenuDto>> GetCategoryMenu()
    {
        Dictionary<string,object> parameter = new Dictionary<string,object>();
        parameter.Add("@StoreId", 1);
        parameter.Add("@LanguageId", 1);
        var result = await Context.QueryAsync<CategoryMenuDto>("[dbo].[sp_GetCategoryMenu]", parameter);
        var list = result.ToList();
        return list;
    }
    public async Task<List<ProductRuleDto>> GetProductRule()
    {
        Dictionary<string, object> parameter = new Dictionary<string, object>();
        var result = await Context.QueryAsync<ProductRuleDto>("[dbo].[sp_GetProductRule]", parameter);
        var list = result.ToList();
        return list;

    }
}
