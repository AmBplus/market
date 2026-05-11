using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ResultHelper;

public class ResultHelper
{
    public int ResultCode { get; set; }
    public string? ResultMessage { get; set; }

    public const int OkResult = 1;
    public const int ErrorResult = -1;


    public ResultOperation GetResult()
    {
        if (ResultCode == OkResult) {

#pragma warning disable CS8604 // Possible null reference argument.
            return ResultOperation.ToSuccessResult(ResultMessage);
        }
        else
        {
            return ResultOperation.ToFailedResult(ResultMessage);
        }
#pragma warning restore CS8604 // Possible null reference argument.

    }
}