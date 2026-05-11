using Microsoft.AspNetCore.Http;
using IranKish.Models;
using System.Threading.Tasks;

namespace IranKish.Interface
{
    public interface IIRanKishVerification
    {
        Task<VerifyResult> Verify(RequestVerify requestVerify);
    }
}
