using System.Threading.Tasks;

namespace MSS.API.Common.Utility
{
    public interface IServiceDiscoveryProvider
    {
        Task<string> GetServiceAsync(string serviceName);
    }
}