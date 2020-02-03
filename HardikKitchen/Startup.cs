using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Taste.DataAccess;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.DataAccess.Data.Repository;
using Test.Utility;
using Stripe;
using Taste.DataAccess.Data.Intializer;

namespace HardikKitchen
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //remove defualtrole to specific roles that we define in static class roles
            //define here so we can do depedency injection from other class like  register.cshtml.cs
            services.AddIdentity<IdentityUser,IdentityRole>()
                .AddDefaultTokenProviders()
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDbInitializer, DbInitializer>();

            //configure session for 10 min timeout  and accept cookie only http and to essesntial
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                });

            //configure StripeSetting class from test.utility and passing paramater from appsetting.json
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

            //setting endpointrouting false and compability version 3.0
            //services.AddMvc(options => options.EnableEndpointRouting = false)
            //    .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            services.AddRazorPages();
            //add runtimecomplialation to controller and view
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //configure Facebook External Login
            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = "766865420489618";
                facebookOptions.AppSecret = "7f160a5c1e46b8e3abad11a0f401254a";
            });

            services.AddAuthentication().AddMicrosoftAccount(MicrosoftOptions =>
            {
                MicrosoftOptions.ClientId = "e0480b64-e18d-4178-80be-2e7c4715e5dc";
                MicrosoftOptions.ClientSecret = "L/q883/yha/zWf7lIDN_c1dP[:iZ?08V";
            });

            //services.ConfigureApplicationCookie(options =>

            //{

            //    options.LoginPath = $"/Identity/Account/Login";

            //    options.LogoutPath = $"/Identity/Account/Logout";

            //    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";

            //});



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            dbInitializer.Initialize();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            // app.UseMvc();

            //configuring api secret key from appsetting.json
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];
        }

    }
    
}
