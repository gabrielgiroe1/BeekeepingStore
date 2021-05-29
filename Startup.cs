using BeekeepingStore.AppDbContext;
using BeekeepingStore.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeekeepingStore.MappingProfiles;

namespace BeekeepingStore
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddMvc();
            services.AddControllersWithViews();

            services.AddIdentity<IdentityUser, IdentityRole>().
           AddEntityFrameworkStores<BeekeepingDbContext>().
           AddDefaultUI().AddDefaultTokenProviders();

            services.AddDbContext<BeekeepingDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Default")));


            //
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            //
            services.AddScoped<IDBInitializer, DBInitializer>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddCloudscribePagination();
            services.AddRazorPages();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDBInitializer dBInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //
                app.UseForwardedHeaders();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                //
                app.UseForwardedHeaders();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();
            //  app.UseMvcWithDefaultRoute();
            dBInitializer.Initialize();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapRazorPages();
                //endpoints.MapControllerRoute(
                //    "ByYearMonth",
                //    "make/propolis/{year:int:length(4)}/{month:int:range(1,12)}",
                //    new { controller = "make", action = "ByYearMonth" },
                //    new { year = @"2017|2018" }
                //    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Honey}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

        }

    }
}
