using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MSS.API.Common;
using MSS.API.Common.Global;
using MSS.API.Core.Infrastructure;
using MSS.API.Dao;
using MSS.Common.Consul;
using System;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            InitConst();
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //压缩 gzip
            services.AddResponseCompression();

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            

            services.AddDapper(Configuration);

            services.AddCSRedisCache(options =>
            {
                options.ConnectionString = this.Configuration["redis:ConnectionString"];
            });
            services.AddEssentialService();

            
            //services.AddAuthentication("Bearer")//添加授权模式
            //.AddIdentityServerAuthentication(Options =>
            //{
            //    Options.Authority = "http://localhost:5000";//授权服务器地址
            //    Options.RequireHttpsMetadata = false;//是否是https
            //    //Options.JwtValidationClockSkew = TimeSpan.FromSeconds(0);//设置时间偏移
            //    Options.ApiName = "MSS_WEBAPI";
            //});
            //跨域 Cors
            services.AddCors(options =>
            {
                //options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                options.AddPolicy("AllowAll", p => p.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            services.AddDistributedMemoryCache();
            //services.AddSession();

            services.AddSingleton<GlobalActionFilter>();
            services.AddMvc(
            options =>
            {
                options.Filters.Add<GlobalActionFilter>();
            }
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info{ Title = "MRO API", Version = "v1" });
                //注入WebAPI注释文件给Swagger  
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, "MyAuthor.xml");
                //c.IncludeXmlComments(xmlPath);

                //c.IgnoreObsoleteActions();
                //////options.IgnoreObsoleteControllers();
                ////// 类、方法标记 [Obsolete]，可以阻止【Swagger文档】生成
                //c.DescribeAllEnumsAsStrings();
                ////c.OperationFilter<FormDataOperationFilter>();

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime, IOptions<ConsulServiceEntity> consulService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseAuthentication();
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //app.Map("/health", HealthMap);
            //app.RegisterConsul(lifetime,consulService);
            //app.UseSession();
            app.UseCors("AllowAll");
            app.UseMvc();
        }

        private void InitConst()
        {
            Common.Const.PAGESIZE = Convert.ToInt32(Configuration["InitConst:PageSize"]);
            INIT_PASSWORD = Configuration["InitConst:Password"];
            PWD_RANDOM_MAX = Convert.ToInt32(Configuration["InitConst:PwdRandomMax"]);
            //REDISConn_AUTH = Configuration["redis:ConnectionString"];
            //REDIS_AUTH_KEY_ACTIONINFO = Configuration["redis:tb_action_info"];
            //REDIS_AUTH_KEY_ROLEACTION = Configuration["redis:tb_role_action"];
            //REDIS_AUTH_KEY_USER = Configuration["redis:tb_user"];
        }

    }
}
