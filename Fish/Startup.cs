using System;
using FISH.DataAccess;
using FISH.DataAccess.Entities;
using FISH.Entities.Context;
using FISH.Models;
using FISH.Services;
using FISH.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FISH
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private readonly string EmailConfirmationTokenProviderName = "ConfirmEmail";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            CommEnvironment.RootPath = env.ContentRootPath;
            CommEnvironment.VirtualDirectory = Configuration.GetSection("VirtualDirectory").GetValue<string>("FilePath");
            CommEnvironment.Connectstring = Configuration.GetConnectionString("DefaultConnection");
            CommEnvironment.SmtpHost = Configuration.GetSection("SmtpSetting").GetValue<string>("SmtpHost");
            CommEnvironment.SmtpPort = Configuration.GetSection("SmtpSetting").GetValue<int>("SmtpPort");
            CommEnvironment.SmtpAccount = Configuration.GetSection("SmtpSetting").GetValue<string>("SmtpAccount");
            CommEnvironment.SmtpPassWord = Configuration.GetSection("SmtpSetting").GetValue<string>("SmtpPassWord");

        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddIdentityDataAccess<MainDbContext, ApplicationUser, ApplicationRole, Guid>();
            services.AddDbContext<MainDbContext>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<MainDbContext, Guid>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<TranslatedIdentityErrorDescriber>()
            .AddTokenProvider<ConfirmEmailDataProtectorTokenProvider<ApplicationUser>>(EmailConfirmationTokenProviderName);


            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Tokens settings
                options.Tokens.EmailConfirmationTokenProvider = EmailConfirmationTokenProviderName;

                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;

                // Cookie settings
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.Cookies.ApplicationCookie.CookieHttpOnly = true;
                options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
                options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOut";
                options.Cookies.ApplicationCookie.AccessDeniedPath = "/Account/AccessDenied";

                // User settings
                options.User.RequireUniqueEmail = false;

                //SignIn settings
                options.SignIn.RequireConfirmedEmail = true;
                //options.SignIn.RequireConfirmedPhoneNumber = true;

            });

            // ConfirmEmail Token Expiration
            services.Configure<ConfirmEmailDataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(7);
            });

            services.AddMvc(options =>
            {
                //options.Filters.Add(new RequireHttpsAttribute()); //SSL
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }
            ).AddDataAnnotationsLocalization()
            .AddViewLocalization();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddScoped<RoleManager<ApplicationRole>>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<UserManager<ApplicationUser>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile($"Logs/Date.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //方便Add 與 Update 取得使用者
            EntityBase.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            app.UseStaticFiles();

            app.UseIdentity();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
