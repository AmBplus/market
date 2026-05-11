
using Framework.ResultHelper;

namespace AdobeConnectSdk;


public interface IAdobeConnectService
{
    public ResultOperation Connect(string apiUrl, string username, string password);
    public ResultOperation<AdobeMeetingDetail> GetMeetingDetail(string scoId);
    public ResultOperation<AdobeMeetingDetail> GetMeetingDetail(string apiUrl, string username, string password,string scoId);
}
