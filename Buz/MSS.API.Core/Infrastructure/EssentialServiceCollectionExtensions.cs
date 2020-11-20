using MSS.API.Core.V1.Business;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSS.API.Common.Utility;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using MSS.API.Common;

namespace MSS.API.Core.Infrastructure
{
    public static class EssentialServiceCollectionExtensions
    {
        public static IServiceCollection AddEssentialService(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IActionGroupService, ActionGroupService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IActionService, ActionService>();
            services.AddTransient<IDictionaryService, DictionaryService>();
            //TODO 需要改成新的java方式的注册发现 services.AddTransient<IServiceDiscoveryProvider, ConsulServiceProvider>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAuthHelper, AuthHelper>();
            services.AddTransient<IUploadFileService, UploadFileService>();
            services.AddTransient<IEquipmentService, EquipmentService>();
            services.AddTransient<IImportExcelConfigService, ImportExcelConfigService>();
            services.AddTransient<IOrgService, OrgService>();
            services.AddTransient<IUserCredService, UserCredService>();
            services.AddTransient<IDocumentService, DocumentService>();
            return services;
        }
    }
}
