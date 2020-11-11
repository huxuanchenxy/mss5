using MSS.API.Model;
using MSS.API.Model.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using MSS.API.Dao.Interface;
using MSS.API.Dao.Implement;
using Dapper.FluentMap;

namespace MSS.API.Dao
{
    public static class DapperServiceCollectionExtensions
    {
        public static IServiceCollection AddDapper(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            var optionsSection = configuration.GetSection("Dapper");
            var options = new DapperOptions();
            optionsSection.Bind(options);
            services.AddSingleton<DapperOptions>(options);
            services.AddTransient<IUserRepo<User>,UserRepo>();
            services.AddTransient<IActionGroupRepo<ActionGroup>, ActionGroupRepo>();
            services.AddTransient<IRoleRepo<Role>, RoleRepo>();
            services.AddTransient<IActionRepo<ActionInfo>, ActionRepo>();
            services.AddTransient<IDictionaryRepo<DictionaryTree>, DictionaryRepo>();
            services.AddTransient<IUploadFileRepo<UploadFile>, UploadFileRepo>();
            services.AddTransient<IEquipmentRepo<Equipment>, EquipmentRepo>();
            services.AddTransient<IImportExcelConfigRepo<ImportExcelConfig>, ImportExcelConfigRepo>();
            services.AddTransient<IOrgRepo<OrgTree>, OrgRepo>();
            services.AddTransient<IUserCredRepo<UserCred>, UserCredRepo>();

            //配置列名映射
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new BaseEntityMap());
                config.AddMap(new UserMap());
                config.AddMap(new EquipmentMap());
                config.AddMap(new ImportExcelConfigMap());
                config.AddMap(new OrgTreeMap());
                config.AddMap(new OrgNodeTypePropertyMap());
                config.AddMap(new OrgNodeTypeMap());
                config.AddMap(new OrgNodePropertyMap());
                config.AddMap(new OrgUserMap());
                config.AddMap(new UserCredMap());
                
            });
            return services;
        }
    }
}
