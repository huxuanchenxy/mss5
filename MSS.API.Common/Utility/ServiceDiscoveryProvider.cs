using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Steeltoe.Discovery.Eureka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSS.API.Common.Utility
{
    public class ServiceDiscoveryProvider : IServiceDiscoveryProvider
    {
        public IConfiguration Configuration { get; }
        public ServiceDiscoveryProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<string> GetServiceAsync(string serviceName)
        {
            string ret = string.Empty;
            DiscoveryClient _discoveryClient = new DiscoveryClient(new EurekaClientConfig
            {
                EurekaServerServiceUrls = Configuration["Eureka:EurekaServerServiceUrls"],
                EurekaServerConnectTimeoutSeconds = int.Parse(Configuration["Eureka:EurekaServerConnectTimeoutSeconds"]),
            });

            var items = _discoveryClient.Applications.GetRegisteredApplications();
            var cur = items.Where(c => c.Name == serviceName).FirstOrDefault();

            //得到服务中心所有服务和它的Url地址
            foreach (var item in _discoveryClient.Applications.GetRegisteredApplications())
            {
                if (item.Name.ToLower() == serviceName.ToLower())
                {
                    ret = item.Instances.FirstOrDefault().HomePageUrl;
                    break;
                }
                //yield return $"{item.Name}={item.Instances.FirstOrDefault().HomePageUrl}";
            }

            return ret;
        }


    }
}
